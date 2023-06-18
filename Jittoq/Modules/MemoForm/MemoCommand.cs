using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jittoq.MemoForm
{
	public partial class MemoCommand : Form
	{
		public MemoCommand()
		{
			InitializeComponent();
		}

		private void MemoCommand_Load(object sender, EventArgs e)
		{
			this.Height = txtCommand.Height;
		}
		
		// public string Command => txtCommand.Text.Trim();
		public string Command { get; private set; }
		
		private void btnOK_Click(object sender, EventArgs e)
		{
			Command = txtCommand.Text.Trim();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void txtCommand_KeyPress(object sender, KeyPressEventArgs e)
		{
			if( txtCommand.Text == "" )
			{
				if( e.KeyChar == '\\' )
				{
					Command = @"\";
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}
		}
	}
}
