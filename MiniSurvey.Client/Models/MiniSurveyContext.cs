using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class MiniSurveyContext : DbContext
    {
        public MiniSurveyContext()
        {
        }

        public MiniSurveyContext(DbContextOptions<MiniSurveyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionOption> QuestionOptions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserResponse> UserResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json");

                IConfiguration Configuration = builder.Build();

                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("MiniSurveyConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Option>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<QuestionOption>(entity =>
            {
                entity.HasOne(d => d.Option)
                    .WithMany(p => p.QuestionOptions)
                    .HasForeignKey(d => d.OptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionOptions_Options");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionOptions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionOptions_Questions");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DateRegistered).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserResponse>(entity =>
            {
                entity.Property(e => e.DateResponded).HasColumnType("datetime");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.UserResponses)
                    .HasForeignKey(d => d.OptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserResponses_Options");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.UserResponses)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserResponses_Questions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserResponses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserResponses_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
