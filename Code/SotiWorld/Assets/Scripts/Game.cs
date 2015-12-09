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
        private static IPublicApiGateway _publicApiGateway;

        public static int TotalGroups { get; private set; }

        public static int TotalDevices { get; private set; }

        public static void GenerateLabyrinth(IPublicApiGateway publicApiGateway)
        {
            if (publicApiGateway == null)
                throw new ArgumentNullException("publicApiGateway");

            _publicApiGateway = publicApiGateway;

            Refresh();
        }

        public static void Refresh()
        {
            if (_publicApiGateway == null)
                throw new InvalidOperationException("Labyrinth has not been created yet");

            var deviceGroups = _publicApiGateway.GetDeviceGroups();
            var devices = _publicApiGateway.GetDevices();

            TotalGroups = deviceGroups.Length;
            TotalDevices = devices.Length;

            _labyrinthMatrix = MapGenerator.GenerateMap(deviceGroups, devices);
        }

        public static Matrix LabyrinthMatrix
        {
            get
            {
                if (_labyrinthMatrix == null)
                {
                    _publicApiGateway = new PublicApiGatewayMock();
                    GenerateLabyrinth(_publicApiGateway);
                }

                return _labyrinthMatrix;
            }
        }
    }
}
