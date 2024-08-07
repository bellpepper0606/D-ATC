# READMEのテンプレート
## 使い方
1. 既にある`README.md`を消す
2. このテンプレートを`README.md`にリネームする
3. 水平線より上(この使い方の部分)を消す
4. `D-ATC`をリポジトリの名前に置換する
5. Todoのところを書く
6. 保存してコミット&プッシュ
---
# D-ATC
[AtsEX](https://github.com/automatic9045/AtsEX)を使ったBve5またはBve6用のプラグイン
AtsEXを使用してD-ATCの動作をシミュレートします。


## プラグインの機能
- D-ATCの信号パターン(現在は1段パターンのみ)と速度制御パターンをシミュレートします。


## 導入方法
### 1. AtsEXの導入
[公式のダウンロードページ](https://automatic9045.github.io/AtsEX.Docs/download/)を参照してください
### 2. 本プラグインの導入
1. [Releases](releases/)から最新版がダウンロードできます
2. 車両アドオンの中に本プラグインを配置します  
    ディレクトリ構成例: 
    ```text
    Scenarios
    └ MyVehicles
    　 └ SampleVehicle
    　 　 ├ Vehicle.txt
    　 　 ├ AtsEXPlugins
    　 　 │ ├ 本プラグイン(D-ATC.dll)
    　 　 │ └ OtherPlugin.dll
    　 　 ├ VehiclePluginUsing.xml
    　 　 ├ Ats
    　 　 │ ├ DetailManager.x64.dll
    　 　 │ ├ DetailManager.x86.dll
    　 　 │ ├ detailmodules.txt
    　 　 │ ├ OtherNormalPlugin.dll
    　 　 │ └ ...
    　 　 ├ Notch
    　 　 │ ├ Notch.txt
    　 　 │ └ ...
    　 　 ├ Panel
    　 　 │ ├ Panel.txt
    　 　 │ └ ...
    　 　 ├ Sound
    　 　 │ ├ Sound.txt
    　 　 │ ├ Motor.txt
    　 　 │ └ ...
    　 　 ├ Parameters.txt
    　 　 └ ...
    ```
3. 設定ファイルを作成し本プラグインの情報を記入します  
   VehiclePluginUsing.xml(例): 
        ```xml
        <?xml version="1.0" encoding="utf-8" ?>
        <AtsExPluginUsing xmlns="http://automatic9045.github.io/ns/xmlschemas/AtsExPluginUsingXmlSchema.xsd">
        	<Assembly Path="AtsEXPlugins\D-ATC.dll" />
        	<Assembly Path="AtsEXPlugins\OtherPlugin.dll" />
        </AtsExPluginUsing>
        ```
   ```
> [!WARNING]
> この項目の内容はすべてが正しい保証がありません
> 正確な情報を得るには以下を参照してください
> - AtsEXの[公式リポジトリ](https://github.com/automatic9045/AtsEX/)
> - AtsEXの[公式サイト](https://automatic9045.github.io/AtsEX/)


## 使い方
- **Todo: 必要に応じて書く**
### パネル
**Todo: 必要に応じて書く**
| index  | 型   | 機能       | 備考              |
| ------ | ---- | ---------- | ----------------- |
| ats001 | uint | 速度絶対値 | 1km刻みで切り捨て |


## ライセンス
- **Todo: `LICENSE`の著作権表示を書き換える**
- **Todo: ライセンスを変更する場合には`LICENSE`を書き換えた後にここも変更する**
- [MIT](LICENSE)
    - できること
        - 商用利用
        - 修正
        - 配布
        - 私的使用
    - ダメなこと
        - 著作者は一切責任を負わない
        - 本プラグインは無保証で提供される


## 動作環境
**Todo: 動作環境を必要に応じて変更**
- Windows
    - Win10 22H2
    - Win11 23H2 or later
- [Bve](https://bvets.net/)
    - BVE Trainsim Version 5.8.7554.391 or later
    - BVE Trainsim Version 6.0.7554.619 or later
- [AtsEX](https://github.com/automatic9045/AtsEX)
    - [ver1.0-RC5 - v1.0.40101.1](https://github.com/automatic9045/AtsEX/releases/tag/v1.0.40101.1) or later


## 開発環境
**Todo: 開発環境を必要に応じて変更**
- [AtsEX](https://github.com/automatic9045/AtsEX)
    - [ver1.0-RC5 - v1.0.40101.1](https://github.com/automatic9045/AtsEX/releases/tag/v1.0.40101.1)
- Win10 22H2
    - Visual Studio 2022
        - Microsoft Visual Studio Community 2022 (64 ビット) - Current Version 17.5.3
- [Bve](https://bvets.net/)
    - BVE Trainsim Version 5.8.7554.391
    - BVE Trainsim Version 6.0.7554.619


## 依存環境
**Todo: 依存環境を必要に応じて変更**
- AtsEx.CoreExtensions (1.0.0-rc1)
- AtsEx.PluginHost (1.0.0-rc5)

(開発者向け)  
間接参照を含めたすべての依存情報については、各プロジェクトのフォルダにある `packages.lock.json` をご確認ください。
