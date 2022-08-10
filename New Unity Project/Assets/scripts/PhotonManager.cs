using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//photonView�ƁAPUN���Ăяo�����Ƃ̂ł��邷�ׂẴR�[���o�b�N/�C�x���g��񋟂��܂��B�g�p�������C�x���g/���\�b�h���I�[�o�[���C�h���Ă��������B
public class PhotonManager : MonoBehaviourPunCallbacks
{
    //�悭����h�L�������g�y�[�W
    //https://doc-api.photonengine.com/ja-jp/pun/current/class_photon_1_1_pun_1_1_photon_network.html
    //https://doc-api.photonengine.com/ja-jp/pun/current/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html
    //https://doc-api.photonengine.com/ja-jp/pun/current/namespace_photon_1_1_realtime.html

    public static PhotonManager instance;//static
    public GameObject loadingPanel;//���[�h�p�l��
    public Text loadingText;//���[�h�e�L�X�g
    public GameObject buttons;//�{�^��


    public GameObject createRoomPanel;//���[���쐬�p�l��
    public Text enterRoomName;//���͂��ꂽ���[�����e�L�X�g


    private void Awake()
    {
        instance = this;//�i�[
    }


    void Start()
    {
        //���j���[�����ׂĕ���
        CloseMenuUI();

        //���[�h�p�l����\�����ăe�L�X�g�X�V
        loadingPanel.SetActive(true);
        loadingText.text = "�l�b�g���[�N�ɐڑ���...";

        //�l�b�g���[�N�ɐڑ����Ă���̂��m�F
        if (!PhotonNetwork.IsConnected)
        {
            //�ŏ��ɐݒ肵��PhotonServerSettings�t�@�C���̐ݒ�ɏ]����Photon�ɐڑ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    /// <summary>
    /// ��U���ׂĂ��\���ɂ���
    /// </summary>
    void CloseMenuUI()//�Ȃ����̂��FUI�؂�ւ������Ɋy������
    {
        loadingPanel.SetActive(false);//���[�h�p�l����\��

        buttons.SetActive(false);//�{�^����\��

        createRoomPanel.SetActive(false);//���[���쐬�p�l��
    }



    //�p�����̃��\�b�h�ł́uvirtual�v�̃L�[���[�h
    //�p����ł́uoverride�v�̃L�[���[�h
    /// <summary>
    /// �N���C�A���g��Master Server�ɐڑ�����Ă��āA�}�b�`���C�L���O�₻�̑��̃^�X�N���s���������������Ƃ��ɌĂяo����܂�
    /// </summary>
    public override void OnConnectedToMaster()//
    {

        PhotonNetwork.JoinLobby();//�}�X�^�[�T�[�o�[��ŁA�f�t�H���g���r�[�ɓ���܂�

        loadingText.text = "���r�[�ւ̎Q��...";//�e�L�X�g�X�V

    }


    /// <summary>
    /// �}�X�^�[�T�[�o�[�̃��r�[�ɓ���Ƃ��ɌĂяo����܂��B
    /// </summary>
    public override void OnJoinedLobby()//
    {

        LobbyMenuDisplay();//

    }


    //���r�[���j���[�\��(�G���[�p�l�����鎞������)
    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        buttons.SetActive(true);
    }

    //�^�C�g���̕����쐬�{�^���������ɌĂԁFUI����Ăяo��
    public void OpenCreateRoomPanel()
    {
        CloseMenuUI();
        createRoomPanel.SetActive(true);
    }

    //�����쐬�{�^���������ɌĂԁFUI����Ăяo��
    public void CreateRoomButton()
    {
        //�C���v�b�g�t�B�[���h�̃e�L�X�g�ɉ������͂���Ă����ꍇ
        if (!string.IsNullOrEmpty(enterRoomName.text))
        {
            //���[���̃I�v�V�������C���X�^���X�����ĕϐ��ɓ���� 
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 6;// �v���C���[�̍ő�Q���l���̐ݒ�i�����ł�20�܂ŁB1�b�Ԃɂ����ł��郁�b�Z�[�W���Ɍ��肪����̂�10�ȏ�͓�Փx�オ��j

            //���[�������(���[�����F�����̐ݒ�)
            PhotonNetwork.CreateRoom(enterRoomName.text, options);


            CloseMenuUI();//���j���[����
            loadingText.text = "���[���쐬��...";
            loadingPanel.SetActive(true);
        }
    }
}