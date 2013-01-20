/*
 * SharpDevelopによって生成
 * ユーザ: shohei
 * 日付: 2012/11/21
 * 時刻: 13:03
 * 
 */
namespace Client
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.OutputBox = new System.Windows.Forms.RichTextBox();
			this.Canvas = new System.Windows.Forms.PictureBox();
			this.InfoText = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
			this.SuspendLayout();
			// 
			// OutputBox
			// 
			this.OutputBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.OutputBox.Location = new System.Drawing.Point(0, 607);
			this.OutputBox.Name = "OutputBox";
			this.OutputBox.Size = new System.Drawing.Size(754, 125);
			this.OutputBox.TabIndex = 0;
			this.OutputBox.Text = "";
			// 
			// Canvas
			// 
			this.Canvas.BackColor = System.Drawing.SystemColors.Desktop;
			this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Canvas.Location = new System.Drawing.Point(0, 19);
			this.Canvas.Name = "Canvas";
			this.Canvas.Size = new System.Drawing.Size(754, 588);
			this.Canvas.TabIndex = 1;
			this.Canvas.TabStop = false;
			// 
			// InfoText
			// 
			this.InfoText.Dock = System.Windows.Forms.DockStyle.Top;
			this.InfoText.Location = new System.Drawing.Point(0, 0);
			this.InfoText.Name = "InfoText";
			this.InfoText.ReadOnly = true;
			this.InfoText.Size = new System.Drawing.Size(754, 19);
			this.InfoText.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(754, 732);
			this.Controls.Add(this.Canvas);
			this.Controls.Add(this.InfoText);
			this.Controls.Add(this.OutputBox);
			this.Name = "MainForm";
			this.Text = "Game";
			((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox InfoText;
		private System.Windows.Forms.PictureBox Canvas;
		private System.Windows.Forms.RichTextBox OutputBox;
		
	}
}
