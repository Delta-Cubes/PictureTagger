using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureTagger.Models.ViewModels
{
    public class TagView
    {
        public int TagID { get; set; }
        public string TagLabel { get; set; }
        public bool TagSelected { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}