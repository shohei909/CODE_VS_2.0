/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/12
 * 時刻: 1FixedWidth	= 11,:41
 * 
 */
using System;
using System.Collections.Generic;

namespace GameCore.Model{	
	public class ModelBlock:IComparable{
		public int 		ID;
		public int		Number;
		public bool[] 	DeadList;
		public int		LifeNum;
		public double	ScoreRate 	= 1;
		public int		MustNumber 	= -1;
		public bool		End;
		public bool		Broken;
		public bool		GroundNG;
		
		public int 		StartX;
		public int 		StartY;
		public int 		X;
		public int 		Y;
		public bool 	Dead;
		
		//連鎖のどの位置に当たるか？連鎖の一部でない場合は-1。
		public int 	Chain;
		//連鎖のどの段階まで足場として必要か。足場として必要でない場合は-1。
		public int	Ground;
		
		public ModelBlock( int id, int x, int y, int chain = -1, int ground = int.MaxValue, bool end = false, double scoreRate = 1, int mustNumber = -1 ){
			ID = id; StartX = X = x; StartY = Y = y; 
			Chain = chain; Ground = ground; MustNumber = mustNumber; ScoreRate = scoreRate; End = end;
			if( chain >= 0 && ground > chain ) Ground = chain;
		}
		
		public void Reset( int sum, int[][] field ){
			Dead 		= false;
			Broken		= false;
			GroundNG	= false;
			X = StartX; Y = StartY;
			Number = field[X][Y];
			
			if( (Number == 0 && MustNumber < 0) || MustNumber == 0 )	DeadList 	= new bool[ LifeNum = sum - 1 ];
			else{
				DeadList	= null;
				LifeNum		= sum - 1;
			}
		}

		public int KillAt( int num ){
			if( DeadList != null && DeadList[--num] == false ){
				DeadList[num] = true;
				return --LifeNum;
			}
			return LifeNum;
		}
		
		public int CompareTo( object o1 ){
			return (ID > (o1 as ModelBlock).ID) ? -1 : 1;
		}
	}
	
	public class BlockGroup{
		public List<ModelBlock> 	Blocks;
		public List<BlockGroup> 	Children;
		
		//このグループの子孫の数 + 1;
		public int FamilyNum = 1;
		
		public bool Broken 		= false;
		public bool Success 	= false;
		public long	Counter 	= 0;
		
		public BlockGroup( List<ModelBlock> blocks ){ Blocks = blocks; }
		
		//targetがchildになりうるか調べて、
		//childになれるなら追加してtrue。
		//ならないなら何もせずfalse。
		public bool ScanAsChild( BlockGroup target ){
			var bs = target.Blocks;
			
			for(int i = 0, l = Blocks.Count; i < l; i++)
				if( bs.IndexOf( Blocks[i] ) == -1 ) return false;
			
			for(int i = 0, l = Blocks.Count; i < l; i++){
				var index 	= bs.IndexOf( Blocks[i] );
				var b 		= bs[index];
				bs[ index ] = bs[ i ];
				bs[ i ] 	= b;
			}
			
			if( Children == null ) Children = new List<BlockGroup>();
			Children.Add( target );
			return true;
		}
		
		public int SetupFamilyNum(){
			FamilyNum = 1;
			if( Children != null )
				for( int i = 0, l = Children.Count; i < l; i++ )
					FamilyNum += Children[i].SetupFamilyNum();
			
			return FamilyNum;
		}
		
		public void Reset(){
			Broken 	= false;
			Success	= false;
		}
	}
		
	public class AnalizeData{
		public double Score;
		public List<int> ChainSegs = new List<int>();
		public int Chain;
		public int MaxChain 	= int.MaxValue;
		public int BrokenCount 	= 0;
		public int BingoCount 	= 0;
		
		//public AnalizeData( int max ){ MaxChain = max; }
	}
	
	public class Pattern{
		public int			NextX;
		public int			NextY;
		public int			NextDir;
		public int			Width;
		public int			Height;
		public int			Chain;
		public object[,] 	Data;
		public double 		ScoreRate;
			
		public Pattern( object[,] data, int chain, int nextX, int nextY = 0, int nextDir = 1, double scoreRate = 1 ){
			Data = data; Chain = chain; NextX = nextX; NextY = nextY; NextDir = nextDir; ScoreRate = scoreRate;
			
			Height 	= Data.GetLength( 0 );
			Width 	= Data.GetLength( 1 );
		}
	}
}
