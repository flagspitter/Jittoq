using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Misc
{
	/***************************************************************
	
		Class Name :    Hotkey
		Extends    :    System.Windows.Forms.Control
		Interfaces :    None
		
		Purpose
			ホットキーの登録と押下確認するクラス
		
		Example:
			foo.Register( Hotkey.MOD_CONTROL | Hotkey.MOD_ALT, Keys.Q, (m,k) => bar );
			
			このような感じで登録する。
			この場合、これ以降、Ctrl+Alt+Q が押されると、(m,k) => bar が実行される。
			ここで m は修飾キーの状態、 k は押されたキーコードを示す。
			
			終了時は、 foo.UnregisterAll();
	
	***************************************************************/
	public class Hotkey : Control
	{
		////////////////////////////////////////////////////////////////
		#region 定数
		////////////////////////////////////////////////////////////////
		
		// メッセージ処理に使う
		private const int WM_HOTKEY = 0x0312;
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region WindowsAPIのためのDLL
		////////////////////////////////////////////////////////////////
		
		[DllImport("user32.dll")]
		extern static int RegisterHotKey(IntPtr HWnd, int ID, int MOD_KEY, int KEY);
		
		[DllImport("user32.dll")]
		extern static int UnregisterHotKey(IntPtr HWnd, int ID);
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 内外で使用する型
		////////////////////////////////////////////////////////////////
		
		private class RegList
		{
			public CallbackFunc Act;
			public int id;
			public int Mod;
			public Keys Key;
			
			public RegList( CallbackFunc a, int i, int m, Keys k )
			{
				Act = a;
				id = i;
				Mod = m;
				Key = k;
			}
		}
		
		public delegate void CallbackFunc( int m, Keys k );
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 内部変数
		////////////////////////////////////////////////////////////////
		
		private List<RegList> Callbacks = new List<RegList>();
		private int cnt;
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region 公開メソッド
		////////////////////////////////////////////////////////////////
		
		public int Register( int modKey, Keys key, CallbackFunc act )
		{
			int curId = cnt;
			RegisterHotKey( this.Handle, curId, modKey, (int)key );
			Callbacks.Add( new RegList( act, curId, modKey, key ) );
			cnt++;
			return curId;
		}
		
		public void UnregisterAll()
		{
			foreach( var tg in Callbacks )
			{
				UnregisterHotKey( this.Handle, tg.id );
			}
		}
		
		#endregion
		
		////////////////////////////////////////////////////////////////
		#region オーバーライドする関数
		////////////////////////////////////////////////////////////////
		
		protected override void WndProc( ref Message m )
		{
			base.WndProc( ref m );
			if( m.Msg == WM_HOTKEY )
			{
				int key = (int)m.WParam;
				RegList tg = Callbacks.Find( (x) => ( x.id == key ) );
				if( tg != null )
				{
					tg.Act( tg.Mod, tg.Key );
				}
			}
		}
		
		#endregion
		
		public Hotkey()
		{
			this.Disposed += (s,e) => UnregisterAll();
		}
	}
}
