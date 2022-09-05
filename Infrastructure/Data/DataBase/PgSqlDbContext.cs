using Domain.Entities;
using Infrastructure.Data.EntityConfigs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DataBase
{
    public class PgSqlDbContext : DbContext
    {
        public PgSqlDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating((modelBuilder));
            modelBuilder.ApplyConfiguration(new WorldConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new LiveStreamConfiguration());
            modelBuilder.ApplyConfiguration(new LinkConfiguration());

            modelBuilder.Entity<Participants>(entity =>
            {
                entity.HasKey(p => new { p.UserId, p.LiveStreamId });
            });

            modelBuilder.Entity<Node>(entity =>
            {
                entity.HasKey(n => new { n.LinkId, n.LiveStreamId });
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<World> Categories { get; set; }
        public DbSet<LiveStream> LiveStreams { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<World> Worlds { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Node> Nodes { get; set; }
    }
}