using Ken_test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Bos
{
    public class MessageBo:BoBase
    {
        public MessageLog MessageLog { get; set; }
        public MessageBo(MessageLog messageLog)
        {
            MessageLog = messageLog;
        }
    }
}
