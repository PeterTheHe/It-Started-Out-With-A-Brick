using UnityEngine;
using System.Collections;

public class LimbLoss : MonoBehaviour {

    public bool isHead;
    public GameObject blood;

    private Manager manager;

    // Use this for initialization
    void Start () {

        isHead = gameObject.name == "Head";
        manager = FindObjectOfType<Manager>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnJointBreak2D (Joint2D brokenJoint)
    {

        //Instantitate the blood gameobject at the broken joint
        GameObject bloodGO = Instantiate(blood, brokenJoint.transform.position, transform.rotation) as GameObject;
        bloodGO.transform.parent = transform.parent.FindChild("Body");

        //Add lost mass to the connected rigidbody to prevent flying
        brokenJoint.connectedBody.GetComponent<Rigidbody2D>().mass += GetComponent<Rigidbody2D>().mass;

        //If this is the head, stop moving (because you're dead)
        if (isHead)
        {
            //Add everything to the dead layer
            transform.parent.gameObject.layer = LayerMask.NameToLayer("Dead");
            foreach (Transform t in transform.parent.GetComponentsInChildren<Transform>())
            {
                if (t.GetComponent<LimbLoss>())
                {
                    t.gameObject.layer = LayerMask.NameToLayer("Dead");
                    t.gameObject.AddComponent<DeadBehaviour>();
                }
            }
            //Tell the manager that you're dead
            manager.AddDeath(transform.parent);

            //Die
            Destroy (transform.parent.GetComponent<CharacterMovement>());
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Dead");
            manager.AddInjury(transform.parent);
        }

    }

}
