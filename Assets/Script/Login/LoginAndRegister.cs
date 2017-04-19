using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;

namespace Script.Login
{
    public class LoginAndRegister : MonoBehaviour, IPhotonPeerListener
    {

        private PhotonPeer peer;

        private string address; //最好在Awake或Start中赋值，Unity 小问题，容易造成值不更改，还有最好写成私有
        private string Server; //同上

        public InputField nameField_login;
        public InputField pwdField_login;

        public InputField nameField_register;
        public InputField pwd_register1;
        public InputField pwd_register2;

        public GameObject LoginPanel;
        public GameObject RegisterPanel;

        public MessagePanel msgp;

        public SpaceCamera sCamera;

        void Start()
        {
            address = "127.0.0.1:5057";
            Server = "LoginServer";
            peer = new PhotonPeer(this, ConnectionProtocol.Udp);
            peer.Connect(address, Server);
        }

        public void LoginCLick()
        {

            var param = new Dictionary<byte, object>();
            param.Add(151, nameField_login.text);
            param.Add(152, pwdField_login.text);
            peer.OpCustom(101, param, true);
        }

        public void RegisterClick()
        {
            string name = nameField_register.text;
            string pwd1 = pwd_register1.text;
            string pwd2 = pwd_register2.text;

            if (pwd1.Equals(pwd2))
            {
                var param = new Dictionary<byte, object>();
                param.Add(151, name);
                param.Add(152, pwd1);
                peer.OpCustom(106, param, true);
            }
            else
            {
                msgp.ShowMsg("两次密码输入不一致!");
            }
        }

        public void RorL()
        {
            LoginPanel.SetActive(!LoginPanel.GetActive());
            RegisterPanel.SetActive(!RegisterPanel.GetActive());
        }

        void Update()
        {
            peer.Service();
        }

        public void DebugReturn(DebugLevel level, string message)
        {
        }

        public void OnEvent(EventData eventData)
        {
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {

            switch (operationResponse.OperationCode)
            {
                case (byte)OpCode.LoginSuccess:
                    LoginSuccess(nameField_login.text);
                    msgp.ShowMsg("login Success");
                    break;
                case (byte)OpCode.LoginFailed:
                    msgp.ShowMsg("login failed");
                    break;
                case (byte)OpCode.LoginFailed_PWD_ERROR:
                    msgp.ShowMsg("login failed 密码错误.");
                    break;
                case (byte)OpCode.LoginFailed_NotExitUserName:
                    msgp.ShowMsg("login failed 不存在该用户.");
                    break;
                case (byte)OpCode.RegisterSuccess:
                    msgp.ShowMsg("Register Success And login Success");
                    LoginSuccess(nameField_register.text);
                    break;
                case (byte)OpCode.RegisterFailed_EXITNAME:
                    msgp.ShowMsg("Register Failed 该用户名已被注册.");
                    break;
            }
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    Debug.Log("Connect");
                    break;
                case StatusCode.Disconnect:
                    Debug.Log("DisConnect");
                    msgp.ShowMsg("断开与服务器的链接");
                    break;
            }
        }

        private void LoginSuccess(string userName)
        {

            PhotonNetwork.playerName = userName;
            LoginPanel.transform.parent.gameObject.SetActive(false);
            sCamera.GetInRoom();
        }

        private void OnDestroy()
        {
            try
            {
                peer.Disconnect();
                peer.StopThread();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

    }
}