using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Shockky.IO;

namespace Shockky.Utilities
{
    public class ObjectCreator
    {
        public static ObjectActivator<T> GetItemConstructor<T>()
        {
            var ctor = typeof(T).GetConstructors().FirstOrDefault();

            if(ctor == null)
                throw new InvalidOperationException("No constructors found!");

            var param = Expression.Parameter(typeof(ShockwaveReader), "input");

            var lambda = Expression.Lambda(
                typeof(ObjectActivator<T>),
                Expression.New(ctor, param),
                param);
            
            return (ObjectActivator<T>)lambda.Compile();
        }

        public delegate T ObjectActivator<T>(ShockwaveReader input);
    }
}
