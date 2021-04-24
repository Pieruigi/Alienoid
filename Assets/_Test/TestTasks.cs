using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TestTasks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Test1();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Test1() 
    {
        Task<string> task = GetUrlData("www.google.it");
        Debug.Log("After GetUrlData()");
    }


    async Task<string> GetUrlData(string url)
    {
        UnityWebRequest request = new UnityWebRequest(url); // function which prepares request for API fetch
        request.SendWebRequest();
        Debug.Log("Request sent");
        if (request.isHttpError)
        {
            throw new System.Exception("HTTP ERROR " + request.error + url);
        }
        string s = request.downloadHandler.text;
        return s;
    }
}
