using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    internal class FloorSection : ITileObject
    {
        private static readonly GameObject Prefab = Resources.Load<GameObject>("FloorSection");

        public void Render(float x, float y, float z)
        {
            // Object.Instantiate(Prefab, new Vector3(x - Prefab.transform.localScale.x * 10 / 2, y - Prefab.transform.localScale.y * 10 / 2 - 5.5f, z - Prefab.transform.localScale.z * 10 / 2), Quaternion.identity);
        }
    }
}
