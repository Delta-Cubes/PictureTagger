using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureTagger.Models.ViewModels
{
	public class PictureView
	{
		public int PictureID { get; set; }
		public string OwnerID { get; set; }
        public string Hash { get; set; }
        public byte[] ThumbnailData { get; set; }
		public string ThumbnailBase64Data { get; set; }
		public string Name { get; set; }
		public virtual ICollection<TagView> Tags { get; set; }

		public static implicit operator PictureView(Picture picture)
		{
			return new PictureView()
			{
				PictureID = picture.PictureID,
				Name = picture.Name,
                Hash = picture.Hash,
				ThumbnailData = picture.ThumbnailData,
				ThumbnailBase64Data = $"data:image/jpeg;base64,{Convert.ToBase64String(picture.ThumbnailData)}",
				OwnerID = picture.OwnerID,
                Tags = picture.Tags.Select(t => new TagView() {
                    TagID = t.TagID,
                    TagLabel = t.TagLabel
                }).ToList()
			};
		}
	}
}