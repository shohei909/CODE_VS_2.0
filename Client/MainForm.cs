/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/11/21
 * 時刻: 1FixedWidth	= 11,:0FixedWidth	= 11,
 * 
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GameCore;
using GameCore.AI;
using GameCore.Model;

namespace Client{
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form{
		
		public GameAI		AI;
		public TextReader 	Output;
		public Game			MainGame;
		public Bitmap		TempImage;
		public Bitmap 		Dummy = new Bitmap( 1, 1 );
		public double		Time = 0;
		public const int CELL_SIZE 		= 14;
		public const int LINE_WIDTH 	= 1;
		
		public Color[] Colors = new Color[10]{
              Color.SkyBlue, Color.SkyBlue, Color.LightGreen, Color.LightGreen, Color.Orange, 
              Color.Orange, Color.Orange, Color.Yellow, Color.Yellow, Color.Red
		};
		
		public MainForm(){
			InitializeComponent();
			Read();
			Start();
		}
		
		//テキストからブロックデータ読み込み
		private void Read(){
			//var input 	= new StreamReader( "../../../../sample2012/sample_input.txt" );
			var input 	= new StreamReader( "../../../../sample2012/sample_input_M.txt" );
			//var input 	= new StreamReader( "../../../../sample2012/sample_input_L.txt" );
			
			Game.Init( input );
			MainGame 	= new Game();
			//AI 			= new EasyAI( MainGame );
			//AI			= new ChainAI( MainGame, Config.SmallAIList[0] );
			AI			= new ChainAI( MainGame, Config.MediumAIList[0] );
			//AI			= new ChainAI( MainGame, Config.LargeAIList[0] );
			
			var lw 		= LINE_WIDTH;
			TempImage 	= new Bitmap( 
                    		Game.Width  * (CELL_SIZE + lw) + lw, 
                    		Game.Height * (CELL_SIZE + lw) + lw,
                    		PixelFormat.Format24bppRgb
                    	);
			Paint 		+= new PaintEventHandler( OnPaint );
			Resize 		+= new EventHandler( OnResize );
		}
		
		private void Start(){
			Timer t = new Timer();
			t.Interval 	=	1; 
			t.Tick 		+= 	new EventHandler( Run );
			t.Start();
		}
		
		private void Run( object o, EventArgs e ){
			var t = (o as Timer);
			Stopwatch sw = new Stopwatch();
			
			ChainModel m = null;
			if( AI is ChainAI ){
				m = (AI as ChainAI).Model;
			}
			
			
			for( int i = 0; i < 1; i++ ){
				if( MainGame.GameOver || MainGame.CurrentStep >= Game.StepNum || MainGame.CurrentStep >= 3 ){
					t.Stop();
					Time += sw.ElapsedMilliseconds;
					
					if( AI is ChainAI ){
						m.Reset( MainGame );
						OutputBox.AppendText( m.ModelOutput( m.Model, MainGame ) + "\n" );
						m.Analize( MainGame.Clone(), true );
						OutputBox.AppendText( m.Output + "\n" );
					}
					
					break;
				}else{
					sw.Start();
					AI.Next();
					sw.Stop();
					OutputBox.AppendText( "0:" + (AI as ChainAI).Output + "\n" );
					OutputBox.AppendText( "0:" + sw.ElapsedMilliseconds + "\n" );
					sw.Reset();
					
					InfoText.Text = "Turn:" + MainGame.CurrentStep + "/"+ Game.StepNum +" Score:" + MainGame.Score + " MaxChain:" + MainGame.MaxChain;
				}
			}
			
			sw.Stop();
			Time += sw.ElapsedMilliseconds;
			Draw();
		}
		
		private void Draw(){
			var g = Graphics.FromImage( TempImage );
			int w = Game.Width, h = Game.Height, lw = LINE_WIDTH;
			var r = new Rectangle( lw, lw, CELL_SIZE, CELL_SIZE );
			var font = new Font(  FontFamily.GenericMonospace, 7 );
			var white = new SolidBrush( Color.White );
			var black = new SolidBrush( Color.Black );
			
			g.Clear( Color.DarkGray );
			for( var i = 0; i < w; i++ ){
				var row = MainGame.Field[i];
				var rl	= MainGame.HeightList[i];
				for( int j = h - 1; j >= 0; j-- ){
					if( j < rl ){
						int num = row[j] & 0x7FFFFF;
						if( num > Game.Sum ){
							g.FillRectangle( black, r );
							g.DrawString( "x", font, white, r.X, r.Y );
						}else{ 
							g.FillRectangle( new SolidBrush(Colors[ (num - 1) % 10 ]), r );
							g.DrawString( "" + num, font, black, r.X, r.Y );
						}
					}else	g.FillRectangle( white, r );
					
					r.Y += CELL_SIZE + lw;
				}
				r.X += CELL_SIZE + lw; r.Y = lw;
			}
			g.Dispose();
			Invalidate();
		}
		
  		private void OnPaint(object o, PaintEventArgs e) {
			Canvas.Image 	= TempImage;
			Canvas.SizeMode = PictureBoxSizeMode.CenterImage; 
		}
		
		private void OnResize(object o, EventArgs e) { Invalidate(); }
		protected override void OnPaintBackground(PaintEventArgs e){}
	}
}