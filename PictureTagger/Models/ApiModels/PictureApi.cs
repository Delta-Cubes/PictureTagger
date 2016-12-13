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
        public byte[] Data { get; set; }
        public string Base64Data { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
        public virtual ICollection<int> TagsIds { get; set; }

        public static implicit operator PictureApi(Picture picture)
        {
            return new PictureApi()
            {
                PictureID = picture.PictureID,
                Name = picture.Name,
                Data = picture.ThumbnailData,
                Base64Data = $"data:image/jpeg;base64,{Convert.ToBase64String(picture.ThumbnailData)}",
                OwnerID = picture.OwnerID,
                TagsIds = picture.Tags.Select(t=> t.TagID).ToList()
            };
        }
    }
}