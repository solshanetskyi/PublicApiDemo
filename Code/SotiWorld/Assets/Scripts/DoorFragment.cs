using UnityEngine;

namespace Assets.Scripts
{
    internal class DoorFragment : ITileObject
    {
        private static readonly GameObject Prefab = Resources.Load<GameObject>("DoorFragment");

        public void Render(float x, float y, float z)
        {
            Object.Instantiate(Prefab, 
                new Vector3(x - Prefab.transform.localScale.x / 2, 
                y - Prefab.transform.localScale.y / 2, 
                z - Prefab.transform.localScale.z / 2), Quaternion.identity);
        }
    }
}