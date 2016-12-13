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
        public virtual ICollection<PictureView> Pictures { get; set; }

        public static implicit operator TagView(Tag tag)
        {
            return new TagView()
            {
                TagID = tag.TagID,
                TagLabel = tag.TagLabel,
                Pictures = tag.Pictures.RealCast<PictureView>().ToList()
            };
        }
    }
}