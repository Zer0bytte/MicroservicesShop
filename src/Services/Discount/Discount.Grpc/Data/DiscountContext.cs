using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; }

    public DiscountContext(DbContextOptions<DiscountContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Coupon>().HasData(
            new Coupon { Id = 1, ProductName = "IphoneX", Description = " SDSDS", Amount = 115 },
            new Coupon { Id = 2, ProductName = "Iphone13", Description = "dasdasd", Amount = 199 }
            );
    }
}
