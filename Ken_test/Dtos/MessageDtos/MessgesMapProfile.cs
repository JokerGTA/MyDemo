using AutoMapper;
using Ken_test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class MessgesMapProfile : Profile
    {
        public MessgesMapProfile()
        {
            CreateMap<MessageLog, MessgesDtoF>()
                .ForMember(target => target.CreateTime, (map) => map.MapFrom(soure => soure.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                 .ForMember(target => target.UserName, (map) => map.MapFrom(soure => soure.UserInfo.NickName));

            CreateMap<MessageLog, MessageRoomDto>()
                .ForMember(target => target.CreateTime, (map) => map.MapFrom(soure => soure.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(target => target.NickName, (map) => map.MapFrom(soure => soure.UserInfo.NickName))
                .ForMember(target => target.HeadPicture, (map) => map.MapFrom(soure => soure.UserInfo.HeadPicture));
        }
    }
}
