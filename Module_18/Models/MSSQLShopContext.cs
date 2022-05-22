using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Module_18
{
    public partial class MSSQLShopContext : DbContext
    {
        public MSSQLShopContext()
        {
        }

        public MSSQLShopContext(DbContextOptions<MSSQLShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<PurchaseClient> PurchaseClients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MSSQLShop;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.EmailClient)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameClient)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PatronymicClient)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SurnameClient)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PurchaseClient>(entity =>
            {

                entity.ToTable("PurchaseClient");

                entity.Property(e => e.EmailBuyer)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameProduct)
                    .IsRequired()
                    .HasMaxLength(70);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
