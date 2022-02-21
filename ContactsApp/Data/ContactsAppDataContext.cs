using System;
using System.Collections.Generic;
using ContactsApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContactsApp.Data
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=localhost;Database=ContactsAppData;Trusted_Connection=True;");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).ValueGeneratedNever();

                entity.Property(e => e.Abn)
                    .HasMaxLength(11)
                    .HasColumnName("ABN")
                    .IsFixedLength();

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.FoundingDate).HasColumnType("date");

                entity.Property(e => e.Website)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<CompanyOffice>(entity =>
            {
                entity.HasKey(e => e.OfficeId);

                entity.Property(e => e.OfficeId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.PostCode)
                    .HasMaxLength(4)
                    .IsFixedLength();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyOffices)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyOffices_Companies");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.ContactId).ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Company)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .HasMaxLength(75)
                    .IsFixedLength();

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Mobile)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Notes)
                    .HasMaxLength(300)
                    .IsFixedLength();

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Picture)
                    .HasMaxLength(255)
                    .IsFixedLength();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Contacts_Categories");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
