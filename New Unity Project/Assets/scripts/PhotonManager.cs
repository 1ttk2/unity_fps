
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//photonViewと、PUNが呼び出すことのできるすべてのコールバック/イベントを提供します。使用したいイベント/メソッドをオーバーライドしてください。
public class PhotonManager : MonoBehaviourPunCallbacks
{
    //よく見るドキュメントページ
    //https://doc-api.photonengine.com/ja-jp/pun/current/class_photon_1_1_pun_1_1_photon_network.html
    //https://doc-api.photonengine.com/ja-jp/pun/current/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html


    public static PhotonManager instance;//static
    public GameObject loadingPanel;//ロードパネル
    public Text loadingText;//ロードテキスト
    public GameObject buttons;//ボタン


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
    }



    //継承元のメソッドでは「virtual」のキーワード
    //継承先では「override」のキーワード
    /// <summary>
    /// クライアントがMaster Serverに接続されていて、マッチメイキングやその他のタスクを行う準備が整ったときに呼び出されます
    /// </summary>
    public override void OnConnectedToMaster()//
    {

        PhotonNetwork.JoinLobby();//マスターサーバー上で、デフォルトロビーに入ります

        loadingText.text = "ロビーへの参加中...";//テキスト更新

    }

    /// <summary>
    /// マスターサーバーのロビーに入るときに呼び出されます。
    /// </summary>
    public override void OnJoinedLobby()//
    {

        LobbyMenuDisplay();//

    }


    //ロビーメニュー表示(エラーパネル閉じる時もこれ)
    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        buttons.SetActive(true);
    }
}