using Ken_test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Repositories
{
    public class MessageLogRepo
    {
        private Ken_testContext _context;
        public MessageLogRepo(Ken_testContext context)
        {
            _context = context;
        }

        public MessageLog Get(int id)
        {
            var result = _context.MessageLogs.Find(id);
            if (result == null)
                throw new Exception("此数据不存在");
            return result;
        }

        public List<MessageLog> GetListByUserId(int userId)
        {
            return _context.MessageLogs.Where(m => m.UserId == userId).ToList();
        }

        public List<MessageLog> GetAllList()
        {
            return _context.MessageLogs.ToList();
        }
    }
}
