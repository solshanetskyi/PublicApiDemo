using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    internal class TextObject : ITileObject
    {
        private static readonly GameObject Prefab = Resources.Load<GameObject>("Text");

        public string Text { get; set; }

        public TextOrientation Orientation { get; set; }

        public int Altitute { get; set; }

        public TextObject(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            Text = text;
            Altitute = 5;
        }

        public TextObject()
        {
            Text = "Text";
            Altitute = 5;
        }

        public void Render(float x, float y, float z)
        {
            var gameObject = Object.Instantiate(Prefab, new Vector3(x - 0.5f, y + Altitute - 6.75f, z - 1.01f), Quaternion.identity) as GameObject;

            switch (Orientation)
            {
                case TextOrientation.East:
                    gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case TextOrientation.South:
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case TextOrientation.North:
                    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case TextOrientation.West:
                    gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.49f, gameObject.transform.position.y, gameObject.transform.position.z);
                    break;
            }

            gameObject.GetComponent<TextMesh>().text = Text;
        }
    }
}
