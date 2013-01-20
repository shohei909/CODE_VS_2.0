/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/11/28
 * 時刻: 20:56
 * 
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GameCore.AI{
	
	/// <summary>
	/// 複数のAIを並列させて実行して、成績の良かった手を採用していく。
	/// </summary>
	public class CombinedAI:GameAI{
		public ChainAI[] 	AIList;
		public int[][]			Log;
		public Game[]			GameList;
		public int Solved;
		public int Step;
		public int Winner 	= 0;
		
		public CombinedAI( Game target, List<AISetting> settingList ):base( target ){
			int l 		= settingList.Count;
			Log			= new int[Game.DefaultBlocks.Length][];
			AIList 		= new ChainAI[l];
			GameList	= new Game[l];
			
			for( int i = 0; i < l; i++ ){
				GameList[i]	= target.Clone();
				AIList[i] 	= new ChainAI( GameList[i], settingList[i] );
			}
		}
		
		public override void Next(){
			if( Step == 0 ){
				var Data 		= new double[AIList.Length][];
				var max 		= double.MinValue;
				int l 			= Game.DefaultBlocks.Length;
				
				var lockObj = new object();
				Parallel.For( 0, AIList.Length, i => {
				
					Stopwatch sw;
					Game g; GameAI AI; int[][] aiLog;
					
			    	lock( lockObj ){
						sw = new Stopwatch();
						sw.Start();
						AI 		= AIList[i];
				    	g 		= GameList[i];
				    	aiLog 	= new int[l][];
					}
					
					for(int j = 0; j < l; j++ ){
						AI.Next();
						aiLog[j] = new int[2]{ AI.LastPos, AI.LastRot };
					}
					
			    	lock( lockObj ){
			    		if( max < g.Score ){
							max 	= g.Score;
							Log 	= aiLog;
							Winner	= i;
						}
				    	
				    	OutputData( i, sw );
					}
				});
		    }
			int[] ans = Log[ Step ];
			Drop( ans[0], ans[1] );
			Step++;
		}
		
		
		public void OutputData( int i, Stopwatch sw ){
			int c = -1;
			
			while( File.Exists("output"+ (++c) +".txt") ){}
			
			StreamWriter writer = new StreamWriter( "output"+ c +".txt" );
			
			writer.WriteLine( DateTime.Now + "ms" );
			writer.WriteLine( sw.ElapsedMilliseconds + "ms" );
			writer.WriteLine( "" );
			
			var g 	= GameList[i];
			var ai 	= AIList[i];
			
			writer.WriteLine( "AI" 		+ i + " ==================" );
			writer.WriteLine( "Score:" 	+ g.Score );
			writer.WriteLine( "Chain:" 	+ g.MaxChain );
			writer.WriteLine( "" );
			writer.WriteLine( "Floor:" 		+ ai.Setting.FloorHeight );
			writer.WriteLine( "FixD :" 		+ ai.Setting.FixedDepth );
			writer.WriteLine( "FixW :" 		+ ai.Setting.FixedWidth );
			writer.WriteLine( "" );
			writer.Close();
		}
		
	}
}