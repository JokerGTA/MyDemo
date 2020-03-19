using AutoMapper;
using Ken_test.Models;
using Ken_test.Repositories;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ken_test.Bos
{
    public class BoPriver
    {
        public IHostingEnvironment _environment;
        public IMapper _mapper;
        public Ken_testContext _context;
        public UserInfoRepo _userInfoRepo;
        public MessageLogRepo _messageLogRepo;
        public BoPriver(
            IHostingEnvironment environment,
            IMapper mapper,
            Ken_testContext context,
            UserInfoRepo userInfoRepo,
            MessageLogRepo messageLogRepo)
        {
            _environment = environment;
            _mapper = mapper;
            _context = context;
            _userInfoRepo = userInfoRepo;
            _messageLogRepo = messageLogRepo;
        }

        internal R NewBo<R, T>(T input) where R : BoBase where T : BaseModel
        {
            Type t = typeof(R);
            var ci = t.GetConstructors();
            foreach (ConstructorInfo c in ci)
            {
                ParameterInfo[] ps = c.GetParameters();
                foreach (ParameterInfo pi in ps)
                {
                    if (pi.ParameterType == typeof(T))
                    {
                        R result = Activator.CreateInstance(t, new object[] { input }) as R;
                        result._boProvider = this;
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
