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
        

        public static readonly Language DefaultLanguage = Language.English;

        

        // UI
        public static readonly Color EnabledColor = Color.white;
        public static readonly Color DisabledColor = new Color32(80, 80, 80, 127);
        public static readonly float ButtonOnClickEffectTime = 0.25f;

        // Leaderboard
        public static readonly int TopPlayers = 20;

        // Level speed info
        public static readonly float DefaultTimeScale = 1.5f;//2.5f;
        public static readonly float LevelSpeedStep = 0.2f;
        //public static readonly int MaxLevelSpeed = 1;

        // Player prefs keys
        public static readonly string CustomSpeedPrefsKey = "CustomSpeed";

    }
}
