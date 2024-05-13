using eHospitalServer.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHospitalServer.Entities.Models;
public sealed class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => string.Join(" ", FirstName, LastName);
    public string IdentityNumber {  get; set; } = string.Empty;
    public string FullAddress {  get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateOnly? DateOfBirth { get; set; }
    public string? BloodType { get; set; }
    public UserType UserType { get; set; } = UserType.Patient;
    public int EmailConfirmCode { get; set; }
    public DateTime EmailConfirmedCodeSendDate { get; set; }
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpires { get; set; }
    [ForeignKey("DoctorDetail")]
    public Guid? DoctorDetailId { get; set; }
    public DoctorDetail? DoctorDetail { get; set; }
    public int? ForgotPasswordCode { get; set; }
    public DateTime? ForgotPasswordCodeSendDate { get; set; }
  
}
