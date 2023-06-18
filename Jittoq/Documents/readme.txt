Jittoq - キーボードで即起動できる雑多ツール群

元々が自作の電卓とかを会社で使いたいために
体裁を少し整えてオープンソースにしてみただけのツールです。
大っぴらに宣伝はしませんが、
万一見つけた人はご自由にお使いください。ライセンスはWTFPLです。
	http://www.wtfpl.net/
	https://ja.wikipedia.org/wiki/WTFPL

【とりあえずの使い方】
・実行ファイルを、てきとーなフォルダに放り込んで実行してください
・タスクトレイに Jittoq のアイコンが表示されているはずです

【FAQ】
Ｑ：自動起動したい
Ａ：shell:startup あたりにショートカットを放り込めば良いんじゃないかな

Ｑ：終了するには？
Ａ：タスクトレイのメニューから Exit で

Ｑ：十徳なのにモジュール４つしかないんだ
Ａ：こまけぇことはいいんだよ

Ｑ：なにこのアイコン
Ａ：すまない、描くのが面倒なのでデフォルトなんだ

Ｑ：なんかショートカット押しても表示されなくなった
Ａ：VitruaWinとか使ってて、他の仮想デスクトップに存在しているとそうなるみたい

Ｑ：ソースコメントないんだけど？Formに全部書いてあるんだけど？テストは？デバッグは？ちゃんとした？
Ａ：この程度のツールでガチ設計とか面倒…

Ｑ：モジュールはアドオン形式にしないの？
Ａ：いろいろ面倒になったし、まあこのままでもいいかなーと

Ｑ：アドオン形式にするからやり方教えてよ
Ａ：IJittokSlaveを実装したクラスをdllで読み込めればOK
　　MainWindow_Loadあたりで IJittokModule を実装したクラスのインスタンスを
　　MainWindow.Controls と MainWindow.Modules に追加して、むにゃむにゃすればＯＫのはず
　　あと、現状のモジュールから MainWindow クラスとか、Commonなんとかを参照している手抜き箇所については要調整
　　面倒…

Ｑ：英語マニュアルとかない？
Ａ：No, I'm a Engrish speaker! All your base!

【ビルドするにあたっての注意】

プロジェクトからは、"YaccLexTools"を参照しています。
https://github.com/ernstc/YaccLexTools
ビルドする場合は、Nuget経由でYaccLexToolsをダウンロードする必要があります。
ちなみに、これはMITライセンスのようです。
https://raw.githubusercontent.com/abriom/YaccLexTools/master/LICENSE

【ライセンス】/ License
This application is licensed under WTFPL Ver.2

Copyright (c) 2022 Flagspitter
This program is free software.
It comes without any warranty, to the extent permitted by applicable law.
You can redistribute it and/or modify it under the
terms of the Do What The Fuck You Want To Public License, Version 2,
as published by Sam Hocevar. See the COPYING.WTFPL file for more details.

【ライセンス（日本語)】
このアプリケーションは、WTFPL Ver.2 の下でライセンスされます。

Copyright (c) 2022 Flagspitter
このアプリケーションはフリーソフトです。適用法で認められる範囲において、いかなる保証もありません。
あなたは、Sam Hocevarが発行した"Do What The Fuck You Want To Public License, Version 2"
に従って再頒布・変更あるいはその両方を行うことができます。
詳細は、COPYING.WTFPL を参照してください。
