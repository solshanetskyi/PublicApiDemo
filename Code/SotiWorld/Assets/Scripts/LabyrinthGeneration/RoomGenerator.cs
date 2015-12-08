using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LabyrinthGeneration
{
    public class RoomGenerator
    {
        public static Matrix Generate(Node parentNode)
        {
            Dictionary<Node, int> offsets = new Dictionary<Node, int>();

            int depth = parentNode.GetMaxDepth();
            int height = depth * (Settings.DefaultHeight + Settings.RoomCoridorLenght);

            var allNodes = new[] { parentNode }.FancyFlatten(n => true, n => n.Nodes).ToArray();

            var biggestLevel = allNodes.GroupBy(n => n.Level).OrderBy(g => g.Sum(n => n.GetSize())).Last();

            Console.WriteLine("Biggest level id " + biggestLevel.Key);
            Console.WriteLine("Biggest level count " + biggestLevel.Sum(n => n.GetSize()));

            int labyrinthWidth = parentNode.GetSize();

            Matrix matrix = new Matrix(labyrinthWidth, height);

            int yOffset = 0;
            int xOffset = 0;

            foreach (var levelGroup in allNodes.GroupBy(n => n.Level))
            {
                Node currentParent = null;

                foreach (Node groupNode in levelGroup)
                {
                    if (groupNode.Parent != null)
                    {
                        if (currentParent != groupNode.Parent)
                        {
                            yOffset = 0;
                        }
                    }

                    currentParent = groupNode.Parent;

                    Room room = new Room(groupNode.GetSize(), groupNode.Name, groupNode.Color, groupNode.Devices);

                    var parentOffset = 0;

                    if (groupNode.Parent != null)
                        parentOffset = offsets[groupNode.Parent];

                    matrix.Merge(room.AsMatrix(), yOffset + parentOffset, xOffset);

                    offsets.Add(groupNode, yOffset + parentOffset);

                    yOffset += groupNode.GetSize() + Settings.Scale;
                }

                xOffset += Settings.DefaultHeight + Settings.RoomCoridorLenght - 1;
                yOffset = 0;
            }

            return matrix;
        }
    }
}