using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

    public float breakThreshold;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D (Collision2D collision)
    {

        if (collision.rigidbody)
        {

            if (Mathf.Abs(collision.rigidbody.velocity.magnitude * collision.rigidbody.mass) > 2)
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }

        }

    }

}
