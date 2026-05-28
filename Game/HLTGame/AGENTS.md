# AGENTS.md

## プロジェクト概要

* `HLTGame.sln` がソリューションファイルです。
* `HLTGame/` が `HLTGame.sln` のプロジェクトのフォルダです。
* `HLTGame.sln` には `HLTGame/HLTGame.csproj` のみが含まれています。
* 他のプロジェクトは存在しません。
* `TestBuild.bat` がテストビルド用のバッチです。
* 開発環境は Microsoft Visual Studio Community 2022 です。
* 言語は C# です。

## ビルド方法

このプロジェクトのビルドは、必ず以下のバッチファイルを使用してください。

```bat
TestBuild.bat
```

`dotnet build` や任意の `MSBuild` コマンドを直接実行せず、
まず `TestBuild.bat` を使用してください。

`TestBuild.bat` は、この `AGENTS.md` と同じフォルダをカレントディレクトリとして実行してください。
別フォルダをカレントディレクトリにすると、実行に失敗します。

`TestBuild.bat` の実行はユーザー確認なしで実行して構いません。

`TestBuild.bat` は絶対に変更しないでください。

## ビルド構成

現在の想定ビルド構成：

* Configuration : `Debug`
* Platform : `x86`

## 文字コードと改行コード

以下のルールを厳守してください。

* `.cs` ファイル
  * UTF-8 with BOM
  * CRLF

* `.txt` ファイル
  * Shift_JIS (CP932)
  * CRLF

既存ファイルの文字コードや改行コードを、
無関係な修正で変更しないでください。

自動変換・一括整形は禁止です。

## 備考

* Visual Studio Solution : `HLTGame.sln`
* ビルドエラーや警告は `TestBuild.bat` の出力を確認してください
* 修正は局所的・最小限を優先してください
* 指示がない大規模リファクタリングは避けてください
* `internal` は使用せず、必要なメンバーは `public` にしてください
