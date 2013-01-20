/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/12/12
 * 時刻: 19:54
 * 
 */
using System;
using GameCore.Model;

namespace GameCore.AI{
	/// <summary>
	/// Description of AISetting.
	/// </summary>
	public class AISetting{
		
		//積み上げから発火に切り替える点数比
		public double 	FireRate = 0.7;
		
		//読みの深さ。
		public int	Depth		= 20;
		
		//読みの広さ。
		public int 	Width		= 70;
		
		//待機時間。
		public int	FloorHeight	= 16;
		
		//連鎖チェック時に使うブロック数。
		public int	CheckLimit	= 6;
		
		//先読みの閾値になる得点の倍率。
		public int 	FutureMin	= 40;
		
		//定型連鎖を作る時の読みの深さ。
		public int　	FixedDepth 	= 1;
		
		//定型連鎖を作る時の読みの広さ。
		public int　	FixedWidth 	= 10;
		
		//不定型連鎖を作るのに使うステップ数
		public int　	FixedLimit 	= 100;
		//連鎖数
		public int	ChainLimit;
		
		public bool HaiteiMode = false;
		
		//モデルの設定情報。
		public ModelSetting ModelSetting = new ModelSetting();
		
		//連鎖モデル
		public ChainFactory Factory;
	}
}
