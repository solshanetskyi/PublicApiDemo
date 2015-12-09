using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Integration
{
    class PublicApiGatewayMock : IPublicApiGateway
    {
        public void Login(string username, string password)
        {
        }

        public DeviceGroup[] GetDeviceGroups()
        {
            List<DeviceGroup> deviceGroups = new List<DeviceGroup>();

            TextAsset groupsResponse = Resources.Load("GetGroupsResponse") as TextAsset;
            //TextAsset groupsResponse = Resources.Load("BigDeviceResponseExample") as TextAsset;

            var root = JSON.Parse(groupsResponse.text);

            foreach (var group in root.Childs)
            {
                DeviceGroup deviceGroup = new DeviceGroup
                {
                    Name = group["Name"].Value,
                    Path = group["Path"].Value,
                    Icon = group["Icon"].Value,
                    Kind = group["Kind"].Value
                };

                deviceGroups.Add(deviceGroup);
            }

            return deviceGroups.ToArray();
        }

        public Device[] GetDevices()
        {
            List<Device> devices = new List<Device>();

            TextAsset devicesResponse = Resources.Load("GetDevicesResponse") as TextAsset;

            var root = JSON.Parse(devicesResponse.text);

            foreach (var deviceNode in root.Childs)
            {
                Device device = new Device
                {
                    Name = deviceNode["DeviceName"].Value,
                    Path = deviceNode["Path"].Value,
                    DeviceId = deviceNode["DeviceId"].Value,
                    MacAddress = deviceNode["MacAddress"].Value,
                    Manufacturer = deviceNode["Manufacturer"].Value,
                    Model = deviceNode["Model"].Value,
                    BatteryStatus = deviceNode["BatteryStatus"].Value,
                    AgentVersion = deviceNode["AgentVersion"].Value
                };

                devices.Add(device);
            }

            return devices.ToArray();
        }

        public void LockDevice(string deviceId)
        {
        }

        public void SendMessageToDevice(string deviceId, string message)
        {
        }
    }
}