using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Runtime.InteropServices;
using Misc;

namespace Jittoq.DesktopPeeker
{
	public class DesktopPeeker : IJittokModule
	{
		private Timer tmrKeyChecker = new Timer();
		public int WaitTime { get; set; } = 400;
		
		public bool Enabled {
			get => tmrKeyChecker.Enabled;
			set => tmrKeyChecker.Enabled = value;
		}
		
		public DesktopPeeker()
		{
		}
		
		public string IniCategory { get; } = "DesktopPeeker";
		
		private bool Status = false;
		private int Count = 0;
		
		////////////////////////////////////////////////////////////////
		#region インターフェースの実装
		////////////////////////////////////////////////////////////////
		
		public string ModuleName => "DesktopPeeker";
		public char   AcceleratorKey => 'D';
		public string Version => "0.1.0";
		
		// UserControlを継承しないため
		public bool Visible { get; set; } = false;
		public int  Height  { get; set; } = 0;
		public int  Width   { get; set; } = 0;
		public void SendToBack() {}
		public void BringToFront() {}
		
		private Keys? PrimaryKey = null;
		private Keys? SecondaryKey =null;
		
		public Dictionary<string,Hotkey.CallbackFunc> Controllers { get; } = new Dictionary<string,Hotkey.CallbackFunc>();
		public List<Control> CursorFixer { get; } = null;
		
		public void Initialize( Inifile ini )
		{
			// this.Hide();
			
			// ini.Exec( IniCategory, "Peek", "Win+Alt+Space", ',', v => Controllers.Add( v.Trim(), Peek ) );
			
			// 都合により、キーは固定せざるを得ない
			// Controllers.Add( "Win+Alt+Space", Peek );
			tmrKeyChecker.Interval = 100;
			tmrKeyChecker.Tick += tmrKeyChecker_Tick;
			
			WaitTime = Int32.Parse( ini.Read( IniCategory, "Wait", "300" ) );
			WaitTime /= tmrKeyChecker.Interval;
			
			Enabled = ( Int32.Parse( ini.Read( IniCategory, "Enabled", "1" ) ) != 0 ) ? true : false;
			
			PrimaryKey   = ReadKey( "PrimaryKey", "LWin" );
			SecondaryKey = ReadKey( "SecondaryKey", "Z" );
			
			return;
			
			Keys? ReadKey( string name, string def )
			{
				var tmp = ini.Read( IniCategory, name, def );
				return ( tmp == "" ) ? 
					null :
					(Keys?)Enum.Parse( typeof(Keys), tmp );
			}
		}
		
		public void AppsActivated()
		{
			// do nothing
		}
		
		public void AppsDeactivated()
		{
			// do nothing
		}
		
		public event EventHandler Activating;
		public event EventHandler Deactivating;
		
		public void Activate()
		{
			Activating?.Invoke( this, null );
		}
		
		public void Deactivate()
		{
			Deactivating?.Invoke( this, null );
		}
		
