using System;

namespace Assets.Scripts.Integration
{
    public class DeviceGroup
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string Icon { get; set; }
        public string Kind { get; set; }

        public int Level
        {
            get
            {
                string[] paths = Path.Split(new[] { @"\", @"\\" }, StringSplitOptions.RemoveEmptyEntries);

                return paths.Length;
            }
        }
    }
}