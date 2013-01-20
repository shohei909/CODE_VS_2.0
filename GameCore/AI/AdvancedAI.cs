/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/11/29
 * 時刻: 19:16
 * 
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.AI;


namespace GameCore.AI{
	/// <summary>
	/// 発火と積み上げを切り替えて実行するAI。
	/// 積み上げ時の得点が、発火時の得点を下回った場合に発火を行う。
	/// 積み上げの際の先読みは、5つの手に絞って、最大7手先まで行う。
	/// 発火は次の1手で取れる点が最大になる手を打つ。
	/// </summary>
	
	public class AdvancedAI:GameAI{
		//積み上げから発火に切り替える点数比
		public double 	FireRate = 0.8;
		
		//読みの深さ。
		public int		Depth;
		//読みの広さ。
		public int 		Width;
		
		//床底上げの高さ
		public int		FloorHeight;
		
		//連鎖チェックの制限。
		public int		CheckLimit;
		//連鎖チェックの制限。
		public int		CheckHeight = 1;
		public int		HeightSpan = 2;
		
		//先読みの閾値になる倍率。
		public int 		FutureMin;
		
		public Move			ChainMove;
		public double		ChainMaxScore;
		public Move			FireMove;
		public double		FireMax;
		
		public Future		RestFuture;
		
		public bool[][] CheckList;
		public List<int>[,][] CheckBlocks;
		public AISetting 	Setting;
		public bool			Resting = true;
		
		public int RestDepth 	= 4;
		public int RestWidth 	= 60;
		
		
		public AdvancedAI( Game target, AISetting setting ):base( target ){
			Depth 		= setting.Depth; 
			Width 		= setting.Width; 
			CheckLimit 	= setting.CheckLimit; 
			FloorHeight	= setting.FloorHeight;
			FireRate	= setting.FireRate;
			FutureMin	= setting.FutureMin;
			Setting		= setting;
			
			CheckBlocks = new List<int>[CheckHeight, Game.Sum-1][];
			
			for( int i = 0; i < CheckHeight; i++ )
				for( int j = 0; j < (Game.Sum - 1); j++ ){
					var row = new List<int>();
					for( int k = 0; k < i * HeightSpan; k++ ) row.Add( Game.Sum + 1 );
					row.Add( j + 1 );
					CheckBlocks[i,j] = new List<int>[1]{ row };
				}
			
			
			CheckList = new bool[Game.StepNum][];
			for( int i = 0; i < Game.StepNum; i++ ){
				var list = CheckList[i] = new bool[ Game.Sum - 1 ];
				int l = i + CheckLimit;
				if( l > Game.StepNum ) l = Game.StepNum;
				
				for( int j = i; j < l; j++ ){
					var b = Game.DefaultBlocks[ j ].Data;
					for( int x = 0; x < Game.Size; x++ )
						for( int y = 0; y < Game.Size; y++ ){
							var c = b[x,y];
							if( 0 < c && c < Game.Sum ) list[c - 1] = true;
						}
				}
			}
		}
		
		public override void Next(){
			if( Resting )	Rest();
			else			Attack();
		}
		
		
		//前半は大人しくして、ブロックを低く保つ。
		public void Rest(){
			var 	futures		= new List<Future>();
			Block 	b 			= Game.DefaultBlocks[ Target.CurrentStep ];
			int 	w 			= Game.Width - Game.Size + 1;
			int		rest 		= Game.DefaultBlocks.Length - Target.CurrentStep - 1;
			int		depth 		= RestDepth < rest ? RestDepth : rest;
			double	baseScore	= Target.Score;
			
			for(int i = 0; i < Game.ROTATE_NUM; i++)
				for(int j = -b.Left[i], jl = w + b.Right[i]; j < jl; j++){
					Game clone = Target.Clone();
					clone.Next(j,i);
					if(! clone.GameOver )
						futures.Add( new Future( new Move( j, i ), clone, clone.Cleanliness() ) );
				}
			
			futures = RestReadFutures( futures, depth );
			
			if( futures.Count == 0 )	Drop( 0, 0 );
			else{
				futures.Sort();
				var f 		= futures[ 0 ] ;
				
				if( f.Target.GetFloorHeight() > FloorHeight ){
					FinishRest(); 
					Next();
					return;
				}
				
				var move 	= f.Root;
				Drop( move.Position, move.Rotate );
			}
		}
		
		
		//枝ごとのスコアの計算
		private List<Future> RestReadFutures( List<Future> futures, int depth ){
			if( depth <= 0 ) 					return futures;
			Prune( futures, RestWidth );
			
			var results = new List<Future>();
			foreach( var f in futures ){
				var game 	= f.Target;
				Block 	b 	= Game.DefaultBlocks[ game.CurrentStep ];
				int 	w 	= Game.Width - Game.Size + 1;
				for(int i = 0; i < Game.ROTATE_NUM; i++)
					for(int j = -b.Left[i], jl = w + b.Right[i]; j < jl; j++){
						Game clone = game.Clone();
						clone.Next(j,i);
						if( clone.GameOver ) continue;
						results.Add( new Future( f.Root, clone, clone.Cleanliness() ) );
					}
			}
			
			return RestReadFutures( results, depth - 1 );
		}
		
		
		//不定型連鎖を組む。
		public void Attack(){
			var 	futures		= new List<Future>();
			Block 	b 			= Game.DefaultBlocks[ Target.CurrentStep ];
			int 	w 			= Game.Width - Game.Size + 1;
			int		rest 		= Game.DefaultBlocks.Length 	- Target.CurrentStep - 1;
			int		depth 		= Depth < rest ? Depth : rest;
			double	baseScore	= Target.Score;
			FireMax = ChainMaxScore　= double.MinValue;
			
			for(int i = 0; i < Game.ROTATE_NUM; i++)
				for(int j = -b.Left[i], jl = w + b.Right[i]; j < jl; j++){
					Game clone = Target.Clone();
					clone.Next(j,i);
					if(! clone.GameOver ){
						double score 	= clone.Score - baseScore;
						var move 	= new Move( j, i, score );
						
						if( FireMax <= score ){
							FireMax		= score;
							FireMove 	= move;
						}
						
						if( depth > 0 ) futures.Add( new Future( move, clone, CalcPotaintial(clone, move, depth) ) );
					}
				}
			
			if( futures.Count == 0 ){
				Fire();
				return;
			}
			ReadFuture( futures, depth - 1 );
			
			if( FireMax * FireRate > ChainMaxScore )	Fire();
			else										ChainUp();
		}
		
