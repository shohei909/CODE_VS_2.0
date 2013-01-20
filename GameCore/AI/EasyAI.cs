/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/11/24
 * 時刻: 0:5FixedWidth	= 11,
 * 
 */
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameCore.AI{
	
	/// <summary>
	/// 一手読みのAI.
	/// </summary>
	public class EasyAI:GameAI{
		public EasyAI( Game target ):base( target ){}
		public override void Next(){
			double max = double.MinValue;
			Block b = Game.DefaultBlocks[ Target.CurrentStep ];
			int w 	= Game.Width - b.Size + 1;
			List<int[]> list = new List<int[]>();
			
			for(int i = 0; i < Game.ROTATE_NUM; i++){
				for(int j = -b.Left[i], l = w + b.Right[i]; j < l; j++){
					var clone = Target.Clone();
					clone.Next( j, i );
					double score = clone.Score;
					if( clone.GameOver ){ continue; }
					if( score >= max ){
						max 	= score;
						if( score > max ) list.Clear();
						list.Add( new int[]{ j, i } );
					}
				}
			}
			if( list.Count == 0 ){ list.Add( new int[]{ 0, 0 } ); }
			var ans = list[ rand.Next( list.Count ) ];
			Drop( ans[0], ans[1] );
		}
	}
}
