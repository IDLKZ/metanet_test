using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Metanet.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace Metanet.Server.Database
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,Role,string,IdentityUserClaim<string>,
        UserRole,IdentityUserLogin<string>,IdentityRoleClaim<string>,IdentityUserToken<string>
        >
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Adding Auto Generated Id
            builder
           .Entity<ApplicationUser>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
            //Adding Next and Previous Lesson
            builder.Entity<Lesson>().HasOne(p => p.PrevLesson).WithOne().OnDelete(DeleteBehavior.SetNull); 
             builder.Entity<Lesson>().HasOne(p => p.NextLesson).WithOne().OnDelete(DeleteBehavior.SetNull);

            //For ApplicationUser -> Roles and UserRole
            builder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            builder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            
            //For Unique Index


        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Material> Materials { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        
    }
}
