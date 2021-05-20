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
        Material material;

        //[SerializeField]
        //bool useGate = false;

        [SerializeField]
        GameObject gateController;

        [SerializeField]
        ParticleSystem fx;

        [SerializeField]
        AudioSource audioSource;

        // We put here all the enemies collapsing inside the black hole in order to add to them a force
        List<Rigidbody> dyingEnemies = new List<Rigidbody>();

        float forceMagnitude = 80f;

        Material mat;
        Color fxColor;

        private void Awake()
        {
            // Set gate handles
            BlackHoleGate bhg = gateController.GetComponent<BlackHoleGate>();
            bhg.OnGateClosed += HandleOnGateClosed;
            bhg.OnGateOpen += HandleOnGateOpen;

            // Create new material
            //mat = new Material(renderersToColorize[0].material);
            mat = new Material(material);
            // Set the new material to each renderer
            foreach (Renderer rend in renderersToColorize)
                rend.sharedMaterial = mat;

            Colorize();
            fx.Play();
            //CheckGate();
        }

        // Start is called before the first frame update
        void Start()
        {
            //if (!useGate || !gateController.GetComponent<BlackHoleGate>().Closed)
            //    fx.Play();
            //else
            //    fx.Stop();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
               Colorize();
               //CheckGate();
                return;
            }

#endif
        }

        private void FixedUpdate()
        {
            
            foreach (Rigidbody enemy in dyingEnemies)
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


        /// <summary>
        /// This method is similar to set enemy type, but simply fade from the old to the new color
        /// </summary>
        /// <param name="type"></param>
        public void SwitchEnemyType(EnemyType type)
        {
            float time = 1.5f;
            Color color = GetActualColor(type);
            foreach (Renderer rend in renderersToColorize)
            {
                rend.material.DOColor(color, time);
            }

            color.a = 0.5f;
            ParticleSystem.MainModule mm = fx.main;

            fxColor = mm.startColor.color;
            DOTween.To(() => fxColor, HandleOnFXColorChanged, color, time);
           

            enemyType = type;
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

        void HandleOnFXColorChanged(Color color)
        {
            fxColor = color;
            ParticleSystem.MainModule mm = fx.main;
            mm.startColor = fxColor;
        }

        IEnumerator DestroyEnemy(GameObject enemy)
        {
            
            // Add to the attraction list
            Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();
            dyingEnemies.Add(enemyRB);

            // Play audio
            audioSource.Play();

            yield return enemy.transform.DOScale(Vector3.zero, 0.5f).WaitForCompletion();

            // Remove fro the list
            dyingEnemies.Remove(enemyRB);

            // Die
            enemy.GetComponent<Enemy>().Die(this);

            

        }

        //void CheckGate()
        //{
        //    if (!useGate)
        //    {
        //        gateController.SetActive(false);
        //    }
        //    else
        //    {
        //        gateController.SetActive(true);
        //    }
        //}

        void Colorize()
        {
            switch (enemyType)
            {
                case EnemyType.Green:
                    ColorizeRenderers(Color.green);
                    ColorizeFX(Color.green);
                    break;
                case EnemyType.Yellow:
                    ColorizeRenderers(Color.yellow);
                    ColorizeFX(Color.yellow);
                    break;
                case EnemyType.Red:
                    ColorizeRenderers(Color.red);
                    ColorizeFX(Color.red);
                    break;
            }
        }

        Color GetActualColor(EnemyType enemyType)
        {
            Color ret = Color.white;
            switch (enemyType)
            {
                case EnemyType.Green:
                    ret =  Color.green;
                    break;
                case EnemyType.Yellow:
                    ret = Color.yellow;
                    break;
                case EnemyType.Red:
                    ret = Color.red;
                    break;
            }
            return ret;
        }

        void ColorizeRenderers(Color color)
        {
            foreach (Renderer r in renderersToColorize)
            {
                r.sharedMaterial.color = color;
                
            }
        }

        void ColorizeFX(Color color)
        {
            color.a = 0.5f;
            ParticleSystem.MainModule mm = fx.main;
            mm.startColor = color;
            
        }

        void HandleOnGateOpen(BlackHoleGate gate)
        {
            if (fx.isPlaying)
                return;

            fx.Play();
        }

        void HandleOnGateClosed(BlackHoleGate gate)
        {
            if (!fx.isPlaying)
                return;

            fx.Stop();
        }

    }

}
