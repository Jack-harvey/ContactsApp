using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContactsApp.tempModel
{
    public partial class ContactsAppDataContext : DbContext
    {
        public ContactsAppDataContext()
        {
        }

        public ContactsAppDataContext(DbContextOptions<ContactsAppDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<CompanyOffice> CompanyOffices { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<VwCompanyCount> VwCompanyCounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=ContactsAppData;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CompanyOffice>(entity =>
            {
                entity.Property(e => e.OfficeId).ValueGeneratedNever();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyOffices)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyOffices_Companies");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.ContactId).ValueGeneratedNever();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contacts_Categories");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Contacts_Companies");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VwCompanyCount>(entity =>
            {
                entity.ToView("vwCompanyCount");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
