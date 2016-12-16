using System.Linq;
using PictureTagger.Repositories;

namespace PictureTagger.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using ViewModels;
	using ApiModels;

	public partial class Picture
	{
		public int PictureID { get; set; }

		[Required]
		[StringLength(128)]
		public string OwnerID { get; set; }

		[Required]
		public byte[] ThumbnailData { get; set; }

		[Required]
		public string Hash { get; set; }

		[Required]
		[StringLength(60)]
		public string Name { get; set; }

		public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

		private static IRepository<Tag> tagRepository = new DatabaseRepository<Tag>(null, false);

		public static implicit operator Picture(PictureView pictureView)
		{
			return new Picture()
			{
				PictureID = pictureView.PictureID,
				Name = pictureView.Name,
                Hash = pictureView.Hash,
				ThumbnailData = pictureView.ThumbnailData,
				OwnerID = pictureView.OwnerID,
				Tags = pictureView.Tags.Select(t => tagRepository.Find(t.TagID)).ToList()
			};
		}

		public static implicit operator Picture(PictureApi pictureApi)
		{
			return new Picture()
			{
				PictureID = pictureApi.PictureID,
				Name = pictureApi.Name,
                Hash = pictureApi.Hash,
				ThumbnailData = pictureApi.ThumbnailData,
				OwnerID = pictureApi.OwnerID,
				Tags = pictureApi.TagsIds.Select(t => tagRepository.Find(t)).ToList()
			};
		}
	}
}