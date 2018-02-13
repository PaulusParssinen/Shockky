using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Shockky.Shockwave.Lingo.Bytecode.Roles
{
	public abstract class Role //TODO: Whatsup with roleindex
	{
		public const int RoleIndexBits = 9;

		static readonly Role[] roles = new Role[1 << RoleIndexBits];
		static int nextRoleIndex = 0;

		public uint Index { get; }
		
		internal Role()
		{
			Index = (uint)Interlocked.Increment(ref nextRoleIndex);
			if (Index >= roles.Length)
				throw new InvalidOperationException("Too many roles");
			roles[Index] = this;
		}

		public abstract bool IsValid(object node);

		public static Role GetByIndex(uint index) => roles[index];
	}

	public class Role<T> : Role where T : class
	{
		public string Name { get; }
		public T NullObject { get; }

		public Role(string name, T nullObject)
			: this(name)
		{
			NullObject = nullObject;
		}
		public Role(string name)
		{
			Name = name;
		}

		public override bool IsValid(object node) => node is T;
	
		public override string ToString() => Name;
	}
}