		public void Exit()
		{
			tmrKeyChecker.Enabled = false;
			if( Status )
			{
				RestoreHiddenWindows();
			}
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 操作
		////////////////////////////////////////////////////////////////
		
		public void Peek( int m, Keys k )
		{
			// Console.WriteLine( "Peek" );
			// MinimizeAll( m );
			// StartPeekDesktop();
			HideAllWindows();
			tmrKeyChecker.Enabled = true;
		}

		#endregion
		
		#region その他のイベント
		
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern short GetKeyState(int nVirtKey);
		
		private void tmrKeyChecker_Tick(object sender, EventArgs e)
		{
			if( Status == false )
			{
				CheckToEnter();
			}
			else
			{
				CheckToRelease();
			}
		}
		
		private void CheckToEnter()
		{
			if( ( GetKeyState( (int)PrimaryKey ) < 0 ) &&
			    ( ( SecondaryKey == null ) || ( GetKeyState( (int)SecondaryKey ) < 0 ) ) )
			{
				if( ++Count > WaitTime )
				{
					HideAllWindows();
					Status = true;
					Count = 0;
				}
			}
			else
			{
				Count = 0;
			}
		}
		
		private void CheckToRelease()
		{
			// Console.WriteLine( $"lwin = {GetKeyState( (int)Keys.LWin )}" );
			if( GetKeyState( (int)PrimaryKey ) >= 0 )
			{
				// Console.WriteLine( "Restore" );
				// RestoreMiminize();
				// EndPeekDesktop();
				RestoreHiddenWindows();
				// tmrKeyChecker.Enabled = false;
				Status = false;
				// keybd_event( (byte)Keys.Escape, 0, 0, (UIntPtr)0 );
				// keybd_event( (byte)Keys.Escape, 0, 2, (UIntPtr)0 );
			}
		}
		
		#endregion
		
		#region 他のウィンドウを操作
		
		// 手動でウィンドウを全て検索して、表示中のウィンドウを手動で隠す
		#if true
		
		private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private extern static bool EnumWindows( EnumWindowsDelegate callback, IntPtr lparam);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private extern static bool IsIconic( IntPtr hWnd );
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private extern static bool IsWindowVisible( IntPtr hWnd );
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private extern static bool IsWindowEnabled( IntPtr hWnd );
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private extern static bool IsWindow( IntPtr hWnd );
		
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int ShowWindow( IntPtr hWnd, int nCmdShow );
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr GetDesktopWindow();
		
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindWindow( string lpClassName, string lpWindowName );
		
		[DllImport("user32.dll")]
		private static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
		
		private List<IntPtr> MinimizedList;
		
		private List<IntPtr> EnumWindowHandles()
		{
			var progman = FindWindow( "Progman", null );
			var desktop = GetDesktopWindow();
			var taskbar = FindWindow( "Shell_traywnd", null );
			
			// PrintWindowStatus( progman );
			// PrintWindowStatus( desktop );
			// PrintWindowStatus( taskbar );
			
			var ret = new List<IntPtr>();
			EnumWindows(
				(h,lp) => {
					if( IsWindow(h) &&
					    IsWindowEnabled(h) &&
					    IsWindowVisible(h) &&
					    ( IsIconic(h) == false ) &&
					    ( h != progman ) &&
					    ( h != desktop ) &&
					    ( h != taskbar ) )
					{
						ret.Add( h );
						// PrintWindowStatus( h );
					}
					return true;
				},
				IntPtr.Zero
			);
			
			return ret;
		}
		
		private void PrintWindowStatus( IntPtr h )
		{
			Console.WriteLine( $"{h} {GetWindowText(h)} / {GetClassName(h)} : Iconic={IsIconic(h)} Visible={IsWindowVisible(h)} Enabled={IsWindowEnabled(h)} Win={IsWindow(h)}" );
		}
		
		private string GetWindowText( IntPtr h )
		{
			var sb = new StringBuilder();
			GetWindowText( h, sb, sb.Capacity );
			return sb.ToString();
		}
		
		private string GetClassName( IntPtr h )
		{
			var sb = new StringBuilder();
			GetClassName( h, sb, sb.Capacity );
			return sb.ToString();
		}
		
		private void HideAllWindows()
		{
			if( ( MinimizedList != null ) && ( MinimizedList.Count > 0 ) )
			{
				RestoreHiddenWindows();
			}
			
			MinimizedList = EnumWindowHandles();
			MinimizedList.ForEach( w => ShowWindow( w, 0 ) );
			MinimizedList.Reverse();
		}
		
		private void RestoreHiddenWindows()
		{
			if( MinimizedList != null )
			{
				MinimizedList.ForEach( w => ShowWindow( w, 5 ) );
				MinimizedList.Clear();
			}
		}
		
		#endif
		
		// Win+M のキーを送り、強引に一時的に最小化
		// 強制TopMostみたいになるウィンドウが出現したりで、都合がよろしくない場合が
		#if false
		[DllImport("user32.dll")]
		private static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
		
		private static void MinimizeAll( int m )
		{
			keybd_event( (byte)Keys.LMenu,       0, 2, (UIntPtr)0 );
			keybd_event( (byte)Keys.RMenu,       0, 2, (UIntPtr)0 );
			keybd_event( (byte)Keys.Space,       0, 2, (UIntPtr)0 );
			
			keybd_event( (byte)Keys.LWin,        0, 0, (UIntPtr)0 );
			keybd_event( (byte)Keys.M,           0, 0, (UIntPtr)0 );
			keybd_event( (byte)Keys.M,           0, 2, (UIntPtr)0 );
			// keybd_event( (byte)Keys.LWin,        0, 2, (UIntPtr)0 );
		}
		
		private static void RestoreMiminize()
		{
			keybd_event( (byte)Keys.LShiftKey, 0, 0, (UIntPtr)0 );
			keybd_event( (byte)Keys.LWin,      0, 0, (UIntPtr)0 );
			keybd_event( (byte)Keys.M,         0, 0, (UIntPtr)0 );
			keybd_event( (byte)Keys.M,         0, 2, (UIntPtr)0 );
			keybd_event( (byte)Keys.LWin,      0, 2, (UIntPtr)0 );
			keybd_event( (byte)Keys.LShiftKey, 0, 2, (UIntPtr)0 );
		}
		#endif
		
		// 実験コード
		// x64 でないと動かない
		// AeroPeekでは、透明化したウィンドウの上からデスクトップを操作できない
		#if false
		public enum PeekTypes : long
		{
			NotUsed = 0,
			Desktop = 1,
			Window = 3
		}
		
		[DllImport("dwmapi.dll", EntryPoint = "#113", SetLastError = true)]
		internal static extern uint DwmpActivateLivePreview( bool peekOn, IntPtr hPeekWindow, IntPtr hTopmostWindow, uint peekType1or3, IntPtr newForWin10 );
		// internal static extern uint DwmpActivateLivePreview( uint sw, IntPtr Handle, IntPtr Caller, uint Method );
		
		public void StartPeekDesktop()
		{
			DwmpActivateLivePreview( true, Parent.Handle, Parent.Handle, 1, IntPtr.Zero );
			// DwmpActivateLivePreview( 1, Parent.Handle, Parent.Handle, 1 );
		}
		
		public void EndPeekDesktop()
		{
			DwmpActivateLivePreview( false, Parent.Handle, Parent.Handle, 1, IntPtr.Zero );
			// DwmpActivateLivePreview( 0, Parent.Handle, Parent.Handle, 1 );
		}
		#endif
		
		#endregion
	}
}
