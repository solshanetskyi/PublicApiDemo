using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LabyrinthGeneration
{
    public class Node
    {
        private string _name;

        public Node(string name)
        {
            _name = name;
            Nodes = new List<Node>();
        }

        public Node(Node parent, string name) : this(name)
        {
            Parent = parent;
        }

        public List<Node> Nodes { get; private set; }

        public string Name
        {
            get { return _name; }
        }

        public int Level
        {
            get
            {
                var level = 1;
                var parent = Parent;

                while (parent != null)
                {
                    level++;
                    parent = parent.Parent;
                }

                return level;
            }
        }

        public Node Parent { get; private set; }

        public int GetSize()
        {
            if (Nodes.Count == 0)
                return Settings.DefaultWidth;

            return Nodes.Sum(n => n.GetSize()) + (Nodes.Count - 1)*Settings.Scale;
        }

        public int GetMaxDepth()
        {
            if (Nodes.Count == 0)
                return Level;

            return Nodes.Max(n => n.GetMaxDepth());
        }

        public int GetMaxWith()
        {
            if (Nodes.Count == 0)
                return Settings.DefaultWidth;

            return Nodes.Count + Settings.Scale + Settings.DefaultHeight;
        }
    }
}