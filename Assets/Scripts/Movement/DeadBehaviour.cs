using UnityEngine;
using System.Collections;

public class DeadBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if (GetComponentInChildren<SuicideGuy>())
        {
            Destroy(GetComponentInChildren<SuicideGuy>());
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            Destroy(gameObject);
        }

    }
}
