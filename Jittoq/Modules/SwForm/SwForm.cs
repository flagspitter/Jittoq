using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Misc;

namespace Jittoq.SwForm
{
	public partial class SwForm : UserControl, IJittokModule
	{
		////////////////////////////////////////////////////////////////
		#region 初期化関係
		////////////////////////////////////////////////////////////////
		
		public SwForm()
		{
			InitializeComponent();
		}
		
		private void SwForm_Load(object sender, EventArgs e)
		{
			// OffsetTime = TimeSpan.Parse( "-00:00:03.00" );
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 計時用
		////////////////////////////////////////////////////////////////
		
		private Stopwatch sw = new Stopwatch();
		private List<TimeSpan> LapList = new List<TimeSpan>();
		
		private LapView frmLap = new LapView();
		
		private TimeSpan OffsetTime = new TimeSpan(0);
		private TimeSpan ZeroTime = new TimeSpan(0);
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region プロパティ
		////////////////////////////////////////////////////////////////
		
		public string IniCategory { get; } = "StopWatch";
		
		public Color SwForeColor { get; set; }
		public Color SwBackColor { get; set; }
		public Font  TextFont  { get; set; }
		
		public string Format { get; set; }
		
		private Form MainForm => (Form)(base.Parent);
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region インターフェースの実装
		////////////////////////////////////////////////////////////////
		
		public string ModuleName => "Stopwatch";
		public char   AcceleratorKey => 'S';
		public string Version => "0.1.0";
		
		public Dictionary<string,Hotkey.CallbackFunc> Controllers { get; } = new Dictionary<string,Hotkey.CallbackFunc>();
		public List<Control> CursorFixer { get; } = null;
		
		public void Initialize( Inifile ini )
		{
			SwForeColor = Common.LoadColorSetting( ini, IniCategory, "ForeResult", "White" );
			SwBackColor = Common.LoadColorSetting( ini, IniCategory, "BackResult", "DimGray" );
			
			btnLapView.ForeColor = SwForeColor;
			lbCounter.ForeColor  = SwForeColor;
			
			btnLapView.BackColor = SwBackColor;
			lbCounter.BackColor  = SwBackColor;
			panel1.BackColor     = SwBackColor;
			
			SetFont( btnLapView, "Font_Lap" );
			SetFont( lbCounter,  "Font_Timer" );
			
			Format    = EscapeTimeFormat( ini.Read( IniCategory, "Format", "hh:mm:ss.ff" ) );
			TextFont  = Common.ReadFontSetting( ini, IniCategory, "Font" );
			
			ini.Exec( IniCategory, "StartStopHide", "Ctrl+Alt+Q", ',', v => Controllers.Add( v.Trim(), hk_StartStopHide ) );
			ini.Exec( IniCategory, "StartStop",     "Ctrl+Alt+Z", ',', v => Controllers.Add( v.Trim(), hk_StartStop ) );
			ini.Exec( IniCategory, "Start",         "Ctrl+Alt+A", ',', v => Controllers.Add( v.Trim(), hk_Start ) );
			ini.Exec( IniCategory, "Stop",          "Ctrl+Alt+S", ',', v => Controllers.Add( v.Trim(), hk_Stop ) );
			ini.Exec( IniCategory, "Reset",         "Ctrl+Alt+X", ',', v => Controllers.Add( v.Trim(), hk_Reset ) );
			ini.Exec( IniCategory, "Lap",           "Ctrl+Alt+W", ',', v => Controllers.Add( v.Trim(), hk_Lap ) );
			ini.Exec( IniCategory, "ShowLap",       "Ctrl+Alt+L", ',', v => Controllers.Add( v.Trim(), hk_ShowLap ) );
			
			return;
			
			string EscapeTimeFormat( string src ) => src.Replace( ":", @"\:" ).Replace( ".", @"\." );
			
			void SetFont( Control target, string key )
			{
				try
				{
					var f = Common.ReadFontSetting( ini, IniCategory, key );
					if( f != null )
					{
						target.Font = f;
					}
				}
				catch( ApplicationException )
				{
					target.Text = "Font Error";
				}
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
			Start();
		}
		
		public void Deactivate()
		{
			// do nothing
		}
		
		public void Exit()
		{
			
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 操作
		////////////////////////////////////////////////////////////////
		
		private void hk_StartStopHide( int m, Keys k ) => StartStopHide();
		private void hk_StartStop    ( int m, Keys k ) => StartOrStop();
		private void hk_Start        ( int m, Keys k ) => Start();
		private void hk_Stop         ( int m, Keys k ) => Stop();
		private void hk_Reset        ( int m, Keys k ) => Reset();
		private void hk_Lap          ( int m, Keys k ) => SetLap();
		private void hk_ShowLap      ( int m, Keys k ) => ShowLap();

		private void btnStartStop_Click(object sender, EventArgs e)
		{
			StartOrStop();
		}

		private void btnLap_Click(object sender, EventArgs e)
		{
			SetLap();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			Reset();
		}

		private void btnLapView_Click(object sender, EventArgs e)
		{
			SetLap();
		}
		
		public void JustShow()
		{
			MainForm.Show();
			if( this.Visible == false )
			{
				this.Show();
				Activating?.Invoke( this, null );
			}
		}
		
		public void JustHide()
		{
			if( this.Visible )
			{
				this.Hide();
				Deactivating?.Invoke( this, null );
			}
		}
		
		public void StartStopHide()
		{
			if( this.Visible )
			{
				if( tmrInterval.Enabled )
				{
					Stop();
				}
				else
				{
					Reset();
					JustHide();
				}
			}
			else
			{
				Start();
			}
		}
		
		public void StartOrStop()
		{
			if( tmrInterval.Enabled )
			{
				Stop();
			}
			else
			{
				Start();
			}
		}
		
		public void Start()
		{
			sw.Start();
			JustShow();
			tmrInterval.Enabled = true;
			btnStartStop.Text = "STOP";
			UpdateCounter();
		}
		
		public void Stop()
		{
			sw.Stop();
			tmrInterval.Enabled = false;
			btnStartStop.Text = "START";
			UpdateCounter();
		}
		
		public void Reset()
		{
			sw.Reset();
			LapList.Clear();
			frmLap.Clear();
			
			if( tmrInterval.Enabled )
			{
				sw.Start();
			}
			
			ResetCounter();
		}
		
		public void SetLap()
		{
			if( tmrInterval.Enabled )
			{
				int last = LapList.Count;
				TimeSpan lap;
				TimeSpan cur = sw.Elapsed;
				LapList.Add( cur );
				
				if( last > 0 )
				{
					lap = cur - LapList[last - 1];
				}
				else
				{
					lap = cur;
				}
				
				frmLap.AddLap( LapList.Count, lap, cur );
				
				var sb = new StringBuilder();
				sb.Append( "[" );
				sb.Append( LapList.Count );
				sb.Append( "] LAP " );
				sb.Append( lap.ToString( Format ) );
				sb.Append( "  SPL " );
				sb.Append( cur.ToString( Format ) );
				btnLapView.Text = sb.ToString();
			}
		}
		
		public void ShowLap()
		{
			if( frmLap.Visible )
			{
				frmLap.Hide();
			}
			else
			{
				frmLap.Show();
			}
		}
		
		private void lbCounter_DoubleClick(object sender, EventArgs e)
		{
			if( tmrInterval.Enabled == false )
			{
				using( var frm = new SwOffset() )
				{
					if( frm.ShowDialog() == DialogResult.OK )
					{
						OffsetTime = frm.Value;
						Reset();
					}
				}
			}
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 計時用
		////////////////////////////////////////////////////////////////
		
		private void UpdateCounter()
		{
			TimeSpan curTime = sw.Elapsed;
			lbCounter.Text = ( (curTime+OffsetTime) >= ZeroTime ) ?
				(curTime+OffsetTime).ToString( Format ) :
				"-" + (curTime+OffsetTime).ToString( Format );
		}
		
		private void ResetCounter()
		{
			sw.Reset();
			UpdateCounter();
		}

		private void tmrInterval_Tick(object sender, EventArgs e)
		{
			UpdateCounter();
		}

		#endregion
	}
}
