using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    [ExecuteInEditMode]
    public class GateSwitcher : GameplaySwitcher
    {
        [System.Serializable]
        class Data
        {
            public BlackHoleGate gate;
            public bool close;
        }

        [SerializeField]
        List<Data> dataList;

        int dir = 1;

        protected override void Init()
        {
            foreach(Data data in dataList)
            {
                if (data.close)
                    data.gate.Close(true);
                else
                    data.gate.Open(true);
            }
        }

        protected override void Switch()
        {
            foreach (Data data in dataList)
            {
                if (data.close)
                {
                    if (dir > 0)
                        data.gate.Open(false);
                    else
                        data.gate.Close(false);
                }
                else
                {
                    if (dir > 0)
                        data.gate.Close(false);
                    else
                        data.gate.Open(false);
                }
                    
            }

            dir *= -1;
        }
    }

}
