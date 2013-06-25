using System.Collections.Generic;
using System.Linq;
using ArchiCop.InfoData;

namespace ArchiCop.Core
{
    public class ArchiCopGraphEngine
    {
        //private readonly List<ArchiCopGraph<ArchiCopVertex>> _dataSources = new List<ArchiCopGraph<ArchiCopVertex>>();
        //private readonly List<ArchiCopGraph<ArchiCopVertex>> _visualStudioDataSources = new List<ArchiCopGraph<ArchiCopVertex>>();
        //private readonly List<ArchiCopGraph<ArchiCopVertex>> _graphs = new List<ArchiCopGraph<ArchiCopVertex>>();

        public static ArchiCopGraph<ArchiCopVertex> GetGraph(DataSourceInfo dataSourceInfo)
        {
            ArchiCopGraph<ArchiCopVertex> graph = GetGraph(dataSourceInfo.LoadEngine);
            
            graph.DisplayName = dataSourceInfo.DisplayName;

            return graph;
        }

        public static ArchiCopGraph<ArchiCopVertex> GetGraph(GraphInfo graphInfo)
        {
            IEnumerable<VertexRegexRule> rules = graphInfo.Rules.Select(
                item =>
                new VertexRegexRule
                {
                    Pattern = item.RulePattern,
                    Value = item.RuleValue
                });

            ArchiCopGraph<ArchiCopVertex> graph = GetGraph(graphInfo.DataSource.LoadEngine, rules.ToArray());

            graph.DisplayName = graphInfo.DisplayName;

            return graph;
        }

        //public ArchiCopGraphEngine(ConfigInfo configInfo)
        //{
        //    IEnumerable<GraphInfo> graphData = configInfo.Graphs;
        //    foreach (string graphName in graphData.GroupBy(item => item.Name).Select(g => g.Key))
        //    {
        //        ArchiCopGraph<ArchiCopVertex> info =
        //            GetGraphFromGraphInfo(graphData.First(item => item.Name == graphName));
        //        _graphs.Add(info);
        //    }

        //    IEnumerable<DataSourceInfo> datasourcesData = configInfo.DataSources;
        //    foreach (string dataSource in datasourcesData.Where(item=>item.LoadEngine.EngineType==LoadEngineType.Data).GroupBy(item => item.Name).Select(g => g.Key))
        //    {
        //        ArchiCopGraph<ArchiCopVertex> info =
        //            GetDataSourceFromDataSourceInfo(datasourcesData.First(item => item.Name == dataSource));
        //        _dataSources.Add(info);
        //    }

        //    IEnumerable<DataSourceInfo> visualStudioDataSourcesData = configInfo.DataSources;
        //    foreach (string dataSource in visualStudioDataSourcesData.Where(item=>item.LoadEngine.EngineType==LoadEngineType.VisualStudio).GroupBy(item => item.Name).Select(g => g.Key))
        //    {
        //        ArchiCopGraph<ArchiCopVertex> info =
        //            GetDataSourceFromDataSourceInfo(visualStudioDataSourcesData.First(item => item.Name == dataSource));
        //        _visualStudioDataSources.Add(info);
        //    }
        //}

        //public IEnumerable<ArchiCopGraph<ArchiCopVertex>> DataSources
        //{
        //    get { return _dataSources; }
        //}

        //public IEnumerable<ArchiCopGraph<ArchiCopVertex>> VisualStudioDataSources
        //{
        //    get { return _visualStudioDataSources; }
        //}

        //public IEnumerable<ArchiCopGraph<ArchiCopVertex>> Graphs
        //{
        //    get { return _graphs; }
        //}

        //private ArchiCopGraph<ArchiCopVertex> GetDataSourceFromDataSourceInfo(DataSourceInfo dataSource)
        //{
        //    ArchiCopGraph<ArchiCopVertex> graph = GetGraph(dataSource.LoadEngine);

        //    graph.DisplayName = dataSource.Name;

        //    return graph;
        //}

        //private ArchiCopGraph<ArchiCopVertex> GetGraphFromGraphInfo(GraphInfo graphInfo)
        //{
        //    IEnumerable<VertexRegexRule> rules = graphInfo.Rules.Select(
        //        item =>
        //        new VertexRegexRule
        //            {
        //                Pattern = item.RulePattern,
        //                Value = item.RuleValue
        //            });

        //    ArchiCopGraph<ArchiCopVertex> graph = GetGraph(graphInfo.DataSource.LoadEngine, rules.ToArray());

        //    graph.DisplayName = graphInfo.DisplayName;

        //    return graph;
        //}

        private static ArchiCopGraph<ArchiCopVertex> GetGraph(LoadEngineInfo loadEngineInfo, params VertexRegexRule[] vertexRegexRules)
        {            
            var loadEngine = (ILoadEngine) loadEngineInfo.CreateLoadEngine();
            var graph = loadEngine.LoadGraph();

            if (vertexRegexRules.Any())
            {
                var edges = new EdgeEngineRegex().ConvertEdges(graph.Edges, vertexRegexRules);
                graph=new ArchiCopGraph<ArchiCopVertex>();
                graph.AddVerticesAndEdgeRange(edges);
            }

            return graph;
        }
    }
}