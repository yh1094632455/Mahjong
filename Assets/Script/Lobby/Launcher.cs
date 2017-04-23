using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace com.Lobby
{
    public class Launcher : Photon.PunBehaviour
    {

        #region PUBLIC

        //客户端版本
        public string _gameVersion = "1.0";

        //玩家名字
        public Text nameField;

        public GameObject lobbyPanel;

        public GameObject roomPanel;

        //房间列表
        public RectTransform LobbyPanel;

        //玩家列表
        public RectTransform playersPanel;

        //退出房间按钮
        public Button btnExit;

        //开始按钮
        public Button btnStart;
        #endregion

        #region PRIVATE 

        private bool isConnecting;
        #endregion

        private void Awake()
        {
            //#不重要
            //强制Log等级为全部
            PhotonNetwork.logLevel = PhotonLogLevel.Full;

            //#关键
            //我们不加入大厅 这里不需要得到房间列表所以不用加入大厅去
            PhotonNetwork.autoJoinLobby = true;

            //#关键
            //这里保证所有主机上调用 PhotonNetwork.LoadLevel() 的时候主机和客户端能同时进入新的场景
            PhotonNetwork.automaticallySyncScene = true;
        }

        void Start()
        {
            Connect();

            SetPlayerName();

            lobbyPanel.transform.DOScaleY(1, 1f);

            btnExit.onClick.AddListener(delegate { StartCoroutine(ExitRoom()); });
            btnStart.onClick.AddListener(delegate { StartGame(); });
        }

        /// <summary>
        /// 连接到大厅
        /// </summary>
        private void Connect()
        {
            isConnecting = true;

            //已經連接上了服務器
            if (PhotonNetwork.connected)
            {
                Debug.Log("Connected");
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings(_gameVersion);
            }
        }
        
        /// <summary>
        /// 成功连接到大厅
        /// </summary>
        public override void OnConnectedToPhoton()
        {
            base.OnConnectedToPhoton();
        }

        /// <summary>
        /// 连接大厅失败
        /// </summary>
        /// <param name="error"></param>
        private void OnFailedToConnect(NetworkConnectionError error)
        {
            Debug.Log("fail to Connect");
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Debug.Log("Launcher Create Room faileds");
        }

        public void SetPlayerName()
        {
            nameField.text = PhotonNetwork.playerName;
        }

        public override void OnReceivedRoomListUpdate()
        {
            Debug.Log("OnReceivedRoomListUpdate");
            RoomInLobby[] ts = LobbyPanel.GetComponentsInChildren<RoomInLobby>();
            foreach (RoomInLobby t in ts)
            {
                Destroy(t.gameObject);
            }

            RoomInfo[] rooms = PhotonNetwork.GetRoomList();
            foreach (RoomInfo room in rooms)
            {
                GameObject g = GameObject.Instantiate(Resources.Load("Lobby/RoomItem") as GameObject);
                RoomInLobby ril = g.GetComponent<RoomInLobby>();

                ril.t.text = room.Name;
                g.name = room.Name;
                g.transform.SetParent(LobbyPanel);
                g.transform.localScale = Vector3.one;
            }
        }

        public override void OnJoinedRoom()
        {

            StartCoroutine(GetInRoom());

            Text[] ts = playersPanel.GetComponentsInChildren<Text>();
            foreach (Text t in ts)
            {
                Destroy(t.gameObject.transform.parent.gameObject);
            }
            PhotonPlayer[] players = PhotonNetwork.playerList;
            foreach (PhotonPlayer player in players)
            {
                GameObject g = GameObject.Instantiate(Resources.Load("Lobby/PlayerItem") as GameObject);
                Text t = g.transform.Find("Text").GetComponent<Text>();
                t.text = player.NickName;
                g.name = player.NickName;
                g.transform.SetParent(playersPanel);
                g.transform.localScale = Vector3.one;
            }
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            GameObject g = GameObject.Instantiate(Resources.Load("Lobby/PlayerItem") as GameObject);
            Text t = g.transform.Find("Text").GetComponent<Text>();
            t.text = newPlayer.NickName;
            g.name = newPlayer.NickName;
            g.transform.SetParent(playersPanel);
            g.transform.localScale = Vector3.one;
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            GameObject g = playersPanel.FindChild(otherPlayer.NickName).gameObject;
            Destroy(g);
        }

        /// <summary>
        /// 创建房间
        /// </summary>
        public void CreateARoom()
        {
            if (PhotonNetwork.connected)
            {
                //创建房间成功
                if (PhotonNetwork.CreateRoom(nameField.text, new RoomOptions { MaxPlayers = 4 }, null))
                {
                    Debug.Log("Launcher.CreateARoom 成功");

                    StartCoroutine(GetInRoom());
                }
            }
        }
        
        /// <summary>
        /// 开始游戏
        /// </summary>
        void StartGame()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                return;
            }

            if (PhotonNetwork.playerList.Length == 4)
            {
                PhotonNetwork.LoadLevel("Desktop");
            }
        }

        /// <summary>
        /// 退出房间
        /// </summary>
        /// <returns></returns>
        IEnumerator ExitRoom()
        {
            roomPanel.transform.DOScaleY(0, 0.8f);
            PhotonNetwork.LeaveRoom();

            yield return new WaitForSeconds(1f);
            lobbyPanel.transform.DOScaleY(1, 1f);
        }

        /// <summary>
        /// 加入房间
        /// </summary>
        /// <returns></returns>
        IEnumerator GetInRoom()
        {
            lobbyPanel.transform.DOScaleY(0,.8f);
            yield return new WaitForSeconds(1f);
            roomPanel.transform.DOScaleY(1, 1f);
        }

    }
}