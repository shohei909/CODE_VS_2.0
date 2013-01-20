/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/11/24
 * 時刻: 1FixedWidth	= 11,:12
 * 
 */
using System;

namespace GameCore.AI{
	/// <summary>
	/// </summary>
	public abstract class GameAI{
		public Game 	Target;
		public int		LastPos;
		public int		LastRot;
		public Random rand = new Random(0);
		
		public GameAI( Game target = null ){ 
			if( target != null ){ Target = target; }
		}
		public abstract void Next();
		public void Drop( int position, int rotate ){
			Target.Next( position, rotate );
			LastPos = position;
			LastRot = rotate;
		}
	}
}
