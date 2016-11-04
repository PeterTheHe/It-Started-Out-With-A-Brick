using UnityEngine;
using System.Collections;

public class StartBrick : MonoBehaviour {

    public AudioClip clip;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D (Collision2D collision)
    {
        
        GetComponent<AudioSource>().PlayOneShot(clip);
        Destroy(this);
         
    }

}
