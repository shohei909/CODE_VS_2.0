/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/11/20
 * 時刻: 16:59
 * 
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GameCore{
	public class Game{
		public int[][] 	Field;
		public int[]	HeightList;
		
		public double	Score;
		public int		MaxChain;
		public int  	CurrentStep;
		public bool 	GameOver 		= false;
		
		
		public int		LastChain;
		public int[]	ChainShape;
		
		private bool	Dead			= false;
		
		public int		ChangedRight;
		public int		ChangedLeft;
		public int		ChangedTop;
		public int		ChangedBottom;
		public bool[]	Marked;
		
		static public int 			Size;
		static public int 			Width;
		static public int 			Height;
		static public int 			Sum;
		static public int 			StepNum;
		static public int 			P;
		static public Block[] 		DefaultBlocks;
		
		public const int ROTATE_NUM 	= 4;
		
		
		///テキストから情報を読み取る。
		static public void Init( TextReader input ){
			string[] line 	= input.ReadLine().Split( ' ' );
			
			Width 		= int.Parse(line[0]);
			Height		= int.Parse(line[1]);
			Size	 	= int.Parse(line[2]);
			Sum		 	= int.Parse(line[3]);
			StepNum		= int.Parse(line[4]);
			
			P 			= Width + 15;
			
			DefaultBlocks 	= new Block[StepNum];
			
			for(int i = 0; i < StepNum; i++ ){
				DefaultBlocks[i] = new Block();
				DefaultBlocks[i].Read( input, Size );
			}
		}
		
		
		public Game(){
			Field 		= new int[Width][];
			HeightList	= new int[Width];
			Marked		= new bool[Width];
			
			int h = Height + Size;
			for( var x = 0; x < Width; x++ )
				Field[x] = new int[ h ];
		}
		
		
		///複製を作る。
		public Game Clone(){
			var clone = new Game();
			clone.Score			= Score;
			clone.MaxChain		= MaxChain;
			clone.CurrentStep	= CurrentStep;
			clone.GameOver		= GameOver;
			
			HeightList.CopyTo( clone.HeightList, 0 );
			
			for(int x = 0; x < Width; x++ )	Field[x].CopyTo( clone.Field[x], 0 );
			
			return clone;
		}
		
		//このゲームをオブジェクトプールに蓄積
		public void Kill(){
			if( Dead ) return;
			Dead = true;
		}
		
		///パックの投下してゲームを進める。
		public void Next( int position, int rotate ){
			if( GameOver ){ return; }
			var block 	= DefaultBlocks[ CurrentStep++ ];
			
			Drop(
				block.Projections[rotate],
				position,
				Size - block.Right[rotate],
				block.Left[rotate]
			);
		}
		
		//指定した配列のブロックを落とす。
		public void Drop( List<int>[] block, int position, int width, int left = 0 ){
			if( GameOver ){ return; }
			LastChain = 0;
			
			ChangedLeft 	= position 	+ left;
			ChangedRight 	= position 	+ width;
			ChangedBottom	= Height 	+ Size;
			ChangedTop		= 0;
			
			
			for(int i = left, x = ChangedLeft; i < width; i++, x++){
				var row = block[i]; 
				if( row.Count == 0 ) continue;
				
				int h = HeightList[x];
				if( h < ChangedBottom ) ChangedBottom = h;
				
				row.CopyTo( Field[x], h );
				h = (HeightList[x] += row.Count);
				if( h > ChangedTop ) ChangedTop = h;
			}
		
			var chain = 0;
			var count = 0;
			var limit = P + (CurrentStep / 100) - 1;
			ChainShape = new int[Width];
			
			while( (count = Mark()) > 0 ){
				Delete();
				
				if( chain >= limit )	Score += (double)(((long)1 << limit) * (chain - limit + 1) * count);
				else					Score += (double)(((long)1 << chain) * count);
				chain++;
			}
			
			for(int x = 0; x < Width; x++ )
				if( HeightList[x] > Height ) GameOver = true;
			
			LastChain = chain;
			if( MaxChain < chain ){ MaxChain = chain; }
		}
		
		//合計がSumと一致する部分を探して、フラグを立てる。
		public int Mark(){
			if( ChangedLeft >= ChangedRight || ChangedBottom >= ChangedTop ) return 0;
			
			int count = 0;
			
			//縦↑
			for( int x = ChangedLeft, xl = ChangedRight; x < xl; x++ ){
				int bottom = 0, sum = 0, y;
				int[] row = Field[x];
				
				//下端を調べる。↓
				for( y = ChangedBottom; y >= 0; y-- )
					if( (sum += Field[x][y] & 0x7FFFFF) > Sum ){
						bottom = y + 1;
						break;
					}
				
				y = HeightList[x];
				int outerLen 	= y - bottom;
				int innerLen	= y - ChangedBottom; 
				y--;
				
				for( int i = 0; i < innerLen; i++, y-- ){
					sum = 0;
					int c = 0;
					
					for( int j = i, yy = y; j < outerLen; j++, yy-- )
						if( (sum += (row[yy] & 0x7FFFFF)) >= Sum ){
							c = j + 1; break;
						}
					
					if( sum == Sum ){
						count += c - i;
						for( int j = i, yy = y; j < c; j++, yy-- ){
							row[yy] 		|= 0x800000;
							Marked[x]		= true;
						}
					}
				}
			}
			
			//横→
			for( int i = ChangedBottom, il = ChangedTop; i < il; i++ ){
				int left = 0, sx = ChangedLeft, sum = 0;
				
				//左端を調べる。←
				for( int x = sx; x >= 0; x-- )
					if( HeightList[x] <= i || (sum += Field[x][i] & 0x7FFFFF) > Sum ){
						left = x + 1;　break;
					}
				
				//内部を調べる。→
				for( int x = sx; x < ChangedRight; x++ )
					if( HeightList[x] <= i || (Field[x][i] & 0x7FFFFF) > Sum ){
						count += MarkLine( x - left, x - sx, (x - 1), i, -1, 0 );
						sx = left = (x + 1);
					}
				
				if( sx >= ChangedRight ) continue;
				
				//右端を調べる。→
				sum = 0;　int right = Width;
				for( int x = ChangedRight - 1; x < Width; x++ )
					if( HeightList[x] <= i || (sum += (Field[x][i] & 0x7FFFFF)) > Sum ){
						right = x;　break;
					}
				
				count += MarkLine( right - left, right - sx, (right - 1), i, -1, 0 );
			}
			
			//ななめ
			int w = ChangedRight - ChangedLeft, h;
			for( int i = ChangedBottom, il = ChangedTop; i < il; i++ ){
				h = il - i;
				count += SequenceMarkLineUp( w < h ? w : h, ChangedLeft, i );
				
				h = i - ChangedBottom + 1;
				count += SequenceMarkLineDown( w < h ? w : h, ChangedLeft, i );
			}
			
			h = ChangedTop - ChangedBottom;
			for( int i = ChangedLeft + 1, il = ChangedRight; i < il; i++ ){
				w = il - i;
				count += SequenceMarkLineUp( w < h ? w : h, i, ChangedBottom );
				count += SequenceMarkLineDown( w < h ? w : h, i, ChangedTop-1 );
			}
			
			return count;
		}
		
		
		
		//指定した範囲を含むMarkLineを実行します。斜め↑
		private int SequenceMarkLineUp( int len, int sx, int sy ){
			int count = 0, leftX = 0, endX = sx + len, sum = 0;
			
			//左端を調べる。
			for( int x = sx, y = sy;; x--, y--){
				if( x < 0 || y < 0 || HeightList[x] <= y || (sum += Field[x][y] & 0x7FFFFF) > Sum ){
					leftX = (x + 1);　break;
				}
			}
	
			//内部を調べる。→
			for( int i = 0, x = sx, y = sy; i < len; i++, x++, y++ )
				if( HeightList[x] <= y || (Field[x][y] & 0x7FFFFF) > Sum ){
					count += MarkLine( x - leftX, x - sx, (x - 1), (y - 1), -1, -1 );
					sx = leftX = (x + 1);
				}
			
			if( sx >= endX ) return count;
			
			//右端を調べる
			int endY = sy + len - 1;
			int rightX = 0, rightY = 0;
			sum = 0;
			for( int x = endX - 1, y = endY;; x++, y++ )
				if( x >= Width || HeightList[x] <= y || (sum += (Field[x][y] & 0x7FFFFF)) > Sum ){
					rightX = x;　rightY = y; break;
				}
			
			count += MarkLine( rightX - leftX, rightX - sx, (rightX - 1), (rightY - 1), -1, -1 );
			return count;
		}
		
		//指定した範囲を含むMarkLineを実行します。斜め↓
		private int SequenceMarkLineDown( int len, int sx, int sy ){
			int count = 0, leftX = 0, endX = sx + len, sum = 0;
			
			//左端を調べる。
			for( int x = sx, y = sy;; x--, y++)
				if( x < 0 || HeightList[x] <= y || (sum += Field[x][y] & 0x7FFFFF) > Sum ){
					leftX = (x + 1);　break;
				}
	
			//内部を調べる。→
			for( int i = 0, x = sx, y = sy; i < len; i++, x++, y-- )
				if( HeightList[x] <= y || (Field[x][y] & 0x7FFFFF) > Sum ){
					count += MarkLine( x - leftX, x - sx, (x - 1), (y + 1), -1, 1 );
					sx = leftX = (x + 1);
				}
			
			if( sx >= endX ) return count;
			
			//右端を調べる
			int endY = sy - len + 1;
			int rightX = 0, rightY = 0;
			sum = 0;
			for( int x = endX - 1, y = endY;; x++, y-- )
				if( x >= Width || y < 0 || HeightList[x] <= y || (sum += (Field[x][y] & 0x7FFFFF)) > Sum ){
					rightX = x;　rightY = y; break;
				}
			
			count += MarkLine( rightX - leftX, rightX - sx, (rightX - 1), (rightY + 1), -1, 1 );
			return count;
		}
		
		
		//指定したライン上のブロックを探索して、Sumに一致する部分に消去フラグを立てる。
		private int MarkLine( int outerLen, int innerLen, int x, int y, int dx, int dy ){
			if( outerLen <= 1 ) return 0;
			int count = 0;
			
			for( int i = 0; i < innerLen; i++, x += dx, y += dy ){
				int sum = 0, c = 0;
				
				for( int j = i, xx = x, yy = y; j < outerLen; j++, xx += dx, yy += dy ){
					int cell = (Field[xx][yy] & 0x7FFFFF);
					if( (sum += cell) >= Sum ){
						c = j + 1; break;
					}
				}
				
				if( sum == Sum && c - i > 1 ){
					count += c - i;
					for( int j = i, xx = x, yy = y; j < c; j++, xx += dx, yy += dy ){
						Field[xx][yy] 	|= 0x800000;
						Marked[xx]		= true;
					}
				}
			}
			return count;
		}
		
		
		//縦方向の指定したライン上のブロックを探索して、Sumに一致する部分に消去フラグを立てる。
		private int MarkLineV( int outerLen, int innerLen, int x, int y ){
			int count = 0;
			int[] row = Field[x];
			for( int i = 0; i < innerLen; i++, y-- ){
				int sum = 0, c = 0;
				
				for( int j = i, yy = y; j < outerLen; j++, yy-- )
					if( (sum += (row[yy] & 0x7FFFFF)) >= Sum ){
						c = j + 1; break;
					}
				
				if( sum == Sum ){
					count += c - i;
					for( int j = i, yy = y; j < c; j++, yy-- ){
						row[yy] 		|= 0x800000;
						Marked[x]		= true;
					}
				}
			}
			
			return count;
		}
		
		
		//消去フラグを立てたブロックを消去。
		public void Delete(){
			ChangedLeft 	= Width;
			ChangedRight 	= 0;
			ChangedBottom	= Height + Size;
			ChangedTop		= 0;
			
			for(int x = 0; x < Width; x++ ){
				if(! Marked[x] ) continue;
				Marked[x] = false;
				
				var row 	= Field[x];
				int h 		= HeightList[x];
				int k 		= 0;
				int	bottom	= -1;
				bool changed	= false;
				
				for(int j = 0; j < h; j++ ){
					int	val = row[j];
					if( j != k ) { row[k] = val; changed = true; }
					
					if( (val & 0x800000) == 0 )		k++;
					else{
						ChainShape[x]++;
						if( bottom == -1 )						bottom = k;
					}
				}
				
				if( changed ){
					if( ChangedTop < k )			ChangedTop		= k;
					if( ChangedLeft　> x ) 			ChangedLeft 	= x;
					if( ChangedBottom > bottom )	ChangedBottom	= bottom;
					ChangedRight = x + 1;
				}
				
				HeightList[x] = k;
				for(; k < h; k++) row[k] = 0;
			}
		}
		
		//高さを取得
		public int GetFloorHeight(){
			int baseY = 0;
			for(int i = 0; i < Width; i++)
				for(int j = baseY; j < Height; j++){
					if( Field[i][j] <= Sum ) 	continue;
					if( baseY <= j ) 			baseY = j + 1;
				}
			
			return baseY;
		}
		
		//盤面の綺麗さを取得。
		public double Cleanliness(){
			int score = 10000, maxF 	= 0, bf   	= 0;
		
			for(int i = 0; i < Width; i++){
				var row = Field[i];
				int l = HeightList[i], f = 0;
				score -= l * 4;
				
				for(int j = 0; j < l; j++)
					if( Field[i][j] > Sum ){
						score -= j * 3 + (j - f);
						if( f < j ) f = j;
					}
				
				if( maxF < f ) 	maxF = f;
				if( i != 0 )	score -= Math.Abs( f - bf ) * 5;
				bf = f;
			}
			
			return score - maxF * 500;
		}
	}
}