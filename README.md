# StringExtension

## 概要

C# / VB で文字列をバイト単位で操作するためのメソッド (MidB, LenB など) をまとめたクラスです。半角文字を長さ 1 バイト、全角文字を長さ 2 バイトとして扱います。

拡張メソッドとして実装しているので、コードの可読性が上がります。メソッド チェーンとしてコードを記述することができます。

## 対象フレームワーク

* .NET Frawework 3.5
* .NET Standard 1.3

## フォルダ構成

* `projects` フォルダ
  * 対象フレームワークごとに分けたプロジェクト ファイルを格納したフォルダです。
  * `StringExtension.sln`: C# で実装したソリューション。
  * `StringExtensionVB.sln`: VB で実装したソリューション。ただし、テスト コードは C# のもの。
* `src` フォルダ
  * メインとなるソース コードを格納したフォルダです。各プロジェクトからリンク参照しています。
* `test` フォルダ
  * NUnit によるテスト コードを格納したフォルダです。各プロジェクトからリンク参照しています。

## 導入方法

1. [ダウンロード ページ](https://github.com/TanaUmbreon/StringExtension/releases)から最新版のソース コードをダウンロードします。
1. 以下のどちらかの手順で使用するプロジェクトに追加します。

* ソース コードから `StringExtension.cs` または `StringExtension.vb` を取り出し、使用するプロジェクトに追加します。
* ソース コードをコンパイルして、生成された `StringExtension.dll` と `StringExtension.xml` を使用するプロジェクトの参照に追加します。

## 使用方法とコード例 (C#)

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
```

メソッド チェーンを用いた場合。

```cs
Console.WriteLine(text.MidB(3, 7).LenB().ToString()); // 出力: "7"
```

## 使用方法とコード例 (VB)

あらかじめ、`Imports` ステートメントでこの拡張メソッドを使用できるようにする必要があります。

```vb
Imports StringExtension
```

メソッドの呼び出しは以下のようにします。

```vb
Dim text As String = "半角1バイト/全角2バイト"

Console.WriteLine($"text のバイト数は {text.LenB()}") ' 出力: "text のバイト数は 23"
Console.WriteLine(text.MidB(3, 7)) ' 出力: "1バイト"
Console.WriteLine(text.LeftB(5)) ' 出力: "半角1"
Console.WriteLine(text.RightB(11)) ' 出力: "全角2バイト"
```

メソッド チェーンを用いた場合。

```vb
Console.WriteLine(text.MidB(3, 7).LenB().ToString()) ' 出力: "7"
```

## ライセンスについて

[MIT ライセンス](LICENSE)で公開しています。

## リリースノート

### 1.0.3 (2018/2/15)

* C#3.0 でコンパイル エラーにならないように修正。
  * nameof 演算子を文字列リテラルに変更。
  * 式形式によるメソッド実装を通常の実装に変更。
* VB2008 (9.0) でコンパイル エラーにならないように修正。
  * NameOf 演算子から文字列リテラルに変更。
  * 名前空間に Global を使わない記述に変更。
  * ByVal キーワードを付与 (なくても問題ない)。

### 1.0.2 (2018/2/13)

* VB での実装を追加。

### 1.0.1 (2018/2/10)

* .NET Frawework 3.5 に対応。

### 1.0.0 (2018/1/8)

* PadLeftB, PadRightB, FixLeftB, FixRightB メソッドを実装。

### alpha 2 (2017/12/10)

* LeftB, RightB メソッドを実装。

### alpha 1 (2017/12/3)

* LenB, MidB メソッドを実装。
