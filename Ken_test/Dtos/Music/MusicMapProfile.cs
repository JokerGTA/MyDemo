using AutoMapper;
using Ken_test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class MusicMapProfile: Profile
    {
        public MusicMapProfile()
        {
            CreateMap<MusicFile, MusicFileDtoF>();
            CreateMap<Music, MusicDtoF>();
        }
    }
}
