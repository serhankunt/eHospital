using eHospitalServer.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHospitalServer.Entities.Configurations;
internal sealed class DoctorDetailConfiguration : IEntityTypeConfiguration<DoctorDetail>
{
    public void Configure(EntityTypeBuilder<DoctorDetail> builder)
    {
        builder.HasKey(key => key.Id);
        builder.Property(p => p.AppointmentPrice).HasColumnType("money");

    }
}
