using System.Linq;
using PictureTagger.Repositories;

namespace PictureTagger.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using ViewModels;
	using ApiModels;

	public partial class Tag
	{
		public int TagID { get; set; }

		[Column("Tag")]
		[Required]
		[StringLength(20)]
		public string TagLabel { get; set; }

		public virtual ICollection<Picture> Pictures { get; set; } = new HashSet<Picture>();

		private static IRepository<Picture> pictureRepository = new DatabaseRepository<Picture>(null, false);

		public static implicit operator Tag(TagView tagView)
		{
			return new Tag()
			{
				TagID = tagView.TagID,
				TagLabel = tagView.TagLabel,
				Pictures = tagView.Pictures.Select(p => pictureRepository.Find(p.PictureID)).ToList()
			};
		}

		public static implicit operator Tag(TagApi tagApi)
		{
			return new Tag()
			{
				TagID = tagApi.TagID,
				TagLabel = tagApi.TagLabel,
				Pictures = tagApi.PicturesIds.Select(p => pictureRepository.Find(p)).ToList()
			};
		}
	}
}