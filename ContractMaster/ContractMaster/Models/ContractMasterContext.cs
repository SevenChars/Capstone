using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContractMaster.models
{
    public partial class ContractMasterContext : DbContext
    {
        public ContractMasterContext()
        {
        }

        public ContractMasterContext(DbContextOptions<ContractMasterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Buyer> Buyer { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<ContactPerson> ContactPerson { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<ContractDetail> ContractDetail { get; set; }
        public virtual DbSet<Contractor> Contractor { get; set; }
        public virtual DbSet<MainCategory> MainCategory { get; set; }
        public virtual DbSet<ProjectData> ProjectData { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=.\\sqlexpress;database=ContractMaster;Trusted_Connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Buyer>(entity =>
            {
                entity.Property(e => e.BuyerId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BuyerEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BuyerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Buyer)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_client_buyer");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.HasOne(d => d.ClientNavigation)
                    .WithOne(p => p.Client)
                    .HasForeignKey<Client>(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_company_client");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PositalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContactPerson>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithOne(p => p.ContactPersonNavigation)
                    .HasForeignKey<ContactPerson>(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_company_contact");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.Property(e => e.ContractId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AddDate).HasColumnType("date");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<ContractDetail>(entity =>
            {
                entity.HasKey(e => e.ContractId);

                entity.Property(e => e.ContractId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BidDate).HasColumnType("date");

                entity.Property(e => e.BidNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BuyerId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContractName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContractorId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.ContractDetail)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Buyer_ContractDetail");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ContractDetail)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MainCategory_ContractDetail");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ContractDetail)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Client_ContractDetail");

                entity.HasOne(d => d.Contract)
                    .WithOne(p => p.ContractDetail)
                    .HasForeignKey<ContractDetail>(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_ContractDetail");

                entity.HasOne(d => d.Contractor)
                    .WithMany(p => p.ContractDetail)
                    .HasForeignKey(d => d.ContractorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contractor_ContractDetail");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.ContractDetail)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubCategory_ContractDetail");
            });

            modelBuilder.Entity<Contractor>(entity =>
            {
                entity.Property(e => e.ContractorId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ProjectCoordinator)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectPrincipal)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContractorNavigation)
                    .WithOne(p => p.Contractor)
                    .HasForeignKey<Contractor>(d => d.ContractorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_company_contractor");
            });

            modelBuilder.Entity<MainCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProjectData>(entity =>
            {
                entity.HasKey(e => e.ContractId);

                entity.Property(e => e.ContractId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActualCompeletionDate).HasColumnType("date");

                entity.Property(e => e.ActualDays)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ActualStartDate).HasColumnType("date");

                entity.Property(e => e.ContractAwardAmount).HasColumnType("money");

                entity.Property(e => e.ContractCompletionAmount).HasColumnType("money");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PromisedDays)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenderTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contract)
                    .WithOne(p => p.ProjectData)
                    .HasForeignKey<ProjectData>(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_ProjectData");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.Property(e => e.SubcategoryId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubcategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_MainCategory_SubCategory");
            });
        }
    }
}
