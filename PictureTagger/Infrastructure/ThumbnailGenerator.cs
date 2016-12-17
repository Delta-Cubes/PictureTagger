using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PictureTagger.Infrastructure
{
	public class ThumbnailGenerator
	{
		/// <summary>
		/// Returns a small but extremely low-quality JPEG of an image.
		/// </summary>
		/// <param name="data">Image data to resize.</param>
		public static byte[] Generate(Stream data)
		{
			// Create a bitmap from the raw image data
			using (var bits = new Bitmap(data))

			// Load it into a new bitmap of size 32x32
			using (var resized = new Bitmap(bits, new Size(32, 32)))

			// Output it as a JPEG with 25% quality
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