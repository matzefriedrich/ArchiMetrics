// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphViewModel.cs" company="Roche">
//   Copyright � Roche 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the GraphViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiCop.UI.ViewModel
{
	using System.Collections.Generic;
	using System.Linq;
	using ArchiMeter.Analysis;
	using ArchiMeter.Common;

	internal class GraphViewModel : WorkspaceViewModel
	{
		private readonly DependencyAnalyzer _analyzer = new DependencyAnalyzer();
		private readonly IEdgeTransformer _filter;
		private readonly IEdgeItemsRepository _repository;
		private EdgeItem[] _allEdges;
		private ProjectGraph _graphToVisualize;

		public GraphViewModel(IEdgeItemsRepository repository, IEdgeTransformer filter)
		{
			_repository = repository;
			_filter = filter;
			LoadAllEdges();
		}

		public ProjectGraph GraphToVisualize
		{
			get
			{
				return _graphToVisualize;
			}

			private set
			{
				if (_graphToVisualize != value)
				{
					_graphToVisualize = value;
					RaisePropertyChanged();
				}
			}
		}

		public override void Update(bool forceUpdate)
		{
			if (forceUpdate)
			{
				LoadAllEdges();
			}
			else
			{
				UpdateInternal();
			}
		}

		protected override void Dispose(bool isDisposing)
		{
			_graphToVisualize = null;
			_allEdges = null;
			base.Dispose(isDisposing);
		}

		private async void UpdateInternal()
		{
			IsLoading = true;
			var g = new ProjectGraph();

			var nonEmptySourceItems = (await _filter.TransformAsync(_allEdges))
				.ToArray();

			var circularReferences = (await _analyzer.GetCircularReferences(nonEmptySourceItems))
				.ToArray();

			var projectVertices = nonEmptySourceItems
				.AsParallel()
				.SelectMany(item =>
					{
						var isCircular = circularReferences.Any(c => c.Contains(item));
						return CreateVertices(item, isCircular);
					})
				.GroupBy(v => v.Name)
				.Select(grouping => grouping.First())
				.ToArray();

			var edges =
				nonEmptySourceItems
				.Where(e => !string.IsNullOrWhiteSpace(e.Dependency))
				.Select(
					dependencyItemViewModel =>
					new ProjectEdge(
						projectVertices.First(item => item.Name == dependencyItemViewModel.Dependant), 
						projectVertices.First(item => item.Name == dependencyItemViewModel.Dependency)))
								   .Where(e => e.Target.Name != e.Source.Name);

			foreach (var vertex in projectVertices)
			{
				g.AddVertex(vertex);
			}

			foreach (var edge in edges)
			{
				g.AddEdge(edge);
			}

			GraphToVisualize = g;
			IsLoading = false;
		}

		private IEnumerable<Vertex> CreateVertices(EdgeItem item, bool isCircular)
		{
			yield return new Vertex(item.Dependant, isCircular, item.DependantComplexity, item.DependantMaintainabilityIndex, item.DependantLinesOfCode);
			if (!string.IsNullOrWhiteSpace(item.Dependency))
			{
				yield return
					new Vertex(item.Dependency, isCircular, item.DependencyComplexity, item.DependencyMaintainabilityIndex, item.DependencyLinesOfCode, item.CodeIssues);
			}
		}

		private async void LoadAllEdges()
		{
			IsLoading = true;
			var edges = await _repository.GetEdgesAsync();
			_allEdges = edges.Where(e => e.Dependant != e.Dependency).ToArray();
			UpdateInternal();
		}
	}
}