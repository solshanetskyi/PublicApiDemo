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

            Console.WriteLine("Depth " + depth);
            Console.WriteLine("Width " + height);

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

                foreach (var group in levelGroup)
                {
                    if (group.Parent != null)
                    {
                        if (currentParent != group.Parent)
                        {
                            yOffset = 0;
                        }
                    }

                    currentParent = group.Parent;

                    Room room = new Room(group.GetSize(), group.Name);

                    var parentOffset = 0;

                    if (group.Parent != null)
                        parentOffset = offsets[group.Parent];

                    matrix.Merge(room.AsMatrix(), yOffset + parentOffset, xOffset);

                    offsets.Add(group, yOffset + parentOffset);

                    yOffset += group.GetSize() + Settings.Scale;
                }

                xOffset += Settings.DefaultHeight + Settings.RoomCoridorLenght - 1;
                yOffset = 0;
            }

            return matrix;
        }
    }
}