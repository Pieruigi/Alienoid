using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class GeneralUtility
    {
        public static string FormatTime(float timeInSec)
        {
            float millis = timeInSec * 1000f;

            millis /= 1000f;
            int min = (int)millis / 60;
            millis %= 60f;

            return string.Format("{0:00}:{1:00.00}", min, millis);
        }
    }

}
