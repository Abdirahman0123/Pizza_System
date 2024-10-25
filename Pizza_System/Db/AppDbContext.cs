using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pizza_System.Model;
using System.Linq;
using System.Security.Principal;
using System.Security.Principal;
using System.Security.Claims;
using Pizza_System.Services;
using System.Reflection.Metadata;
using System.Reflection.Emit;

namespace Pizza_System.Db
{
    public class AppDbContext : IdentityDbContext<User>
    {
        //private readonly IGetUserClaims claims;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
         //this.claims = claims;
        }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>()
                                               .Ignore(c => c.LockoutEnabled)
                                               .Ignore(c => c.NormalizedUserName)
                                               .Ignore(c => c.PhoneNumberConfirmed)
                                               .Ignore(c => c.TwoFactorEnabled)
                                               .Ignore(c => c.LockoutEnd)
                                               .Ignore(c => c.LockoutEnabled)                                             
                                               .Ignore(c => c.AccessFailedCount);*/

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Order>(x => x.HasKey(p => new { p.OrderId, p.UserId, p.MenuId }));

            builder.Entity<Order>()
                .HasOne(u => u.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(p => p.UserId)
                ;

            builder.Entity<Order>()
                .HasOne(u => u.Menu)
                .WithMany(u => u.Orders)
                .HasForeignKey(p => p.MenuId)
                ;

            //var user = accessor.HttpContext.User.Identity.Name;
            //var user = User.findFirstValue(ClaimTypes.NameIdentifier);
            //builder.Entity<Order>().ApplyGlobalFilters(u => u.UserId == claims.CurrentUserId);
            //builder.Entity<Order>().HasQueryFilter(d => d.UserId == claims.CurrentUserId);
            //ModelBuilder.ApplyGlobalFilters<Order>(x => x.DeletedAt == null);
            //builder.Entity<Order>().HasQueryFilter(x => x.UserId == claims.CurrentUserId);

        }
}
}
