using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LabyrinthGeneration
{
    public class Node
    {
        private string _path;
        private string _name;
        private string _color;

        private List<DeviceInfo> _deviceInfos;

        public Node(string path, string name, string color)
        {
            _path = path;
            _name = name;
            _color = color;

            Nodes = new List<Node>();

            _deviceInfos = new List<DeviceInfo>();
        }

        public Node(Node parent, string path, string name, string color) : this(path, name, color)
        {
            Parent = parent;
        }

        public void SetDevices(IEnumerable<DeviceInfo> devices)
        {
            _deviceInfos.AddRange(devices);
        }

        public List<Node> Nodes { get; private set; }

        public string Path
        {
            get { return _path; }
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

        public string Name
        {
            get { return _name; }
        }

        public string Color
        {
            get { return _color; }
        }

        public List<DeviceInfo> Devices
        {
            get { return _deviceInfos; }
        }

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

    public class DeviceInfo
    {
        public string Name { get; set; }
    }
}