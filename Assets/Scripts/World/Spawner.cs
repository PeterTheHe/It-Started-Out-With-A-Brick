using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject[] stuffToSpawn;
    public float delay = 3f;
    public int numberPerWave = 10;

    private float counter;

	// Use this for initialization
	void Start() { 

	}
	
	// Update is called once per frame
	void Update () {

        if (counter > delay)
        {
            Instantiate(stuffToSpawn[Random.Range(0, stuffToSpawn.Length)], transform.position, Quaternion.identity);
            counter = 0;
        }
        else
        {
            counter += Time.deltaTime;
        }

	}
}
