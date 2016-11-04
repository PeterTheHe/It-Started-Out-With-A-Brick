using UnityEngine;
using System.Collections;

public class RandomClean : MonoBehaviour {

    public GameObject destroyer;
    public float minTime = 20;
    public float maxTime = 50;

    //randomly reset the field

	// Use this for initialization
	void Start () {

        StartCoroutine(Clean());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Clean()
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        destroyer.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        destroyer.SetActive(false);
    }
}
