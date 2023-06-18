using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jittoq
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			// 多重起動を防止
			if( System.Diagnostics.Process.GetProcessesByName (
					System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1 )
			{
				;
			}
			else
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainWindow());
			}
		}
	}
}
