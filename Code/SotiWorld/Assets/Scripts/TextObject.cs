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
        private static readonly Dictionary<TextColor, Material> Materials = new Dictionary<TextColor, Material>();  

        private static readonly GameObject Prefab = Resources.Load<GameObject>("Text");
        
        public string Text { get; set; }

        public TextOrientation Orientation { get; set; }

        public TextColor TextColor { get; set; }

        public int Altitude { get; set; }

        static TextObject()
        {
            Materials.Add(TextColor.Red, Resources.Load<Material>("RedText"));
            Materials.Add(TextColor.Green, Resources.Load<Material>("GreenText"));
            Materials.Add(TextColor.Blue, Resources.Load<Material>("BlueText"));
            Materials.Add(TextColor.Yellow, Resources.Load<Material>("YellowText"));
            Materials.Add(TextColor.Purple, Resources.Load<Material>("PurpleText"));
            Materials.Add(TextColor.Cyan, Resources.Load<Material>("CyanText"));
        }

        public TextObject(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            Text = text;
            Altitude = 5;
        }

        public TextObject()
        {
            Text = "Text";
            Altitude = 5;
        }

        public void Render(float x, float y, float z)
        {
            var gameObject = Object.Instantiate(Prefab, new Vector3(x - 0.5f, y + Altitude - 6.75f, z - 1.01f), Quaternion.identity) as GameObject;

            gameObject.GetComponent<Renderer>().materials = new[] { Materials[TextColor] };

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
                    break;
            }

            gameObject.GetComponent<TextMesh>().text = Text;
        }
    }
}
