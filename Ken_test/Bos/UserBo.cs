using Ken_test.Dtos;
using Ken_test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Bos
{
    public class UserBo : BoBase
    {
        public UserInfo UserInfo { get; set; }
        public UserBo(UserInfo userInfo)
        {
            UserInfo = userInfo;
        }

        internal void SendMsg(string msg)
        {
            MessageLog messageLog = new MessageLog { MsgContext = msg };
            if (UserInfo.MessageLogs != null)
            {
                UserInfo.MessageLogs.Add(messageLog);
                _boProvider._context.UserInfos.Update(UserInfo);
            }
            else
            {
                UserInfo.MessageLogs = new List<MessageLog>() { messageLog };
                _boProvider._context.UserInfos.Add(UserInfo);
            }

            SaveChange();
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="input"></param>
        internal void Save(UserModifyDto input)
        {
            if (UserInfo == null)
            {
                UserInfo = _boProvider._mapper.Map<UserInfo>(input);
                UserInfo.MessageLogs = new List<MessageLog>() { };
                _boProvider._context.UserInfos.Add(UserInfo);                
            }
            else {
                UserInfo.NickName = input.NickName;
                UserInfo.IPAddress = input.IPAddress;
                UserInfo.HeadPicture = input.HeadPicture;
                _boProvider._context.UserInfos.Update(UserInfo);
            }
            SaveChange();
        }

    }
}
