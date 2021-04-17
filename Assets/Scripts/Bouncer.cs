using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Zom.Pie
{
	public class Bouncer : MonoBehaviour
	{
		public UnityAction<Bouncer> OnBounce;

		[SerializeField]
		[Range(0f,1f)]
		float bouncePower = 0;

		[SerializeField]
		float minImpulse = 0;

		//[SerializeField]
		//[Range(0f, 1f)]
		//float velBounceMul = 0.55f;

		float bounceMul = 2.75f;
		float bounceMagnitude;

        private void Awake()
        {
			bounceMagnitude = bouncePower * bounceMul;
		}

        // Start is called before the first frame update
        void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}



        private void OnCollisionEnter(Collision collision)
        {
			
			if (bouncePower == 0)
				return;

            if (Tag.Enemy.ToString().Equals(collision.collider.tag))
			{
				// It's an alien
				// We don't care about multiple contact points
				ContactPoint contactPoint = collision.contacts[0];

				Debug.LogFormat("RelativeVel: {0}", collision.relativeVelocity.magnitude);
				Debug.LogFormat("Impulse: {0}", collision.impulse.magnitude);

				float forceMag = bounceMagnitude;// + collision.relativeVelocity.magnitude * velBounceMul;

				

				if (minImpulse > 0)
				{
					if (collision.impulse.magnitude < minImpulse)
						forceMag += 0.75f;
				}

				Vector3 forceDir = -contactPoint.normal;


				collision.gameObject.GetComponent<Rigidbody>().AddForce(forceMag * forceDir, ForceMode.Impulse);

				
			}

			OnBounce?.Invoke(this);


		}
    }

	



}

