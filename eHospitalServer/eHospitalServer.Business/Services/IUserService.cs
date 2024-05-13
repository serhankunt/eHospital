using eHospitalServer.Entities.DTOs;
using eHospitalServer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eHospitalServer.Business.Services;
public interface IUserService
{
    Task<Result<string>> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken);
    Task<Result<Guid>> CreatePatientAsync(CreatePatientDto request, CancellationToken cancellationToken);
    
}
