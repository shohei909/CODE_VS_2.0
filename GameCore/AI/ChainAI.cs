/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/0FixedWidth	= 11,
 * 時刻: 12:44
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCore.Model;

namespace GameCore.AI{
	
	/// <summary>
	/// 定型連鎖を導入したAI
	/// </summary>
	public class ChainAI:AdvancedAI{
		public ChainModel	Model;
		public int			FixedDepth;
		public int			FixedWidth;
		public int			FixedLimit;
		public bool			FixedMode 	= true;
		public bool			HaiteiMode	= true;
			
		public long			ScoreMin	= 100000;
		public string 		Output 		= "";
		
		public int			FirstFire	= -1;
		public Game			BeforeFire;
		public Game			FinishFix;
		
		public ChainAI( Game target, AISetting setting ):base( target, setting ){
			FixedDepth 	= setting.FixedDepth;
			FixedWidth	= setting.FixedWidth;
			FixedLimit	= setting.FixedLimit;
			HaiteiMode	= setting.HaiteiMode;
		}
		
		public override void Next(){
			Output = "";
			if( Resting ) 			Rest();
			else if( FixedMode )	FixedAttack();
			else 					Attack();
		}
		
		private void FixedAttack(){
			var data = Model.Analize( Target.Clone() );
			
			Output += Target.CurrentStep + ":\n" + data.BrokenCount;
			Output += "\n" + data.Score;
			
			Output += "\n" + data.MaxChain;
			Output += "\n" + data.Chain;
			Output += "\n" + string.Join(",", data.ChainSegs.Select(s => s.ToString()).ToArray());
			Output += "\n" + data.BingoCount +"/"+ Model.BingoMax;
			
			if( 
			   	(Game.DefaultBlocks.Length - Target.CurrentStep < FixedLimit && data.BingoCount == Model.BingoMax && !HaiteiMode) || 
			   	Game.DefaultBlocks.Length - Target.CurrentStep < 25
			   		){
				FixedMode = false;
				FinishFix = Target.Clone();
				Attack();
				return;
			}
			
			var futures	= new List<Future>();
			Block 	b 		= Game.DefaultBlocks[ Target.CurrentStep ];
			int 	w 		= Game.Width - Game.Size + 1;
			int		rest 	= Game.DefaultBlocks.Length - Target.CurrentStep - 1;
			int		depth 	= FixedDepth < rest ? FixedDepth : rest;
			
			for(int i = 0; i < Game.ROTATE_NUM; i++){
				for(int j = -b.Left[i], jl = w + b.Right[i]; j < jl; j++){
					Game clone = Target.Clone();
					clone.Next(j,i);
					if( clone.GameOver ) continue;
					data = Model.Analize( clone.Clone() );
					var move = new Move(j, i);
					futures.Add( new Future( move, clone, data.Score ) );
				}
			}
			
			futures = FixedReadFutures( futures, depth );
			
			if( futures.Count == 0 ){
				FixedMode = false;
				FinishFix = Target.Clone();
				Attack();
			}else{
				futures.Sort();
				var move = futures[0].Root;
				Output += "\n" + futures[0].Potaintial;
				Drop( move.Position, move.Rotate );
			}
		}
		
		private List<Future> FixedReadFutures( List<Future> futures, int depth ){
			if( depth <= 0 ) 					return futures;
			if( Prune( futures, FixedWidth ) ) 	return futures;
			
			var results = new List<Future>();
			foreach( var f in futures ){
				var game 	= f.Target;
				Block 	b 	= Game.DefaultBlocks[ game.CurrentStep ];
				int 	w 	= Game.Width - Game.Size + 1;
				for(int i = 0; i < Game.ROTATE_NUM; i++){
					for(int j = -b.Left[i], jl = w + b.Right[i]; j < jl; j++){
						Game clone = game.Clone();
						clone.Next(j,i);
						if( clone.GameOver ) continue;
						var data = Model.Analize( clone.Clone() );
						results.Add( new Future( f.Root, clone, data.Score ) );
					}
				}
			}
			return FixedReadFutures( results, depth - 1 );
		}
		
		protected override void Fire(){
			if( FirstFire < 0 ){
				BeforeFire = Target.Clone();
				Depth 		= 1;
				Width 		= 1;
				FirstFire 	= Target.CurrentStep;
			}
			base.Fire();
		}
		
		protected override void FinishRest(){
			base.FinishRest();
			Model = new ChainModel( Target, Setting.Factory, Setting.ChainLimit, Setting.HaiteiMode, Setting.ModelSetting );
		}
		
		protected override void ChainUp(){
			if( ChainMaxScore < ScoreMin ) 	Fire();
			else							base.ChainUp();
		}
	}
}
