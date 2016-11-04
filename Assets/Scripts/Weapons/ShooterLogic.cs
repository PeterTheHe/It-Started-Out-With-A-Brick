using UnityEngine;
using System.Collections;

public class ShooterLogic : MonoBehaviour {

    public int ammo;
    public int ammoType;
    public float shootForce;
    public float shootDelay;
    public GameObject[] projectiles;

    private int currentAmmoType;
    private float counter;
    private Transform muzzle;

    // Use this for initialization
    void Start () {

        currentAmmoType = ammoType % projectiles.Length; //wrap back if we go over
        muzzle = transform.FindChild("Muzzle");

	}
	
	// Update is called once per frame
	void Update () {

        if (counter > Time.deltaTime)
        {

            //Shoot if there's ammo
            
            if (ammo != 0)
            {

                GameObject go = Instantiate(projectiles[currentAmmoType], muzzle.position, transform.rotation) as GameObject;
                Vector2 directionAway = (muzzle.position - transform.position).normalized;
                go.GetComponent<Rigidbody2D>().AddForce(directionAway * shootForce);
 
                ammo--;
            }

            counter = 0;

        }

        counter += Time.deltaTime;

	}
}
