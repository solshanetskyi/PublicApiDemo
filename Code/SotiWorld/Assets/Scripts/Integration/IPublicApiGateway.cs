namespace Assets.Scripts.Integration
{
    public interface IPublicApiGateway
    {
        DeviceGroup[] GetDeviceGroups();

        Device[] GetDevices();

        void LockDevice(string deviceId);

        void SendMessageToDevice(string deviceId, string message);

        InstalledApplication[] GetInstalledApplications(string deviceId);
    }
}