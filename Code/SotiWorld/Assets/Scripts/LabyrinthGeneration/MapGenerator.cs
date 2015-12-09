using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Integration;

namespace Assets.Scripts.LabyrinthGeneration
{
    public static class MapGenerator
    {
        public static Matrix GenerateMap(DeviceGroup[] deviceGroups, Device[] devices)
        {
            Node rootNode = new Node("Default", "", "Red");

            DeviceGroup[] orderedDeviceGroups = deviceGroups.OrderBy(g => g.Level).ToArray();

            List<Node> nodes = new List<Node>();
            nodes.Add(rootNode);

            foreach (DeviceGroup deviceGroup in orderedDeviceGroups)
            {
                AddGroup(rootNode, deviceGroup, nodes, devices);
            }

            Matrix matrix = RoomGenerator.Generate(rootNode);
            
            matrix.Tiles[matrix.Tiles.Count/2][3].Add(new Tile(TileType.Player));

            //Closing the front door
            foreach (var tiles in matrix.Tiles)
            {
                if (tiles[0].Any(t => t.TileType == TileType.Door))
                {
                    tiles[0].RemoveAll(t => t.TileType == TileType.Door);
                    tiles[0].Add(new Tile(TileType.Wall));
                }
            }

            return matrix;
        }

        private static void AddGroup(Node topNode, DeviceGroup deviceGroup, List<Node> processedNodes, Device[] devices)
        {
            string[] paths = deviceGroup.Path.Split(new[] {@"\", @"\\"}, StringSplitOptions.RemoveEmptyEntries);

            Node groupNode = null;

            if (paths.Length == 1)
            {
                groupNode = new Node(topNode, deviceGroup.Path, deviceGroup.Name, deviceGroup.Icon);

                groupNode.SetDevices(devices.Where(d => d.Path == deviceGroup.Path).Select(d => new DeviceInfo
                {
                    DeviceId = d.DeviceId
                }));

                topNode.Nodes.Add(groupNode);
                processedNodes.Add(groupNode);

                return;
            }

            int indexOfName = deviceGroup.Path.LastIndexOf(@"\", StringComparison.Ordinal);

            string parentPath = deviceGroup.Path.Remove(indexOfName);

            Node parentNode = processedNodes.Single(n => n.Path == parentPath);

            groupNode = new Node(parentNode, deviceGroup.Path, deviceGroup.Name, deviceGroup.Icon);

            groupNode.SetDevices(devices.Where(d => d.Path == deviceGroup.Path).Select(d => new DeviceInfo
            {
                DeviceId = d.DeviceId
            }));

            parentNode.Nodes.Add(groupNode);
            processedNodes.Add(groupNode);
        }
    }
}
