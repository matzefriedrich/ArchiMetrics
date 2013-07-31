// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyntaxDeclarations.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the SyntaxDeclarations type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.Analysis.Metrics
{
	using System.Collections.Generic;
	using Roslyn.Compilers.CSharp;

	internal class SyntaxDeclarations
	{
		public IEnumerable<NamespaceDeclarationSyntax> NamespaceDeclarations { get; set; }

		public IEnumerable<TypeDeclarationSyntax> TypeDeclarations { get; set; }

		public IEnumerable<MemberDeclarationSyntax> MemberDeclarations { get; set; }
		
		public IEnumerable<StatementSyntax> Statements { get; set; }
	}
}