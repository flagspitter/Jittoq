namespace Jittoq
{
	partial class MainWindow
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.moduleCalc = new Jittoq.CalcForm.CalcForm();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.moduleSw = new Jittoq.SwForm.SwForm();
			this.moduleMemo = new Jittoq.MemoForm.MemoForm();
			this.SuspendLayout();
			// 
			// moduleCalc
			// 
			this.moduleCalc.BackExpression = System.Drawing.Color.Empty;
			this.moduleCalc.BackExpressionDeactivated = System.Drawing.Color.Empty;
			this.moduleCalc.BackResult = System.Drawing.Color.Empty;
			this.moduleCalc.BackResultHex = System.Drawing.Color.Empty;
			this.moduleCalc.Dock = System.Windows.Forms.DockStyle.Top;
			this.moduleCalc.ExpressionFont = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.moduleCalc.ForeExpression = System.Drawing.Color.Empty;
			this.moduleCalc.ForeExpressionDeactivated = System.Drawing.Color.Empty;
			this.moduleCalc.ForeResult = System.Drawing.Color.Empty;
			this.moduleCalc.ForeResultHex = System.Drawing.Color.Empty;
			this.moduleCalc.ForeResultHexPast = System.Drawing.Color.Empty;
			this.moduleCalc.ForeResultPast = System.Drawing.Color.Empty;
			this.moduleCalc.Location = new System.Drawing.Point(0, 0);
			this.moduleCalc.Name = "moduleCalc";
			this.moduleCalc.ResultFonmt = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.moduleCalc.ResultHexFonmt = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.moduleCalc.Size = new System.Drawing.Size(225, 116);
			this.moduleCalc.TabIndex = 0;
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "icon";
			this.notifyIcon1.Visible = true;
			// 
			// moduleSw
			// 
			this.moduleSw.BackColor = System.Drawing.Color.DimGray;
			this.moduleSw.Dock = System.Windows.Forms.DockStyle.Top;
			this.moduleSw.Format = null;
			this.moduleSw.Location = new System.Drawing.Point(0, 116);
			this.moduleSw.Name = "moduleSw";
			this.moduleSw.Size = new System.Drawing.Size(225, 78);
			this.moduleSw.SwBackColor = System.Drawing.Color.Empty;
			this.moduleSw.SwForeColor = System.Drawing.Color.Empty;
			this.moduleSw.TabIndex = 1;
			this.moduleSw.TextFont = null;
			// 
			// moduleMemo
			// 
			this.moduleMemo.Dock = System.Windows.Forms.DockStyle.Top;
			this.moduleMemo.Location = new System.Drawing.Point(0, 194);
			this.moduleMemo.Name = "moduleMemo";
			this.moduleMemo.Size = new System.Drawing.Size(225, 58);
			this.moduleMemo.TabIndex = 2;
			this.moduleMemo.TextBackColor = System.Drawing.Color.DimGray;
			this.moduleMemo.TextForeColor = System.Drawing.Color.White;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DimGray;
			this.ClientSize = new System.Drawing.Size(225, 264);
			this.Controls.Add(this.moduleMemo);
			this.Controls.Add(this.moduleSw);
			this.Controls.Add(this.moduleCalc);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.Name = "MainWindow";
			this.Text = "JIttoq";
			this.Activated += new System.EventHandler(this.MainWindow_Activated);
			this.Deactivate += new System.EventHandler(this.MainWindow_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.Shown += new System.EventHandler(this.MainWindow_Shown);
			this.ResumeLayout(false);

		}

		#endregion

		private CalcForm.CalcForm moduleCalc;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private SwForm.SwForm moduleSw;
		private MemoForm.MemoForm moduleMemo;
	}
}

