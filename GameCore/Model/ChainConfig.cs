/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/14
 * 時刻: 12:52
 * 
 */
using System;
namespace GameCore.Model{
	/// <summary>
	/// Description of ChainConfigcs.
	/// </summary>
	public class ChainConfig {
		
		//なんでもOKマス
		public static object F = "Free";
		//空白マス
		public static object E = "Empty";
		
		
		//連鎖尾爆弾
		public static int[] Mx0 	= {-1, 0, -1};
		public static int[] Mx8 	= {-1, 8, -1};
		public static int[] Mx2 	= {-1, 2, -1};
		public static int[] Mx1 	= {-1, 1, -1};
		public static int[] Mx19 	= {-1,19, -1};
		public static int[] Mx21 	= {-1,21, -1};
		public static int[] Mx9 	= {-1, 9, -1};
		
		
		
		public static int[] G   = {-1, -1, 500};
		public static int[] M01 = {0, 1, 0};
		public static int[] M03 = {0, 3, 0};
		public static int[] M07 = {0, 7, 0};
		public static int[] M09 = {0, 9, 0};
		public static int[] M019 = {0, 19, 0};
		
		public static int[] M1_1 	= {1, 1, 1};
		public static int[] M1_19 	= {1, 19, 1};
		
		public static int[] M18 = {1, 8, 1};
		public static int[] M12 = {1, 2, 1};
		
		
		//ズラース法=============================================================
		
		//ズラース。
		public static Pattern SimpleBody　= new Pattern( new object[3,2]{
			{F,0},
			{F,F},
			{0,F},
		}, 1, 1, 0, 1, 1 );
		
		//ズラース。
		public static Pattern SimpleBodyX　= new Pattern( new object[4,3]{
			{F,F,1},
			{F,0,F},
			{F,1,F},
			{0,F,F},
		}, 2, 2, 0, 1, 1 );
		
		//階段連鎖
		public static Pattern AdvancedBody　= new Pattern( new object[5,3]{
			{F,F,0},
			{F,F,F},
			{F,0,F},
			{F,F,F},
			{0,F,F},
		}, 1, 1, 0, 1, 1 );
		
		
		//N字連鎖
		public static Pattern AdvancedBodyN　= new Pattern( new object[5,3]{
			{F,F,1},
			{0,F,F},
			{1,F,F},
			{0,1,F},
			{0,F,F},
		}, 2, 2, 0, 1, 1 );
		
		
		//階段連鎖
		public static Pattern AdvancedBodyT　= new Pattern( new object[9,4]{
			{F,F,0,F},
			{F,F,F,F},
			{F,F,F,F},
			{F,F,1,F},
			{F,0,F,F},
			{F,1,F,F},
			{F,F,F,F},
			{1,F,F,F},
			{0,F,F,F},
		}, 1, 1, 0, 1, 1 );
		
		//2連鎖使って折り返し。
		public static Pattern SimpleJoint0 = new Pattern( new object[6,2]{
			{F,F},
			{1,F},
			{F,0},
			{F,1},
			{0,F},
			{F,F},
		}, 2, 0, 3, -1, 5 );
		
		//3連鎖使って折り返し。
		public static Pattern SimpleJoint1 = new Pattern( new object[6,2]{
			{2,0},
			{F,1},
			{F,2},
			{F,1},
			{0,F},
			{F,F},
		}, 3, 0, 3, -1, 2 );
		
		//4連鎖使って折り返し。
		public static Pattern SimpleJoint2 = new Pattern( new object[6,2]{
			{F,0},
			{3,1},
			{F,2},
			{F,3},
			{F,2},
			{0,1},
		}, 4, 0, 3, -1, 2 );
		
		//3連鎖使って折り返し。
		public static Pattern SimpleJoint3 = new Pattern( new object[5,2]{
			{2,0},
			{F,1},
			{F,2},
			{F,1},
			{0,F},
		}, 3, 0, 3, -1, 4 );
		
		
		//2連鎖使って折り返し。
		public static Pattern AdvancedJoint = new Pattern( new object[10,4]{
			{4,F,F,F},
			{F,3,F,F},
			{F,4,F,F},
			{F,F,3,1},
			{F,F,4,2},
			{F,F,F,3},
			{F,F,0,2},
			{F,0,F,2},
			{F,F,1,F},
			{0,1,F,F},
		}, 5, 1, 6, -1, 18 );
		
		
		