		private void ReadFuture( List<Future> futures, int depth ){
			if( depth <= 0 ) 												return;
			if( Prune( futures, ChainMove, Width, ChainMaxScore / FutureMin ) )	return;
			
			var results = new List<Future>();
			
			foreach( var f in futures ){
				var game		= f.Target;
				Block 	b 		= Game.DefaultBlocks[ game.CurrentStep ];
				int w 			= Game.Width - Game.Size + 1;
				for(int i = 0; i < Game.ROTATE_NUM; i++)
					for(int j = -b.Left[i], jl = w + b.Right[i]; j < jl; j++){
						Game clone = game.Clone();
						clone.Next(j,i);
						
						if( clone.GameOver )　continue;
						double 	score = clone.Score - game.Score;
						var p = CalcPotaintial( clone, f.Root, depth );
						results.Add( new Future( f.Root, clone, p ) );
					}
			}
			
			ReadFuture( results, depth - 1 );
		}
		
		
		//評点
		private double CalcPotaintial( Game game, Move move, int depth ){
			double 	max = double.MinValue;
			Block 	b = Game.DefaultBlocks[ game.CurrentStep　];
			int 	w = Game.Width - Game.Size + 1;
			
			//次のブロックを落下させてみる。
			for(int i = 0; i < Game.ROTATE_NUM; i++ ){
				for(int j = -b.Left[i], jl = w + b.Right[i]; j < jl; j++){
					Game clone 			=  game.Clone();
					clone.Next(j,i);
					
					if( clone.GameOver ) continue;
					double score = clone.Score - game.Score;
					if( max < score ){ max = score; }
				}
			}
			
			if( ChainMaxScore <= max ){ 
				ChainMaxScore 	= max;
				ChainMove 		= move;
			}
			
			if( depth <= 1 ) return max;
			
			int h0 = 0, h1 = 0, h2 = game.HeightList[0];
			var cl = CheckList[ game.CurrentStep ];
			int end = (Game.Sum-1);
			
			//連鎖テスト用のブロックを落下させてみる。
			for( int x = 0; x < Game.Width; x++ ){
				h0 = h1; h1 = h2;
				if( x + 1 < Game.Width )	h2 = game.HeightList[ x + 1 ];
				else						h2 = 0;
				
				for( int i = 0; i < CheckHeight; i++ ){
					if( h0 <= h1 + i * HeightSpan && h2 <= h1 + i * HeightSpan ) break;
					
					for( int j = 0; j < end; j++ ){
						if(! cl[ j ] ) continue;
						Game clone = game.Clone();
						clone.Drop( CheckBlocks[i,j], x, 1, 0 );
						clone.Kill();
						
						if( clone.GameOver || clone.LastChain == 0 )　continue;
						double score = (clone.Score - game.Score);
						if( max < score ) max = score;
					}
				}
			}
			return max;
		}
		
		//先読みする手を枝刈り、残りの手が1種類になったら、True。
		protected static bool Prune( List<Future> futures, int width ){
			int l = futures.Count;
			if( l < 2 ){ return true; }
			futures.Sort();
			
			if( l > width ){
				futures.RemoveRange( width, l - width );
				l = width;
			}
			
			var first = futures[0].Root;
			for( int i = 1; i < l; i++ )
				if( first != futures[i].Root ) return false;
				
			return true;
		}
		
		//先読みする手を足切り点付きで枝刈り、残りの手が1種類になったら、True。
		protected static bool Prune( List<Future> futures, Move currentMove, int width, double minScore ){
			futures.Sort();
			
			int l = futures.Count;
			
			if( l > width ){
				futures.RemoveRange( width, l - width );
				l = width;
			}
			
			for( int i = 0; i < l; i++ )
				if( futures[i].Potaintial < minScore ){
					futures.RemoveRange(i, l - i);
					l = i;
					break;
				}
			
			for( int i = 0; i < l; i++ )
				if( currentMove != futures[i].Root ) return false;
				
			return true;
		}
		
		protected virtual void Fire(){ Drop( FireMove.Position, FireMove.Rotate ); }
		protected virtual void ChainUp(){ Drop( ChainMove.Position, ChainMove.Rotate ); }
		protected virtual void FinishRest(){ Resting = false; }
	}
	
	//着手
	public class Move{
		public int 	Rotate;
		public int 	Position;
		public Game	Target;
		
		//発火時の点数。次の1手で取れる得点。
		public double	FireScore;
		public Move( int position, int rotate, double fireScore = 0 ){
			Rotate = rotate; Position = position; 
			FireScore = fireScore;
		}
	}
	
	//先読み予想
	public class Future:IComparable{
		public double 		Potaintial;
		public Move 		Root;
		public Game 		Target;
		
		public Future( Move root, Game target, double potaintial ){
			Root = root; Target = target; Potaintial = potaintial;
		}
		
		public int CompareTo( object o1 ){
			return (Potaintial > (o1 as Future).Potaintial) ? -1 : 1;
		}
	}
}
