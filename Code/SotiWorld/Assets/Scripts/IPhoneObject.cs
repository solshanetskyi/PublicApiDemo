using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    internal class IPhoneObject : ITileObject
    {
        private static readonly GameObject Prefab = Resources.Load<GameObject>("IPhone");

        public string Text { get; set; }

        public string DeviceId { get; set; }

        public TextOrientation Orientation { get; set; }

        public void Render(float x, float y, float z)
        {
            GameObject gameObject;

            switch (Orientation)
            {
                case TextOrientation.East:
                    gameObject = Object.Instantiate(Prefab, new Vector3(x, -3, z - 1f), Quaternion.identity) as GameObject;
                    gameObject.GetComponent<BoxCollider>().name = DeviceId;
                    gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case TextOrientation.West:
                    gameObject = Object.Instantiate(Prefab, new Vector3(x-1, -3, z - 1f), Quaternion.identity) as GameObject;
                    gameObject.GetComponent<BoxCollider>().name = DeviceId;
                    gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}