		//発火口。スペースを確保
		public static Pattern SimpleHead = new Pattern( new object[4,3]{
			{F,F,Mx0},
			{F,E,F  },
			{0,F,F  },
			{F,F,F  },
		}, 1, 1 );
		
		//発火口。スペースを確保
		public static Pattern AdvancedHead = new Pattern( new object[4,5]{
			{F,F,0,F,Mx0 },
			{F,0,F,E,F },
			{F,F,1,F,F },
			{0,1,F,F,F },
		}, 2, 1 );
		
		
		//N字用発火口。
		public static Pattern AdvancedHeadN = new Pattern( new object[4,2]{
			{0,F},
			{1,E},
			{0,F},
			{0,F},
		}, 2, 1 );
		
		//Sサイズ、縦型爆弾
		public static Pattern SmallBomb = new Pattern( new object[15,3]{
      		{ Mx1 , G  , F   },
			{ Mx1 , G  , F   },
			{ Mx1 , G  , F   },
			{ Mx1 , G  , F   },
			{ Mx1 , G  , F   },
			{ Mx1 , M01, F   },
			{ Mx2 , F  , F   },
			{ M09 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , 1   },
			{ Mx1 , F  , F   },
			{ Mx1 , 1  , F   }
		}, 2, 2, 0, 1, 7 );
		
		//Sサイズ、縦型爆弾4
		public static Pattern SmallBombB = new Pattern( new object[16,3]{
      		{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , M01, F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx2 , F  , F   },
			{ M09 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , 1  , F   },
			{ Mx1 , 2  , 3   },
			{ Mx1 , 1  , F   },
			{ 2   , 3  , F   }
		}, 4, 2, 0, 1, 5 );
		
		
		//ズラース法 + 爆弾。 Long型。
		public static ChainFactory SmallBombChainEL　= new ChainFactory(
			SmallBomb,
			SimpleBody,
			SimpleJoint2,
			SimpleHead,
			2
		);
		
		//ズラース法 + 爆弾。 Long型。
		public static ChainFactory SmallBombChainFL　= new ChainFactory(
			SmallBombB,
			SimpleBody,
			SimpleJoint2,
			SimpleHead,
			2
		);
		
		
		//Mサイズ、連鎖尾爆弾
		public static Pattern MediumBomb = new Pattern( new object[23,3]{
      		{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , 0  , F   },
			{ Mx2 , F  , F   },
			{ 0	  , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx2 , F  , 1   },
			{ Mx2 , F  , F   },
			{ Mx2 , 1  , F   }
		}, 2, 2, 0, 1, 11 );
		
		
		//Mサイズ、連鎖尾爆弾
		public static Pattern MediumBombB = new Pattern( new object[23,4]{
      		{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx1 , Mx1, F  , F   },
			{ Mx1 , Mx1, F  , F   },
			{ Mx1 , Mx1, F  , F   },
			{ Mx1 , Mx1, M01, F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx19, M019,F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx2 , Mx2, F  , F   },
			{ Mx1 , Mx1, F  , F   },
			{ Mx1 , Mx1, F  , 1   },
			{ Mx1 , Mx1, F  , F   },
			{ Mx1 , Mx1, 1  , F   }
		}, 2, 3, 0, 1, 8 );
		
		/*
		//Mサイズ、連鎖尾爆弾C
		public static Pattern MediumBombC = new Pattern( new object[23,5]{
      		{ Mx2 , F   , Mx2,  F  , F   },
			{ Mx2 , F   , Mx2,  F  , F   },
			{ Mx2 , F   , Mx2,  F  , F   },
			{ Mx2 , F   , Mx2,  F  , F   },
			{ Mx2 , F   , Mx2,  F  , F   },
			{ Mx2 , F   , Mx2,  F  , F   },
			{ Mx1 , F   , Mx1,  F  , F   },
			{ Mx1 , F   , Mx1,  F  , F   },
			{ Mx1 , F   , Mx1,  F  , F   },
			{ Mx1 , Mx1 , Mx1,  M01, F   },
			{ Mx2 , Mx21, Mx2,  F  , F   },
			{ Mx19, Mx19, M019, F  , F   },
			{ Mx2 , Mx21, Mx2,  F  , F   },
			{ Mx2 , Mx21, Mx2,  F  , F   },
			{ Mx2 , Mx21, Mx2,  F  , F   },
			{ Mx2 , Mx21, Mx2,  F  , F   },
			{ Mx2 , Mx21, Mx2,  F  , F   },
			{ Mx2 , Mx21, Mx2,  F  , F   },
			{ Mx2 , Mx21, Mx2,  F  , F   },
			{ Mx1 , Mx21, Mx1,  F  , F   },
			{ Mx1 , Mx21, Mx1,  F  , 1   },
			{ Mx1 , Mx21, Mx1,  F  , F   },
			{ Mx1 , Mx21, Mx1,  1  , F   }
		}, 2, 4, 0, 1, 10 );
		*/
		
