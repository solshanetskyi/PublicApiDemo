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
        public static IPublicApiGateway PublicApiGateway { get; private set; }

        public static string ActiveDeviceId { get; set; }

        public static int TotalGroups { get; private set; }

        public static int TotalDevices { get; private set; }

        public static Dictionary<string, Device> _devices = new Dictionary<string, Device>();
        public static Dictionary<string, InstalledApplication[]> _installedApplications = new Dictionary<string, InstalledApplication[]>();

        public static void GenerateLabyrinth(IPublicApiGateway publicApiGateway)
        {
            if (publicApiGateway == null)
                throw new ArgumentNullException("publicApiGateway");

            PublicApiGateway = publicApiGateway;

            Refresh();
        }

        public static void Refresh()
        {
            if (PublicApiGateway == null)
                throw new InvalidOperationException("Labyrinth has not been created yet");

            ActiveDeviceId = null;

            var deviceGroups = PublicApiGateway.GetDeviceGroups();
            var devices = PublicApiGateway.GetDevices();

            foreach (Device device in devices)
            {
                var installedApps = PublicApiGateway.GetInstalledApplications(device.DeviceId);

                _installedApplications.Clear();

                _installedApplications.Add(device.DeviceId, installedApps);
            }

            _devices = devices.ToDictionary(d => d.DeviceId, d => d);

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
                    PublicApiGateway = new PublicApiGatewayMock();
                    GenerateLabyrinth(PublicApiGateway);
                }

                return _labyrinthMatrix;
            }
        }

        public static Device GetDeviceById(string deviceId)
        {
            return _devices[deviceId];
        }

        public static InstalledApplication[] GetInstalledApplictionsById(string deviceId)
        {
            return _installedApplications[deviceId];
        }
    }
}
