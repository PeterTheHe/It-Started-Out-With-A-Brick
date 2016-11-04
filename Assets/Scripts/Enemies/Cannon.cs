using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

    public GameObject projectile;
    public float shootForce = 1000f;
    public float reloadDelay = 3;
    public float moveSpeed = 1;
    public Transform muzzle;
    public AudioSource source;
    public AudioClip sound;
    

    private bool canShoot;
    private SpriteRenderer r;

	// Use this for initialization
	void Start () {

        canShoot = true;
        muzzle = transform.FindChild("Muzzle");

        source = GetComponent<AudioSource>();
        r = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if (r.isVisible)
        {
            if (canShoot)
            {
                GameObject go = Instantiate(projectile, muzzle.position, Quaternion.identity) as GameObject;
                go.GetComponent<Rigidbody2D>().AddForce(-transform.right * shootForce);
                source.PlayOneShot(sound);
                StartCoroutine(Reload());
            }
        }

        //very very dirty physics 
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
	
	}

    IEnumerator Reload()
    {

        canShoot = false;
        yield return new WaitForSeconds(reloadDelay);
        canShoot = true;

    }

    void OnJointBreak2D (Joint2D joint)
    {
        Destroy(this);
    }

}
