using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Integration
{
    public class PublicApiGateway : IPublicApiGateway
    {
        private readonly string _url;
        private readonly string _clientId;
        private readonly string _clientSecret;

        private string Token = "/token";
        private string DeviceGroups = "/devicegroups";
        private string Devices = "/devices";
        private string DeviceAction = "/devices/{0}/actions";

        private static string _accessToken;

        public PublicApiGateway(string url, string clientId, string clientSecret)
        {
            _url = url;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public void Login(string username, string password)
        {
            WWWForm form = new WWWForm();

            form.AddField("grant_type", "password");
            form.AddField("username", username);
            form.AddField("password", password);

            string userNamePassword = string.Format("{0}:{1}", _clientId, _clientSecret);

            var tokenBytes = System.Text.Encoding.ASCII.GetBytes(userNamePassword);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(tokenBytes));

            WWW www = new WWW(_url + Token, form.data, headers);

            while (!www.isDone)
            {}

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Error during login operation." + www.error);
                throw new InvalidOperationException("Error during login operation. " + www.error);
            }

            var root = SimpleJSON.JSON.Parse(www.text);

            _accessToken =  root["access_token"];
        }

        public DeviceGroup[] GetDeviceGroups()
        {
            List<DeviceGroup> deviceGroups = new List<DeviceGroup>();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + _accessToken);
            headers.Add("Accept", "application/json");

            WWW www = new WWW(_url + DeviceGroups, null, headers);

            while (!www.isDone)
            {}

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Error during retrieving device groups. " + www.error);
            }

            var root = JSON.Parse(www.text);

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

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + _accessToken);
            headers.Add("Accept", "application/json");

            WWW www = new WWW(_url + Devices, null, headers);

            while (!www.isDone)
            { }

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Error during retrieving devices. " + www.error);
            }

            var root = JSON.Parse(www.text);

            foreach (var group in root.Childs)
            {
                Device deviceGroup = new Device
                {
                    Name = group["DeviceName"].Value,
                    Path = group["Path"].Value
                };

                devices.Add(deviceGroup);
            }

            return devices.ToArray();
        }

        public void LockDevice(string deviceId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + _accessToken);
            headers.Add("Accept", "application/json");
            headers.Add("Content-Type", "application/json");

            string request = "{ \"Action\": \"Lock\" }";
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(request);

            WWW www = new WWW(_url + string.Format(DeviceAction, deviceId), requestBytes, headers);

            while (!www.isDone)
            { }

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Error during lock operation." + www.error);
            }

            var root = SimpleJSON.JSON.Parse(www.text);
        }

        public void SendMessageToDevice(string deviceId, string message)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + _accessToken);
            headers.Add("Accept", "application/json");
            headers.Add("Content-Type", "application/json");

            string request = "{ \"Action\": \"SendMessage\", \"Message\":\"{messagetemplate}\"}".Replace("{messagetemplate}", message);
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(request);

            WWW www = new WWW(_url + string.Format(DeviceAction, deviceId), requestBytes, headers);

            while (!www.isDone)
            { }

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Error during send message operation." + www.error);
            }

            var root = SimpleJSON.JSON.Parse(www.text);
        }
    }
}
