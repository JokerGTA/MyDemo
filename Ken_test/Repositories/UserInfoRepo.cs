using Ken_test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Repositories
{
    public class UserInfoRepo
    {
        private Ken_testContext _context;
        public UserInfoRepo(Ken_testContext context)
        {
            _context = context;            
        }

        public UserInfo Get(int id)
        {
            var result = _context.UserInfos.Find(id);
            if (result == null)
                throw new Exception("此数据不存在");
            return result;
        }

        public UserInfo GetByIp(string ip)
        {
            return _context.UserInfos.FirstOrDefault(m=>m.NickName == ip);            
        }
    }
}
