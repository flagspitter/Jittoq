using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Misc;

// このモジュールは、１行メモ / 1TextBox で保管・表示・編集する
// キーボードから操作することで、コマンド実行できるようにする
//   \fg=foo 文字色を変更
//   \bg=foo 背景色を変更
//   \alm=datetime 指定日時にアラーム
// メモの内容は、終了時か編集時にシリアライズ
// シリアライズしない、フリーのTextBoxを１つ用意する（新規用）

namespace Jittoq.MemoForm
{
	public partial class MemoForm : UserControl, IJittokModule
	{
		public string IniCategory { get; } = "Memo";
		private string MemoFile;
		private TextBox CurrentBox;
		
		public class Memo
		{
			public string OriginalText = "";
			public string FgCommand = "";
			public string BgCommand = "";
			public string AlmCommand = "";
			
			[System.Xml.Serialization.XmlIgnore]
			public TextBox Ctrl = new TextBox();
		}
		
		public List<Memo> MemoList;
		
		private MainWindow MainForm => (MainWindow)(base.Parent);
		
		////////////////////////////////////////////////////////////////
		#region 設定可能なプロパティ
		////////////////////////////////////////////////////////////////
		
		public Color TextBackColor { get; set; } = Color.DimGray;
		public Color TextForeColor { get; set; } = Color.White;
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 初期化関係
		////////////////////////////////////////////////////////////////
		
