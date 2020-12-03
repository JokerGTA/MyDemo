using AutoMapper;
using Ken_test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos.UserDtos
{
    public class UserMapProfile:Profile
    {
        public UserMapProfile()
        {
            CreateMap<UserModifyDto, UserInfo>();
                // .ForMember(target => target.UserName, (map) => map.MapFrom(soure => soure.UserInfo.NickName));
        }
    }
}
