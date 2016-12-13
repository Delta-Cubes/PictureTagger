using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace PictureTagger.Infrastructure
{
	public class ThumbnailGenerator
	{
		/// <summary>
		/// Returns a small, low-quality JPEG of an image.
		/// </summary>
		/// <param name="data">Image data to resize.</param>
		public static byte[] Generate(Stream data)
		{
			using (var bits = new Bitmap(data))
			using (var resized = new Bitmap(bits, new Size(32, 32)))
			using (var output = new MemoryStream())
			{
				var codec = ImageCodecInfo.GetImageEncoders().First(e => e.MimeType == "image/jpeg");
				var encoderParams = new EncoderParameters(1);
				encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 25L);

				resized.Save(output, codec, encoderParams);
				return output.ToArray();
			}
		}
	}
}