using AutoMapper;
using eHospitalServer.Entities.DTOs;
using eHospitalServer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHospitalServer.DataAccess.Mapping;
internal sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(member => member.FirstName, options => options.MapFrom(src => src.FirstName.Trim()))
            .ForMember(member => member.LastName, options => options.MapFrom(src => src.LastName.Trim()))
            .ForMember(member => member.Email, options => options.MapFrom(src => src.Email!.ToLower().Trim()))
            .ForMember(member => member.FullAddress, options => options.MapFrom(src => src.FullAddress.Trim()))
            .ForMember(member => member.UserName, options =>
                        options.MapFrom(src => src.UserName == null ?
                            src.FirstName
                            .Trim()
                            .ToLower()
                            .Replace(" ","")
                            .Replace("İ","I")
                            .Replace("ç","c")
                            .Replace("ü","u")
                            .Replace("ö","o")
                            .Replace("ğ", "g")
                            .Replace("ş", "s")
                            .Replace("ğ", "g")
                            .Replace("Ö","o")   
                            .Replace("ö", "o")
                            : src.UserName.Trim().ToLower()));

        CreateMap<CreatePatientDto, User>()
            .ForMember(member => member.FirstName, options => options.MapFrom(src => src.FirstName.Trim()))
            .ForMember(member => member.LastName, options => options.MapFrom(src => src.LastName.Trim()))
            .ForMember(member => member.Email, options => options.MapFrom(src => src.Email!.ToLower().Trim()))
            .ForMember(member => member.FullAddress, options => options.MapFrom(src => src.FullAddress.Trim()))
            .ForMember(member => member.UserName, options => 
                        options.MapFrom(src => src.FirstName
                            .Trim()
                            .ToLower()
                            .Replace(" ", "")
                            .Replace("ğ","g")
                            .Replace("ş", "s")
                            .Replace("ğ", "g")));

        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(member=>member.StartDate, options=>options.MapFrom(src=>DateTime.SpecifyKind(src.StartDate,DateTimeKind.Utc)))
            .ForMember(member=>member.EndDate, options=>options.MapFrom(src=>DateTime.SpecifyKind(src.EndDate,DateTimeKind.Utc)))
            ;
    }
}
