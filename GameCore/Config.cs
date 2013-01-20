/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/12
 * 時刻: 20:45
 * 
 */
using System;
using System.Collections.Generic;
using GameCore.AI;
using GameCore.Model;

namespace GameCore{
	
	/// <summary>
	/// Description of Config.
	/// </summary>
	public class Config{
		public static List<AISetting> SmallAIList = new List<AISetting>(){
			new AISetting(){
				Depth 		= 25,
				Width 		= 40,
				CheckLimit	= 20,
				FloorHeight = -1,
				FireRate	= 0.2,
				FutureMin	= 12000,
				FixedDepth	= 4,
				FixedWidth	= 50,
				FixedLimit 	= 40,
				Factory		= ChainConfig.SmallBombChainEL,
				ChainLimit	= 40
			},
			new AISetting(){
				Depth 		= 40,
				Width 		= 150,
				CheckLimit	= 20,
				FloorHeight = -1,
				FireRate	= 0.2,
				FutureMin	= 12000,
				FixedDepth	= 5,
				FixedWidth	= 80,
				FixedLimit 	= 40,
				Factory		= ChainConfig.SmallBombChainEL,
				ChainLimit	= 40
			},
			new AISetting(){
				Depth 		= 40,
				Width 		= 150,
				CheckLimit	= 20,
				FloorHeight = -1,
				FireRate	= 0.2,
				FutureMin	= 12000,
				FixedDepth	= 6,
				FixedWidth	= 100,
				FixedLimit 	= 40,
				Factory		= ChainConfig.SmallBombChainEL,
				ChainLimit	= 40
			},
			new AISetting(){
				Depth 		= 40,
				Width 		= 150,
				CheckLimit	= 20,
				FloorHeight = -1,
				FireRate	= 0.2,
				FutureMin	= 12000,
				FixedDepth	= 7,
				FixedWidth	= 120,
				FixedLimit 	= 40,
				Factory		= ChainConfig.SmallBombChainEL,
				ChainLimit	= 40
			},
			new AISetting(){
				Depth 		= 40,
				Width 		= 150,
				CheckLimit	= 20,
				FloorHeight = -1,
				FireRate	= 0.2,
				FutureMin	= 12000,
				FixedDepth	= 8,
				FixedWidth	= 60,
				FixedLimit 	= 40,
				Factory		= ChainConfig.SmallBombChainEL,
				ChainLimit	= 40
			},
			new AISetting(){
				Depth 		= 40,
				Width 		= 150,
				CheckLimit	= 20,
				FloorHeight = -1,
				FireRate	= 0.2,
				FutureMin	= 12000,
				FixedDepth	= 4,
				FixedWidth	= 150,
				FixedLimit 	= 40,
				Factory		= ChainConfig.SmallBombChainEL,
				ChainLimit	= 40
			},
			new AISetting(){
				Depth 		= 40,
				Width 		= 150,
				CheckLimit	= 20,
				FloorHeight = -1,
				FireRate	= 0.2,
				FutureMin	= 12000,
				FixedDepth	= 9,
				FixedWidth	= 50,
				FixedLimit 	= 40,
				Factory		= ChainConfig.SmallBombChainEL,
				ChainLimit	= 40
			},
		};
		
		
		
		public static List<AISetting> MediumAIList = new List<AISetting>(){
			new AISetting(){
				Depth 		= 30,
				Width 		= 72, 
				CheckLimit 	= 12,
				FloorHeight = -1,
				FireRate	= 0.9,
				FutureMin	= 2000,
				FixedDepth	= 6,
				FixedWidth	= 70,
				FixedLimit	= 50,
				Factory		= ChainConfig.MediumBombChain,
				ChainLimit	= 71
			},
			new AISetting(){
				Depth 		= 30,
				Width 		= 72, 
				CheckLimit 	= 12,
				FloorHeight = -1,
				FireRate	= 0.95,
				FutureMin	= 2000,
				FixedDepth	= 8,
				FixedWidth	= 40,
				FixedLimit	= 50,
				Factory		= ChainConfig.MediumBombChain,
				ChainLimit	= 71
			},
			new AISetting(){
				Depth 		= 30,
				Width 		= 72, 
				CheckLimit 	= 12,
				FloorHeight = -1,
				FireRate	= 0.95,
				FutureMin	= 2000,
				FixedDepth	= 8,
				FixedWidth	= 200,
				FixedLimit	= 50,
				Factory		= ChainConfig.MediumBombChain,
				ChainLimit	= 71
			},
			new AISetting(){
				Depth 		= 30,
				Width 		= 72, 
				CheckLimit 	= 12,
				FloorHeight = -1,
				FireRate	= 0.9,
				FutureMin	= 2000,
				FixedDepth	= 4,
				FixedWidth	= 60,
				FixedLimit	= 50,
				Factory		= ChainConfig.MediumBombChain,
				ChainLimit	= 70
			},
			new AISetting(){
				Depth 		= 30,
				Width 		= 72, 
				CheckLimit 	= 12,
				FloorHeight = -1,
				FireRate	= 0.9,
				FutureMin	= 2000,
				FixedDepth	= 5,
				FixedWidth	= 60,
				FixedLimit	= 50,
				Factory		= ChainConfig.MediumBombChain,
				ChainLimit	= 70
			},
			new AISetting(){
				Depth 		= 30,
				Width 		= 72, 
				CheckLimit 	= 12,
				FloorHeight = -1,
				FireRate	= 0.9,
				FutureMin	= 2000,
				FixedDepth	= 4,
				FixedWidth	= 120,
				FixedLimit	= 50,
				Factory		= ChainConfig.MediumBombChain,
				ChainLimit	= 70
			},
		};
		
		public static List<AISetting> LargeAIList = new List<AISetting>(){
			new AISetting(){
				Depth 		= 12,
				Width 		= 25, 
				CheckLimit 	= 6,
				FloorHeight = -1,
				FireRate	= 0.95,
				FutureMin	= 30,
				FixedDepth	= 5,
				FixedWidth	= 50,
				Factory		= ChainConfig.LargeBombChain,
				FixedLimit 	= 1000,
				ModelSetting = ModelSetting.LargeSetting,
				ChainLimit	= 16
			},
		};
	}
}