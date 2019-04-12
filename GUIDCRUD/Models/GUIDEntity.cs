using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GUIDCRUD.Models
{
    /// <summary>
    /// Initaites the GUID Entity class
    /// </summary>
    public partial class GUIDEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public GUIDEntity()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public GUIDEntity(string user)
        {
            User = user;
        }

        /// <summary>
        /// 
        /// </summary>
        public string GuidValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string User { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        public int? Expire { get; set; }

    }

    /// <summary>
    /// Setup the configuration
    /// </summary>
    public class GUIDConfiguration : IEntityTypeConfiguration<GUIDEntity>
    {

        /// <summary>
        /// setup object
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<GUIDEntity> builder)
        {
            builder.ToTable("GUIDEntity", "dbo");

            builder.HasKey(p => p.GuidValue);

            builder.Property(p => p.User).HasColumnType("varchar(100))").IsRequired();           

            builder.Property(p => p.Expire).HasColumnType("int");

        }
    }

    /// <summary>
    /// Load the DB context
    /// </summary>
    public class GUIDEntityDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public GUIDEntityDbContext(DbContextOptions<GUIDEntityDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Constructing Model to load the data after reterival
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GUIDConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<GUIDEntity> GuidEntities { get; set; }
    }
}
