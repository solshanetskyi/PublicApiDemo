namespace LabyrinthGenerator
{
    using System.Collections.Generic;
    using System.Linq;

    public class Node
    {
        private List<Node> _nodes;

        private Node _parent;

        private string _name;

        public Node(string name)
        {
            this._name = name;
            this._nodes = new List<Node>();
        }

        public Node(Node parent, string name) :this(name)
        {
            this._parent = parent;
        }

        public List<Node> Nodes
        {
            get
            {
                return this._nodes;
            }
        }

        public int Level
        {
            get
            {
                int level = 1;
                Node parent = this._parent;

                while (parent != null)
                {
                    level++;
                    parent = parent.Parent;
                }

                return level;
            }
        }

        public Node Parent
        {
            get
            {
                return this._parent;
            }
        }

        public int GetSize()
        {
            if (this._nodes.Count == 0)
                return Settings.DefaultWidth;

            return this._nodes.Sum(n => n.GetSize()) + (this._nodes.Count - 1) * Settings.Scale;
        }

        public int GetMaxDepth()
        {
            if (this._nodes.Count == 0)
                return this.Level;

            return this._nodes.Max(n => n.GetMaxDepth());
        }

        public int GetMaxWith()
        {
            if (this._nodes.Count == 0)
                return Settings.DefaultWidth;

            return this._nodes.Count + (Settings.Scale + Settings.DefaultHeight);
        }
    }
}