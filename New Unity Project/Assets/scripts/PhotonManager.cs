using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//photonViewと、PUNが呼び出すことのできるすべてのコールバック/イベントを提供します。使用したいイベント/メソッドをオーバーライドしてください。
public class PhotonManager : MonoBehaviourPunCallbacks
{
    //よく見るドキュメントページ
    //https://doc-api.photonengine.com/ja-jp/pun/current/class_photon_1_1_pun_1_1_photon_network.html
    //https://doc-api.photonengine.com/ja-jp/pun/current/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html
    //https://doc-api.photonengine.com/ja-jp/pun/current/namespace_photon_1_1_realtime.html

    public static PhotonManager instance;//static
    public GameObject loadingPanel;//ロードパネル
    public Text loadingText;//ロードテキスト
    public GameObject buttons;//ボタン


    public GameObject createRoomPanel;//ルーム作成パネル
    public Text enterRoomName;//入力されたルーム名テキスト


    public GameObject roomPanel;//ルームパネル
    public Text roomName;//ルーム名テキスト


    public GameObject errorPanel;//エラーパネル
    public Text errorText;//エラーテキスト


    public GameObject roomListPanel;//ルーム一覧パネル


    public Room originalRoomButton;//ルームボタン格納
    public GameObject roomButtonContent;//ルームボタンの親オブジェクト
    Dictionary<string, RoomInfo> roomsList = new Dictionary<string, RoomInfo>();//ルームの情報を扱う辞書
    private List<Room> allRoomButtons = new List<Room>();//ルームボタンを扱うリスト


    private void Awake()
    {
        instance = this;//格納
    }


    void Start()
    {
        //メニューをすべて閉じる
        CloseMenuUI();

        //ロードパネルを表示してテキスト更新
        loadingPanel.SetActive(true);
        loadingText.text = "ネットワークに接続中...";

        //ネットワークに接続しているのか確認
        if (!PhotonNetwork.IsConnected)
        {
            //最初に設定したPhotonServerSettingsファイルの設定に従ってPhotonに接続
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    /// <summary>
    /// 一旦すべてを非表示にする
    /// </summary>
    void CloseMenuUI()//なぜ作るのか：UI切り替えが非常に楽だから
    {
        loadingPanel.SetActive(false);//ロードパネル非表示

        buttons.SetActive(false);//ボタン非表示

        createRoomPanel.SetActive(false);//ルーム作成パネル

        roomPanel.SetActive(false);//ルームパネル

        errorPanel.SetActive(false);//エラーパネル

        roomListPanel.SetActive(false);//ルーム一覧パネル
    }



    //継承元のメソッドでは「virtual」のキーワード
    //継承先では「override」のキーワード
    /// <summary>
    /// クライアントがMaster Serverに接続されていて、マッチメイキングやその他のタスクを行う準備が整ったときに呼び出されます
    /// </summary>
    public override void OnConnectedToMaster()//
    {

        PhotonNetwork.JoinLobby();//マスターサーバー上で、デフォルトロビーに入ります

        loadingText.text = "ロビーへの参加...";//テキスト更新

    }


    /// <summary>
    /// マスターサーバーのロビーに入るときに呼び出されます。
    /// </summary>
    public override void OnJoinedLobby()//
    {

        LobbyMenuDisplay();


        roomsList.Clear();//辞書の初期化

    }


    //ロビーメニュー表示(エラーパネル閉じる時もこれ)
    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        buttons.SetActive(true);
    }

    //タイトルの部屋作成ボタン押下時に呼ぶ：UIから呼び出す
    public void OpenCreateRoomPanel()
    {
        CloseMenuUI();
        createRoomPanel.SetActive(true);
    }

    //部屋作成ボタン押下時に呼ぶ：UIから呼び出す
    public void CreateRoomButton()
    {
        //インプットフィールドのテキストに何か入力されていた場合
        if (!string.IsNullOrEmpty(enterRoomName.text))
        {
            //ルームのオプションをインスタンス化して変数に入れる 
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 6;// プレイヤーの最大参加人数の設定（無料版は20まで。1秒間にやり取りできるメッセージ数に限りがあるので10以上は難易度上がる）

            //ルームを作る(ルーム名：部屋の設定)
            PhotonNetwork.CreateRoom(enterRoomName.text, options);


            CloseMenuUI();//メニュー閉じる
            loadingText.text = "ルーム作成中...";
            loadingPanel.SetActive(true);
        }
    }


    //ルームに参加したら呼ばれる関数
    public override void OnJoinedRoom()
    {
        CloseMenuUI();//一旦すべてを閉じる
        roomPanel.SetActive(true);//ルームパネル表示

        roomName.text = PhotonNetwork.CurrentRoom.Name;//現在いるルームを取得し、テキストにルーム名を反映
    }


    //退出ボタン押下時に呼ばれる。参加中の部屋を抜ける
    public void LeavRoom()
    {
        //現在のルームを出て、マスターサーバーに戻って、ルームに参加したりルームを作成したりできます
        PhotonNetwork.LeaveRoom();

        CloseMenuUI();

        loadingText.text = "退出中・・・";
        loadingPanel.SetActive(true);
    }


    //ルームを抜けたときに呼ばれる
    public override void OnLeftRoom()
    {
        LobbyMenuDisplay();
    }

    //サーバーがルームを作成できなかったときに呼び出されます。
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "ルームの作成に失敗しました" + message;
        CloseMenuUI();
        errorPanel.SetActive(true);
    }


    //ルーム一覧画面を開く：ボタンから呼ぶ
    public void FindRoom()
    {
        CloseMenuUI();
        roomListPanel.SetActive(true);

    }


    //Master Serverのロビーにいる間に、ルームリストを更新するために呼び出されます。
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//
    {
        UpdateRoomList(roomList);//ルーム情報を辞書に格納
    }

    //ルームの情報を辞書に
    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)//ルームの数分ループ
        {
            RoomInfo info = roomList[i];//ルーム情報を変数に格納

            if (info.RemovedFromList)//ロビーで使用され、リストされなくなった部屋をマークします（満室、閉鎖、または非表示）
            {
                roomsList.Remove(info.Name);//辞書から削除
            }
            else
            {
                roomsList[info.Name] = info;//ルーム名をキーにして、辞書に追加
            }
        }

        RoomListDisplay(roomsList);//辞書にあるすべてのルームを表示
    }

    //ルーム表示
    void RoomListDisplay(Dictionary<string, RoomInfo> cachedRoomList)
    {
        //辞書のキー/値　でforeachを回す
        foreach (var roomInfo in cachedRoomList)
        {
            //ルームボタン作成
            Room newButton = Instantiate(originalRoomButton);
            //生成したボタンにルームの情報を設定
            newButton.RegisterRoomDetails(roomInfo.Value);
            //生成したボタンに親の設定
            newButton.transform.SetParent(roomButtonContent.transform);
            //リストに追加
            allRoomButtons.Add(newButton);
        }


    }
}