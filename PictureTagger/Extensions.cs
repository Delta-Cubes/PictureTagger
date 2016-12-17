using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PictureTagger
{
	public static class Extensions
	{
		/// <summary>
		/// Call implicit/explicit cast overloads for each item in a collection.
		/// Why is this required?  https://github.com/dotnet/corefx/issues/14511
		/// </summary>
		/// <typeparam name="T">New type to cast to.</typeparam>
		/// <param name="collection">Collection of old types.</param>
		public static IEnumerable<T> RealCast<T>(this IEnumerable<dynamic> collection)
			where T : class => collection.Select(i => (T)i);

		/// <summary>
		/// Call implicit/explicit cast overloads for each item in a collection.
		/// </summary>
		/// <typeparam name="T">New type to cast to.</typeparam>
		/// <param name="collection">Collection of old types.</param>
		public static IQueryable<T> RealCast<T>(this IQueryable<object> collection)
			where T : class => collection.Select(i => (T)i);

		/// <summary>
		/// Convert a string with potentially filesystem-unsafe characters into a hex string.
		/// </summary>
		/// <param name="str">Potentially unsafe string.</param>
		/// <returns>Hex encoded version of the string</returns>
		public static string SafeString(this string str)
		{
			var builder = new StringBuilder();
			for (int i = 0; i < str.Length; ++i)
				builder.Append(Convert.ToString(str[i], 16));

			return builder.ToString();
		}
	}
}