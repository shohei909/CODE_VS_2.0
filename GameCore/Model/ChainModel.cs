/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/03
 * 時刻: 13:09
 * 
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameCore.Model{
	/// <summary>
	/// 定型連鎖に使う連鎖用の型。
	/// </summary>
	
	public class ChainModel{
		
		//型
		public ModelBlock[][] 		Model;
		//すべてのブロック。
		public List<ModelBlock> 	AllBlocks;
		//破壊しなくてはいけないブロックの組み合わせ。
		public List<BlockGroup> 	ChainGroups;
		//連鎖中に破壊しちゃいけないブロック。
		public List<BlockGroup>[] 	SolidGroups;
		//連鎖中に破壊しちゃいけないブロックのルート
		public List<BlockGroup> 	RootSolidGroups;
		
		//空にしとく位置
		public List<ModelBlock> 	MustBlocks;
		//連鎖の上に乗っかってるブロック
		public List<ModelBlock> 	CeilBlocks;
		//邪魔ブロック推奨の位置
		public List<ModelBlock> 	GroundBlocks;
		
		public double BingoScore;
		public double LizhiScore;
		public double CeilScore;
		public double CeilHScore;
		public double BrokenScore;
		public double MaxChainScore;
		public double ChainScore;
		public double GroundScore;
		public double GroundHScore;
		public double GroundNGScore;
		
		public bool HaiteiMode;
		
//		public double SolidScore = 0.5;
		
		//
		public int    BingoMax;
		public int	  BrokenLimit;
		public int	  ChainLimit;
		
		public int	IDCount;
		public int 	Sum;
		public int	Width;
		public int	Bottom;
		public int	Height;
		public int	SegCount;
		
		private bool SegAdded;
		
		public long AnalizeCounter = 0;
			
		public static double	Time1 = 0;
		public static double	Time2 = 0;
		public static double	Time3 = 0;
		
		public static Stopwatch sw1 	= new Stopwatch();
		public static Stopwatch sw2 	= new Stopwatch();
		public static Stopwatch sw3 	= new Stopwatch();
		public string Output 		= "";
		private bool Changed 		= false;
		
		
		public ChainModel( Game game, ChainFactory factory, int chainLimit, bool haiteiMode, ModelSetting setting ){
			setting.Setup( this );
			
			Width 			= Game.Width;
			Height 			= Game.Height + Game.Size;
			Sum				= Game.Sum;
			ChainLimit		= chainLimit;
			HaiteiMode		= haiteiMode;
			
			AllBlocks 			= new List<ModelBlock>();
			ChainGroups		= new List<BlockGroup>();
			SolidGroups		= new List<BlockGroup>[Sum - 1];
			for(int i = 0; i < (Sum - 1); i++) 	SolidGroups[i] = new List<BlockGroup>();
			
			Model = new ModelBlock[Width][];
			for(int i = 0; i < Width; i++)
				Model[i] = new ModelBlock[Height];
			
			factory.Make( this, game, ChainLimit );
			
			for(int i = 0; i < ChainLimit; i++) ChainGroups.Add( new BlockGroup( new List<ModelBlock>() ) );
			
			SetupGroup( game );
			Reset( game );
		}
		
		//連鎖モデルを整形。リストを作成。
		public void SetupGroup( Game game ){
			
			//各種ブロックのリストを作成
			MustBlocks 		= new List<ModelBlock>();
			CeilBlocks 		= new List<ModelBlock>();
			GroundBlocks 	= new List<ModelBlock>();
			ModelBlock firedBlock	= null;
			ModelBlock fireBlock 	= null;
			
			for(int i = 0; i < Width; i++){
				for(int j = 0; j < Height; j++){ 
					var b = Model[i][j];
					if( b != null ){
						AllBlocks.Add(b);
						if( b.Chain >= 0 ){
							ChainGroups[ b.Chain ].Blocks.Add( b );
							if( b.Chain == ChainLimit - 1 )		firedBlock = b;
						}
						if( b.MustNumber > -1 ){
							MustBlocks.Add( b );
							if( b.End ){
								fireBlock = b;
								ChainGroups[ ChainLimit - 1 ].Blocks.Add( b );
							}
						}
						else if( b.Ground == int.MaxValue )		CeilBlocks.Add( b );
						else if( b.Chain == -1 ) 				GroundBlocks.Add( b );
					}
				}
			}
			
			//
			if( HaiteiMode ) SetHeadNumber( game, fireBlock, firedBlock );
			
			//連鎖を実行して、壊れちゃいけないグループを回収する。
			for( int i = ChainLimit - 1; i >= 0; i-- )
				ChainGroups[i].Blocks.Sort();
			
			for( int i = ChainLimit - 1; i >= 0; i-- ){
				CollectSolid( i );
				BreakAt( i );
			}
			
			MakeHeap();
			BingoMax = MustBlocks.Count + ChainLimit - 1; 
			
		}
		
		
		//海底用に発火ブロックの数字をそろえる
		public void SetHeadNumber( Game game, ModelBlock fire, ModelBlock fired ){
			int max = 0;
			var block = Game.DefaultBlocks[ Game.StepNum - 1 ];
			for( int x = 0; x < Game.Size; x++ ){
				for( int y = 0; y < Game.Size; y++ ){
					var d = block.Data[x,y];
					if( d > max && d < Sum ) max = d;
				}
			}
			
			if( max > 0 ){
				fire.MustNumber		= max;
				fired.MustNumber 	= Sum - max;
				MustBlocks.Add( fired );
			}
		}
		
		//現在の連鎖モデルから、非破壊グループを見つけて登録。
		private void CollectSolid( int chain ){
			//縦
			for( int i = 0; i < Width; i++ )
				SetSolid( 0, 1, i, 0, Height, chain );
			
			//横
			for( int i = 0; i < Height; i++ ){
				SetSolid( 1, 0, 0, i, Width, chain );
			}
			
			//ななめ→↓
			int w = Width -1;
			int h = Height -1;
			for( int i = 1; i < Height; i++ ){
				int c = (Width < i + 1) ? Width : (i + 1);
				SetSolid( 1, -1, 0, i, c, chain );
			}
			for( int i = 1; i < w - 1; i++ ){
				int c = (Height < Width - i) ? Height : Width - i;
				SetSolid( 1, -1, i, h, c, chain );
			}
				
			//ななめ↓←
			for( int i = 1; i < Height; i++ ){
				int c = (Width < i + 1) ? Width : (i + 1);
				
				SetSolid( -1, -1, w, i, c, chain );
			}
			for( int i = w - 1; i >= 1; i-- ){
				int c = (Height < i + 1) ? Height : i + 1;
				SetSolid( -1, -1, i, h, c, chain );
			}
		}
		
		
		//指定した列から、非破壊グループを見つけて登録。
		private void SetSolid( int dx, int dy, int sx, int sy, int count, int chain ){
			if( count < 2 ) return;
			int il = count > Sum ? Sum : count;
			
			for(int i = 2; i <= il; i++ )
				for(int j = 0, jl = count - i + 1, x = sx, y = sy; j < jl; j++, x += dx, y += dy ){
					List<ModelBlock> g = new List<ModelBlock>();
					bool solid = false;
					for(int k = 0, xx = x, yy = y; k < i; k++, xx += dx, yy += dy ){
						ModelBlock b;
						g.Add( b = Model[xx][yy] );
						if( (b.Chain < 0 && b.MustNumber < 0) || b.MustNumber == 0 || b.MustNumber > Sum ){
							solid = false;
							break;
						}
						if( b.Ground < chain ) solid = true;
					}
					if( solid ){
						g.Sort();
						if(! AlreadyExists( g ) )
							SolidGroups[i - 2].Add( new BlockGroup( g ) );
					}
				}
		}
		
		
		//連鎖の状態を分析する。
		public AnalizeData Analize( Game game, bool outon = false ){
			AnalizeData			result			= new AnalizeData();
			Reset( game );
			CalcScore( game, result );
			//SimulateChain( game, result, outon );
			
			result.Score += result.MaxChain 	* MaxChainScore;
			
			foreach( int c in result.ChainSegs )
				result.Score += (c - 1) * ChainScore;
			
			return result;
		}
		
		//シミュレート前に連鎖モデルをリセット。
		public void Reset( Game game ){
			for(int i = 0; i < Width; i++)
				for(int j = 0; j < Height; j++)
					Model[i][j] = null;
			
			foreach( ModelBlock b in AllBlocks ){
				b.Reset( Sum, game.Field );
				Model[b.X][b.Y] = b;
			}
			
			foreach(var g in ChainGroups)
				g.Reset();
		}
		
		
		//chainで指定した、部分の連鎖ブロックを壊して全体のブロックを落とす。
		public void BreakAt(int chain){
			var bs = ChainGroups[chain].Blocks;
			for(int i = 0, l = bs.Count; i < l; i++ ){
				var b = bs[i];
				b.Dead = true;
				for(int x = b.X, y = b.Y + 1; y < Height; y++){
					var b2 = Model[x][y - 1] = Model[x][y];
					if( b2 == null ) break;
					b2.Y--; 
				}
			}
		}
		
		
		//指定した非破壊グループが、すでに登録されているか確認。
		public bool AlreadyExists( List<ModelBlock> blocks ){
			foreach( var g in SolidGroups[ blocks.Count - 2 ] )
				if( g.Blocks.SequenceEqual( blocks ) ) return true;
			
			
			//連鎖グループと同じ、ブロックを含んでいれば登録する必要なし。
			foreach( var g in ChainGroups ){
				/**/
				var bs = g.Blocks;
				var include = true;
				
				for(int i = 0, l = bs.Count; i < l; i++)
					if( blocks.IndexOf( bs[i] ) == -1 ){
						include = false; break;
					}
				
				if( include ) return true;
				
				/**/
				//if( g.Blocks.SequenceEqual( blocks ) ) return true;
			}
			
			//	
			
			return false;
		}
		
		
		//高速化用の木構造を作成
		public void MakeHeap(){
			RootSolidGroups = new List<BlockGroup>();
			for(int i = 0, il = SolidGroups.Length; i < il; i++ ){
				var gs = SolidGroups[i];
				for(int j = 0, jl = gs.Count; j < jl; j++ ){
					BlockGroup g = gs[j];
					for(int n = i - 1; n >= 0; n-- ){
						var gs2 = SolidGroups[n];
						for(int m = 0, ml = gs2.Count; m < ml; m++ ){
							BlockGroup g2 = gs2[m];
							if( g2.ScanAsChild( g ) ){ goto BREAK_N; }
						}
					}
					RootSolidGroups.Add(g);
					BREAK_N:;
				}
			}
			
			for( int i = 0, il = RootSolidGroups.Count; i < il; i++ )
				RootSolidGroups[i].SetupFamilyNum();
		}
		
		
		//連鎖の進行度計算
		public void CalcScore( Game game, AnalizeData result ){
			AnalizeCounter++;
			Changed = false;
				
			//空領域を確認
			foreach( ModelBlock b in MustBlocks )
				if( b.MustNumber == b.Number){
					if( b.End && b.Number != 0 ) 	AddBrokenBlock( result, b );
					else{
						result.Score += BingoScore * b.ScoreRate * 8;
						result.BingoCount++;
					}
				}else{
					if( b.Number != 0 ){
						if( b.Number > Sum )	AddBrokenBlock( result, b );
						else{
							result.Score 	+= GroundNGScore;
							b.GroundNG 		 = true;
						}
					}
					b.Number = b.MustNumber;
				}
			
			
			foreach( ModelBlock b in CeilBlocks )
				if( b.Number != 0 )
					if(b.Number > Sum)	result.Score += CeilScore * 4;
					else				result.Score += CeilScore;
			
			
			foreach( ModelBlock b in GroundBlocks )
				if( b.Number > Sum ){	
					result.Score += GroundScore * b.ScoreRate;
					result.Score += (Height - b.Y) * GroundHScore;
				}else if(b.Number != 0){
					result.Score += GroundNGScore;
					b.GroundNG = true;
				}else{
					b.Number = Sum + 1;
				}
			
			SegCount = 0;
			var first = true;
			int count = 0;
			
			do{
				Changed = false;
				for(int i = 0, l = ChainGroups.Count; i < l; i++){
					if( CheckChainGroup( result, ChainGroups[i], i, first )　)	return;
					if( first && !SegAdded && SegCount > 0 ){
						result.ChainSegs.Add( SegCount );
						SegCount = 0;
					}
				}
				
				for( int i = 0, il = RootSolidGroups.Count; i < il; i++ )
					if( CheckSolidGroup( result, RootSolidGroups[i], first, 0, 0 ) ) 	return;
				
				first = false;
			}while( Changed || count++ < 2 );
			
			
			//間違ったブロックの上に邪魔ブロックを置かない
			for( int i = 0; i < Width; i++ ){
				double c = 0;
				var row = Model[i];
				for( int j = Bottom; j < Height; j++ ){
					var b = row[j];
					if( c > 0 ){
						var n = game.Field[i][j];
						if( n > Sum ){
							result.Score += BrokenScore * c;
						}else{
							result.Score += GroundNGScore * c;
						}
					}
					if( b.GroundNG ) 	c += b.ScoreRate;
				}
			}
			
			
		}
		
		//連鎖の一部の数字チェック
		private bool CheckChainGroup( AnalizeData result, BlockGroup g, int num, bool first ){
			if( g.Broken || g.Success )	return false;
			
			List<ModelBlock> emptys = new List<ModelBlock>();
			var emptyCount 		= 0;
			int sum 			= 0;
			SegAdded = false;
			
			for( int j = g.Blocks.Count - 1; j >= 0; j-- ){
				var b = g.Blocks[j];
				if( b.Number > 0 ){
					int bn = b.Number;
					sum += bn;
					if( bn > Sum )
						if( AddBrokenBlock( result, b ) ) return true;
					if( sum + j + emptyCount > Sum ){
						if( AddBrokenGroup( result, g ) ) return true;
						return false;
					}
				}else{
					emptyCount++;
					emptys.Add( b );
				}
			}
			
			if( emptyCount == 0 ){
				if( sum == Sum ){
					g.Success = true;
					if( first ){
						int d = 4 + num;
						if( d > 40 ) d = 40;
						result.Score += BingoScore * g.Blocks[0].ScoreRate * 40 / d;
						
						result.BingoCount++;
						SegCount++;
						SegAdded = true;
					}
				}else{
					if( AddBrokenGroup( result, g ) ) return true;
				}
			}else{
				if( first ){
					int d = 4 + num;
					if( d > 40 ) d = 40;
					result.Score += (g.Blocks.Count - emptyCount) *  LizhiScore * g.Blocks[0].ScoreRate * 40 / d;
				}
			
				if( emptyCount == 1 ){
					emptys[0].Number 	= Sum - sum;
					g.Success 		= true;
				}else if( emptyCount == 2 && !first ){
					//片方の候補の消去を、もう片方に反映。
					int max = Sum - sum - 1;
					if(emptys[0].LifeNum < max || emptys[1].LifeNum < max){
						var e0 = emptys[0];
						var e1 = emptys[1];
						var d0 = e0.DeadList;
						var d1 = e1.DeadList;
						for( int i = 0; i < max; i++ ){
							if( d0[i] == true ) e1.KillAt( max - i );
							if( d1[i] == true ) e0.KillAt( max - i );
						}
						
						int c = e0.LifeNum;
						if( c == 0 ){
							if( AddBrokenBlock( result, e0 ) ) return true;
							if( AddBrokenBlock( result, e1 ) ) return true;
							g.Broken 	= true;
						}else if( c == 1 ){
							e0.Number = Array.IndexOf( e0.DeadList, false ) + 1;
							e1.Number = Array.IndexOf( e1.DeadList, false ) + 1;
							g.Success 	= true;
						}
					}
				}
			}
			
			return false;
		}
		
		private bool AddBrokenBlock( AnalizeData result, ModelBlock b ){
			if( b.Broken ) 	return false;
			b.GroundNG = b.Broken 	 = true;
			return AddBroken(result);
		}
		
		private bool AddBrokenGroup( AnalizeData result, BlockGroup g ){
			if( g.Broken ) return false;
			g.Broken = true;
			foreach( var b in g.Blocks ) b.GroundNG = true;
			
			return AddBroken(result);
		}
		
		private bool AddBroken( AnalizeData result, int rate = 1 ){
			result.Score += BrokenScore * 1;
			result.BrokenCount++;
			if( result.BrokenCount > BrokenLimit ) return true;
			return false;
		}
		
		
		//
		//Trueを返した場合、中断して十分に低いスコアを返す
		private bool CheckSolidGroup( AnalizeData result, BlockGroup g, bool first, int pos, int sum, ModelBlock empty = null, int emptyCount = 0 ){
			if( g.Counter != AnalizeCounter ){
				g.Reset();
				g.Counter = AnalizeCounter;
			}else if( g.Broken || g.Success )	return false;
			var blocks = g.Blocks;
			
			for( int l = blocks.Count, max = l + 1; pos < l; pos++ ){
				var b = blocks[pos];
				if( b.Number > 0 )				sum += b.Number;
				else if( emptyCount++ == 0 )	empty = b;
			}
			
			if( sum > Sum - emptyCount ){
				g.Success = true;
				//if( first ) result.Score += SolidScore * g.FamilyNum;
				return false;
			}else{
				if( emptyCount == 0 ){
					if( sum == Sum ){
						if( AddBrokenGroup( result, g ) ) 	return true;
						return false;
					}else if( sum > Sum ){
						g.Success = true;
						return false;
						//if( first ) result.Score += SolidScore * g.FamilyNum;
					}
				}else if( emptyCount == 1 ){
					int bc 	= empty.LifeNum;
					int c 	= empty.KillAt( Sum - sum );
					if( bc != c ){
						if( c == 0 ){
							if( AddBrokenBlock( result, empty ) ) return true;
							return false;
						}else{
							Changed = true;
							if( c == 1 ){
								empty.Number = Array.IndexOf( empty.DeadList, false ) + 1;
								
								sum 	+= empty.Number;
								empty 	= null;
								emptyCount--;
								
								if( sum > Sum ){
									g.Success = true;
									return false;
								}
							}
						}
					}
				}
			}
			
			var children = g.Children;
			if( children != null )
				for( int i = 0, l = children.Count; i < l; i++ )
					if( CheckSolidGroup( result, children[i], first, pos, sum, empty, emptyCount ) ) return true;
			
			return false;
		}
		
		/*
		private void SimulateChain(Game game, AnalizeData result, bool outon){
			Output = "";
			
			//ChainBlocksの位置にあるブロックを後ろから消していき、
			//発生した連鎖数を数える。
			for(int i = ChainLimit - 1; i >= 0; ){
				if( outon ) { Output += ModelOutput(Model,game); }
				i = DeleteChain( game, result, i );
			}
			
			//result.Score += result.Chain 		* ChainScore;
		}
		
		
		
		//ゲーム状態の変化に連鎖モデルの変化を同期させる。
		private int DeleteChain( Game game, AnalizeData result, int headNum ){
			List<ModelBlock> delete = ChainGroups[headNum].Blocks;
			var Field 			= game.Field;
			var HeightList 		= game.HeightList;
			
			//消去位置をマーク
			int count = 0, l = delete.Count, sum = 0;
			for(int i = 0; i < l; i++){
				ModelBlock b = delete[i];
				if( b == null || b.Dead ){ continue; }
				var val = Field[b.X][b.Y];
				if( val == 0 ) continue;
				
				sum += val;
				Field[b.X][b.Y] 	|= 0x800000;	
				game.Marked[b.X] 	= true;
			}
			
			if( sum != Game.Sum ){
				for(int i = 0; i < l; i++){
					ModelBlock b = delete[i];
					sum += Field[b.X][b.Y];
				}
			}
			
			bool 	perfect = true;
			int 	chain 	= 0;
			int 	num 	= headNum;
			int		next 	= headNum - 1;
			
			Output += "start:" + headNum + "\n";
			
			game.ChangedLeft 	= 0;
			game.ChangedRight	= Game.Width;
			game.ChangedBottom	= 0;
			game.ChangedTop 	= Game.Height + Game.Size;
			
			//連鎖を見る
			do{
				Output += "count:" + count + "\n";
			
				chain++;
				int hitNum	= 0;
				
				//目印を付けた部分を削除
				for(int i = 0, il = Width; i < il; i++){
					if(! game.Marked[i] ){ 
						game.Marked[i] = false;
						continue;
					}
					
					var row 	= Field[i];
					var mr		= Model[i];
					int h 		= HeightList[i];
					int k 		= 0;
					
					for(int j = 0; j < h; j++ ){
						int	val = row[j];
						if( j != k ){
							row[k] = val;
							var b = mr[k] = mr[j];
							if( b != null ){ b.Y = k; }
						}
						if( (val & 0x800000) == 0 ) k++;
						else{
							var b = mr[j];
							if( b != null ){
								b.Dead = true;
								if( b.Chain == num ){ hitNum++; }
							}
						}
					}
					
					for(; k < Height; k++){
						row[k] 	= 0; mr[k] 	= null;
					}
				}
				
				Output += "num:" + num + "\n";
				
				if( perfect && num != headNum && num >= 0 ){
					if( hitNum != ChainGroups[num].Blocks.Count ){
						perfect = false;
						next = num - 1;
						if( headNum - num > 1 ){
							result.ChainSegs.Add( headNum - num - 1 );
						}
					}
				}
				
				num--;
				
				//新しい消去フラグを探す。
				count = game.Mark();
			}while( count > 0 );
			
			if( perfect ){
				next = num;
				if( headNum - num > 1 )	result.ChainSegs.Add( headNum - num - 1 );
				if( num < 0 ) 			result.Chain = headNum;
			}
			
			return next;
		}
		*/
		
		//
		public string ModelOutput( ModelBlock[][] model, Game game ){
			string result = "";
			for(int i = Height - 1; i >= 0; i--){
				for(int j = 0; j < Width; j++){
					var b = game.Field[j][i];
					result +=( " " + b.ToString( "D2" ) );
				}
				result +=( "\n" );
			}
			result +=( "\n" );
			
			for(int i = Height - 1; i >= 0; i--){
				for(int j = 0; j < Width; j++){
					var b = model[j][i];
					if( b == null )					result +=( " Nul" );
					else if( b.Chain >= 0 )			result +=( " C" + b.Chain.ToString( "D2" ) );
					else if( b.MustNumber >= 0 )	result +=( " M" + b.MustNumber.ToString( "D2" ) );
					else if( b.Ground != int.MaxValue )	
													result +=( " G" + b.Ground.ToString( "D2" ) );
					else						result +=( " Xxx" );
				}
				result +=( "\n" );
			}
			return result + "\n";
		}
	}
}