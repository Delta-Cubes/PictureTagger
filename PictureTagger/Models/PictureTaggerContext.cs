namespace PictureTagger.Models
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class PictureTaggerContext : DbContext
	{
		public PictureTaggerContext()
			: base("name=PictureTaggerContext")
		{
		}

		public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
		public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
		public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
		public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
		public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
		public virtual DbSet<Picture> Pictures { get; set; }
		public virtual DbSet<Tag> Tags { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AspNetRole>()
				.HasMany(e => e.AspNetUsers)
				.WithMany(e => e.AspNetRoles)
				.Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

			modelBuilder.Entity<AspNetUser>()
				.HasMany(e => e.AspNetUserClaims)
				.WithRequired(e => e.AspNetUser)
				.HasForeignKey(e => e.UserId);

			modelBuilder.Entity<AspNetUser>()
				.HasMany(e => e.AspNetUserLogins)
				.WithRequired(e => e.AspNetUser)
				.HasForeignKey(e => e.UserId);

			modelBuilder.Entity<AspNetUser>()
				.HasMany(e => e.Pictures)
				.WithRequired(e => e.AspNetUser)
				.HasForeignKey(e => e.OwnerID)
				.WillCascadeOnDelete(false);

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
				.Property(e => e.Tag1)
				.IsUnicode(false);
		}
	}
}
