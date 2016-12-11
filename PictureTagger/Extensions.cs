using System.Collections.Generic;
using System.Linq;

namespace PictureTagger
{
	public static class Extensions
	{
		/// <summary>
		/// Call implicit/explicit cast overloads for each item in a collection.
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
	}
}