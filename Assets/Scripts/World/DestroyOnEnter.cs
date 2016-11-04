using UnityEngine;
using System.Collections;

public class DestroyOnEnter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D other)
    {
        //If it's an enemy...
        if (other.gameObject.GetComponent <CharacterMovement>())
        {
            Destroy(other.transform.parent.gameObject);
        }

        //Otherwise, do it normally
        Destroy(other.gameObject);
        
    }

}
