using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureTagger.Models.ViewModels
{
	public class TagView
    {
        public int TagID { get; set; }
        public string TagLabel { get; set; }
        public bool TagSelected { get; set; }
        public virtual ICollection<PictureView> Pictures { get; set; }

        public static implicit operator TagView(Tag tag)
        {
            return new TagView()
            {
                TagID = tag.TagID,
                TagLabel = tag.TagLabel,
                Pictures = tag.Pictures.Select(p => new PictureView() {
                    PictureID = p.PictureID,
                    Name = p.Name,
                    Hash = p.Hash,
                    ThumbnailData = p.ThumbnailData,
                    ThumbnailBase64Data = $"data:image/jpeg;base64,{Convert.ToBase64String(p.ThumbnailData)}",
                    OwnerID = p.OwnerID
                }).ToList()
            };
        }
    }
}