		//中
		//ズラース法 + 爆弾。
		public static ChainFactory MediumBombChain　= new ChainFactory(
			MediumBomb,
			SimpleBody,
			SimpleJoint0,
			SimpleHead,
			2
		);
		
		//中
		//ズラース法 + 爆弾。
		public static ChainFactory MediumBombChainB　= new ChainFactory(
			MediumBombB,
			SimpleBody,
			SimpleJoint0,
			SimpleHead,
			3
		);
		
		/*
		//中
		//ズラース法 + 爆弾。
		public static ChainFactory MediumBombChainC　= new ChainFactory(
			MediumBombC,
			SimpleBody,
			SimpleJoint0,
			SimpleHead,
			4
		);*/

		
		
		//Mサイズ、連鎖尾爆弾
		public static Pattern LargeBomb = new Pattern( new object[36,3]{
		                                              	
      		{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , 0  , F   },
			{ Mx2 , F  , F   },
			{ 0   , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx2 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , F   },
			{ Mx1 , F  , 1   },
			{ Mx1 , F  , F   },
			{ Mx2 , 1  , F   },
			
		}, 2, 2, 0, 1, 4 );
		
		
		//大
		//ズラース法 + 爆弾。
		public static ChainFactory LargeBombChain　= new ChainFactory(
			AdvancedBodyN,
			AdvancedBodyN,
			AdvancedJoint,
			AdvancedHeadN
		);
		
//===================================================================================================================================
//ボツ案==============================================================================================================================
//===================================================================================================================================


		//連鎖尾1、挟み込みでカサまし。
		public static Pattern SimpleTail0 = new Pattern( new object[4,2]{
			{F,1},
			{0,F},
			{1,F},
			{0,F},
		}, 2, 1, 0, 1, 2 );
		
		//連鎖尾2、さらにカサまし。
		public static Pattern SimpleTail1 = new Pattern( new object[5,2]{
			{0,F},
			{F,2},
			{1,F},
			{2,0},
			{1,F},
		}, 3, 1, 0, 1, 3 );
		
		//連鎖尾3、Joint2用、1連鎖追加。
		public static Pattern SimpleTail3 = new Pattern( new object[5,2]{
			{F,1},
			{F,F},
			{0,F},
			{1,F},
			{0,F},
		}, 2, 1, 0, 1, 3 );
		
		
		//ズラース法 (2 + 10 * 4)
		public static ChainFactory SimpleChain0 = new ChainFactory(
			SimpleTail0,
			SimpleBody,
			SimpleJoint1,
			SimpleHead
		);
		
		//ズラース法 ver2 (3 + 10 * 4) 
		public static ChainFactory SimpleChain1 = new ChainFactory(
			SimpleTail1,
			SimpleBody,
			SimpleJoint1,
			SimpleHead
		);
		
		
		//ズラース法 ver3 (1 + 11 * 4)
		public static ChainFactory SimpleChain2 = new ChainFactory(
			SimpleBody,
			SimpleBody,
			SimpleJoint2,
			SimpleHead
		);
		
		//ズラース法 ver4 (2 + 11 * 4) 
		public static ChainFactory SimpleChain3 = new ChainFactory(
			SimpleTail3,
			SimpleBody,
			SimpleJoint2,
			SimpleHead
		);
		
		//ズラース法 Full ver5
		public static ChainFactory SimpleChain4 = new ChainFactory(
			SimpleBody,
			SimpleBody,
			SimpleJoint0,
			SimpleHead
		);
		
