namespace Jittoq.SwForm
{
    partial class SwForm
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
			this.lbCounter = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.tmrInterval = new System.Windows.Forms.Timer(this.components);
			this.btnLap = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnLapView = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbCounter
			// 
			this.lbCounter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbCounter.Font = new System.Drawing.Font("MS UI Gothic", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lbCounter.ForeColor = System.Drawing.Color.White;
			this.lbCounter.Location = new System.Drawing.Point(0, 19);
			this.lbCounter.Name = "lbCounter";
			this.lbCounter.Size = new System.Drawing.Size(222, 33);
			this.lbCounter.TabIndex = 10;
			this.lbCounter.Text = "00:00:00.00";
			this.lbCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbCounter.DoubleClick += new System.EventHandler(this.lbCounter_DoubleClick);
			// 
			// btnReset
			// 
			this.btnReset.BackColor = System.Drawing.Color.Crimson;
			this.btnReset.ForeColor = System.Drawing.Color.White;
			this.btnReset.Location = new System.Drawing.Point(101, 2);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(39, 24);
			this.btnReset.TabIndex = 2;
			this.btnReset.Text = "CLR";
			this.btnReset.UseVisualStyleBackColor = false;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnStartStop
			// 
			this.btnStartStop.BackColor = System.Drawing.Color.Blue;
			this.btnStartStop.ForeColor = System.Drawing.Color.White;
			this.btnStartStop.Location = new System.Drawing.Point(0, 2);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(50, 24);
			this.btnStartStop.TabIndex = 1;
			this.btnStartStop.Text = "START";
			this.btnStartStop.UseVisualStyleBackColor = false;
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// tmrInterval
			// 
			this.tmrInterval.Interval = 30;
			this.tmrInterval.Tick += new System.EventHandler(this.tmrInterval_Tick);
			// 
			// btnLap
			// 
			this.btnLap.BackColor = System.Drawing.Color.Blue;
			this.btnLap.ForeColor = System.Drawing.Color.White;
			this.btnLap.Location = new System.Drawing.Point(56, 2);
			this.btnLap.Name = "btnLap";
			this.btnLap.Size = new System.Drawing.Size(39, 24);
			this.btnLap.TabIndex = 3;
			this.btnLap.Text = "LAP";
			this.btnLap.UseVisualStyleBackColor = false;
			this.btnLap.Click += new System.EventHandler(this.btnLap_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnReset);
			this.panel1.Controls.Add(this.btnLap);
			this.panel1.Controls.Add(this.btnStartStop);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 52);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(222, 26);
			this.panel1.TabIndex = 12;
			// 
			// btnLapView
			// 
			this.btnLapView.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.btnLapView.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnLapView.ForeColor = System.Drawing.Color.White;
			this.btnLapView.Location = new System.Drawing.Point(0, 0);
			this.btnLapView.Name = "btnLapView";
			this.btnLapView.Size = new System.Drawing.Size(222, 19);
			this.btnLapView.TabIndex = 13;
			this.btnLapView.Text = "[-] LAP 00:00:00.00  SPL 00:00:00.00";
			this.btnLapView.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnLapView.UseVisualStyleBackColor = false;
			this.btnLapView.Click += new System.EventHandler(this.btnLapView_Click);
			// 
			// SwForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DimGray;
			this.Controls.Add(this.lbCounter);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnLapView);
			this.Name = "SwForm";
			this.Size = new System.Drawing.Size(222, 78);
			this.Load += new System.EventHandler(this.SwForm_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.Label lbCounter;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.Timer tmrInterval;
		private System.Windows.Forms.Button btnLap;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnLapView;
	}
}
