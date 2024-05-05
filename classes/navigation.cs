namespace deliver
{
    public class Node
    {
        public string Id {get;}
        public List<Edge> Edges {get; set;}     //connections to neighbours

        public Node(string id)
        {
            Id = id;
            Edges = new List<Edge>();
        }
    }
    public class Edge
    {
        public Node To {get;}       // destination node
        public Node From {get;}     // origin node
        public int Distance {get;}

        public Edge(Node from, Node to, int dist)
        {
            To = to;
            From = from;
            Distance = dist;
        }
    }
    public class Dijkstra
    {
        public Dictionary<Node, int> Distances {get; private set;}      //stores the node's distance from source node
        public Dictionary<Node, Node> Previous {get; private set;}      //

        public void Execute(Node source)
        {
            List<Node> nodes = new List<Node>();
            // gather the neighbour nodes of source node
            foreach(Edge edge in source.Edges)
            {
                nodes.Add(edge.To);
            }

            Distances = new ();
            Previous = new Dictionary<Node, Node>();

            //priority queue sorts distances from smallest to source node to largest
            SortedSet<Node> priorityQueue = new SortedSet<Node>(Comparer<Node>.Create((node1, node2) => Distances[node1].CompareTo(Distances[node2])));
            
            foreach (Node node in nodes)
            {
                Distances[node]= int.MaxValue;
                Previous[node] = null;
                priorityQueue.Add(node);
            }
            Distances[source] = 0;

            while(priorityQueue.Count() > 0)
            {
                Node current = priorityQueue.Min;       //get the node with the shortest distance
                priorityQueue.Remove(current);
                foreach(Edge edge in current.Edges)
                {
                    int alt = Distances[current] + edge.Distance;
                    if(alt < Distances[edge.To])
                    {
                        Distances[edge.To] = alt;
                        Previous[edge.To] = current;
                    }
                    // refresh priority queue
                    priorityQueue.Remove(edge.To);
                    priorityQueue.Add(edge.To);
                }

            }
        }
    }
}