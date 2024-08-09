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
    　 　 │ ├ 設定ファイル(D-ATC.ini)
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
3. 設定ファイル(D-ATC.ini)に、情報を記入します
   (デフォルト値):
    ```ini
        [Data]
        Decelation = 3.0
        Stopmargin = 60
        Limitmargin = 10
    ```

5. VehiclePluginUsing.xmlに本プラグインの情報を記入します  
   (例): 
    ```xml
        <?xml version="1.0" encoding="utf-8" ?>
        <AtsExPluginUsing xmlns="http://automatic9045.github.io/ns/xmlschemas/AtsExPluginUsingXmlSchema.xsd">
        	<Assembly Path="AtsEXPlugins\D-ATC.dll" />
        	<Assembly Path="AtsEXPlugins\OtherPlugin.dll" />
        </AtsExPluginUsing>
   ```
> [!WARNING]
> この項目の内容はすべてが正しい保証がありません
> 正確な情報を得るには以下を参照してください
> - AtsEXの[公式リポジトリ](https://github.com/automatic9045/AtsEX/)
> - AtsEXの[公式サイト](https://automatic9045.github.io/AtsEX/)


## 使い方
- **Todo: 必要に応じて書く**
### パネル
| index | 型   | 機能       | 備考               |
| ----- | ---- | ---------- | ----------------- |
| ats52 | bool | ATC電源 | 特になし |
| ats53 | bool | ATC | 特になし |
| ats55 | bool | ATC開放 | 特になし |
| ats56 | bool | ATC非常運転 | 特になし |
| ats57 | bool | ATC非常 | 特になし |
| ats58 | bool | ATC常用 | 特になし |
| ats58 | bool | デジタルATC | 特になし |
| ats66 | int  | D-ATC現示速度 | 5km刻み(E231系など用) |
| ats67 | int  | D-ATC現示速度 | 1km刻み |
| ats68 | bool | パターン接近 | 特になし |
| ats71 | bool | ATC-01 | ATC0km現示 |
### サウンド
| index | 機能        | 備考               |
| ----- | ---------- | ----------------- |
| ats |  |  |

## 

## ライセンス
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
- Windows
    - Win10 22H2
    - Win11 23H2 or later
- [Bve](https://bvets.net/)
    - BVE Trainsim Version 5.8.7554.391 or later
    - BVE Trainsim Version 6.0.7554.619 or later
- [AtsEX](https://github.com/automatic9045/AtsEX)
    - [ver1.0-RC9 - v1.0.40627.1](https://github.com/automatic9045/AtsEX/releases/tag/v1.0.40627.1) or later


## 開発環境
- [AtsEX](https://github.com/automatic9045/AtsEX)
    - [ver1.0-RC9 - v1.0.40627.1](https://github.com/automatic9045/AtsEX/releases/tag/v1.0.40627.1)
- Win11 23H2
    - Visual Studio 2022
        - Microsoft Visual Studio Community 2022 (64 ビット) - Current Version 17.10.5
- [Bve](https://bvets.net/)
    - BVE Trainsim Version 5.8.7554.391
    - BVE Trainsim Version 6.0.7554.619


## 依存環境
- AtsEx.CoreExtensions (1.0.0-rc9)
- AtsEx.PluginHost (1.0.0-rc9)

(開発者向け)  
間接参照を含めたすべての依存情報については、各プロジェクトのフォルダにある `packages.lock.json` をご確認ください。
