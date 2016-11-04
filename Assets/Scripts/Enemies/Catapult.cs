using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour {

    public WheelJoint2D wheel;
    public HingeJoint2D armature;

    public float forwardSpeed = -400f;
    public float fireSpeed = -200f;
    public float reloadTime = 3f;
    public float fireTime = 1f; //Time it takes the armature to fire

    public GameObject projectile;
    public Transform projecticleSpawnPoint;

    private JointMotor2D wheelMotor;
    private JointMotor2D armatureMotor;
    private Transform catapultPoint;
    private SpriteRenderer r;
    private bool canFire;
    private bool retreating;
    public GameObject currentProjectile;

	// Use this for initialization
	void Start () {

       
        r = transform.FindChild("Arm").FindChild("Holder").GetComponent<SpriteRenderer>();
        
        wheel.useMotor = true;
        armature.useMotor = true;

        armatureMotor.motorSpeed = 0;
        armature.motor = armatureMotor;

        catapultPoint = GameObject.Find("CatapultPlace").transform;
        projecticleSpawnPoint = transform.FindChild("Base").FindChild("ProjectileSpawnPoint");

        StartCoroutine(Reload());

    }
	
	// Update is called once per frame
	void Update () {

        try
        {

            wheelMotor = wheel.motor;
            armatureMotor = armature.motor;
            armatureMotor.maxMotorTorque = 5000;

            int direction = (int)Mathf.Sign((catapultPoint.position - wheel.transform.position).x);

            if (direction == -1)
            {
                wheelMotor.motorSpeed = forwardSpeed;
                wheel.motor = wheelMotor;
            }
            else
            {
                
                wheelMotor.motorSpeed = -forwardSpeed;
                wheel.motor = wheelMotor;

                if (canFire)
                {
              
                    //Fire stuff
                    armatureMotor.motorSpeed = fireSpeed;
                    currentProjectile = null;
                    StartCoroutine(Reload());
                }
                else
                {
                    armatureMotor.motorSpeed = -fireSpeed;
                }

                armature.motor = armatureMotor;
            }


        }
        catch (MissingReferenceException e)
        {
            //We've been killed
            Destroy(this);
        }       

	}

    bool startedReload = false;
    IEnumerator Reload()
    {

        if (!startedReload)
        {
            startedReload = true;
            yield return new WaitForSeconds(fireTime);
            canFire = false;
            yield return new WaitForSeconds(reloadTime);
            canFire = true;

            //Reload the projectile
            currentProjectile = Instantiate(projectile, projecticleSpawnPoint.position, Quaternion.identity) as GameObject;

            //Make sure we can collide with it
            foreach (Transform t in currentProjectile.GetComponentsInChildren<Transform>())
            {
                t.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            startedReload = false;
        }

    }

}
