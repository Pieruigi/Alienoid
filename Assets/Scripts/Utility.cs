using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

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

        public static IEnumerator GetTextureFromUrlAsync(string url, UnityAction<bool, Texture> callback)
        {
            Debug.Log("GetTexture(" + url + ")");
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.responseCode != 200)
                callback?.Invoke(false, null);
            else
                callback?.Invoke(true, DownloadHandlerTexture.GetContent(www));
            
            
        }
    }

}
