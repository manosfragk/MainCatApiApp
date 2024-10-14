using CatApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CatApiApp.Data
{
    /// <summary>
    /// Represents the database context for the Cat API application, responsible for managing the interaction
    /// with the Cats and Tags entities in the database.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DataContext"/> class.
    /// </remarks>
    /// <param name="options">The options to be used by the DbContext.</param>
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {

        /// <summary>
        /// Gets or sets the collection of <see cref="CatEntity"/> stored in the database.
        /// </summary>
        public DbSet<CatEntity> Cats { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="TagEntity"/> stored in the database.
        /// </summary>
        public DbSet<TagEntity> Tags { get; set; }

        /// <summary>
        /// Configures the relationships between the Cats and Tags entities.
        /// </summary>
        /// <param name="modelBuilder">The builder used to define the entity relationships.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-many relationship between Cats and Tags
            modelBuilder.Entity<CatEntity>()
                .HasMany(c => c.Tags)
                .WithMany(t => t.Cats)
                .UsingEntity<Dictionary<string, object>>(
                "CatEntityTagEntity",
                j => j.HasOne<TagEntity>().WithMany().HasForeignKey("TagsId"),
                j => j.HasOne<CatEntity>().WithMany().HasForeignKey("CatsId")
            );
        }
    }
}
