using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Misc;

namespace Jittoq
{
	interface IJittokModule
	{
		void Initialize( Inifile ini ); // 最初に呼び出される。引数は設定ファイルへのアクセス用
		void AppsActivated();   // アプリ全体がアクティブになる時に呼び出される
		void AppsDeactivated(); // アプリ全体がアクティブでなくなる時に呼び出される
		void Activate();        // 当該モジュールをアプリ側からアクティブにする時に呼び出される
		void Deactivate();      // 当該モジュールをアプリ側から非アクティブにする時に呼び出される
		void Exit();            // アプリケーション自体終了する時に呼び出される
		
		// このモジュールの呼び出し方法を規定する
		// 呼び出しのためのGlobal hotkeyを示す文字列(例 "Ctrl+Alt+Enter") と
		// 対応するコールバック関数 void foo( int m, Keys k ) の組
		Dictionary<string,Hotkey.CallbackFunc> Controllers { get; }
		
		event EventHandler Activating;   // モジュールが自発的にアクティブになることをアプリ側に通知
		event EventHandler Deactivating; // モジュールが自発的に非アクティブになることをアプリ側に通知
		
		string ModuleName { get; }      // モジュールの名前
		char   AcceleratorKey  { get; } // タスクトレイからモジュールを呼び出すときのアクセラレータキー
		string Version { get; }         // モジュールのバージョン
		
		List<Control> CursorFixer { get; } // このコントロールの上ではマウスカーソルを変化させない
		
		// UserControlがすでに持っている
		// UserControlを継承しない場合は、何らかの形で実装しなければならない
		bool Visible { get; set; }
		int  Height  { get; set; }
		int  Width   { get; set; }
		void SendToBack();
		void BringToFront();
	}
}
