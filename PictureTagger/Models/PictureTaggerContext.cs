namespace PictureTagger.Models
{
	using System.Data.Entity;

	public partial class PictureTaggerContext : DbContext
	{
		public PictureTaggerContext()
			: base("name=DefaultConnection")
		{
		}

		public virtual DbSet<Picture> Pictures { get; set; }
		public virtual DbSet<Tag> Tags { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Picture>()
				.Property(e => e.FileType)
				.IsUnicode(false);

			modelBuilder.Entity<Picture>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<Picture>()
				.HasMany(e => e.Tags)
				.WithMany(e => e.Pictures)
				.Map(m => m.ToTable("PictureTags").MapLeftKey("PictureID").MapRightKey("TagID"));

			modelBuilder.Entity<Tag>()
				.Property(e => e.TagLabel)
				.IsUnicode(false);
		}
	}
}
