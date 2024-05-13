using AutoMapper;
using eHospitalServer.Business.Services;
using eHospitalServer.DataAccess.Extensions;
using eHospitalServer.Entities.DTOs;
using eHospitalServer.Entities.Enums;
using eHospitalServer.Entities.Models;
using GenericEmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eHospitalServer.DataAccess.Services;
public class UserService(
    UserManager<User> userManager,
    IMapper mapper) : IUserService
{
    public async Task<Result<string>> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken)
    {

        if(request.Email is not null)
        {
            bool isEmailExist = await userManager.Users.AnyAsync(p=>p.Email == request.Email);
            if (isEmailExist)
            {
                return Result<string>.Failure(StatusCodes.Status409Conflict, "Email already has taken");
            }
        }

        if(request.IdentityNumber != "11111111111")
        {
            bool isIdentityNumberExist = await userManager.Users.AnyAsync(p => p.IdentityNumber == request.IdentityNumber);
            if (isIdentityNumberExist)
            {
                return Result<string>.Failure(StatusCodes.Status409Conflict, "Identity number already has taken");
            }
        }

        User user = mapper.Map<User>(request);

        bool isUserNameExists = await userManager.Users.AnyAsync(p=>p.UserName == user.UserName);
        if(isUserNameExists)
        {
            return Result<string>.Failure(StatusCodes.Status409Conflict, "User name already has taken");
        }
        

        Random random = new();

        bool isEmailConfirmCodeExists = true;
        if(isEmailConfirmCodeExists)
        {
            user.EmailConfirmCode = random.Next(100000, 999999);
            if(!userManager.Users.Any(p=>p.EmailConfirmCode == user.EmailConfirmCode))
            {
                isEmailConfirmCodeExists= false;
            }
        }

        
        user.EmailConfirmedCodeSendDate = DateTime.UtcNow;

        if(request.Specialty is not null)
        {
            user.DoctorDetail = new DoctorDetail()
            {
                Specialty = (Specialty)request.Specialty,
                WorkingDays = request.WorkingDays ?? new()
            };
        }

        IdentityResult result;
        if(request.Password is not null)
        {
           result =  await userManager.CreateAsync(user, request.Password);
        }
        else
        {
            result = await userManager.CreateAsync(user);
        }

        if(result.Succeeded)
        {
            return Result<string>.Succeed("User create is successful");
        }

        return Result<string>.Failure(500,result.Errors.Select(s=>s.Description).ToList());
    }
    public async Task<Result<Guid>> CreatePatientAsync(CreatePatientDto request, CancellationToken cancellationToken)
    {

        if (request.Email is not null)
        {
            bool isEmailExist = await userManager.Users.AnyAsync(p => p.Email == request.Email);
            if (isEmailExist)
            {
                return Result<Guid>.Failure(StatusCodes.Status409Conflict, "Email already has taken");
            }
        }

        if (request.IdentityNumber != "11111111111")
        {
            bool isIdentityNumberExist = await userManager.Users.AnyAsync(p => p.IdentityNumber == request.IdentityNumber);
            if (isIdentityNumberExist)
            {
                return Result<Guid>.Failure(StatusCodes.Status409Conflict, "Identity number already has taken");
            }
        }

        User user = mapper.Map<User>(request);

        user.UserType = UserType.Patient;
        int number = 0;
        while (await userManager.Users.AnyAsync(p => p.UserName == user.UserName))
        {
            number++;
            user.UserName += number;
        }


        Random random = new();

        bool isEmailConfirmCodeExists = true;
        if (isEmailConfirmCodeExists)
        {
            user.EmailConfirmCode = random.Next(100000, 999999);
            if (!userManager.Users.Any(p => p.EmailConfirmCode == user.EmailConfirmCode))
            {
                isEmailConfirmCodeExists = false;
            }
        }

        user.EmailConfirmedCodeSendDate = DateTime.UtcNow;

        IdentityResult result = await userManager.CreateAsync(user);   
     
        if (!result.Succeeded)
        {
            return Result<Guid>.Failure(500, result.Errors.Select(s => s.Description).ToList());
        }
        return Result<Guid>.Succeed(user.Id);

    }
    

   
}
