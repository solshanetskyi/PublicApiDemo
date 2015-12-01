namespace Assets.Scripts.Integration
{
    public interface IPublicApiGateway
    {
        void Login(string username, string password);

        DeviceGroup[] GetDeviceGroups();

        Device[] GetDevices();
    }
}