using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;

namespace Misc
{
	/***************************************************************
	
		Class Name :    ExtendedMethods
		Extends    :    None
		Interfaces :    None
		
		Purpose
			拡張メソッド用
	
	***************************************************************/
	public static class ExtendedMethods
	{
		// ラムダ式でInvokeするための拡張メソッド
		// 通常通りにInvokeを記述し、ラムダ式で書いた場合にこちらが呼び出される
		// ついでに、Invokeが必要ない場合は通常呼び出しする。
		// (例)  foo.Invoke( () => foo.Text = "Invoked!" );
		public static void Invoke( this Control c, Action act )
		{
			if( c.IsHandleCreated )
			{
				if( c.InvokeRequired )
				{
					c.Invoke( (MethodInvoker)( () => act() ) );
				}
				else
				{
					act();
				}
			}
		}
		
		public static bool IsIn<T>( this T self, T min, T max, Range rangeMode = Range.Inclusive ) where T : IComparable
		{
			bool ret = false;
			
			if( rangeMode == Range.Exclusive )
			{
				if( ( self.CompareTo( min ) > 0 ) && ( self.CompareTo( max ) < 0 ) )
				{
					ret = true;
				}
			}
			else
			{
				if( ( self.CompareTo( min ) >= 0 ) && ( self.CompareTo( max ) <= 0 ) )
				{
					ret = true;
				}
			}
			
			return ret;
		}
		
		public static bool Not( this bool c ) => !c;
	}
	
	public enum Range
	{
		Inclusive,
		Exclusive,
	}
	
	public static class XmlSaver
	{
		////////////////////////////////////////////////////////////////
		#region xmlファイルアクセス
		////////////////////////////////////////////////////////////////
		
		/***************************************************************
			Name        Save<T>
			Params      string dirName 
			            string baseFile
			            T obj 
			RetCode     
			
			Purpose     ステータスデータの保存
			            Generic型 T には、対応するList<Item派生>を指定する
		***************************************************************/
		public static void Save<T>( string dirName, string baseFile, T obj )
		{
			string fileName = dirName + "\\" + baseFile;
			
			if( !Directory.Exists( dirName ) )
			{
				Directory.CreateDirectory( dirName );
			}
			
			var serializer = new System.Xml.Serialization.XmlSerializer( typeof( T ) );
			
			using( var sw = new System.IO.StreamWriter(
					fileName, false, new System.Text.UTF8Encoding( false ) ) )
			{
				serializer.Serialize( sw, obj );
			}
		}
		
		/***************************************************************
			Name        Load<T>
			Params      string dirName 
			            string baseFile
			RetCode     生成したインスタンス
			
			Purpose     ステータスデータの読み込み
			            Generic型 T には、対応するList<Item派生>を指定する
		***************************************************************/
		public static T Load<T>( string dirName, string baseFile ) where T : class, new()
		{
			string fileName = dirName + "\\" + baseFile;
			
			T ret = null;
			
			XmlDocument doc = new XmlDocument();
			doc.PreserveWhitespace = true;
			doc.Load(fileName);
			
			try
			{
				using( var reader = new XmlNodeReader(doc.DocumentElement) )
				{
					var serializer = new XmlSerializer( typeof(T) );
					ret = (T)serializer.Deserialize(reader);
				}
			}
			catch
			{
				ret = new T();
				Save<T>( dirName, baseFile, ret );
			}
			
			return ret;
		}
		
		#endregion
	}
	
	public static class Misc
	{
		////////////////////////////////////////////////////////////////
		#region コンソール表示関係
		////////////////////////////////////////////////////////////////
		
		private const UInt32 StdOutputHandle = 0xFFFFFFF5;
		
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
		
		[DllImport("kernel32.dll")]
		private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);
		
		[DllImport("kernel32")]
		private static extern bool AllocConsole();
		
		[DllImport("kernel32")]
		private static extern bool FreeConsole();
		
