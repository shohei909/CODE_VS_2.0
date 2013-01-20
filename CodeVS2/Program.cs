/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/19
 * 時刻: 2:02
 * 
 */
using System;
using System.IO;
using GameCore;
using GameCore.AI;

namespace CodeVS2 {
	class Program {
		public static void Main(string[] args){
			//動作確認用
			//Game.Init( new StreamReader( "../../../../sample2012/sample_input.txt" ) );
			
			Game.Init( Console.In );
			Game game = new Game();
			
			//Sサイズ用AI
			//GameAI AI = new CombinedAI( game, Config.SmallAIList );
			
			//Mサイズ用AI
			//GameAI AI = new CombinedAI( game, Config.MediumAIList );
			
			//Lサイズ用AI
			GameAI AI = new AdvancedAI( game, Config.LargeAIList[0] );
			
			
			while( !game.GameOver && game.CurrentStep < Game.StepNum ){
				AI.Next();
				Console.Write( AI.LastPos + " " + AI.LastRot + "\n" );
			}
		}
	}
}