using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HealthInsurance.Models;

public partial class HealthInsuranceContext : DbContext
{
    public HealthInsuranceContext()
    {
    }

    public HealthInsuranceContext(DbContextOptions<HealthInsuranceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Hospital> Hospitals { get; set; }

    public virtual DbSet<PoliciesEmp> PoliciesEmps { get; set; }

    public virtual DbSet<PoliciesReqDetail> PoliciesReqDetails { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<PolicyAppDetail> PolicyAppDetails { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-EOU16SFT\\SQLEXPRESS;Initial Catalog=HealthInsurance;User ID=sa;Password=1234567;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__2D971CAC471A4EAD");

            entity.ToTable("Company");

            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompanyUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyURL");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11F620A7EA");

            entity.ToTable("Employee");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Joindate).HasColumnType("datetime");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PolicyStatus)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Employee__IdAcco__2D27B809");

            entity.HasOne(d => d.Policy).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PolicyId)
                .HasConstraintName("FK__Employee__Policy__2C3393D0");
        });

        modelBuilder.Entity<Hospital>(entity =>
        {
            entity.HasKey(e => e.HospitalId).HasName("PK__Hospital__38C2E5AFE3FDE2E6");

            entity.ToTable("Hospital");

            entity.Property(e => e.HospitalName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HospitalUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HospitalURL");
            entity.Property(e => e.Location)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PoliciesEmp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PoliciesEmp");

            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Emi)
                .HasColumnType("money")
                .HasColumnName("EMI");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PolicyAmount).HasColumnType("money");
            entity.Property(e => e.PolicyDuration).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PolicyEnddate).HasColumnType("datetime");
            entity.Property(e => e.PolicyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PolicyStartdate).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany()
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__PoliciesE__Compa__35BCFE0A");

            entity.HasOne(d => d.Hospital).WithMany()
                .HasForeignKey(d => d.HospitalId)
                .HasConstraintName("FK__PoliciesE__Hospi__36B12243");

            entity.HasOne(d => d.Policy).WithMany()
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PoliciesE__Polic__34C8D9D1");
        });

        modelBuilder.Entity<PoliciesReqDetail>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Policies__33A8517A665487AD");

            entity.ToTable("PoliciesReqDetail");

            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Emi)
                .HasColumnType("money")
                .HasColumnName("EMI");
            entity.Property(e => e.PolicyAmount).HasColumnType("money");
            entity.Property(e => e.PolicyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.PoliciesReqDetails)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__PoliciesR__Compa__30F848ED");

            entity.HasOne(d => d.Policy).WithMany(p => p.PoliciesReqDetails)
                .HasForeignKey(d => d.PolicyId)
                .HasConstraintName("FK__PoliciesR__Polic__300424B4");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__Policies__2E1339A469DDDBA0");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Emi)
                .HasColumnType("money")
                .HasColumnName("EMI");
            entity.Property(e => e.PolicyDescription)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PolicyName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PolicyAppDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PolicyAppDetail");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Reason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Request).WithMany()
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK__PolicyApp__Reque__32E0915F");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PK__UserLogi__B7B00CE38F42CB28");

            entity.ToTable("UserLogin");

            entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
