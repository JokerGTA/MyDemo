using Ken_test.Dtos;
using Ken_test.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// 利用位运算进行权限管理
        /// </summary>
        internal void Authority()
        {
            var ADD = 1; // 增加权限
            var UPD = 2; // 修改权限
            var SEL = 4; // 查找权限
            var DEL = 8; // 删除权限

            // 给予某种权限用到"位或"运算符
            var GROUP_A = ADD | UPD | SEL | DEL; // A 拥有增删改查权限
            var GROUP_B = ADD | UPD | SEL; // B 拥有增改查权限
            var GROUP_C = ADD | UPD; // C 拥有增改权限
            
            // 禁止某种权限用"位与"和"位非"运算符
            var GROUP_D = GROUP_C & ~UPD; // D 只拥有了增权限
            
            Debug.WriteLine("A用户组成员是否有增加权限" + ((GROUP_A & ADD) > 0));
            Debug.WriteLine("B用户组成员是否有删除权限" + ((GROUP_B & DEL) > 0));
            Debug.WriteLine("B用户组成员是否有增加权限" + ((GROUP_D & ADD) > 0));
            Debug.WriteLine("B用户组成员是否有修改权限" + ((GROUP_D & UPD) > 0));
        }

    }
}
