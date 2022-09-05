using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData
            (
                new User
                {
                    Id = new Guid("4f590a70-94ce-4d15-9494-5248280c2ce3"),
                    Username = "Admin_mediasoup_server",
                    Avatar = "im am here",
                    FirstName = "mediasoup",
                    LastName = "server",
                    IsAdmin = true,
                    PhoneNumber = "09009009000",
                    CreateDateTime = DateTime.UtcNow,
                    IsRegisterCompleted = true,
                    RefreshToken = "qkjbcoi238yehasd"
                }
            );
        }
    }
}