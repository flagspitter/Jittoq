using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Diagnostics;
using Misc;

namespace Jittoq
{
	public partial class MainWindow : Form
	{
		////////////////////////////////////////////////////////////////
		#region メンバ変数等
		////////////////////////////////////////////////////////////////
		
		// ホットキー制御用
		private Hotkey hk = new Hotkey();
		
		// 設定用
		private Inifile Ini = new Inifile( Const.AplicationPath + "\\Settings.ini" );
		private Inifile StatusIni = new Inifile( Const.AplicationPath + "\\Status.ini" );
		private string CatSystem = "System";
		private string CatView = "View";
		
		// ウィンドウサイズ制御用
		private FormForceResizer Resizer;
		private int MinWinWidth = 220;
		private int MinWinHeight = 95;
		private double Opac_a;
		private double Opac_d;
		
		// 子
		private List<IJittokModule> Modules = new List<IJittokModule>(); // new() で済ませたい
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 初期化関係
		////////////////////////////////////////////////////////////////
		
		public MainWindow()
		{
			InitializeComponent();
			
			ShowInTaskbar = false;
			TopMost = true;
			
			// タスクトレイの準備
			notifyIcon1.Visible = true;
			notifyIcon1.Text = "Jittoq";
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{
			// dllにした方が良いんだろうな…
			Modules.Add( moduleCalc );
			Modules.Add( moduleSw );
			Modules.Add( new Jittoq.DesktopPeeker.DesktopPeeker() );
			Modules.Add( moduleMemo );
			
			// タスクトレイでの右クリックメニュー
			ContextMenuStrip menu = new ContextMenuStrip(); 
			foreach( var m in Modules )
			{
				string name = m.ModuleName;
				if( m.AcceleratorKey != '\0' )
				{
					name += $"(&{m.AcceleratorKey})";
				}
				menu.Items.Add( new ToolStripMenuItem( name, null, (_s,_e) => m.Activate() ) ); // todo 破棄パターン _
			}
			menu.Items.Add( new System.Windows.Forms.ToolStripSeparator() ); 
			menu.Items.Add( new ToolStripMenuItem( "ResetPosition(&P)", null, (_s,_e) => Left=Top=0 ) );
			menu.Items.Add( new ToolStripMenuItem( "Exit(&X)", null, (_s,_e) => Exit() ) );
			notifyIcon1.ContextMenuStrip = menu;
			
			//
			Modules.ForEach( c => c.Initialize( Ini ) );
			Modules.ForEach( c => c.Visible = false );
			Modules.ForEach( c => c.Activating += (s,_e) => Wake((IJittokModule)s) );
			Modules.ForEach( c => c.Deactivating += (s,_e) => Rest((IJittokModule)s) );
			
			foreach( var m in Modules )
			{
				foreach( var c in m.Controllers )
				{
					// Console.WriteLine( $"{c.Key} : {c.Value}" );
					RegisterHk( c.Key, c.Value );
				}
			}
			
			Resizer = new FormForceResizer( this, MinWinWidth, MinWinHeight );
			Resizer.EnableVertical = false;
			
			foreach( var m in Modules )
			{
				if( m.CursorFixer != null )
				{
					Resizer.Excluded.AddRange( m.CursorFixer.ToArray() );
				}
			}
			
			LoadSettings();
		}

		private void MainWindow_Shown(object sender, EventArgs e)
		{
			ResizeVertical();
		}
		
		private void RegisterHk( string key, Hotkey.CallbackFunc cf )
		{
			if( key != "" )
			{
				string[] keylist = key.Split( '+' );
				int modKey = 0;
				Keys? normalKey = null;
				
				for( int i = 0; i<keylist.Length-1; i++ )
				{
					modKey |= KeyCode.Str2ModKey( keylist[i] );
				}
				
				if( keylist.Length > 0 )
				{
					normalKey = KeyCode.Str2Keys( keylist[ keylist.Length - 1 ] );
				}
				
				if( normalKey != null )
				{
					hk.Register( modKey, normalKey.Value, cf );
				}
			}
		}
		
		private void LoadSettings()
		{
			int x  = Int32.Parse(  StatusIni.Read( CatView, "Left", "0" ) );
			int y  = Int32.Parse(  StatusIni.Read( CatView, "Top", "0" ) );
			int w  = Int32.Parse(  StatusIni.Read( CatView, "Width", "225" ) );
			Opac_a = Double.Parse( Ini.Read( CatSystem, "Opac_Active", "0.9" ) );
			Opac_d = Double.Parse( Ini.Read( CatSystem, "Opac_Deactive", "0.8" ) );
			
			this.Left = x;
			this.Top = y;
			this.Width = w;
			this.Opacity = ( Form.ActiveForm == this ) ? Opac_a : Opac_d;
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 終了処理
		////////////////////////////////////////////////////////////////
		
		private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
		{
			hk.UnregisterAll();
			Modules.ForEach( m => m.Exit() );
		}
		
		private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			StatusIni.Write( CatView, "Top",  this.Top );
			StatusIni.Write( CatView, "Left", this.Left );
			StatusIni.Write( CatView, "Width",  this.Width );
			StatusIni.Write( CatView, "Height", this.Height );
			
			if( this.Visible && ( e.CloseReason == CloseReason.UserClosing ) )
			{
				e.Cancel = true;
				this.Hide();
			}
		}
		
		#endregion

		////////////////////////////////////////////////////////////////
		#region ウィンドウ制御関係
		////////////////////////////////////////////////////////////////
		
		private void MainWindow_Activated(object sender, EventArgs e)
		{
			Modules.ForEach( c => c.AppsActivated() );
			this.Opacity = Opac_a;
		}
		
		private void MainWindow_Deactivate(object sender, EventArgs e)
		{
			Modules.ForEach( c => c.AppsDeactivated() );
			this.Opacity = Opac_d;
		}
		
		public new void Show()
		{
			if( this.Visible == false )
			{
				base.Show();
			}
		}
		
		private const int WS_EX_NOACTIVATE = 0x8000000;
		protected override CreateParams CreateParams
		{
			get
			{
				var cp = base.CreateParams;
				if( base.DesignMode == false )
				{
					cp.ExStyle |= WS_EX_NOACTIVATE;
				}
				
				return cp;
			}
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 総合制御
		////////////////////////////////////////////////////////////////
		
		private void Wake( IJittokModule c )
		{
			c.Visible = true;
			ResizeVertical();
			
			// Console.WriteLine( $"Wake {c.ModuleName}" );
			
			// 下に見切れないようにする
			var scr = System.Windows.Forms.Screen.FromControl( this );
			if( ( this.Bottom + c.Height ) >= scr.Bounds.Height )
			{
				// Console.WriteLine( $"Add to top: Win( {Top}, {Left} ) - ( {Right}, {Bottom} ), c.Height={c.Height}" );
				c.SendToBack();
				this.Top -= c.Height;
			}
			else
			{
				// Console.WriteLine( $"Add to Bottom: Win( {Top}, {Left} ) - ( {Right}, {Bottom} ), c.Height={c.Height}" );
				c.BringToFront();
			}
			// Console.WriteLine( $"            -> Win( {Top}, {Left} ) - ( {Right}, {Bottom} ), c.Height={c.Height}" );
		}
		
		private void Rest( IJittokModule c )
		{
			c.Visible = false;
			ResizeVertical();
		}
		
		private void ResizeVertical()
		{
			int sumHeight = 0;
			
			Modules.ForEach( c => sumHeight += ( c.Visible ? c.Height : 0 ) );
			
			if( sumHeight == 0 )
			{
				this.Hide();
			}
			else
			{
				this.Show();
				this.Height = sumHeight;
			}
		}
		
		private void Exit()
		{
			this.Hide();
			this.Close();
		}

		#endregion
	}

	public static class Common
	{
		public static Color LoadColorSetting( Inifile ini, string cat, string key, string def )
		{
			// ini.Init( cat, key, def );
			string tmp = ini.Read( cat, key, def );
			
			Color ret = Color.FromName(def);
			
			try
			{
				ret = ColorTranslator.FromHtml( tmp );
			}
			catch
			{
				MessageBox.Show(
					$"Color specification {tmp} for {key} is invalid.\nDefault color ({def}) is used instead.",
					"Karculator : Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
			
			return ret;
		}
		
		public static Font ReadFontSetting( Inifile ini, string cat, string key, string defaultName = "", int defaultSize = 12 )
		{
			string director = ini.Read( cat, key, "" );
			var dirItems = director.Split(',');
			string name;
			int size;
			
			if( dirItems.Length == 0 )
			{
				name = defaultName;
				size = defaultSize;
			}
			else if( dirItems.Length == 1 )
			{
				name = dirItems[0].Trim();
				size = defaultSize;
			}
			else
			{
				name = dirItems[0].Trim();
				size = Int32.Parse( dirItems[1].Trim() );
			}
			
			var fontList = new InstalledFontCollection()?.Families ?? null;
			Font ret;
			
			if( name != "" )
			{
				if( fontList.FirstOrDefault( v => v.Name == name ) != null )
				{
					ret = new Font( name, size );
				}
				else
				{
					throw new ApplicationException( "Invalid Font Name" );
				}
			}
			else
			{
				ret = null;
			}
			
			return ret;
		}
	}
}
