using System.Linq;
using PictureTagger.Repositories;

namespace PictureTagger.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using PictureTagger.Models;
    using PictureTagger.Models.ViewModels;
    using PictureTagger.Models.ApiModels;

    public partial class Tag
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tag()
        {
            Pictures = new HashSet<Picture>();
        }

        public int TagID { get; set; }

        [Column("Tag")]
        [Required]
        [StringLength(20)]
        public string TagLabel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Picture> Pictures { get; set; }

        private static IRepository<Picture> pictureRepository = new DatabaseRepository<Picture>(true);

        public static implicit operator Tag(TagView tagView)
        {
            return new Tag()
            {
                TagID = tagView.TagID,
                TagLabel = tagView.TagLabel,
                Pictures = tagView.Pictures.Select(p => (Picture)p).ToList()
            };
        }

        public static implicit operator Tag(TagApi tagApi)
        {
            return new Tag()
            {
                TagID = tagApi.TagID,
                TagLabel = tagApi.TagLabel,
                Pictures = tagApi.PicturesIds.Select(p => pictureRepository.Get(p)).ToList()
            };
        }
    }
}
