using System.Linq;
using PictureTagger.Repositories;

namespace PictureTagger.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using PictureTagger.Models.ViewModels;
    using PictureTagger.Models.ApiModels;

    public partial class Picture
    {
        public int PictureID { get; set; }

        [Required]
        [StringLength(128)]
        public string OwnerID { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

		private static IRepository<Tag> tagRepository = new DatabaseRepository<Tag>(true);

		public static implicit operator Picture(PictureView pictureView)
        {
            return new Picture()
            {
                PictureID = pictureView.PictureID,
                Name = pictureView.Name,
                FileType = pictureView.FileType,
                Data = pictureView.Data,
                OwnerID = pictureView.OwnerID,
                Tags = pictureView.TagsIds.Select(t => tagRepository.Find(t)).ToList()
            };
        }

        public static implicit operator Picture(PictureApi pictureApi)
        {
            return new Picture()
            {
                PictureID = pictureApi.PictureID,
                Name = pictureApi.Name,
                FileType = pictureApi.FileType,
                Data = pictureApi.Data,
                OwnerID = pictureApi.OwnerID,
                Tags = pictureApi.TagsIds.Select(t => tagRepository.Find(t)).ToList()
            };
        }
    }
}
