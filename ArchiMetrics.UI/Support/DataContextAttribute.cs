// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContextAttribute.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the DataContextAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.UI.Support
{
	using System;

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DataContextAttribute : Attribute
	{
		public DataContextAttribute(Type dataContextType)
		{
			DataContextType = dataContextType;
		}

		public Type DataContextType { get; private set; }
	}
}