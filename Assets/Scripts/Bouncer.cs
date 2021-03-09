using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
	public class Bouncer : MonoBehaviour
	{
		[SerializeField]
		[Range(0f,1f)]
		float bouncePower = 0;

		float bounceMul = 2.75f;

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

				float forceMag = bounceMul * (bouncePower + collision.relativeVelocity.magnitude * 0.175f);

				Vector3 forceDir = -contactPoint.normal;
				Debug.Log("ContactNormal:" + contactPoint.normal);

				collision.gameObject.GetComponent<Rigidbody>().AddForce(forceMag * forceDir, ForceMode.Impulse);
			}

			


		}
    }

	



}

