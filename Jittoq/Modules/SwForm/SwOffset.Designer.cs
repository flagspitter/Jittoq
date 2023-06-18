namespace Jittoq.SwForm
{
	partial class SwOffset
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
			this.label1 = new System.Windows.Forms.Label();
			this.btnAccept = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.numOffset = new System.Windows.Forms.NumericUpDown();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numOffset)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 12);
			this.label1.TabIndex = 6;
			this.label1.Text = "Offset Time";
			// 
			// btnAccept
			// 
			this.btnAccept.Location = new System.Drawing.Point(42, 44);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(75, 23);
			this.btnAccept.TabIndex = 8;
			this.btnAccept.Text = "OK";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(187, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(24, 12);
			this.label2.TabIndex = 7;
			this.label2.Text = "Sec";
			// 
			// numOffset
			// 
			this.numOffset.DecimalPlaces = 1;
			this.numOffset.Location = new System.Drawing.Point(84, 14);
			this.numOffset.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
			this.numOffset.Minimum = new decimal(new int[] {
            86400,
            0,
            0,
            -2147483648});
			this.numOffset.Name = "numOffset";
			this.numOffset.Size = new System.Drawing.Size(97, 19);
			this.numOffset.TabIndex = 5;
			this.numOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(136, 44);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// SwOffset
			// 
			this.AcceptButton = this.btnAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(235, 79);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numOffset);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SwOffset";
			this.ShowInTaskbar = false;
			this.Text = "Stopwatch";
			((System.ComponentModel.ISupportInitialize)(this.numOffset)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numOffset;
		private System.Windows.Forms.Button btnCancel;
	}
}