using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PictureTagger
{
	/// <summary>
	/// Homemade probably garbage Dependency Injection/IoC thing!
	/// </summary>
	public static class DI
	{
		/// <summary>
		/// Entities to instiate every time they are requested.
		/// </summary>
		private static Dictionary<Type, Type> _transients { get; } = new Dictionary<Type, Type>();

		/// <summary>
		/// Already-instantiated entities scoped to an object reference.
		/// </summary>
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

			// Transients are valuable so use them first
			if (transientImpl != null)
			{
				return Instantiate<object>(transientImpl, context);
			}

			// No context?  No scope?  No deal.
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

		/// <summary>
		/// Remove all registered objects in the context.
		/// </summary>
		/// <param name="context">Context reference.</param>
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
			// Must have parameters, otherwise why bother
			// Don't care about parameters with default values
			// Only care about distinct types for now
			// The Except(knownTypes) thing is just my crappy solution to test if all things are satisfied, probably could do the same with .All() or something
			var injectConstructor = type
				.GetConstructors()
				.Where(c => c.GetParameters().Length > 0)
				.FirstOrDefault(c => c.GetParameters()
					.Where(p => !p.HasDefaultValue)
					.Select(p => p.ParameterType)
					.Distinct()
					.Except(knownTypes)
					.Count() == 0);

			// If a valid constructor was found, use it
			if (injectConstructor != null)
			{
				var parameters = InjectParameters(context, injectConstructor).ToArray();
				return (T)injectConstructor.Invoke(parameters);
			}

			// No compatible constructor found, attempt to call the parameterless constructor
			return (T)Activator.CreateInstance(type);
		}

		/// <summary>
		/// Lazily get instances for each parameter in a valid constructor.
		/// </summary>
		/// <param name="context">Context reference to look in for scoped entities.</param>
		/// <param name="constructor">Valid constructor accepting nothing but known/registered types.</param>
		private static IEnumerable<object> InjectParameters(object context, ConstructorInfo constructor)
		{
			foreach (var param in constructor.GetParameters())
				yield return GetInstance(context, param.ParameterType);
		}
	}
}