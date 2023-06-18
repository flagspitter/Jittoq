namespace Jittoq.CalcForm
{
	partial class CalcForm
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

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.lbResult = new System.Windows.Forms.Label();
			this.tmrInterval = new System.Windows.Forms.Timer(this.components);
			this.txtExpression = new System.Windows.Forms.TextBox();
			this.lbResultHex = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lbResult
			// 
			this.lbResult.BackColor = System.Drawing.Color.DimGray;
			this.lbResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbResult.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbResult.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lbResult.ForeColor = System.Drawing.Color.White;
			this.lbResult.Location = new System.Drawing.Point(0, 0);
			this.lbResult.Name = "lbResult";
			this.lbResult.Size = new System.Drawing.Size(220, 32);
			this.lbResult.TabIndex = 4;
			this.lbResult.Text = "---";
			this.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lbResult.Paint += new System.Windows.Forms.PaintEventHandler(this.lbResult_Paint);
			// 
			// tmrInterval
			// 
			this.tmrInterval.Interval = 50;
			this.tmrInterval.Tick += new System.EventHandler(this.tmrInterval_Tick);
			// 
			// txtExpression
			// 
			this.txtExpression.BackColor = System.Drawing.Color.Black;
			this.txtExpression.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtExpression.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.txtExpression.ForeColor = System.Drawing.Color.White;
			this.txtExpression.Location = new System.Drawing.Point(0, 54);
			this.txtExpression.Multiline = true;
			this.txtExpression.Name = "txtExpression";
			this.txtExpression.Size = new System.Drawing.Size(220, 41);
			this.txtExpression.TabIndex = 5;
			this.txtExpression.TextChanged += new System.EventHandler(this.txtExpression_TextChanged);
			this.txtExpression.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExpression_KeyDown);
			this.txtExpression.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtExpression_KeyPress);
			// 
			// lbResultHex
			// 
			this.lbResultHex.BackColor = System.Drawing.Color.DimGray;
			this.lbResultHex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbResultHex.Dock = System.Windows.Forms.DockStyle.Top;
			this.lbResultHex.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lbResultHex.ForeColor = System.Drawing.Color.White;
			this.lbResultHex.Location = new System.Drawing.Point(0, 32);
			this.lbResultHex.Name = "lbResultHex";
			this.lbResultHex.Size = new System.Drawing.Size(220, 22);
			this.lbResultHex.TabIndex = 6;
			this.lbResultHex.Text = "---";
			this.lbResultHex.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// CalcForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtExpression);
			this.Controls.Add(this.lbResultHex);
			this.Controls.Add(this.lbResult);
			this.Name = "CalcForm";
			this.Size = new System.Drawing.Size(220, 95);
			this.Load += new System.EventHandler(this.CalcForm_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CalcForm_KeyDown);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CalcForm_KeyPress);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbResult;
		private System.Windows.Forms.Timer tmrInterval;
		private System.Windows.Forms.TextBox txtExpression;
		private System.Windows.Forms.Label lbResultHex;
	}
}
