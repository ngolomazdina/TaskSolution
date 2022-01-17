using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace TaskWebApp.Models.Mappings
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
                cfg.CreateMap<TaskWebApp.Models.Task, TaskWcf.Model.DTO.Task>().ReverseMap();
            });

            _mapper = new Mapper(config);
        }
    }
}