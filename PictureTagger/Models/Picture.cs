namespace PictureTagger.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using ViewModels;
	using ApiModels;

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

		public static implicit operator Picture(PictureView pictureView)
        {
            return new Picture()
            {
                PictureID = pictureView.PictureID,
                Name = pictureView.Name,
                FileType = pictureView.FileType,
                Data = pictureView.Data,
                OwnerID = pictureView.OwnerID
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
                OwnerID = pictureApi.OwnerID
            };
        }
    }
}
