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
        private readonly string _userName;
        private readonly string _password;

        private string Token = "/token";
        private string DeviceGroups = "/devicegroups";
        private string Devices = "/devices";
        private string DeviceAction = "/devices/{0}/actions";
        private string DeviceApplications = "/devices/{0}/installedApplications";

        static PublicApiGateway()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (o, cert, chain, e) => true;
        }

        public PublicApiGateway(string url, string clientId, string clientSecret, string userName, string password)
        {
            _url = url;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _userName = userName;
            _password = password;
        }

        private string Login()
        {
            WWWForm form = new WWWForm();

            form.AddField("grant_type", "password");
            form.AddField("username", _userName);
            form.AddField("password", _password);

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

            return root["access_token"];
        }

        public DeviceGroup[] GetDeviceGroups()
        {
            string accessToken = Login();

            List<DeviceGroup> deviceGroups = new List<DeviceGroup>();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + accessToken);
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
            string accessToken = Login();

            List<Device> devices = new List<Device>();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + accessToken);
            headers.Add("Accept", "application/json");

            WWW www = new WWW(_url + Devices, null, headers);

            while (!www.isDone)
            { }

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Error during retrieving devices. " + www.error);
            }

            var root = JSON.Parse(www.text);

            foreach (var deviceNode in root.Childs)
            {
                Device deviceGroup = new Device
                {
                    Name = deviceNode["DeviceName"].Value,
                    Path = deviceNode["Path"].Value,
                    DeviceId = deviceNode["DeviceId"].Value,
                    MacAddress = deviceNode["MACAddress"].Value,
                    Manufacturer = deviceNode["Manufacturer"].Value,
                    Model = deviceNode["Model"].Value,
                    BatteryStatus = deviceNode["BatteryStatus"].Value,
                    AgentVersion = deviceNode["AgentVersion"].Value
                };

                devices.Add(deviceGroup);
            }

            return devices.ToArray();
        }

        public void LockDevice(string deviceId)
        {
            string accessToken = Login();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + accessToken);
            headers.Add("Accept", "application/json");
            headers.Add("Content-Type", "application/json");

            string request = "{ \"Action\": \"Lock\" }";
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(request);

            WWW www = new WWW(_url + string.Format(DeviceAction, deviceId), requestBytes, headers);

            while (!www.isDone)
            { }

            if (!string.IsNullOrEmpty(www.error))
            {
                throw new InvalidOperationException("Error during lock operation." + www.error);
            }

            var root = SimpleJSON.JSON.Parse(www.text);
        }

        public void SendMessageToDevice(string deviceId, string message)
        {
            string accessToken = Login();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + accessToken);
            headers.Add("Accept", "application/json");
            headers.Add("Content-Type", "application/json");

            string request = "{ \"Action\": \"SendMessage\", \"Message\":\"{messagetemplate}\"}".Replace("{messagetemplate}", message);
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(request);

            WWW www = new WWW(_url + string.Format(DeviceAction, deviceId), requestBytes, headers);

            while (!www.isDone)
            { }

            if (!string.IsNullOrEmpty(www.error))
            {
                throw new InvalidOperationException("Error during lock operation." + www.error);
            }

            var root = SimpleJSON.JSON.Parse(www.text);
        }

        public InstalledApplication[] GetInstalledApplications(string deviceId)
        {
            string accessToken = Login();

            List<InstalledApplication> installedApps = new List<InstalledApplication>();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + accessToken);
            headers.Add("Accept", "application/json");

            WWW www = new WWW(_url + string.Format(DeviceApplications, deviceId), null, headers);

            while (!www.isDone)
            { }

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log("Error during getting installed applications." + www.error);
            }

            var root = JSON.Parse(www.text);

            foreach (var applicationNode in root.Childs)
            {
                InstalledApplication installedApplication = new InstalledApplication
                {
                    Name = applicationNode["Name"].Value,
                    ApplicationId = applicationNode["ApplicationId"].Value,
                    DeviceId = applicationNode["DeviceId"].Value,
                };

                installedApps.Add(installedApplication);
            }

            return installedApps.ToArray();
        }
    }
}
