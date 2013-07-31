// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentLanguageRuleBase.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the CommentLanguageRuleBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.CodeReview.Trivia
{
	using System.Linq;
	using System.Text.RegularExpressions;

	using Common;
	using Roslyn.Compilers.CSharp;

	internal abstract class CommentLanguageRuleBase : TriviaEvaluationBase
	{
		private readonly ISpellChecker _spellChecker;
		private static readonly Regex XmlRegex = new Regex("<.+?>", RegexOptions.Compiled);

		public CommentLanguageRuleBase(ISpellChecker spellChecker)
		{
			_spellChecker = spellChecker;
		}

		protected override EvaluationResult EvaluateImpl(SyntaxTrivia node)
		{
			var commentWords = node.ToFullString()
								   .Trim('/', '*')
								   .Trim()
								   .Split(' ')
								   .Select(RemoveXml)
								   .Select(s => s.TrimEnd('.', ','))
								   .ToArray();
			var errorCount = commentWords.Aggregate(0, (i, s) => i + (_spellChecker.Spell(s) ? 0 : 1));
			if (errorCount >= 0.50 * commentWords.Length)
			{
				return new EvaluationResult
						   {
							   Comment = "Suspicious language comment",
							   ErrorCount = 1,
							   ImpactLevel = ImpactLevel.Member,
							   Quality = CodeQuality.NeedsReview,
							   QualityAttribute = QualityAttribute.Maintainability | QualityAttribute.Conformance,
							   Snippet = node.ToFullString()
						   };
			}

			return null;
		}

		private string RemoveXml(string input)
		{
			return XmlRegex.Replace(input, string.Empty);
		}
	}
}