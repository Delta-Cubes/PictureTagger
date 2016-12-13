using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureTagger.Models.ApiModels
{
    public class PictureApi
    {
        public int PictureID { get; set; }
        public string OwnerID { get; set; }
        public string Hash { get; set; }
        public byte[] ThumbnailData { get; set; }
        public string ThumbnailBase64Data { get; set; }
        public string Name { get; set; }
        public virtual ICollection<int> TagsIds { get; set; }

        public static implicit operator PictureApi(Picture picture)
        {
            return new PictureApi()
            {
                PictureID = picture.PictureID,
                Name = picture.Name,
                Hash = picture.Hash,
                ThumbnailData = picture.ThumbnailData,
                ThumbnailBase64Data = $"data:image/jpeg;base64,{Convert.ToBase64String(picture.ThumbnailData)}",
                OwnerID = picture.OwnerID,
                TagsIds = picture.Tags.Select(t=> t.TagID).ToList()
            };
        }
    }
}