// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HiddenTypeDependencyRuleTests.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2013
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the HiddenTypeDependencyRuleTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.CodeReview.Rules.Tests.Rules.Semantic
{
	using System.Threading.Tasks;
	using ArchiMetrics.Analysis;
	using ArchiMetrics.CodeReview.Rules.Semantic;
	using ArchiMetrics.Common;
	using NUnit.Framework;
	using Roslyn.Compilers;

	public sealed class HiddenTypeDependencyRuleTests
	{
		private HiddenTypeDependencyRuleTests()
		{
		}

		public class GivenAHiddenTypeDependencyRule : SolutionTestsBase
		{
			private NodeReviewer _inspector;

			[SetUp]
			public void Setup()
			{
				_inspector = new NodeReviewer(new[] { new HiddenTypeDependencyRule() });
			}

			[TestCase(@"namespace MyNamespace
{
	public class MyFactory<T> where T : new()
{
	public T Create()
	{
		return new T();
	}
}

	public class MyClass
	{
		public object GetItem()
		{
			var factory = new Factory<ArchiMetrics.Common.ModelSettings>();
			return factory.Create();
		}
	}
}")]
			public async Task WhenMethodContainsHiddenDependencyThenReturnsError(string code)
			{
				var references = new[] { new MetadataFileReference(typeof(ModelSettings).Assembly.Location) };
				var solution = CreateSolution(references, code);
				var results = await _inspector.Inspect(solution);

				CollectionAssert.IsNotEmpty(results);
			}
		}
	}
}