using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Contracts.TypeCodes;

namespace Core.TypeCodes
{
	/// <summary>
	/// Base class for type codes
	/// </summary>
	/// <typeparam name="T">Type of code property</typeparam>
	/// <typeparam name="TTypeCode">Type of type code</typeparam>
	public abstract class TypeCodeBase<T, TTypeCode> : ITypeCodeBase<T> where TTypeCode : TypeCodeBase<T, TTypeCode>, ITypeCodeBase<T>
	{
		public string Description { get; }
		public T Code { get; }

		protected static ConcurrentDictionary<T, TTypeCode> AllTypes = new ConcurrentDictionary<T, TTypeCode>();

		public static IEnumerable<TTypeCode> Options => AllTypes.Values;

		protected TypeCodeBase (T code, string description)
		{
			Code = code;
			Description = description;
			AllTypes[code] = (TTypeCode)this;
		}

		/// <summary>
		/// Converts <see cref="TypeCodeBase&lt;T, TTypeCode&gt;"/> to <see cref="int"/>
		/// </summary>
		/// <param name="typeCode">Type of code property</param>
		public static implicit operator T (TypeCodeBase<T, TTypeCode> typeCode)
		{
			return typeCode.Code;
		}

		public static bool operator == (TypeCodeBase<T, TTypeCode> a, TypeCodeBase<T, TTypeCode> b)
		{
			if (ReferenceEquals(a, b))
			{
				return true;
			}

			// If one is null, but not both, return false.
			if ((object)a == null || (object)b == null)
			{
				return false;
			}

			// Return true if the fields match:
			return a.Equals(b);
		}

		public static bool operator != (TypeCodeBase<T, TTypeCode> a, TypeCodeBase<T, TTypeCode> b)
		{
			return !(a == b);
		}


		/// <summary>
		/// Creates code instance from its code representation
		/// </summary>
		public static TTypeCode Create (T code, TTypeCode defaultValue = null)
		{
			if (!AllTypes.ContainsKey(code))
			{
				throw new ArgumentException("Not supported code provided: " + code);
			}

			return AllTypes[code];
		}

		#region Equals

		protected bool Equals (TypeCodeBase<T, TTypeCode> other)
		{
			return string.Equals(Description, other.Description) && EqualityComparer<T>.Default.Equals(Code, other.Code);
		}

		public override bool Equals (object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;

			return Equals((TypeCodeBase<T, TTypeCode>)obj);
		}

		public override int GetHashCode ()
		{
			return EqualityComparer<T>.Default.GetHashCode(Code);
		}

		#endregion

		public override string ToString ()
		{
			return Code + " - " + Description;
		}
	}
}