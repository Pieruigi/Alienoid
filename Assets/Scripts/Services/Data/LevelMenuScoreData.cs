using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie.Services
{
    public class LevelMenuScoreData
    {
        private class Data
        {
            public int levelId;
            public float playerScore;
            public float allTimeScore;
            public float monthlyScore;
            public int playerPosition;

            public Data()
            {

            }

            public Data(int levelId, float playerScore, float allTimeScore, float monthlyScore, int playerPosition)
            {
                this.levelId = levelId;
                this.playerPosition = playerPosition;
                this.playerScore = playerScore;
                this.monthlyScore = monthlyScore;
                this.allTimeScore = allTimeScore;
            }

            public override string ToString()
            {
                return string.Format("[LevelId:{0}, pScore:{1}, pPos:{2}]", levelId, playerScore, playerPosition);
            }

        }

        List<Data> data;

        public LevelMenuScoreData()
        {
            data = new List<Data>();
        }

        public void AddPlayerScore(int levelId, float score)
        {
            Data d = GetLevelData(levelId);

            d.playerScore = score;
        }

        public void AddPlayerPosition(int levelId, int position)
        {
            Data d = GetLevelData(levelId);
            d.playerPosition = position;
        }

        public void AddMonthlyRecord(int levelId, float score)
        {
            Data d = GetLevelData(levelId);

            d.monthlyScore = score;
        }

        public void AddAllTimeRecord(int levelId, float score)
        {
            Data d = GetLevelData(levelId);
            d.allTimeScore = score;
        }

        Data GetLevelData(int levelId)
        {
            // Find or create data
            Data d = data.Find(d => d.levelId == levelId);
            if (d == null)
            {
                d = new Data();
                d.levelId = levelId;
                data.Add(d);
            }

            return d;
        }

        public override string ToString()
        {
            string ret = "";
            foreach(Data d in data)
            {
                ret += d.ToString() + "\n";
            }

            return ret;
        }
    }

}
