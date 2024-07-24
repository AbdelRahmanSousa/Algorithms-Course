using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public static class PlagiarismChecking
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given an UNDIRECTED Graph of matching pairs and a query pair, find the min number of connections between the nodes of the given pair (if any)
        /// </summary>
        /// <param name="edges">array of matching pairs</param>
        /// <param name="query">query pair</param>
        /// <returns>min number of connections between the nodes of the query pair (if any)</returns>
        private class QueueItem {
            internal int pathLength;
            internal string currentVertex;
        }
        public static int CheckPlagiarism(Tuple<string, string>[] edges, Tuple<string, string> query)
        {
            Dictionary<string, List<string>> adjacencyGraph = new Dictionary<string, List<string>>();
            foreach(var edge in edges)
            {
                if (!adjacencyGraph.ContainsKey(edge.Item1)) { adjacencyGraph.Add(edge.Item1, new List<string>() { edge.Item2 }); }
                else adjacencyGraph[edge.Item1].Add(edge.Item2);
                if (!adjacencyGraph.ContainsKey(edge.Item2)) { adjacencyGraph.Add(edge.Item2, new List<string>() { edge.Item1 }); } 
                else adjacencyGraph[edge.Item2].Add(edge.Item1);
            }
            HashSet<string> already = new HashSet<string>();
            Queue<QueueItem> traversedPaths = new Queue<QueueItem>();
            int best = adjacencyGraph.Count;
            traversedPaths.Enqueue(new QueueItem() { pathLength = 0, currentVertex = query.Item1});
            while (traversedPaths.Count > 0) {
                QueueItem currentItem = traversedPaths.Dequeue();
                already.Add(currentItem.currentVertex);
                if (adjacencyGraph[currentItem.currentVertex].Contains(query.Item2) && best > currentItem.pathLength + 1) 
                    best = currentItem.pathLength + 1;
                if (currentItem.pathLength + 1 < best)
                {
                    foreach (var x in adjacencyGraph[currentItem.currentVertex])
                    {
                        if (!already.Contains(x))
                        {
                            traversedPaths.Enqueue(new QueueItem() { pathLength = currentItem.pathLength + 1, currentVertex = x}); 
                        }
                    }
                }
            }
            if (best == adjacencyGraph.Count) { return 0; }
            return best;
        }
        #endregion
    }
}
