using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class Pool<T> where T: Object
    {
        List<T> objects;

        public Pool(int capacity)
        {
            objects = new List<T>();

            for (int i = 0; i < capacity; i++)
                objects.Add(default(T));
        }
    }

}
