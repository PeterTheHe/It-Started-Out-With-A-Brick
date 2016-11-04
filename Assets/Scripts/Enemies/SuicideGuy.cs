using UnityEngine;
using System.Collections;

public class SuicideGuy : MonoBehaviour {

    public AudioSource source;
    public AudioClip explosionSound;
    public GameObject explosion;

    public float delay = 10;
    public float explosionForce = 100;
    public float radius = 20;

	// Use this for initialization
	IEnumerator Start () {

        //Assign stuff
        if (!source)
            source = GetComponent<AudioSource>();


        //Wait a bit
        yield return new WaitForSeconds(delay);

        //Explode with sound!
        Instantiate(explosion, transform.position, Quaternion.identity);
        source.PlayOneShot(explosionSound);


        //Apply force to all nearby objects within range
        foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, radius))
        {

            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

            if (rb)
            {
                Vector2 direction = rb.position - (Vector2)transform.position;
                rb.AddForce(direction.normalized * (explosionForce * 1 / (direction.magnitude + 0.01f)));
            }

        }



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
