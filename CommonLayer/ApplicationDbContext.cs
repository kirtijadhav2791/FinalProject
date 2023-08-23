using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<UserDetail> UserDetail { get; set; }

        public DbSet<AddressDetails> AddressDetails { get; set; }

        public DbSet<CardDetails> CardDetails { get; set; }

        public DbSet<CustomerDetails> CustomerDetails { get; set; }

        public DbSet<FeedbackDetail> FeedbackDetail { get; set; }

        public DbSet<ProductDetails> ProductDetails { get; set; }

        public DbSet<WishListDetails> WishListDetails { get; set; }
    }
}