		public MemoForm()
		{
			InitializeComponent();
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region インターフェースの実装
		////////////////////////////////////////////////////////////////
		
		public string ModuleName => "Memo";
		public char   AcceleratorKey => 'M';
		public string Version => "0.1.0";
		
		public Dictionary<string,Hotkey.CallbackFunc> Controllers { get; } = new Dictionary<string,Hotkey.CallbackFunc>();
		
		public List<Control> CursorFixer { get; } = new List<Control>();
		
		public void Initialize( Inifile ini )
		{
			MemoFile = ini.Read( IniCategory, "File", "memo.xml" );
			
			try
			{
				MemoList = XmlSaver.Load<List<Memo>>( Const.AplicationPath, MemoFile );
			}
			catch
			{
				MemoList = new List<Memo>();
			}
			
			int h = 0;
			foreach( var m in MemoList )
			{
				m.Ctrl = CreateTextBox();
				this.Controls.Add( m.Ctrl );
				m.Ctrl.BringToFront();
				m.Ctrl.Leave += MemoBoxLeaved;
				
				m.Ctrl.BackColor = TranslateColor( m.BgCommand, TextBackColor );
				m.Ctrl.ForeColor = TranslateColor( m.FgCommand, TextForeColor );
				
				m.Ctrl.Tag = m;
				m.Ctrl.Text = System.Net.WebUtility.HtmlDecode( m.OriginalText );
				h += m.Ctrl.Height;
			}
			
			this.Height = h;
			CreateNewBox();
			
			CurrentBox.Focus();
			
			ini.Exec( IniCategory, "Show", "Ctrl+Alt+M", ',', v => Controllers.Add( v.Trim(), hk_ShowHide ) );
		}
		
		private void CreateNewBox()
		{
			CurrentBox = CreateTextBox();
			CurrentBox.Text = "";
			this.Controls.Add( CurrentBox );
			CurrentBox.BringToFront();
			CurrentBox.Leave += CurrentBoxLeaved;
			
			this.Height += CurrentBox.Height;
			MainForm.Height += CurrentBox.Height;
		}
		
		public void AppsActivated()
		{
			// Do nothing
			var c = this.ActiveControl;
			if( c is TextBox )
			{
				((TextBox)c).BorderStyle = BorderStyle.Fixed3D;
			}
		}
		
		public void AppsDeactivated()
		{
			var c = this.ActiveControl;
			if( c == CurrentBox )
			{
				ProcessLeavingCurrent( (TextBox)c );
			}
			if( c is TextBox )
			{
				((TextBox)c).BorderStyle = BorderStyle.FixedSingle;
			}
			
			Exit(); // 終了するわけではないが、終了と同じ処理を行う
		}
		
		public event EventHandler Activating;
		public event EventHandler Deactivating;
		
		public void Activate()
		{
			MainForm.Show();
			MainForm.Activate();
			this.Show();
			Activating?.Invoke( this, null );
			CurrentBox.Focus();
		}
		
		public void Deactivate()
		{
			this.Hide();
			Deactivating?.Invoke( this, null );
		}
		
		public void Exit()
		{
			foreach( var m in MemoList )
			{
				m.OriginalText = System.Security.SecurityElement.Escape( m.Ctrl.Text );
			}
			
			XmlSaver.Save<List<Memo>>( Const.AplicationPath, MemoFile, MemoList );
			
			// Console.WriteLine( "memo : Saved" );
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 操作
		////////////////////////////////////////////////////////////////
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region テキストボックス関係の処理
		////////////////////////////////////////////////////////////////
		
		private TextBox CreateTextBox()
		{
			var ret = new TextBox();
			
			ret.Text = "";
			ret.BackColor = TextBackColor;
			ret.ForeColor = TextForeColor;
			ret.AutoSize = true;
			ret.Multiline = true;
			ret.WordWrap = true;
			ret.Dock = DockStyle.Top;
			ret.TextChanged += TextBox_TextChanged;
			ret.KeyPress += TextBox_KeyPress;
			ret.KeyDown += TextBox_KeyDown;
			ret.Enter += (s,e) => Reselect( (TextBox)s );
			ret.BorderStyle = BorderStyle.FixedSingle;
			
			return ret;
			
			void Reselect( TextBox t )
			{
				foreach( var m in MemoList )
				{
					m.Ctrl.BorderStyle = BorderStyle.FixedSingle;
				}
				CurrentBox.BorderStyle = BorderStyle.FixedSingle;
				
				t.BorderStyle = BorderStyle.Fixed3D;
				t.Focus();
				t.SelectionStart = t.Text.Length;
				t.SelectionLength = 0;
			}
		}
		
		private void TextBox_TextChanged( object sender, EventArgs e )
		{
			var self = (TextBox)sender;
			int lastSize = self.Height;
			
			self.Height = MeasureVertivalSize( self );
			
			int dif = self.Height - lastSize;
			this.Height += dif;
			MainForm.Height += dif;
		}
		
		private int MeasureVertivalSize( TextBox t )
		{
			int ret;
			
			string testString = "l";
			int baseHeihgt = TextRenderer.MeasureText( testString, t.Font ).Height;
			
			if( t.Text != "" )
			{
				int lines = 0;
				while( t.GetFirstCharIndexFromLine( lines ) >= 0 )
				{
					lines++;
				}
				
				int textHeihgt = ( lines == 0 ) ? baseHeihgt : lines * baseHeihgt;
				ret = textHeihgt + t.Margin.Vertical;
			}
			else
			{
				ret = baseHeihgt + t.Margin.Vertical;
			}
			
			return ret;
		}
		
		private void CurrentBoxLeaved( object sender, EventArgs e )
		{
			var self = (TextBox)sender;
			ProcessLeavingCurrent( self );
		}
		
		private void ProcessLeavingCurrent( TextBox t )
		{
			if( t.Text.Trim() != "" )
			{
				var newMemo = new Memo();
				newMemo.Ctrl = t;
				t.Tag = newMemo;
				t.Leave -= CurrentBoxLeaved;
				t.Leave += MemoBoxLeaved;
				MemoList.Add( newMemo );
				CreateNewBox();
			}
		}
		
		private void MemoBoxLeaved( object sender, EventArgs e )
		{
			var self = (TextBox)sender;
			ProcessLeavingMemoBox( self );
		}
		
		private void ProcessLeavingMemoBox( TextBox t )
		{
			if( t.Text.Trim() == "" )
			{
				this.Height -= t.Height;
				MainForm.Height -= t.Height;
				MemoList.Remove( (Memo)t.Tag );
				this.Controls.Remove( t );
			}
		}

		private void MemoForm_Leave(object sender, EventArgs e)
		{
			var c = this.ActiveControl;
			if( c is TextBox )
			{
				if( c == CurrentBox )
				{
					ProcessLeavingCurrent( (TextBox)c );
				}
				else
				{
					ProcessLeavingMemoBox( (TextBox)c );
				}
			}
		}
		
		private void TextBox_KeyPress( object sender, KeyPressEventArgs e )
		{
			var self = (TextBox)sender;
			
			if( ( self.Text != "" ) && ( self != CurrentBox ) && ( e.KeyChar == '\\' ) )
			{
				using( var f = new MemoCommand() )
				{
					f.StartPosition = FormStartPosition.Manual;
					f.Left = MainForm.Left + 8;
					f.Top = MainForm.Top + this.Top + self.Bottom - 2;
					f.Width = MainForm.Width - 8;
					f.TopMost = true;
					f.ShowDialog();
					
					if( f.DialogResult == DialogResult.OK )
					{
						ExecuteCommand( f.Command, self );
					}
				}
				self.Focus();
				e.Handled = true;
			}
		}
		
		private void ExecuteCommand( string cmd, TextBox t )
		{
			if( cmd.Trim() != "" )
			{
				var m = (Memo)t.Tag;
				// parse
				var argv = cmd.Split( '=' );
				
				switch( argv[0].Trim().ToLower() )
				{
				case "fg":
					m.FgCommand = GetArg( argv, 1 );
					m.Ctrl.ForeColor = TranslateColor( m.FgCommand, TextForeColor );
					break;
				
				case "bg":
					m.BgCommand = GetArg( argv, 1 );
					m.Ctrl.BackColor = TranslateColor( m.BgCommand, TextBackColor );
					break;
				
				case "normal":
					Template( m, "White", "DimGray" );
					break;
				
				case "warning":
					Template( m, "Black", "#ffff80" );
					break;
				
				case "notice":
					Template( m, "Black", "LightGreen" );
					break;
				
				case "fatal":
					Template( m, "Black", "pink" );
					break;
				
				case @"\":
					t.Text = t.Text.Insert( t.SelectionStart, @"\" );
					break;
				
				case "del":
					t.Text = "";
					// ProcessLeavingMemoBox( t );
					CurrentBox.Focus();
					break;
				
				default:
					break;
				}
				
			}
			
			return;
			
			string GetArg( string[] args, int index )
			{
				string ret = "";
				if( index < args.Length )
				{
					ret = args[index].Trim();
				}
				return ret;
			}
			
			void Template( Memo m, string fg, string bg )
			{
				m.FgCommand = fg;
				m.BgCommand = bg;
				m.Ctrl.ForeColor = TranslateColor( m.FgCommand, TextForeColor );
				m.Ctrl.BackColor = TranslateColor( m.BgCommand, TextBackColor );
			}
		}
		
		private static Color TranslateColor( string s, Color def )
		{
			Color ret;
			
			if( s == "" )
			{
				ret = def;
			}
			else
			{
				try
				{
					ret = ColorTranslator.FromHtml( s );
				}
				catch
				{
					ret = def;
				}
			}
			
			return ret;
		}
		
		private void TextBox_KeyDown( object sender, KeyEventArgs e )
		{
			var self = (TextBox)sender;
			
			if( e.KeyCode == Keys.Up )
			{
				if( MemoList.Count == 0 )
				{
					ProcessLeavingCurrent( CurrentBox );
				}
				else if( self == CurrentBox )
				{
					SelectControl( MemoList.Count - 1 );
				}
				else
				{
					SelectControl( MemoList.IndexOf( (Memo)ActiveControl.Tag ) - 1 );
				}
				e.Handled = true;
			}
			
			if( e.KeyCode == Keys.Down )
			{
				if( MemoList.Count == 0 )
				{
					ProcessLeavingCurrent( CurrentBox );
				}
				else if( self == CurrentBox )
				{
					SelectControl( 0 );
				}
				else
				{
					SelectControl( MemoList.IndexOf( (Memo)ActiveControl.Tag ) + 1 );
				}
				e.Handled = true;
			}
			
			// Console.WriteLine( $"KeyDown : {e.KeyCode}" );
			
			return;
			
			void SelectControl( int index )
			{
				if( ( index < 0 ) || ( index >= MemoList.Count ) )
				{
					// Currentを選択
					CurrentBox.Focus();
				}
				else
				{
					MemoList[ index ].Ctrl.Focus();
				}
			}
		}

		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 操作
		////////////////////////////////////////////////////////////////
		
		public void hk_ShowHide( int m, Keys k )
		{
			if( ( Form.ActiveForm != Parent ) || ( this.Visible == false ) )
			{
				hk_Show();
			}
			else
			{
				hk_Hide();
			}
		}
		
		public void hk_Show()
		{
			MainForm.Show();
			MainForm.Activate();
			this.Show();
			Activating?.Invoke( this, null );
			CurrentBox.Focus();
		}
		
		public void hk_Hide()
		{
			this.Hide();
			Deactivating?.Invoke( this, null );
		}
		
		#endregion
	}
}
