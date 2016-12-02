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
        public virtual ICollection<Int32> TagsIds { get; set; }

        public static implicit operator PictureApi(Picture picture)
        {
            return new PictureApi()
            {
                PictureID = picture.PictureID,
                Name = picture.Name,
                FileType = picture.FileType,
                Data = picture.Data,
                OwnerID = picture.OwnerID
            };
        }
    }
}