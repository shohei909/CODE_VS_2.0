/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/20
 * 時刻: 15:43
 * 
 */
using System;

namespace GameCore.Model
{
	/// <summary>
	/// Description of ModelSetting.
	/// </summary>
	public class ModelSetting{
		public double BingoScore 	= 30000;
		public double LizhiScore 	= 7000;
		public double CeilScore 	= -4000;
		public double MaxChainScore = 0;
		public double ChainScore 	= 20000;
		public double GroundScore	= 12000;
		public double GroundHScore	= 3000;
		public double GroundNGScore	= -100000;
		public double BrokenScore 	= -60000000;
		public int BrokenLimit		= 2;
		public bool HaiteiMode		= false;
		
		
		public void Setup( ChainModel m ){
			m.BingoScore 	= BingoScore; 
			m.LizhiScore	= LizhiScore;
			m.CeilScore		= CeilScore;
			m.BrokenScore	= BrokenScore;
			m.MaxChainScore	= MaxChainScore;
			m.ChainScore	= ChainScore;
			m.GroundHScore	= GroundHScore;
			m.GroundNGScore	= GroundNGScore;
			m.GroundScore 	= GroundScore;
			m.BrokenLimit	= BrokenLimit;
		}
		
		public static ModelSetting LargeSetting = new ModelSetting(){
			BrokenLimit 	= 30,
			GroundNGScore	= -500000
		};
	}
}
