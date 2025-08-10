
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Orderly.Server.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductTag> ProductTags { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ChatThread> ChatThreads { get; set; }

        public DbSet<ChatParticipant> ChatParticipants { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure user table

            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable("UserTbl");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.PublicId)
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(12);

                entity.HasIndex(u => u.PublicId)
                    .IsUnique();

                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Address)
                    .HasMaxLength(250);

                entity.Property(u => u.CreatedAt)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("RoleTbl");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRolesTbl");
            });

            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaimsTbl");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLoginsTbl");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaimsTbl");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserTokensTbl");
            });

            // Configure the product table

            builder.Entity<Product>(entity =>
            {
                entity.ToTable("ProductTbl");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.PublicId)
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(12);

                entity.HasIndex(p => p.PublicId)
                    .IsUnique();

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(p => p.Category)
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Stock)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(p => p.Sales)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(p => p.OwnerId)
                    .IsRequired();

                entity.HasOne(p => p.Owner)
                    .WithMany(u => u.Products)
                    .HasForeignKey(p => p.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.CreatedAt)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            // Configure product tag table

            builder.Entity<ProductTag>(entity =>
            {
                entity.ToTable("ProductTagTbl");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(t => t.ProductId)
                    .IsRequired();

                entity.HasOne(t => t.Product)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(t => t.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure the order table

            builder.Entity<Order>(entity =>
            {
                entity.ToTable("OrderTbl");

                entity.HasKey(o => o.Id);

                entity.Property(o => o.PublicId)
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(12);

                entity.HasIndex(o => o.PublicId)
                    .IsUnique();

                entity.HasOne(o => o.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(o => o.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Customer)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            // Configure chat thread table

            builder.Entity<ChatThread>(entity =>
            {
                entity.ToTable("ChatThreadTbl");

                entity.HasKey(ct => ct.Id);

                entity.Property(ct => ct.PublicId)
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(12);

                entity.Property(ct => ct.Subject)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(ct => ct.ContextType)
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(ct => ct.ContextId)
                    .IsRequired();

                entity.Property(ct => ct.CreatedAt)
                    .IsRequired();

                entity.Property(u => u.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            // Configure the chat participant table

            builder.Entity<ChatParticipant>(entity =>
            {
                entity.ToTable("ChatParticipantTbl");

                entity.HasKey(cp => new { cp.ChatThreadId, cp.UserId });

                entity.Property(cp => cp.ChatThreadId)
                    .IsRequired();

                entity.HasOne(cp => cp.ChatThread)
                    .WithMany(ct => ct.Participants)
                    .HasForeignKey(p => p.ChatThreadId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cp => cp.User)
                    .WithMany()
                    .HasForeignKey(cp => cp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure the chat message table

            builder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessageTbl");

                entity.Property(cm => cm.Id)
                    .IsRequired();

                entity.Property(cm => cm.PublicId)
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(12);

                entity.Property(cm => cm.ChatThreadId)
                    .IsRequired();

                entity.HasOne(cm => cm.ChatThread)
                    .WithMany(ct => ct.Messages)
                    .HasForeignKey(cm => cm.ChatThreadId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(cm => cm.SenderId)
                    .IsRequired();

                entity.HasOne(cm => cm.Sender)
                    .WithMany()
                    .HasForeignKey(cm => cm.SenderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(cm => cm.Content)
                    .IsRequired()
                    .HasMaxLength(5000);

                entity.Property(cm => cm.CreatedAt)
                    .IsRequired();

                entity.Property(u => u.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);
            });
        }
    }
}
