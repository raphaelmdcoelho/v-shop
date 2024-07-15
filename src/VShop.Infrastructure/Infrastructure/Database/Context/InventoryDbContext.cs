using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=vshop.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO: create configuration files:
        // modelBuilder.ApplyConfiguration(new ProductConfiguration());
        // modelBuilder.ApplyConfiguration(new CategoryConfiguration());

        modelBuilder.Entity<Product>().ToTable("Product");        

        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(p => p.Description)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(p => p.Quantity)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(p => p.ImageUrl)
            .IsRequired(false);

        modelBuilder.Entity<Product>()
            .Property(p => p.CreatedAt)
            .IsRequired();  

        modelBuilder.Entity<Product>()
            .Property(p => p.UpdatedAt)
            .IsRequired(false);

        modelBuilder.Entity<Product>()
            .Property(p => p.IsDeleted)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<Category>().ToTable("Category");

        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired();

        modelBuilder.Entity<Category>()
            .Property(c => c.Description)
            .IsRequired();

        modelBuilder.Entity<Category>()
            .Property(c => c.CreatedAt)
            .IsRequired();

        modelBuilder.Entity<Category>()
            .Property(c => c.UpdatedAt)
            .IsRequired(false);

        modelBuilder.Entity<Category>()
            .Property(c => c.IsDeleted)
            .IsRequired();

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Electronic products", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false },
            new Category { Id = 2, Name = "Clothing", Description = "Clothing products", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false }
        );
        
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Description = "Laptop", Price = 1000, Quantity = 10, ImageUrl = "https://www.google.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false, CategoryId = 1 },
            new Product { Id = 2, Name = "T-shirt", Description = "T-shirt", Price = 20, Quantity = 100, ImageUrl = "https://www.google.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false, CategoryId = 2 }
        );

        // TODO: move seed to dedicated file
        // TODO: Apply migrations
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
}