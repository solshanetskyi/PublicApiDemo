﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Integration;

namespace Assets.Scripts.LabyrinthGeneration
{
    public static class MapGenerator
    {
        public static string GenerateMap(DeviceGroup[] deviceGroups)
        {
            Node rootNode = new Node("Default");

            DeviceGroup[] orderedDeviceGroups = deviceGroups.OrderBy(g => g.Level).ToArray();

            List<Node> nodes = new List<Node>();
            nodes.Add(rootNode);

            foreach (DeviceGroup deviceGroup in orderedDeviceGroups)
            {
                AddGroup(rootNode, deviceGroup, nodes);
            }

            Matrix matrix = RoomGenerator.Generate(rootNode);

            return matrix.AsString();
        }

        private static void AddGroup(Node topNode, DeviceGroup deviceGroup, List<Node> processedNodes)
        {
            string[] paths = deviceGroup.Path.Split(new[] {@"\", @"\\"}, StringSplitOptions.RemoveEmptyEntries);

            Node groupNode = null;

            if (paths.Length == 1)
            {
                groupNode = new Node(topNode, deviceGroup.Path);

                topNode.Nodes.Add(groupNode);
                processedNodes.Add(groupNode);
                return;
            }

            int indexOfName = deviceGroup.Path.LastIndexOf(@"\", StringComparison.Ordinal);

            string parentPath = deviceGroup.Path.Remove(indexOfName);

            Node parentNode = processedNodes.Single(n => n.Name == parentPath);

            groupNode = new Node(parentNode, deviceGroup.Path);

            parentNode.Nodes.Add(groupNode);
            processedNodes.Add(groupNode);
        }
    }
}