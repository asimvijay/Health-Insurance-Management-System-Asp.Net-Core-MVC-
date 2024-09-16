using Microsoft.EntityFrameworkCore;

namespace HealthInsurance.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<AdminAccount> Admin{ get; set; }
        public DbSet<CompanyDetails> Companies { get; set; }
        public DbSet<EmpRegister> Employees { get; set; }
        public DbSet<HospitalInfo> Hospitals { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PoliciesOnEmployees> PoliciesOnEmployees { get; set; }
        public DbSet<PolicyApprovalDetails> PolicyApprovals { get; set; }
        public DbSet<PolicyRequestDetails> PolicyRequests { get; set; }
        public DbSet<PolicyTotalDescription> PolicyDescriptions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique constraint on UserName for AdminAccount
            modelBuilder.Entity<AdminAccount>()
                .HasIndex(a => a.UserName)
                .IsUnique();

            // Configuring relationships
            modelBuilder.Entity<EmpRegister>()
                .HasOne(e => e.Company)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Company)
                .WithMany(c => c.Policies)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PoliciesOnEmployees>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.Policies)
                .HasForeignKey(pe => pe.EmpNo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PoliciesOnEmployees>()
                .HasOne(pe => pe.Policy)
                .WithMany(p => p.PoliciesOnEmployees)
                .HasForeignKey(pe => pe.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PolicyApprovalDetails>()
                .HasOne(pa => pa.Policy)
                .WithMany()
                .HasForeignKey(pa => pa.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PolicyRequestDetails>()
                .HasOne(pr => pr.Employee)
                .WithMany()
                .HasForeignKey(pr => pr.EmpNo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PolicyRequestDetails>()
                .HasOne(pr => pr.Policy)
                .WithMany()
                .HasForeignKey(pr => pr.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PolicyRequestDetails>()
                .HasOne(pr => pr.Company)
                .WithMany()
                .HasForeignKey(pr => pr.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
