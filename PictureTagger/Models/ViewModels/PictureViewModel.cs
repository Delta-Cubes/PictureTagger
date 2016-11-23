using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureTagger.Models.ViewModels
{
    public class PictureViewModel
    {
        public int PictureID { get; set; }
        public string OwnerID { get; set; }
        public byte[] Data { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TagViewModel> Tags { get; set; }
    }
}