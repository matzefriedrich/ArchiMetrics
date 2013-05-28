﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeCoupling.cs" company="Roche">
//   Copyright © Roche 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TypeCoupling type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ArchiMeter.Common.Metrics
{
	using System;

	public class TypeCoupling : IComparable
	{
		private readonly string _fullName;

		public TypeCoupling(string className, string namespaceName, string assemblyName)
		{
			ClassName = className;
			Namespace = namespaceName;
			Assembly = assemblyName;
			_fullName = string.Format("{0}.{1}, {2}", namespaceName, className, assemblyName);
		}

		public string ClassName { get; set; }

		public string Namespace { get; set; }

		public string Assembly { get; set; }

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj"/> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj"/>. Greater than zero This instance follows <paramref name="obj"/> in the sort order. 
		/// </returns>
		/// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception>
		public int CompareTo(object obj)
		{
			var other = obj as TypeCoupling;
			return other == null
					   ? -1
					   : string.Compare(_fullName, other._fullName, StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool operator ==(TypeCoupling c1, TypeCoupling c2)
		{
			return ReferenceEquals(c1, null)
					   ? ReferenceEquals(c2, null)
					   : c1.CompareTo(c2) == 0;
		}

		public static bool operator !=(TypeCoupling c1, TypeCoupling c2)
		{
			return ReferenceEquals(c1, null)
					   ? !ReferenceEquals(c2, null)
					   : c1.CompareTo(c2) != 0;
		}

		public static bool operator <(TypeCoupling c1, TypeCoupling c2)
		{
			return !ReferenceEquals(c1, null) && c1.CompareTo(c2) < 0;
		}

		public static bool operator >(TypeCoupling c1, TypeCoupling c2)
		{
			return !ReferenceEquals(c1, null) && c1.CompareTo(c2) > 0;
		}

		public override string ToString()
		{
			return _fullName;
		}

		public override int GetHashCode()
		{
			return _fullName.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return CompareTo(obj) == 0;
		}
	}
}