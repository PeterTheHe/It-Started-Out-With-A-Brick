using UnityEngine;
using System.Collections;

public class Crossbow : MonoBehaviour {

    public Transform bow;
    public float range = 50;
    public GameObject arrow;
    public float shootForce = 1000;
    public Transform muzzle;
    public float reloadTime = 5;

    bool canShoot;

	// Use this for initialization
	void Start () {
        canShoot = true;
	}
	
	// Update is called once per frame
	void Update () {


            //Look at mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            float radians = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
            float degrees = (180 / Mathf.PI) * radians;
            bow.rotation = Quaternion.Euler(0, 0, degrees);

        if (canShoot)
        {
            //Shoot
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject arrowGO = Instantiate(arrow, muzzle.position, bow.rotation) as GameObject;
                arrowGO.GetComponent<Rigidbody2D>().AddForce(shootForce * bow.right);
                StartCoroutine(Reload());
            }

        }
	}

    IEnumerator Reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }

}
