/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/12
 * 時刻: 18:45
 * 
 */
using System;
using GameCore.Model;

namespace GameCore{
	
	/// <summary>
	/// 連鎖モデルを生成します。
	/// </summary>
	public class ChainFactory{
		public Pattern Body;
		public Pattern Joint;
		public Pattern Tail;
		public Pattern Head;
		public Pattern JointLeft;
		public int Wall;
		private static int IDCount;
		
		public ChainFactory( Pattern tail, Pattern body, Pattern joint, Pattern head, int wall = 0, Pattern jointLeft = null ){
			Tail = tail; Body = body; Joint = joint; Head = head; Wall = wall; JointLeft = jointLeft; 
		}
		
		public void Make( ChainModel target, Game game, int chainLimit ){
			ModelBlock[][] model = target.Model;
			int x = 0, y = game.GetFloorHeight(), dir = 1, chain = 0, bottom = y;
			target.Bottom = y;
			Pattern p;
			IDCount = 0;
			
			//目標形を用意する。
			SetPattern( model, p = Tail, chain, x, y, dir );
			while( true ){
				x 		+= p.NextX * dir;
				dir 	*= p.NextDir;
				chain 	+= p.Chain;
				y 		+= p.NextY;
				var j	= (dir > 0 || JointLeft == null) ? Joint : JointLeft;
				int nx = x + dir * (j.Width - 1);
				if( chain >= chainLimit ) break;
				if( (dir < 0 && nx <= Wall) || (dir > 0 && Game.Width - 1 <= nx) ){
						SetPattern( model, p = j, chain, x, y, dir );
				}else 	SetPattern( model, p = Body, chain, x, y, dir );
			}
			
			SetPattern( model, p = Head, chain, x, y, dir );
			chain 	+= p.Chain;
			SetupModel( model, game, target.Bottom );
			
			target.ChainLimit 	= chain;
		}
		
		//パターンを配置
		public void SetPattern( ModelBlock[][] model, Pattern pattern, int chain, int x, int y, int dir ){
			object[,] data = pattern.Data;
			for( int i = 0; i < pattern.Width; i++ ){
				for( int j = 0; j < pattern.Height; j++ ){
					object cell 	= data[j,i];
					int mx		= x + i * dir;
					int my		= y + pattern.Height - j - 1;
					
					if( cell is int ){
						int num = chain + (int)cell;
						model[mx][my] = new ModelBlock(IDCount++, mx, my, num, num, false, pattern.ScoreRate);
					}else if( cell == ChainConfig.E ){
						model[mx][my] = new ModelBlock(IDCount++, mx, my, -1, int.MaxValue, true, 1, 0 );
					}else if( cell is int[] ){
						int[] nums = cell as int[];
						model[mx][my] = new ModelBlock(IDCount++, mx, my, nums[0], nums[2], false, 1, nums[1]);
					}else{
						if( model[mx][my] == null ){
							model[mx][my] = new ModelBlock(IDCount++, mx, my, -1, int.MaxValue, false, pattern.ScoreRate);
						}else{
							if( model[mx][my].ScoreRate < pattern.ScoreRate ) 
								model[mx][my].ScoreRate = pattern.ScoreRate;
						}
					}
				}
			}
		}
		
		//足場の保存期間を指定、ブロックの空きを埋める。
		public void SetupModel( ModelBlock[][] model, Game game, int bottom ){
			for( int i = 0, w = model.Length; i < w; i++ ){
				int min = int.MaxValue;
				for( int j = model[i].Length - 1; j >= 0; j--){
					if( model[i][j] == null ){
						if( j < bottom && game.Field[i][j] > Game.Sum )	
							model[i][j] = new ModelBlock(IDCount++, i, j, -1, min, false, 1, game.Field[i][j] );
						else								
							model[i][j] = new ModelBlock(IDCount++, i, j, -1, min);
					
						continue;
					}
					var block = model[i][j];
					if( block.Chain >= 0 )				if( min > block.Chain ) min = block.Chain;
					if( block.Ground == int.MaxValue ) 	block.Ground = min;
				}
			}
		}
		
	}
}
