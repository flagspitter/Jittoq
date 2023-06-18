namespace Jittoq.SwForm
{
	partial class LapView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lstLap = new Misc.SortableListView();
			this.clmNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.clmLap = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.clmSpl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstLap
			// 
			this.lstLap.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstLap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmNo,
            this.clmLap,
            this.clmSpl});
			this.lstLap.Dock = System.Windows.Forms.DockStyle.Top;
			this.lstLap.GridLines = true;
			this.lstLap.Location = new System.Drawing.Point(0, 0);
			this.lstLap.Name = "lstLap";
			this.lstLap.OwnerDraw = true;
			this.lstLap.Size = new System.Drawing.Size(287, 225);
			this.lstLap.SortTarget = -1;
			this.lstLap.TabIndex = 0;
			this.lstLap.UseCompatibleStateImageBehavior = false;
			this.lstLap.View = System.Windows.Forms.View.Details;
			// 
			// clmNo
			// 
			this.clmNo.Text = "No";
			// 
			// clmLap
			// 
			this.clmLap.Text = "LAP";
			this.clmLap.Width = 100;
			// 
			// clmSpl
			// 
			this.clmSpl.Text = "SPLIT";
			this.clmSpl.Width = 100;
			// 
			// btnCopy
			// 
			this.btnCopy.Location = new System.Drawing.Point(0, 231);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(145, 36);
			this.btnCopy.TabIndex = 97;
			this.btnCopy.Text = "クリップボードにコピー(&C)";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(170, 231);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(117, 36);
			this.btnClose.TabIndex = 98;
			this.btnClose.Text = "閉じる(&X)";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// LapView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(287, 270);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnCopy);
			this.Controls.Add(this.lstLap);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "LapView";
			this.Text = "LAP - Jittoq";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LapView_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private Misc.SortableListView lstLap;
		private System.Windows.Forms.ColumnHeader clmNo;
		private System.Windows.Forms.ColumnHeader clmLap;
		private System.Windows.Forms.ColumnHeader clmSpl;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.Button btnClose;
	}
}