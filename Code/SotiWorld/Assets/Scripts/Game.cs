using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Integration;
using Assets.Scripts.LabyrinthGeneration;

namespace Assets.Scripts
{
    public static class Game
    {
        private static Matrix _labyrinthMatrix;

        public static void GenerateLabyrinth(IPublicApiGateway publicApiGateway)
        {
            if (publicApiGateway == null)
                throw new ArgumentNullException("publicApiGateway");

            var deviceGroups = publicApiGateway.GetDeviceGroups();
            _labyrinthMatrix = MapGenerator.GenerateMap(deviceGroups);
        }

        public static Matrix LabyrinthMatrix
        {
            get
            {
                if (_labyrinthMatrix == null)
                    throw new InvalidOperationException("Labyrinth has not been generated yet");

                return _labyrinthMatrix;
            }
        }
    }
}
