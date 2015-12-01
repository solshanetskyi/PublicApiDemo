using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Integration
{
    public class PublicApiGateway
    {
        private readonly string _url;
        private readonly string _clientId;
        private readonly string _clientSecret;

        private string Token = "/token";

        private WWW _currentOperation = null;

        public PublicApiGateway(string url, string clientId, string clientSecret)
        {
            _url = url;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public void LoginStart()
        {
            WWWForm form = new WWWForm();

            form.AddField("grant_type", "password");
            form.AddField("username", "Administrator");
            form.AddField("password", "1");

            string userNamePassword = string.Format("{0}:{1}", _clientId, _clientSecret);

            var tokenBytes = System.Text.Encoding.ASCII.GetBytes(userNamePassword);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(tokenBytes));

//            WWW www = new WWW("https://owl.corp.soti.net/mobicontrol/api/token", form.data, header);
            WWW www = new WWW(_url + Token, form.data, headers);
            _currentOperation = www;
        }

        public bool IsOperationComplete
        {
            get
            {
                if (_currentOperation == null)
                    return true;

                return _currentOperation.isDone;
            }
        }

        public string LoginEnd()
        {
            if (!_currentOperation.isDone)
            {
                Debug.Log("Current Operation is not done yet.");
                throw new InvalidOperationException("Current Operation is not done yet.");
            }

            if (!string.IsNullOrEmpty(_currentOperation.error))
            {
                Debug.Log("Error during login operation.");
                throw new InvalidOperationException("Error during login operation. " + _currentOperation.error);
            }

            var root = SimpleJSON.JSON.Parse(_currentOperation.text);

            _currentOperation = null;

            return root["access_code"];
        }
    }

    public class DeviceGroup
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
