using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Script.Login
{
    public enum OpCode : byte
    {
        Login = 101,
        LoginSuccess = 102,
        LoginFailed = 103,
        LoginFailed_NotExitUserName = 104,
        LoginFailed_PWD_ERROR = 105,

        Register = 106,
        RegisterSuccess = 107,
        RegisterFailed = 108,
        RegisterFailed_EXITNAME = 109
    }

    public enum OpKey : byte
    {
        RoomID = 150,
        UserName = 151,
        Password = 152
    }

    public enum ClientState : byte
    {
        DisConnect,
        Connect,
        LoginSuccess,
        LoginFailed
    }
}
