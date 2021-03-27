using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public enum Tag { Player, Bullet, Enemy }
    public enum Layer { RaycastPlane }

    public enum Language { English, Italian }

    public class Constants
    {
        public static readonly float DefaultTimeScale = 1.5f;

        public static readonly Language DefaultLanguage = Language.English;

        public static readonly int MaxLevelSpeed = 3;

        // UI
        public static readonly Color EnabledColor = Color.white;
        public static readonly Color DisabledColor = new Color32(80, 80, 80, 127);

    }
}
