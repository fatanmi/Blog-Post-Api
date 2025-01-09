using Microsoft.EntityFrameworkCore;
using ServerLibrary.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> option) : base(option) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly define the one-to-many relationship between Post and Comment
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)           // Post has many Comments
                .WithOne()                          // Each Comment has one Post
                .HasForeignKey(c => c.PostId)       // Foreign key in Comment table
                .OnDelete(DeleteBehavior.Cascade);  // Optional: Cascade delete behavior

            base.OnModelCreating(modelBuilder);
        }
    }
}
