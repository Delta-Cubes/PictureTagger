using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PictureTagger
{
	public static class DI
	{
		private static Dictionary<Type, Type> _transients { get; } = new Dictionary<Type, Type>();
		private static Dictionary<object, HashSet<object>> _scoped { get; } = new Dictionary<object, HashSet<object>>();

		/// <summary>
		/// Return a list of known types in the current context.
		/// </summary>
		/// <param name="context">Scope to check.</param>
		private static IEnumerable<Type> GetKnownTypes(object context)
		{
			var knownTransients = _transients.Select(s => s.Key);

			if (context == null) return knownTransients;

			var knownScoped = _scoped
				.FirstOrDefault(t => t.Key == context)
				.Value
				.Select(t => t.GetType());

			return knownTransients.Union(knownScoped);
		}

		/// <summary>
		/// Get whatever instance is currently available for the type in the container.
		/// </summary>
		/// <param name="context">Scope to check.</param>
		/// <param name="type">Type to find.</param>
		private static object GetInstance(object context, Type type)
		{
			// First look for transients
			var transientImpl = _transients
				.FirstOrDefault(s => s.Key == type)
				.Value;

			if (transientImpl != null)
			{
				return Instantiate<object>(transientImpl, context);
			}

			if (context == null) return null;

			// If nothing was found, attempt to find a matching scoped
			var scopedImpl = _scoped
				.Where(t => t.Key == context)
				.Select(t => t.Value)
				.FirstOrDefault()
				.FirstOrDefault(t => t.GetType() == type);

			return scopedImpl;
		}

		/// <summary>
		/// Registers a class which will be instantiated when required.
		/// </summary>
		/// <typeparam name="TInterface">Interface that is to be implemented.</typeparam>
		/// <typeparam name="TImplementation">Class implementing the interface.</typeparam>
		internal static void RegisterTransient<TInterface, TImplementation>()
			where TImplementation : class
		{
			_transients.Add(typeof(TInterface), typeof(TImplementation));
		}

		/// <summary>
		/// Register a class that already exists, scoped to a context.
		/// </summary>
		/// <typeparam name="TItem">Type of item to register.</typeparam>
		/// <param name="context">Key to determine which scope the container should look in.</param>
		/// <param name="item">Instantiated class to return.</param>
		internal static void RegisterScoped<TItem>(object context, TItem item)
		{
			if (context == null) throw new ArgumentNullException(nameof(context), "Scope variable cannot be null.");
			lock (_scoped)
			{
				if (!_scoped.ContainsKey(context)) _scoped[context] = new HashSet<object>();
				_scoped[context].Add(item);
			}
		}

		internal static void UnregisterScoped(object context)
		{
			_scoped.Remove(context);
		}

		/// <summary>
		/// Instantiate a class, injecting known objects into the constructor where available.
		/// </summary>
		/// <typeparam name="T">Type to cast the new object as.</typeparam>
		/// <param name="type"></param>
		/// <param name="context">Context used to look for scoped (optional).</param>
		/// <returns></returns>
		internal static T Instantiate<T>(Type type, object context = null)
			where T : class
		{
			var knownTypes = GetKnownTypes(context);

			// Find a compatible constructor to inject into
			var injectConstructor = type
				.GetConstructors()
				.Where(c => c.GetParameters().Length > 0)
				.FirstOrDefault(c => c.GetParameters()
					.Where(p => !p.HasDefaultValue)
					.Select(p => p.ParameterType)
					.Distinct()
					.Except(knownTypes)
					.Count() == 0);

			if (injectConstructor != null)
			{
				var parameters = InjectParameters(context, injectConstructor).ToArray();
				return (T)injectConstructor.Invoke(parameters);
			}

			// No compatible constructor found, attempt to call the parameterless constructor
			return (T)Activator.CreateInstance(type);
		}

		private static IEnumerable<object> InjectParameters(object context, ConstructorInfo constructor)
		{
			foreach (var param in constructor.GetParameters())
				yield return GetInstance(context, param.ParameterType);
		}
	}
}