namespace PictureTagger.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Picture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Picture()
        {
            Tags = new HashSet<Tag>();
        }

        public int PictureID { get; set; }

        [Required]
        [StringLength(128)]
        public string OwnerID { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        [StringLength(4)]
        public string FileType { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
