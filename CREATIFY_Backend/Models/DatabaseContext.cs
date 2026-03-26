using Microsoft.EntityFrameworkCore;

namespace CREATIFY_Backend.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        
        public DbSet<FormSubmission> FormSubmissions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<FormSubmission>().ToTable("FormSubmissions");
            
            modelBuilder.Entity<FormSubmission>()
                .HasIndex(f => f.SubmittedAt);
                
            modelBuilder.Entity<FormSubmission>()
                .HasIndex(f => f.FormType);
                
            modelBuilder.Entity<FormSubmission>()
                .HasIndex(f => f.Status);
        }
    }
}