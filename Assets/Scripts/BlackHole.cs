using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Zom.Pie
{
    [ExecuteInEditMode]
    public class BlackHole : MonoBehaviour
    {
        [SerializeField]
        EnemyType enemyType = EnemyType.Green;
        public EnemyType EnemyType
        {
            get { return enemyType; }
        }

        [SerializeField]
        List<MeshRenderer> renderersToColorize;

        [SerializeField]
        Material greenMaterial;

        [SerializeField]
        Material yellowMaterial;

        [SerializeField]
        Material redMaterial;

        [SerializeField]
        bool useGate = false;

        [SerializeField]
        GameObject gateController;

        // We put here all the enemies collapsing inside the black hole in order to add to them a force
        List<Rigidbody> dyingEnemies = new List<Rigidbody>();

        float forceMagnitude = 80f;


        private void Awake()
        {
            Colorize();
            CheckGate();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Colorize();
                CheckGate();
            }
                
#endif
        }

        private void FixedUpdate()
        {
            foreach(Rigidbody enemy in dyingEnemies)
            {
                // Compute the force direction
                Vector3 dir = transform.position - enemy.position;
                dir.z = 0;

                float attractionForce = 20;
                Vector3 targetVel = attractionForce * dir;

                // Add force
                //enemy.velocity = enemy.velocity.magnitude * dir.normalized;
                float velChange = Time.fixedDeltaTime * 80f;
                enemy.velocity = Vector3.MoveTowards(enemy.velocity, targetVel, velChange);
            }
        }

        public void SetEnemyType(EnemyType type)
        {
            enemyType = type;
            Colorize();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Tag.Enemy.ToString().Equals(other.tag))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (!enemy.Dying)
                {
                    enemy.Dying = true; 
                    StartCoroutine(DestroyEnemy(other.gameObject));
                }
                    
            }
        }

        IEnumerator DestroyEnemy(GameObject enemy)
        {
            
            // Add to the attraction list
            Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();
            dyingEnemies.Add(enemyRB);

            yield return enemy.transform.DOScale(Vector3.zero, 0.5f).WaitForCompletion();

            // Remove fro the list
            dyingEnemies.Remove(enemyRB);

            // Die
            enemy.GetComponent<Enemy>().Die(this);

        }

        void CheckGate()
        {
            if (!useGate)
            {
                gateController.SetActive(false);
            }
            else
            {
                gateController.SetActive(true);
            }
        }

        void Colorize()
        {
            switch (enemyType)
            {
                case EnemyType.Green:
                    
                    ColorizeRenderers(greenMaterial);
                    break;
                case EnemyType.Yellow:
                    ColorizeRenderers(yellowMaterial);
                    break;
                case EnemyType.Red:
                    ColorizeRenderers(redMaterial);
                    break;
            }
        }

        void ColorizeRenderers(Material mat)
        {
            foreach(Renderer r in renderersToColorize)
            {
                r.sharedMaterial = mat;
            }
        }

    }

}
