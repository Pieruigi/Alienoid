using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class Spawners : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> spawners; // 0: right

        [SerializeField]
        Sprite green, yellow, red;

        Sprite empty;

        int nextId;

        float sizeSelection = 1.2f;
   
        private void Awake()
        {
            empty = spawners[0].GetComponent<Image>().sprite;
        }

        // Start is called before the first frame update
        void Start()
        {
            

            // Initialize UI
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void Initialize()
        {
            // Setting handle
            EnemySpawnerManager.Instance.OnEnemySpawned += HandleOnEnemySpawned;
            
            // Get the first spawner
            nextId = EnemySpawnerManager.Instance.NextSpawnerId;

            for(int i=0; i<spawners.Count; i++)
            {
                
                if (i == nextId)
                {
                    // Set size
                    spawners[i].transform.localScale = Vector3.one * sizeSelection;

                    // Set the sprite
                    SetEnemySprite(LevelManager.Instance.NextEnemyTypeToSpawn);
                }
         
                
            }

      
        }

        void Switch()
        {
            float time = 0.5f;

            // Resize old spawner
            spawners[nextId].transform.DOScale(1, time).SetEase(Ease.OutElastic);
            spawners[nextId].GetComponent<Image>().sprite = empty;

            nextId++;
            if (nextId >= spawners.Count) nextId = 0;

            // Resize new spawner
            spawners[nextId].transform.DOScale(sizeSelection, time).SetEase(Ease.OutElastic);
            
            // Set the sprite
            if(LevelManager.Instance.HasNextEnemyToSpawn)
                SetEnemySprite(LevelManager.Instance.NextEnemyTypeToSpawn);

        }

        void HandleOnEnemySpawned(Enemy enemy)
        {
            Switch();
        }

        void SetEnemySprite(EnemyType enemyType)
        {
            Debug.Log("OnNextToSpawn:" + enemyType);

            Sprite s = null;
            switch (enemyType)
            {
                case EnemyType.Green:
                    s = green;
                    break;
                case EnemyType.Yellow:
                    s = yellow;
                    break;
                case EnemyType.Red:
                    s = red;
                    break;
            }
            spawners[nextId].GetComponent<Image>().sprite = s;
        }

        
    }

}
