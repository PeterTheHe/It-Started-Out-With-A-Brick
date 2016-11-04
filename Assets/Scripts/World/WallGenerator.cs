using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallGenerator : MonoBehaviour {

    public GameObject brick;
    public int height = 25;
    public int width = 7;

    public float sizeOfBrick = 1.4f; //when the brick of 100x100 is at a scale of 0.05x0.05

    private List<GameObject> bricks = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                GameObject go = Instantiate(brick, transform.position + (Vector3.right * sizeOfBrick * i) + (Vector3.up * sizeOfBrick * j), Quaternion.identity, transform) as GameObject;
                bricks.Add(go);

            }
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Reset()
    {

        foreach (GameObject go in bricks)
        {
            Destroy(go);
        }
        bricks.Clear();
        Start();

    }

}