		/*
		//壁際を使った発火口。
		public static Pattern SimpleHeadJoint = new Pattern( new object[5,3]{
			{E,3,1},
			{E,4,2},
			{F,0,3},
			{F,F,2},
			{0,1,F},
		}, 5, 1, 0, 0, 6 );
		
		//ズラース法 Full ver
		public static ChainFactory SimpleChainFull0 = new ChainFactory(
			SimpleTail1,
			SimpleBody,
			SimpleJoint1,
			SimpleHeadJoint
		);
		
		//ズラース法 Full ver2
		public static ChainFactory SimpleChainFull1 = new ChainFactory(
			SimpleBody,
			SimpleBody,
			SimpleJoint2,
			SimpleHeadJoint
		);
		
		//ズラース法 Full ver3
		public static ChainFactory SimpleChainFull2 = new ChainFactory(
			SimpleTail3,
			SimpleBody,
			SimpleJoint2,
			SimpleHeadJoint
		);
		*/
		/*
		// +2
		public static Pattern SimpleBombS2 = new Pattern( new object[8,10]{
			{F  , F  , F  , F  , M1 , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , 0  , 3  , F  , F  , F  , F  },
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{F  , F  , F  , 1  , 2  , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , 3  , F  , F  , F  , F  , F  },
			{F  , F  , F  , 2  , 1  , F  , F  , F  , F  , F  },
			{M2 , M1 , M2 , M2 , 0  , M2 , M2 , M1 , M2 , M2 }
		}, 4, 5, 1,	1, 3 );
		
		// +3
		public static Pattern SimpleBombS3 = new Pattern( new object[8,10]{
			{F  , F  , F  , F  , M1 , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{F  , F  , F  , 1  , 0  , 4  , F  , F  , F  , F  },
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{F  , F  , F  , 2  , 3  , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , 4  , F  , F  , F  , F  , F  },
			{F  , F  , 2  , 3  , 1  , F  , F  , F  , F  , F  },
			{M2 , M1 , M2 , M2 , 0  , M2 , M2 , M1 , M2 , M2 }
		}, 5, 5, 1,	1, 3 );
		
		// +4
		public static Pattern SimpleBombS4 = new Pattern( new object[8,10]{
			{F  , F  , F  , 1  , M1 , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , 4  , 5  , F  , F  , F  , F  },
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{F  , F  , 2  , 3  , 0  , F  , F  , F  , F  , F  },
			{F  , F  , F  , 4  , 5  , F  , F  , F  , F  , F  },
			{F  , F  , 3  , 2  , 1  , F  , F  , F  , F  , F  },
			{M2 , M1 , M2 , M2 , 0  , M2 , M2 , M1 , M2 , M2 }
		}, 5, 5, 1,	1, 3 );
		
		//基本形
		public static Pattern SimpleBombS = new Pattern( new object[4,10]{
			{F  , F  , F  , F  , M1 , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , 0  , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{M2 , M1 , M2 , M2 , 0  , M2 , M2 , M1 , M2 , M2 }
		}, 1, 4, 1,	1, 4 );
		
		//ズラース法 + 爆弾。
		public static ChainFactory SmallBombChain　= new ChainFactory(
			SimpleBombS,
			SimpleBody,
			SimpleJoint1,
			SimpleHead
		);
		
		
		//ズラース法 + 爆弾、挟み込みでカサまし。
		public static ChainFactory SmallBombChain2　= new ChainFactory(
			SimpleBombS2,
			SimpleBody,
			SimpleJoint1,
			SimpleHead
		);
		
		//ズラース法 + 爆弾、さらにでカサまし。
		public static ChainFactory SmallBombChain3　= new ChainFactory(
			SimpleBombS3,
			SimpleBody,
			SimpleJoint1,
			SimpleHead
		);
		
		//ズラース法 + 爆弾、4つカサまし
		public static ChainFactory SmallBombChain4　= new ChainFactory(
			SimpleBombS3,
			SimpleBody,
			SimpleJoint1,
			SimpleHead
		);
		*/
		
		
		
		
		//連鎖尾爆弾　基本形B
		public static Pattern SimpleBombSB = new Pattern( new object[6,10]{
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{F  , F  , F  , F  , F  , F  , F  , F  , F  , F  },
			{F  , F  , F  , Mx8, 1  , 2  , F  , F  , F  , F  },
			{F  , F  , F  , M03, Mx1, F  , F  , F  , F  , F  },
			{F  , F  , F  , 1  , 2  , F  , F  , F  , F  , F  },
			{Mx2, Mx1, Mx2, Mx2, M07, Mx2, Mx2, Mx1, Mx2, Mx2}
		}, 3, 5, 1,	1, 13 );
		
		
		//連鎖尾爆弾 応用型
		public static Pattern SimpleBombSC = new Pattern( new object[10,10]{
			{16 , 20 , 22 , F  , F  , F  , F  , F  , F  , F  },
			{17 , 21 , F  , F  , F  , F  , F  , F  , F  , F  },
			{18 , 22 , F  , F  , F  , F  , F  , F  , F  , F  },
			{19 , 21 , 13 , 12 , 11 , 10 , 9  , 8  , F  , 4  },
			{20 , F  , F  , F  , F  , F  , F  , F  , 7  , 5  },
			{19 , 15 , F  , F  , 12 , 11 , 10 , 9  , 8  , 6  },
			{18 , F  , 14 , 13 , F  , Mx8, 1  , 2  , 3  , 7  },
			{17 , 16 , 15 , F  , Mx8, M03, F  , F  , F  , 6  },
			{F  , 14 , F  , F  , Mx1, 1  , 2  , 3  , 4  , 5  },
			{Mx2, Mx1, Mx2, Mx2, M07, Mx2, Mx2, Mx1, Mx2, Mx2}
		}, 23, 2, 7, 1, 4 );
		
		
		//連鎖尾爆弾 応用型2
		public static Pattern SimpleBombSD = new Pattern( new object[10,10]{
			{17 , 21 , 23 , F  , F  , F  , F  , F  , F  , F  },
			{18 , 22 , F  , F  , F  , F  , F  , F  , F  , F  },
			{19 , 23 , F  , F  , F  , F  , F  , F  , F  , F  },
			{20 , 22 , 13 , 12 , 11 , 10 , 9  , 8  , F  , 4  },
			{21 , 15 , F  , F  , F  , F  , F  , F  , 7  , 5  },
			{20 , F  , F  , F  , 12 , 11 , 10 , 9  , 8  , 6  },
			{19 , 16 , 14 , 13 , F  , Mx8, 1  , 2  , 3  , 7  },
			{18 , 17 , 15 , F  , Mx8, M03, F  , F  , F  , 6  },
			{F  , 16 , 14 , F  , Mx1, 1  , 2  , 3  , 4  , 5  },
			{Mx2, Mx1, Mx2, Mx2, M07, Mx2, Mx2, Mx1, Mx2, Mx2}
		}, 24, 2, 7, 1, 4 );
		
		
		/*
		//小
		//ズラース法 + 爆弾。
		public static ChainFactory SmallBombChainB　= new ChainFactory(
			SimpleBombSB,
			SimpleBody,
			SimpleJoint2,
			SimpleJointHead
		);
		
		//ズラース法 + 爆弾。
		public static ChainFactory SmallBombChainC　= new ChainFactory(
			SimpleBombSC,
			SimpleBody,
			SimpleJoint2,
			SimpleJointHead
		);
		
		//ズラース法 + 爆弾。
		public static ChainFactory SmallBombChainD　= new ChainFactory(
			SimpleBombSD,
			SimpleBody,
			SimpleJoint2,
			SimpleJointHead
		);*/
		
		//ズラース法 + 爆弾。 Long型。
		public static ChainFactory SmallBombChainBL　= new ChainFactory(
			SimpleBombSB,
			SimpleBody,
			SimpleJoint2,
			SimpleHead
		);
		
		//ズラース法 + 爆弾。 Long型。
		public static ChainFactory SmallBombChainCL　= new ChainFactory(
			SimpleBombSC,
			SimpleBody,
			SimpleJoint2,
			SimpleHead
		);
		
		//ズラース法 + 爆弾。 Long型。
		public static ChainFactory SmallBombChainDL　= new ChainFactory(
			SimpleBombSD,
			SimpleBody,
			SimpleJoint2,
			SimpleHead
		);
		
	}
}
