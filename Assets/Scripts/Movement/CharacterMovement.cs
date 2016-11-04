using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    public Rigidbody2D rb;

    public float upForce = 10;
    public float sidewaysForce = 10;
    public AnimationCurve upAnimationCurve;
    public AnimationCurve rightAnimationCurve;

    public LayerMask groundLayers;

    private bool grounded;

    // Use this for initialization
    void Start () {

        //Get stuff set up
        rb = transform.FindChild("Head").GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {

        //Check if grounded
        grounded = Physics2D.Raycast(transform.position, 12 * Vector2.down, groundLayers);


        //Move if grounded
        if (grounded)
        {
            try {
                //Random movement based on curves
                rb.AddForce(Vector2.up * upForce * upAnimationCurve.Evaluate(Time.time % 1) + Vector2.left * (Mathf.Sign (Random.Range (-10, 10))) * sidewaysForce * rightAnimationCurve.Evaluate(Time.time % 1));
            }
            catch (MissingReferenceException e){
                Destroy (this);
            }

        }

    }
}
