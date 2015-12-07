using UnityEngine;

namespace Assets.Scripts
{
    internal class IPhoneObject : ITileObject
    {
        private static readonly GameObject Prefab = Resources.Load<GameObject>("IPhone");

        public void Render(float x, float y, float z)
        {
            var gameObject = Object.Instantiate(Prefab, new Vector3(x, -3, z - 1f), Quaternion.identity) as GameObject;

            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}