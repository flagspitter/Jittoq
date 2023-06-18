using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jittoq.SwForm
{
	public partial class SwOffset : Form
	{
		public SwOffset()
		{
			InitializeComponent();
		}
		
		public TimeSpan Value => new TimeSpan( 0, 0, 0, (int)numOffset.Value, (int)(numOffset.Value * 1000) % 1000 );

		private void btnAccept_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
