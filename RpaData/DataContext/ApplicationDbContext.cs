using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RpaData.Models;

namespace RpaData.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<tblPharmacy> tblPharmacies { get; set; }
        public DbSet<tblCertificates> tblCertificates { get; set; }
        public DbSet<tblDocuments> tblDocuments { get; set; }
        public DbSet<tblPayments> tblPayments { get; set; }
        public DbSet<tblPharmacists> tblPharmacists { get; set; }
        public DbSet<tblSubscriptions> tblSubscriptions { get; set; }
        public DbSet<tblEvents> tblEvents { get; set; }
        public DbSet<tblEventsHistory> tblEventsHistory { get; set; }
        public DbSet<tblCodes> tblCodes { get; set; }
        public DbSet<tblEmail> tblEmails { get; set; }
        public DbSet<tblCommunication> tblCommunications { get; set; }
        public DbSet<tblCommunicationLogs> tblCommunicationLogs { get; set; }
        public virtual DbSet<tblInvoices> tblInvoices { get; set; }
        public virtual DbSet<tblQualifications> tblQualifications { get; set; }
        public virtual DbSet<tblQualifications_Pharmacist> tblQualifications_Pharmacists { get; set; }
        public virtual DbSet<tblResources> tblResources { get; set; }
        public virtual DbSet<tblMailingList> tblMailingList { get; set; }
        public virtual DbSet<tblMailingListClients> tblMailingListClients { get; set; }
        public virtual DbSet<tblJobs> tblJobs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

               base.OnModelCreating(modelBuilder);
                    modelBuilder.Entity<tblEventsHistory>()
                        .HasOne<tblEvents>(s => s.Event)
                        .WithMany(g => g.tblEventsHistory)
                        .HasForeignKey(s => s.EventId);


            modelBuilder.Entity<tblPayments>()
                        .Property(o => o.AmountPaid)
                         .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<tblInvoices>()
                        .Property(o => o.Amount)
                        .HasColumnType("decimal(18,4)");
        }
    }
}
