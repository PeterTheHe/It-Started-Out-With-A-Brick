using UnityEngine;
using System.Collections;

public class BatteringRam : MonoBehaviour {

    public WheelJoint2D wheel;
    public float forwardSpeed = -400f;

    private bool retreating;
    private JointMotor2D motor;
    private SpriteRenderer r;

	// Use this for initialization
	void Start () {

        
        retreating = false;
        wheel.useMotor = true;
        r = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {

        motor = wheel.motor;
        motor.motorSpeed = retreating ? -forwardSpeed : forwardSpeed;
        wheel.motor = motor;

        if (retreating && !r.isVisible)
        {
            retreating = false;
        }

	}

    void OnCollisionEnter2D (Collision2D collision)
    {

        //If we hit the wall, retreat
        if (collision.collider.tag == "Wall")
        {
            retreating = true;
        }

    }

}
