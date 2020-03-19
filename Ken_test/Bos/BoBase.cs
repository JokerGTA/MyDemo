using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Bos
{
    public class BoBase
    {
        public BoPriver _boProvider { get; set; }

        public void SaveChange()
        {
            _boProvider._context.SaveChanges();
        }
    }
}
