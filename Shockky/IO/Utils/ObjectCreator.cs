using System;
using System.Linq;
using System.Linq.Expressions;
using Shockky.Shockwave;

namespace Shockky.IO.Utils
{
	public class ObjectCreator
	{
		public static ObjectActivator<T> GetShockwaveItemConstructor<T>() 
		    where T : ShockwaveItem
		{
			var ctor = typeof(T).GetConstructors().FirstOrDefault() 
			           ?? throw new InvalidOperationException("No constructors found");

			var param = Expression.Parameter(typeof(ShockwaveReader));

			var lambda = Expression.Lambda(
				typeof(ObjectActivator<T>),
				Expression.New(ctor, param),
				param);

			return (ObjectActivator<T>)lambda.Compile();
		}

		public delegate T ObjectActivator<out T>(ShockwaveReader input);
	}
}