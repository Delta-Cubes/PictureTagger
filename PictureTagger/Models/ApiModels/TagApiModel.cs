using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureTagger.Models.ApiModels
{
    public class TagApiModel
    {
        public int TagID { get; set; }
        public string TagLabel { get; set; }
        public bool TagSelected { get; set; }
        public virtual ICollection<Int32> PicturesIds { get; set; }
    }
}