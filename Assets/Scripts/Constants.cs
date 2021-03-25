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

    }
}
