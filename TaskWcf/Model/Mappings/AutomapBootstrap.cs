using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace TaskWcf.Model.Mappings
{
    public static class AutomapBootstrap
    {
        static Mapper _mapper;

        public static Mapper Mapper {
            get {
                if (_mapper == null)
                    InitializeMap();

                return _mapper;
            }            
        }

        static void InitializeMap()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<DB.sp_GetAvailableStates_Result, DTO.State>();
                cfg.CreateMap<DB.User, DTO.User>().ForMember(dtou => dtou.Name, dbu => dbu.MapFrom(u => $"{u.FirstName} {u.SecondName} {u.Surname}")); ;
                cfg.CreateMap<DB.sp_GetTaskList_Result, DTO.Task>();                
                cfg.CreateMap<DB.Task, DTO.Task>().ReverseMap();
            });

            _mapper = new Mapper(config);
        }
    }
}