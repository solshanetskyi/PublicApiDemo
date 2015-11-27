namespace Integration
{
    class PublicApiGatewayMock : IPublicApiGateway
    {
        public DeviceGroup[] GetDeviceGroups()
        {
            return new[]
            {
                new DeviceGroup
                {
                    Name = "AA",
                    Path = "\\AA"
                },
                new DeviceGroup
                {
                    Name = "BB",
                    Path = "\\AA\\BB"
                },
                new DeviceGroup
                {
                    Name = "CC",
                    Path = "\\AA\\CC"
                }
            };
        }
    }
}