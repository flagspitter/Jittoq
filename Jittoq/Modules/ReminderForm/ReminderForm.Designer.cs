namespace Jittoq.TodoForm
{
	partial class ReminderForm
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
			this.lstLap = new Misc.SortableListView();
			this.clmLim = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.clmItems = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// lstLap
			// 
			this.lstLap.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstLap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmLim,
            this.clmItems});
			this.lstLap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstLap.GridLines = true;
			this.lstLap.LabelEdit = true;
			this.lstLap.Location = new System.Drawing.Point(0, 0);
			this.lstLap.Name = "lstLap";
			this.lstLap.OwnerDraw = true;
			this.lstLap.Size = new System.Drawing.Size(268, 156);
			this.lstLap.SortTarget = -1;
			this.lstLap.TabIndex = 1;
			this.lstLap.UseCompatibleStateImageBehavior = false;
			this.lstLap.View = System.Windows.Forms.View.Details;
			// 
			// clmLim
			// 
			this.clmLim.Text = "Limit";
			// 
			// clmItems
			// 
			this.clmItems.Text = "Item";
			this.clmItems.Width = 100;
			// 
			// ReminderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DimGray;
			this.Controls.Add(this.lstLap);
			this.Name = "ReminderForm";
			this.Size = new System.Drawing.Size(268, 156);
			this.ResumeLayout(false);

		}

		#endregion

		private Misc.SortableListView lstLap;
		private System.Windows.Forms.ColumnHeader clmLim;
		private System.Windows.Forms.ColumnHeader clmItems;
	}
}