		public static void ShowConsole()
		{
			AllocConsole();
			
			IntPtr defaultStdout = new IntPtr(7);
			IntPtr currentStdout = GetStdHandle(StdOutputHandle);
			
			if( currentStdout != defaultStdout )
			{
				// reset stdout
				SetStdHandle( StdOutputHandle, defaultStdout );
			}
			
			// reopen stdout
			TextWriter writer =
				new StreamWriter( Console.OpenStandardOutput(), System.Text.Encoding.GetEncoding("shift_jis") ) {
					AutoFlush = true
				};
			
			Console.SetOut( writer );
		}
		
		public static void HideConsole()
		{
			FreeConsole();
		}
		
		#endregion
	}
	
	public static class KeyCode
	{
		public const int MOD_ALT     = 0x0001;
		public const int MOD_CONTROL = 0x0002;
		public const int MOD_SHIFT   = 0x0004;
		public const int MOD_WIN     = 0x0008;
		
		public const int MOD_CONTROL_ALT   = MOD_CONTROL | MOD_ALT;
		public const int MOD_CONTROL_SHIFT = MOD_CONTROL | MOD_SHIFT;
		public const int MOD_CONTROL_WIN   = MOD_CONTROL | MOD_WIN;
		
		public const int MOD_SHIFT_ALT   = MOD_SHIFT | MOD_ALT;
		public const int MOD_SHIFT_CTRL  = MOD_SHIFT | MOD_CONTROL;
		public const int MOD_SHIFT_WIN   = MOD_SHIFT | MOD_WIN;
		
		public const int MOD_WIN_ALT   = MOD_WIN | MOD_ALT;
		public const int MOD_WIN_CTRL  = MOD_WIN | MOD_CONTROL;
		public const int MOD_WIN_SHIFT = MOD_WIN | MOD_SHIFT;
		
		public const int MOD_CONTROL_SHIFT_WIN = MOD_CONTROL | MOD_SHIFT | MOD_WIN;
		public const int MOD_ALT_SHIFT_WIN     = MOD_ALT     | MOD_SHIFT | MOD_WIN;
		public const int MOD_CONTROL_ALT_WIN   = MOD_CONTROL | MOD_ALT   | MOD_WIN;
		public const int MOD_CONTROL_SHIFT_ALT = MOD_CONTROL | MOD_SHIFT | MOD_ALT;
		
		////////////////////////////////////////////////////////////////
		#region キーコード変換
		////////////////////////////////////////////////////////////////
		
		public static Keys? Str2Keys( string str )
		{
			Keys? ret = null;
			
			string tmp = str.ToUpper();
			
			ret = ( tmp != "A"                  ) ? ret : Keys.A;
			ret = ( tmp != "ADD"                ) ? ret : Keys.Add;
			ret = ( tmp != "ALT"                ) ? ret : Keys.Alt;
			ret = ( tmp != "APPS"               ) ? ret : Keys.Apps;
			ret = ( tmp != "ATTN"               ) ? ret : Keys.Attn;
			ret = ( tmp != "B"                  ) ? ret : Keys.B;
			ret = ( tmp != "BACK"               ) ? ret : Keys.Back;
			ret = ( tmp != "BROWSERBACK"        ) ? ret : Keys.BrowserBack;
			ret = ( tmp != "BROWSERFAVORITES"   ) ? ret : Keys.BrowserFavorites;
			ret = ( tmp != "BROWSERFORWARD"     ) ? ret : Keys.BrowserForward;
			ret = ( tmp != "BROWSERHOME"        ) ? ret : Keys.BrowserHome;
			ret = ( tmp != "BROWSERREFRESH"     ) ? ret : Keys.BrowserRefresh;
			ret = ( tmp != "BROWSERSEARCH"      ) ? ret : Keys.BrowserSearch;
			ret = ( tmp != "BROWSERSTOP"        ) ? ret : Keys.BrowserStop;
			ret = ( tmp != "C"                  ) ? ret : Keys.C;
			ret = ( tmp != "CANCEL"             ) ? ret : Keys.Cancel;
			ret = ( tmp != "CAPITAL"            ) ? ret : Keys.Capital;
			ret = ( tmp != "CAPSLOCK"           ) ? ret : Keys.CapsLock;
			ret = ( tmp != "CLEAR"              ) ? ret : Keys.Clear;
			ret = ( tmp != "CONTROL"            ) ? ret : Keys.Control;
			ret = ( tmp != "CONTROLKEY"         ) ? ret : Keys.ControlKey;
			ret = ( tmp != "CRSEL"              ) ? ret : Keys.Crsel;
			ret = ( tmp != "D"                  ) ? ret : Keys.D;
			ret = ( tmp != "D0"                 ) ? ret : Keys.D0;
			ret = ( tmp != "D1"                 ) ? ret : Keys.D1;
			ret = ( tmp != "D2"                 ) ? ret : Keys.D2;
			ret = ( tmp != "D3"                 ) ? ret : Keys.D3;
			ret = ( tmp != "D4"                 ) ? ret : Keys.D4;
			ret = ( tmp != "D5"                 ) ? ret : Keys.D5;
			ret = ( tmp != "D6"                 ) ? ret : Keys.D6;
			ret = ( tmp != "D7"                 ) ? ret : Keys.D7;
			ret = ( tmp != "D8"                 ) ? ret : Keys.D8;
			ret = ( tmp != "D9"                 ) ? ret : Keys.D9;
			ret = ( tmp != "0"                  ) ? ret : Keys.D0;
			ret = ( tmp != "1"                  ) ? ret : Keys.D1;
			ret = ( tmp != "2"                  ) ? ret : Keys.D2;
			ret = ( tmp != "3"                  ) ? ret : Keys.D3;
			ret = ( tmp != "4"                  ) ? ret : Keys.D4;
			ret = ( tmp != "5"                  ) ? ret : Keys.D5;
			ret = ( tmp != "6"                  ) ? ret : Keys.D6;
			ret = ( tmp != "7"                  ) ? ret : Keys.D7;
			ret = ( tmp != "8"                  ) ? ret : Keys.D8;
			ret = ( tmp != "9"                  ) ? ret : Keys.D9;
			ret = ( tmp != "DECIMAL"            ) ? ret : Keys.Decimal;
			ret = ( tmp != "DELETE"             ) ? ret : Keys.Delete;
			ret = ( tmp != "DIVIDE"             ) ? ret : Keys.Divide;
			ret = ( tmp != "DOWN"               ) ? ret : Keys.Down;
			ret = ( tmp != "E"                  ) ? ret : Keys.E;
			ret = ( tmp != "END"                ) ? ret : Keys.End;
			ret = ( tmp != "ENTER"              ) ? ret : Keys.Enter;
			ret = ( tmp != "ERASEEOF"           ) ? ret : Keys.EraseEof;
			ret = ( tmp != "ESCAPE"             ) ? ret : Keys.Escape;
			ret = ( tmp != "EXECUTE"            ) ? ret : Keys.Execute;
			ret = ( tmp != "EXSEL"              ) ? ret : Keys.Exsel;
			ret = ( tmp != "F"                  ) ? ret : Keys.F;
			ret = ( tmp != "F1"                 ) ? ret : Keys.F1;
			ret = ( tmp != "F10"                ) ? ret : Keys.F10;
			ret = ( tmp != "F11"                ) ? ret : Keys.F11;
			ret = ( tmp != "F12"                ) ? ret : Keys.F12;
			ret = ( tmp != "F13"                ) ? ret : Keys.F13;
			ret = ( tmp != "F14"                ) ? ret : Keys.F14;
			ret = ( tmp != "F15"                ) ? ret : Keys.F15;
			ret = ( tmp != "F16"                ) ? ret : Keys.F16;
			ret = ( tmp != "F17"                ) ? ret : Keys.F17;
			ret = ( tmp != "F18"                ) ? ret : Keys.F18;
			ret = ( tmp != "F19"                ) ? ret : Keys.F19;
			ret = ( tmp != "F2"                 ) ? ret : Keys.F2;
			ret = ( tmp != "F20"                ) ? ret : Keys.F20;
			ret = ( tmp != "F21"                ) ? ret : Keys.F21;
			ret = ( tmp != "F22"                ) ? ret : Keys.F22;
			ret = ( tmp != "F23"                ) ? ret : Keys.F23;
			ret = ( tmp != "F24"                ) ? ret : Keys.F24;
			ret = ( tmp != "F3"                 ) ? ret : Keys.F3;
			ret = ( tmp != "F4"                 ) ? ret : Keys.F4;
			ret = ( tmp != "F5"                 ) ? ret : Keys.F5;
			ret = ( tmp != "F6"                 ) ? ret : Keys.F6;
			ret = ( tmp != "F7"                 ) ? ret : Keys.F7;
			ret = ( tmp != "F8"                 ) ? ret : Keys.F8;
			ret = ( tmp != "F9"                 ) ? ret : Keys.F9;
			ret = ( tmp != "FINALMODE"          ) ? ret : Keys.FinalMode;
			ret = ( tmp != "G"                  ) ? ret : Keys.G;
			ret = ( tmp != "H"                  ) ? ret : Keys.H;
			ret = ( tmp != "HANGUELMODE"        ) ? ret : Keys.HanguelMode;
			ret = ( tmp != "HANGULMODE"         ) ? ret : Keys.HangulMode;
			ret = ( tmp != "HANJAMODE"          ) ? ret : Keys.HanjaMode;
			ret = ( tmp != "HELP"               ) ? ret : Keys.Help;
			ret = ( tmp != "HOME"               ) ? ret : Keys.Home;
			ret = ( tmp != "I"                  ) ? ret : Keys.I;
			ret = ( tmp != "IMEACCEPT"          ) ? ret : Keys.IMEAccept;
			ret = ( tmp != "IMEACEEPT"          ) ? ret : Keys.IMEAceept;
			ret = ( tmp != "IMECONVERT"         ) ? ret : Keys.IMEConvert;
			ret = ( tmp != "IMEMODECHANGE"      ) ? ret : Keys.IMEModeChange;
			ret = ( tmp != "IMENONCONVERT"      ) ? ret : Keys.IMENonconvert;
			ret = ( tmp != "INSERT"             ) ? ret : Keys.Insert;
			ret = ( tmp != "J"                  ) ? ret : Keys.J;
			ret = ( tmp != "JUNJAMODE"          ) ? ret : Keys.JunjaMode;
			ret = ( tmp != "K"                  ) ? ret : Keys.K;
			ret = ( tmp != "KANAMODE"           ) ? ret : Keys.KanaMode;
			ret = ( tmp != "KANJIMODE"          ) ? ret : Keys.KanjiMode;
			ret = ( tmp != "KEYCODE"            ) ? ret : Keys.KeyCode;
			ret = ( tmp != "L"                  ) ? ret : Keys.L;
			ret = ( tmp != "LAUNCHAPPLICATION1" ) ? ret : Keys.LaunchApplication1;
			ret = ( tmp != "LAUNCHAPPLICATION2" ) ? ret : Keys.LaunchApplication2;
			ret = ( tmp != "LAUNCHMAIL"         ) ? ret : Keys.LaunchMail;
			ret = ( tmp != "LBUTTON"            ) ? ret : Keys.LButton;
			ret = ( tmp != "LCONTROLKEY"        ) ? ret : Keys.LControlKey;
			ret = ( tmp != "LEFT"               ) ? ret : Keys.Left;
			ret = ( tmp != "LINEFEED"           ) ? ret : Keys.LineFeed;
			ret = ( tmp != "LMENU"              ) ? ret : Keys.LMenu;
			ret = ( tmp != "LSHIFTKEY"          ) ? ret : Keys.LShiftKey;
			ret = ( tmp != "LWIN"               ) ? ret : Keys.LWin;
			ret = ( tmp != "M"                  ) ? ret : Keys.M;
			ret = ( tmp != "MBUTTON"            ) ? ret : Keys.MButton;
			ret = ( tmp != "MEDIANEXTTRACK"     ) ? ret : Keys.MediaNextTrack;
			ret = ( tmp != "MEDIAPLAYPAUSE"     ) ? ret : Keys.MediaPlayPause;
			ret = ( tmp != "MEDIAPREVIOUSTRACK" ) ? ret : Keys.MediaPreviousTrack;
			ret = ( tmp != "MEDIASTOP"          ) ? ret : Keys.MediaStop;
			ret = ( tmp != "MENU"               ) ? ret : Keys.Menu;
			ret = ( tmp != "MODIFIERS"          ) ? ret : Keys.Modifiers;
			ret = ( tmp != "MULTIPLY"           ) ? ret : Keys.Multiply;
			ret = ( tmp != "N"                  ) ? ret : Keys.N;
			ret = ( tmp != "NEXT"               ) ? ret : Keys.Next;
			ret = ( tmp != "NONAME"             ) ? ret : Keys.NoName;
			ret = ( tmp != "NONE"               ) ? ret : Keys.None;
			ret = ( tmp != "NUMLOCK"            ) ? ret : Keys.NumLock;
			ret = ( tmp != "NUMPAD0"            ) ? ret : Keys.NumPad0;
			ret = ( tmp != "NUMPAD1"            ) ? ret : Keys.NumPad1;
			ret = ( tmp != "NUMPAD2"            ) ? ret : Keys.NumPad2;
			ret = ( tmp != "NUMPAD3"            ) ? ret : Keys.NumPad3;
			ret = ( tmp != "NUMPAD4"            ) ? ret : Keys.NumPad4;
			ret = ( tmp != "NUMPAD5"            ) ? ret : Keys.NumPad5;
			ret = ( tmp != "NUMPAD6"            ) ? ret : Keys.NumPad6;
			ret = ( tmp != "NUMPAD7"            ) ? ret : Keys.NumPad7;
			ret = ( tmp != "NUMPAD8"            ) ? ret : Keys.NumPad8;
			ret = ( tmp != "NUMPAD9"            ) ? ret : Keys.NumPad9;
			ret = ( tmp != "O"                  ) ? ret : Keys.O;
			ret = ( tmp != "OEM1"               ) ? ret : Keys.Oem1;
			ret = ( tmp != "OEM102"             ) ? ret : Keys.Oem102;
			ret = ( tmp != "OEM2"               ) ? ret : Keys.Oem2;
			ret = ( tmp != "OEM3"               ) ? ret : Keys.Oem3;
			ret = ( tmp != "OEM4"               ) ? ret : Keys.Oem4;
			ret = ( tmp != "OEM5"               ) ? ret : Keys.Oem5;
			ret = ( tmp != "OEM6"               ) ? ret : Keys.Oem6;
			ret = ( tmp != "OEM7"               ) ? ret : Keys.Oem7;
			ret = ( tmp != "OEM8"               ) ? ret : Keys.Oem8;
			ret = ( tmp != "OEMBACKSLASH"       ) ? ret : Keys.OemBackslash;
			ret = ( tmp != "OEMCLEAR"           ) ? ret : Keys.OemClear;
			ret = ( tmp != "OEMCLOSEBRACKETS"   ) ? ret : Keys.OemCloseBrackets;
			ret = ( tmp != "OEMCOMMA"           ) ? ret : Keys.Oemcomma;
			ret = ( tmp != "OEMMINUS"           ) ? ret : Keys.OemMinus;
			ret = ( tmp != "OEMOPENBRACKETS"    ) ? ret : Keys.OemOpenBrackets;
			ret = ( tmp != "OEMPERIOD"          ) ? ret : Keys.OemPeriod;
			ret = ( tmp != "OEMPIPE"            ) ? ret : Keys.OemPipe;
			ret = ( tmp != "OEMPLUS"            ) ? ret : Keys.Oemplus;
			ret = ( tmp != "OEMQUESTION"        ) ? ret : Keys.OemQuestion;
			ret = ( tmp != "OEMQUOTES"          ) ? ret : Keys.OemQuotes;
			ret = ( tmp != "OEMSEMICOLON"       ) ? ret : Keys.OemSemicolon;
			ret = ( tmp != "OEMTILDE"           ) ? ret : Keys.Oemtilde;
			ret = ( tmp != "P"                  ) ? ret : Keys.P;
			ret = ( tmp != "PA1"                ) ? ret : Keys.Pa1;
			ret = ( tmp != "PACKET"             ) ? ret : Keys.Packet;
			ret = ( tmp != "PAGEDOWN"           ) ? ret : Keys.PageDown;
			ret = ( tmp != "PAGEUP"             ) ? ret : Keys.PageUp;
			ret = ( tmp != "PAUSE"              ) ? ret : Keys.Pause;
			ret = ( tmp != "PLAY"               ) ? ret : Keys.Play;
			ret = ( tmp != "PRINT"              ) ? ret : Keys.Print;
			ret = ( tmp != "PRINTSCREEN"        ) ? ret : Keys.PrintScreen;
			ret = ( tmp != "PRIOR"              ) ? ret : Keys.Prior;
			ret = ( tmp != "PROCESSKEY"         ) ? ret : Keys.ProcessKey;
			ret = ( tmp != "Q"                  ) ? ret : Keys.Q;
			ret = ( tmp != "R"                  ) ? ret : Keys.R;
			ret = ( tmp != "RBUTTON"            ) ? ret : Keys.RButton;
			ret = ( tmp != "RCONTROLKEY"        ) ? ret : Keys.RControlKey;
			ret = ( tmp != "RETURN"             ) ? ret : Keys.Return;
			ret = ( tmp != "RIGHT"              ) ? ret : Keys.Right;
			ret = ( tmp != "RMENU"              ) ? ret : Keys.RMenu;
			ret = ( tmp != "RSHIFTKEY"          ) ? ret : Keys.RShiftKey;
			ret = ( tmp != "RWIN"               ) ? ret : Keys.RWin;
			ret = ( tmp != "S"                  ) ? ret : Keys.S;
			ret = ( tmp != "SCROLL"             ) ? ret : Keys.Scroll;
			ret = ( tmp != "SELECT"             ) ? ret : Keys.Select;
			ret = ( tmp != "SELECTMEDIA"        ) ? ret : Keys.SelectMedia;
			ret = ( tmp != "SEPARATOR"          ) ? ret : Keys.Separator;
			ret = ( tmp != "SHIFT"              ) ? ret : Keys.Shift;
			ret = ( tmp != "SHIFTKEY"           ) ? ret : Keys.ShiftKey;
			ret = ( tmp != "SLEEP"              ) ? ret : Keys.Sleep;
			ret = ( tmp != "SNAPSHOT"           ) ? ret : Keys.Snapshot;
			ret = ( tmp != "SPACE"              ) ? ret : Keys.Space;
			ret = ( tmp != "SUBTRACT"           ) ? ret : Keys.Subtract;
			ret = ( tmp != "T"                  ) ? ret : Keys.T;
			ret = ( tmp != "TAB"                ) ? ret : Keys.Tab;
			ret = ( tmp != "U"                  ) ? ret : Keys.U;
			ret = ( tmp != "UP"                 ) ? ret : Keys.Up;
			ret = ( tmp != "V"                  ) ? ret : Keys.V;
			ret = ( tmp != "VOLUMEDOWN"         ) ? ret : Keys.VolumeDown;
			ret = ( tmp != "VOLUMEMUTE"         ) ? ret : Keys.VolumeMute;
			ret = ( tmp != "VOLUMEUP"           ) ? ret : Keys.VolumeUp;
			ret = ( tmp != "W"                  ) ? ret : Keys.W;
			ret = ( tmp != "X"                  ) ? ret : Keys.X;
			ret = ( tmp != "XBUTTON1"           ) ? ret : Keys.XButton1;
			ret = ( tmp != "XBUTTON2"           ) ? ret : Keys.XButton2;
			ret = ( tmp != "Y"                  ) ? ret : Keys.Y;
			ret = ( tmp != "Z"                  ) ? ret : Keys.Z;
			ret = ( tmp != "ZOOM"               ) ? ret : Keys.Zoom;
			
			return ret;
		}
		
		public static int Str2ModKey( string str )
		{
			int ret = 0;
			
			switch( str.ToUpper() )
			{
			case "CTRL":
			case "CONTROL":
				ret = MOD_CONTROL;
				break;
			
			case "SHIFT":
				ret = MOD_SHIFT;
				break;
			
			case "ALT":
				ret = MOD_ALT;
				break;
			
			case "WIN":
			case "CMD":
			case "COMMAND":
				ret = MOD_WIN;
				break;
			
			default:
				// 不正な定義
				ret = 0;
				break;
			}
			
			return ret;
		}
		
		#endregion
	}
	
	public class Inifile
	{
		////////////////////////////////////////////////////////////////
		#region 定数
		////////////////////////////////////////////////////////////////
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region WindowsAPIのためのDLL
		////////////////////////////////////////////////////////////////
		
		[DllImport("KERNEL32.DLL")]
		public static extern uint WritePrivateProfileString(
			string lpAppName,
			string lpKeyName,
			string lpString,
			string lpFileName );
		
		[DllImport("KERNEL32.DLL")]
		public static extern uint GetPrivateProfileString(
			string lpAppName,
			string lpKeyName,
			string lpDefault,
			StringBuilder lpReturnedString,
			uint nSize,
			string lpFileName );
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region プロパティ
		////////////////////////////////////////////////////////////////
		
		public string FileName { get; set; }
		public int    WorkSize { get; set; }
		
		public bool Exists {
			get {
				return System.IO.File.Exists( FileName );
			}
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region コンストラクタ
		////////////////////////////////////////////////////////////////
		
		public Inifile( string fn )
		{
			FileName = fn;
			WorkSize = 1024;
		}
		
		public Inifile() : this( "" ) // : this( ... ) で、自身のオーバーロードを呼び出せる
		{
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 公開メソッド
		////////////////////////////////////////////////////////////////
		
		public void Write( string category, string key, object val )
		{
			// objectは、全ての型の基底クラス
			// .ToString はobjectクラスで定義されている
			// (このクラスも、実はこっそりobject派生になっている)
			// つまり、object型へはどんな型でも代入することが出来る。(値型でさえも！→boxing)
			WritePrivateProfileString( category, key, val.ToString(), FileName );
		}
		
		// 面倒な制約があるので、一旦stringで受ける
		public string Read( string category, string key, string def )
		{
			StringBuilder sb = new StringBuilder( WorkSize );
			
			GetPrivateProfileString(
				category,
				key,
				def,
				sb,
				Convert.ToUInt32( sb.Capacity ),
				FileName );
			
			string ret = sb.ToString();
			
			// GetPrivateProfileString で読むと、コメントが除去されない
			// 値に ; が必要なケースは無いであろう
			var tmp = ret.Split( ';' );
			
			return tmp.Length > 0 ? tmp[0].Trim() : ret;
		}
		
		public void Init( string category, string key, string def )
		{
			string data = Read( category, key, def );
			Write( category, key, data );
		}
		
		public IEnumerable<string> ReadMulti( string category, string key, string def, char sepa )
		{
			string full = Read( category, key, def );
			var list = full.Split( sepa );
			
			foreach( var val in list )
			{
				yield return val;
			}
		}
		
		public void Exec( string category, string key, string def, char sepa, Action<string> act )
		{
			foreach( var val in ReadMulti( category, key, def, sepa ) )
			{
				act( val );
			}
		}
		
		#endregion
	}
	
	public static class Const
	{
		////////////////////////////////////////////////////////////////
		#region 各種ファイル名
		////////////////////////////////////////////////////////////////
		
		public static string BasePath {
			get {
				return Application.CommonAppDataPath + "\\..";
			}
		}
		
		public static string AplicationPath {
			get {
				return
					System.IO.Path.GetDirectoryName(
						System.Reflection.Assembly.GetExecutingAssembly().Location
					);
			}
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 表記関係
		////////////////////////////////////////////////////////////////
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 定数
		////////////////////////////////////////////////////////////////
		
		// ログの保持ファイル数
		// 最小1、設計上は最大1000
		public const int MaxAppLog = 10;
		
		#endregion
	}
}
