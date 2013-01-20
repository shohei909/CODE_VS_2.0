/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/11/22
 * 時刻: 12:32
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace GameCore{
	
	public class Block{
		
		public int[,]			Data;
		public List<int>[][]	Projections;
		///右側の余白の大きさ。0-。
		public int[]			Right;
		///左側の余白の大きさ。0-。
		public int[]			Left;
		public int				Size;
		
		
		public void Init( int[,] data, int size ){
			this.Size = size;
			Data = data;
		}
		
		public void Read( TextReader input, int size ){
			this.Size = size;
			Data = new int[size,size];
			
			for(int y = size - 1; y >= 0; y--){
				string[] line = input.ReadLine().Split( ' ' );
				for(int x = 0; x < size; x++ ){
					Data[x,y] = int.Parse( line[x] );
				}
			}
			
			Setup();
			input.ReadLine();
		}
		
		private void Setup(){
			var last = Size - 1;
			
			//0°,90°,180°,270°の回転行列
			var rot = new int[4,2,3]{
				{
					{1,0,0},
					{0,1,0}
				},{
					{0,-1,last},
					{1,0,0}
				},{
					{-1,0,last},
					{0,-1,last}
				},{
					{0,1,0},
					{-1,0,last}
				},
			};
			
			Projections = new List<int>[Game.ROTATE_NUM][];
			Left		= new int[Game.ROTATE_NUM];
			Right		= new int[Game.ROTATE_NUM];
			
			for(int i = 0; i < Game.ROTATE_NUM; i++){
				Projections[i] = new List<int>[Size];
				for(int j = 0; j < Size; j++){
					Projections[i][j] = new List<int>();
					for(int k = 0; k < Size; k++ ){
						int x = rot[i,0,0] * j + rot[i,0,1] * k + rot[i,0,2];
						int y = rot[i,1,0] * j + rot[i,1,1] * k + rot[i,1,2];
						int d = Data[x,y];
						if( d != 0 ) Projections[i][j].Add( d );
					}
				}
				int ri, lf = ri = Size - 1;
				for(int j = 0; j < Size; j++){
					if( Projections[i][j].Count > 0 ){ lf = Left[i] = j; break; }
				}
				for(int j = Size - 1; 0 <= j; j--){
					if( Projections[i][j].Count > 0 ){ ri = Right[i] = Size - 1 - j; break; }
				}
			}
		}
		
	}
}
