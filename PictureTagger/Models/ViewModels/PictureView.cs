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
        public byte[] Data { get; set; }
        public string Base64Data { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TagView> Tags { get; set; }

        public static explicit operator PictureView(Picture picture)
        {
            return new PictureView()
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