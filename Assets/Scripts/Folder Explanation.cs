/*
 * Application Dataフォルダー解説
 * ---各応募者の情報assetが格納されている
 *  |-Applicant_X：応募者のassetで、応募者情報の大本。基本的に履歴書の情報がまとめられており、加えて質問紙情報、検索結果情報、正解・不正解フラグがついている
 *  |-Interview-Applicant_X：質問と回答が１セットで、それを指定数分、保持している
 *  |-SearchData-Applicant_X：指定の検索タイプ（氏名、メアド、場所、顔画像など）と、検索語が一致するときに、検索結果として返す項目（複数可）を１セットで、それを指定数分、保持している。サイトの種類によって、当てはめる情報が異なる（SNS、GitHub、個人ブログなど）
 *  |-SiteData
 *      |-SNS-Applicant_X：SNS画面で表示する情報を保持している。投稿に関しては、投稿日時・投稿文・投稿画像（あれば）を１セットで、それを指定数分、登録可能
 *      |-GitHub-Applicant_X：GitHub画面で表示する情報を保持している
 *      |-PersonalBlog-Applicant_X：個人ブログ画面で表示する情報を保持している
 *      |-xxx-True_X：各サイトの、不正応募者がコピーしてきた元のサイト情報



 * Fontフォルダー解説
 *  ---使用した外部フォント保存場所
 *  |-cp_period：チェックポイント.(ピリオド)を使用（https://yokutobanaitori.web.fc2.com/quizfont.html#quizfont6）
 *  |-WEBDINGS：WindowsのデフォルトフォントであるWEBDINGSを使用
 *  |-WINGDING：WindowsのデフォルトフォントであるWINGDINGを使用



 * Imagesフォルダー解説
 *  ---要所で使用した画像の保存場所（面倒なので端折ります）

 
 * Scriptsフォルダー解説
 *  ---言わず及ばず、システムコード

 * -----------------------
 *  @EyeCatchフォルダ
 * -----------------------
 *  Animation
 *  ---トランジション・アイキャッチをコードで制御（.animの使い方がよくわからなかったのでスルーした。完成してから普通に使えそうなことが判明したけど…無念）
 *  |-EyeCatch：アイキャッチを再生する。EyeCatchにアタッチ
 *  |-EyeAnimation_1：アイキャッチ試作1号機。EyeCatchにアタッチ


 * -----------------------
 *  @Gameフォルダ
 * -----------------------
 *  Configration
 *  ---ゲームに関与しないコンフィグ機能。現状BGM変更機能しかついていない
 *  |-BGM
 *      |-BGMManager：使用するBGMをまとめて保持、かつBGM再生命令を保持。BGMSystemにアタッチ
 *      |-ScrollingTextUI：曲名を横方向にスライドさせるためのコード。BGMNameにアタッチ
 *  |-ConfigMenuUI：コンフィグ画面の遷移や、曲の再生に関する命令を実行。ConfigPopupRootにアタッチ


 *  Game System Call
 *  ---ゲームのシステムの根幹。これによってゲームを終了したりする
 *  |-ApplicantManager：応募者の情報を取得し、各ウィンドウに表示していく。GameManagerにアタッチ
 *  |-WindowLayoutSystem：ゲーム開始時やアイキャッチ後の、各ウィンドウのデフォルト配置を決定。GameManagerにアタッチ
 

 *  how to make Data
 *  ---ゲーム内で使用する各応募者のゲームデータ情報を生成するためのシステム拡張コード（No attach）
 *  |-Search
 *      |-SearchData：応募者の履歴書情報から検索機能を実行した際の、検索結果asset生成。Create/WorkersPlease/Applicant Search Dataより作成
 *      |-AIImageDetectionData：顔画像のAI生成かどうかを決めるasset生成。ApplicantDataで呼び出しているので、ApplicantDataを生成するとデフォルトで項目がアリ
 *      |-SitePage
 *          |-SitePageData：○○PageDataコードが、ページを司るコードであることの宣言を担っている（らしい）
 *          |-SNSPageData：応募者のSNSデータasset生成。Create/Search/SNS Page Dataより作成
 *          |-GitHubPageData：応募者のGitHubデータasset生成。Create/Search/GitHub Page Dataより作成
 *          |-PersonalSitePageData：応募者の個人ブログデータasset生成。Create/Search/Personal Site Page Dataより作成
 *  |-ApplicantData：応募者asset生成。Create/WorkersPlease/Applicant Dataより作成
 *  |-InterviewData：応募者のインタビューasset生成。Create/WorkersPlease/Interview Dataより作成


 *  Interview
 *  ---インタビュー情報生成における質問＆回答制御
 *  |-InterviewItem：インタビューウィンドウに、質問＆回答をセットの塊として宣言。InterviewItem.prefabにアタッチ（prefabを作成する時点でアタッチ済み）
 *  |-InterviewQuestionData：InterviewDataで生成するassetに、質問と回答を入力する領域を宣言（No attach）


 *  IpLog
 *  ---IPのログ情報を格納する制御
 *  |-IPLogEntry：JSONLの1行を受け取るデータクラス
 *  |-IPLogRow：IPLogRowにおける情報格納場所の指定と格納。IPLogRow.prefabにアタッチ
 

 *  prefab
 *  |-IPLog
 *      |-IPLogRow：JSONLの情報を格納する構造をまとめたprefab。
 *  |-Search
 *      |-BitHug
 *          |-BitHugPage.prefab：BitHugのサイト構造をまとめたprefab。
 *      |-PersonalBlog
 *          |-BlogArticleItem.prefab：個人ブログの投稿欄の表示デザインをまとめたprefab。PersonalBlogPageDataで登録した投稿内容を表示
 *          |-PersonalBlogPage.prefab：個人ブログのサイト構造をまとめたprefab。
 *      |-SNS
 *          |-SNSPage.prefab：SNSのサイト構造をまとめたprefab。
 *          |-SNSPostItem.prefab：SNSの投稿欄の表示デザインをまとめたprefab。SNSPageDataで登録した投稿内容を表示
 *      |-SearchResultItem.prefab：検索結果の表示デザインをまとめたprefab。検索した語に登録した検索結果を表示する
 *  |-InterviewItem.prefab：Hierarchyに生成するprefab



 *  Search
 *  ---検索機能の主機能、および検索機能に連なるコード
 *  |-SearchTargetType：履歴書情報から、いずれを検索したのかを区別可能。SearchEntryDataにて使用
 *  |-SearchEngine
 *      |-SearchEntryData：検索語ごとに、「どの検索語タイプか」「具体的な検索語」「検索結果として返すサイト一覧」を保存
 *      |-SearchResultData：検索後に出てくるサイトの概要表示のデータ宣言。SearchEntryDataでデータ入力

 *      |-SearchableText：SearchTabボタンを押した後に、履歴書に書いてある情報をクリックすると検索タブに情報がコピーされる。各テキスト情報にアタッチ
 *      |-SearchSiteType：検索して出てくるサイトの一覧。今後追加するサイトを追記していく。SearchResultDataにて使用
 *      |-SearchResultItem：検索結果サイトに、アイコン・サイト名・サイトURL・サイト概要を１セットの塊として宣言。SearchResultItem.prefabにアタッチ（prefabを作成する時点でアタッチ済み）

 *  |-SitePage
 *      |-SNS
 *          |-SNSPostItem：SNSの投稿欄に、アイコン・ユーザ名・投稿日時・投稿文・投稿画像を１セットで宣言。SNSPostItem.prefabにアタッチ（prefabを作成する時点でアタッチ済み）
 *      |-PersonalBlog
 *          |-BlogArticleItem：個人ブログの投稿欄に、画像・投稿題名・投稿年代・投稿要約を１セットで宣言。BlogArtticleItem.prefabにアタッチ（prefabを作成する時点でアタッチ済み）
 

 *  UI
 *  ---表示するウィンドウに、情報（UI）を表示する
 *  |-Interview
 *      |-InterviewWindowUI：インタビューウィンドウで、質問数分Prefabを生成し、assetから情報を抜き出し格納。InterviewWindowにアタッチ

 *  |-IPLog
 *      |-IPLogWindowUI：JSONLファイルを読み込み、Rowにprefabを生成する。IPLogWindowにアタッチ

 *  |-Resume
 *      |-ResumeWindowUI：Resumeウィンドウで、応募者の情報を格納場所を指定し、assetから情報を抜き出し格納。ResumeWindowにアタッチ
 *      |-SearchableImage：AI顔画像判定でタブにコピーできる。ResumeのFaceImageにアタッチ（使わないで良いかも。結局）
 *      |-SearchableText：Resumeに登録した情報をクリックすることで、検索タブにコピーできる。Resumeの各TextValueにアタッチ
 *      |-StampArea：スタンプを選択したうえでクリックされると、いずれかのスタンプが押下され、アイキャッチが流れ次の応募者に遷移する。アニメーションのパラメータも付与。StampAreaにアタッチ

 *  |-Search
 *      |-SearchWindow
 *          |-BrowserPageManagerUI：SearchWindowUIが表示命令を呼び出す場で、こちらはどのページを表示するかの命令を保持。SearchSystemにアタッチ
 *          |-SearchWindowUI：検索ウィンドウで、設定した検索結果情報分Prefabを生成し、assetから情報を抜き出し格納。また、各サイトに遷移する機能(各UIの命令の呼び出し)も実行している。SearchWindowにアタッチ
 *          |-SearchLoadingUI：検索中の画面のアニメーションを含めたUIコード。SearchLoadPageにアタッチ
 *      |-Site
 *          |-BitHug
 *              |-BitHugPageUI：GitHub画面で、GitHub情報を格納する場所を指定し、assetから情報を抜き出し格納する命令を保持。Site/BitHugPageにアタッチ
 *          |-PersonalBlog
 *              |-PersonalBlogPageUI：Personal画面で、Personal情報を格納する場所を指定し、assetから情報を抜き出し格納する命令を保持。Site/PersonalBlogPageにアタッチ
 *          |-SNS
 *              |-SNSPageUI：SNS画面で、SNS情報を格納する場所を指定し、assetから情報を抜き出し格納する命令を保持。SNS/SNSPageにアタッチ
 *      |-AIImageDetectorUI：画像生成AI判定画面
 *  |-StampUI：TaskBarのStampButtonを押したときに、採用・非採用の選択ウィンドウタブが表示される。StampButtonの色も変更される。StampSystemにアタッチ



 *  Window
 *  ---表示するウィンドウの諸操作制御関連
 *  |-GameWindow：各ウィンドウのContent内容を表示する＆最小化・最大化・クローズ機能搭載。各ウィンドウにアタッチ
 *  |-WindowAnimation：各ウィンドウが最小化・表示されるときのアニメーション。各ウィンドウにアタッチ
 *  |-WindowDragHandler：ウィンドウをドラッグして動かせるようにする。各ウィンドウのTitleBarにアタッチ
 *  |-WindowFocusHandler：選択したウィンドウを最前面にする機能。各ウィンドウにアタッチ
 *  |-WindowResizeHandler：ウィンドウの右下をドラッグすることでサイズ変更が可能。各ウィンドウのResizeHandleにアタッチ
 
 * -----------------------
 *  @Game Start
 * -----------------------
 *  |-ButtonEffect
 *      |-CreditPopupUI：Setting同様、Creditウィンドウが表示されるようにする。CreditPopupUIにアタッチ
 *      |-StartMenuItemEffect：スタートに表示されるボタンにカーソルを合わせると、アニメーションが起こる。各ボタンにアタッチ
 *  |-HowtoPlay
 *      |-HowToPlay：Popupの開閉、Page遷移など、遊び方の遷移に関わるコード。HowToPlaySystemにアタッチ
 *      |-TutorialSlideshow：各Pageの画像切り替え管理。各Pageにアタッチ
 *  |-GameFlowManager：Start⇒Game⇒Endの画面遷移を制御する。Game Flow Systemにアタッチ
 *  |-LogoAnimation：スタート画面のロゴアニメーションを作成。Logoにアタッチ
 *

 * -----------------------
 *  @Game Finish
 * -----------------------
 *  |-prefab
 *      |-ApplicantResult.prefab：リザルト画面で、各応募者の解説をするノード
 *  |-ApplicantResultData：応募者１人につき、ユーザがどちらを選択したか・正解だったかを保持。ノーアタッチ
 *  |-GameResultManager：ゲーム全体の判定履歴を保持。GameResultManagerにアタッチ
 *  |-ApplicantResult：ApplicantResult.prefabの情報格納場所を指定。ApplicantResult.prefabにアタッチ
 *  |-EndResultUI：結果画面を生成する。ResultWindowにアタッチ


 * Musicフォルダー解説
 *  ---ゲーム内で使用する音源フォルダ
 *  |-BGM（MusMus様より使用させていただきました。{フリーBGM・音楽素材MusMus https://musmus.main.jp}）
 *      |-MusMus-BGM-062 -[tyousa] dbd file No.03-
 *      |-MusMus-BGM-090 -majoxtuba taye san-
 *      |-MusMus-BGM-143 -hametsu no ringo-
 *      |-MusMus-BGM-146 -[tansaku] dbd file No.05-
 *      |-MusMus-BGM-156 -mabuta wo tozireba-
 *  |-Stamp Sound（otoLogic様より使用させていただきました。{BGM・ジングル・効果音のフリー素材｜OtoLogic https://otologic.jp/}）
 *      |-Onoma-Pop03-1(High)
 *      |-Onoma-Pop03-1(Mid)
 *      |-Onoma-Pop03-1(Low)


 * Materialsフォルダー解説
 *  ---Panelなど、Imageではないオブジェクトに演出を加えるためのmaterial保存場所
 *  |-RoundedPanelMaterial：Panelを角丸にするためのマテリアル
 


 * Shadersフォルダー解説
 *  ---Materialsフォルダーに保存されているmaterialを作るためのコード保存場所
 *  |-RoundedUI：RoundedPanelMaterialを作るためのShader。実はコードで作られているという...
 */


/*
 * 今できてること
 *  |-データ機能
 *      |-応募者情報、インタビュー情報を格納するasset作成機能
 *      |-各サイトのデザイン、および表示する内容を格納するasset作成機能
 
 *  |-データ表示機能
 *      |-応募者情報、インタビュー情報を各ウィンドウに順番に表示する機能（IP Logは未実装）
 *      |-検索後、検索に応じて表示するサイトの制御
 *      |-検索で出てくるサイトのデータを表示する機能

 *  |-操作関連
 *      |-ウィンドウ移動機能
 *      |-氏名やEmailアドレスを選択したうえで、検索ウィンドウで検索する機能
 *      |-応募者を承認/拒否するスタンプを押す機能

 *  |-演出関連
 *      |-次の応募者へ移動するアイキャッチの作成
 *      |-BGM再生と、BGM選択のコンフィグ機能
 *      |-スタンプを押下した時のサウンド再生機能
 *      |-アイキャッチ再生後、順番に各ウィンドウをデフォルトのサイズと配置で表示。Searchウィンドウのみ、初期表示はしない。
 *      |-スタート画面・ゲーム画面に遷移するシステム
 *      |-スタート・リザルト画面のデザイン

 *  |-その他
 *      |-スタート画面にクレジット表記
 */