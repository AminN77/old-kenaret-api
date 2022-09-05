using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class LiveStreamConfiguration : IEntityTypeConfiguration<LiveStream>
    {
        public void Configure(EntityTypeBuilder<LiveStream> builder)
        {
           
        }
    }
}