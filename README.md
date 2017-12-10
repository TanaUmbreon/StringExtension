# StringExtension

## 概要

昔からある基幹システムなどによくある、文字列の長さを半角文字 1 バイト、全角文字 2 バイトとして扱う仕組みで必須になる文字列操作 (いわゆる Excel にある MIDB, LENB 関数など) をまとめたクラスです。

拡張メソッドとして実装しているので、コードの可読性が格段に上がります。

メソッド チェーンとしてコードを記述することができます。

## 準備

1. [ダウンロード ページ](https://github.com/TanaUmbreon/StringExtension/releases)から最新版のソース コードをダウンロードします。
1. 以下のどちらかの手順で使用するプロジェクトに追加します。

* ソース コードから `StringExtension.cs` を取り出し、使用するプロジェクトに追加します。
* ソース コードをコンパイルして、生成された `StringExtension.dll` と `StringExtension.xml` を使用するプロジェクトの参照に追加します。

## 使用方法とコード例

あらかじめ、`using` ディレクティブでこの拡張メソッドを使用できるようにする必要があります。

```cs
using StringExtension;
```

メソッドの呼び出しは以下のようにします。

```cs
string text = "半角1バイト/全角2バイト";

Console.WriteLine($"text のバイト数は {text.LenB()}"); // 出力: "text のバイト数は 23"
Console.WriteLine(text.MidB(3, 7)); // 出力: "1バイト"
Console.WriteLine(text.LeftB(5)); // 出力: "半角1"
Console.WriteLine(text.RightB(11)); // 出力: "全角2バイト"

// メソッド チェーンを用いた場合
Console.WriteLine(text.MidB(3, 7).LenB().ToString()); // 出力: "7"
```

## ライセンスについて

[MIT ライセンス](LICENSE)で公開しています。

## リリースノート

### alpha 2 (2017/12/10)

* LeftB, RightB メソッドを実装。

### alpha 1 (2017/12/3)

* LenB, MidB メソッドを実装。
