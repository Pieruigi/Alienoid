using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zom.Pie.Services;

public class TestTasks : MonoBehaviour
{
    class Player 
    {
        public string playerName;
        public DateTime creationDate;

        public static implicit operator Task<object>(Player v)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("[Player name:{0}, CreationDate:{1}", playerName, creationDate);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Test1("Pippo").ConfigureAwait(false);
        LeaderboardManager.Instance.GetLevelMenuScoreDataAsync(GetLevelMenuScoreDataAsyncCallback).ConfigureAwait(false);
        Debug.Log("Main thread continue");
        //Test2("Pippo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetLevelMenuScoreDataAsyncCallback(LevelMenuScoreData data)
    {
        Debug.Log("Callback");
        Debug.Log(data);
    }

    async Task Test1(string playerName) 
    {
        Player player = await CreatePlayer(playerName);
        PrintPlayer(player);
    }

    void Test2(string playerName)
    {
        Debug.Log("Test 2 started");
        CreatePlayer(playerName).ContinueWith(task => {
            if (task.IsCompleted)
            {
                PrintPlayer(task.Result);
            }
        });
        Debug.Log("Test 2 completed");
    }

    async Task<Player> CreatePlayer(string name)
    {
        Debug.Log("CreatePlayer() started");
        await Task.Delay(3);

        Player p = new Player();
        p.playerName = name;
        p.creationDate = DateTime.UtcNow;

        Debug.Log("CreatePlayer() completed");
        return p;
    }

    void PrintPlayer(Player p)
    {
        Debug.Log(p);
    }
}
