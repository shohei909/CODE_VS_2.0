CODE_VS_2.0
===========

CODE VS 2.0予選コード (shohei909 総合4位、学生2位)

開発環境
言語:   C#
OS:     Windows7
IME:    SharpDevelop

=====
各プロジェクトについて。

Client.csproj
自作クライアント。公式クライアントを起動しないで動作を確認したいときに起動。

CodeVS2.csproj
公式クライアント向けの実行ファイルの生成。
CodeVS2/Program.csで各サイズごとの分岐をしてる。

GameCore.csproj
上二つの共通部分。


==== 
GameCore内の各ファイルについて

Game.cs
ブロック消去など、ゲームをシミュレーションするクラス。

Config.cs
各サイズごとの設定用クラス。



AI/EasyAI.cs
テスト用の一手読みAI。ランダムよりも雑魚。

AI/AdvansedAI.cs
不定型連鎖のAI。Small,Medium,Largeで使用。

AI/ChainAI.cs
定型連鎖のAI。Small,Mediumで使用。

AI/CombinedAI.cs
複数のAIを並列させて実行して、最終的にスコアの良かったAIの手を採用する。Small,Mediumで使用。



Model/ChainModel.cs。
定型連鎖の評価などを行う。定型連鎖を行う中核部分。

Model/ChainFactory.cs
連鎖の全体像を盤面のサイズに合わせて生成してくれるクラス。

Model/ChainConfig.cs
いろんな連鎖の設定の残骸たち。
