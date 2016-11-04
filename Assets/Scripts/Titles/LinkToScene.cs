using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LinkToScene : MonoBehaviour {

    public float time;
    public int scene;
    public int mainLevel = 2;
    public bool isQuiz;


	// Use this for initialization
	IEnumerator Start () {

        if (!isQuiz)
        {
            yield return new WaitForSecondsRealtime(time);
            SceneManager.LoadSceneAsync(scene);
        }

     }
	
	// Update is called once per frame
	void Update () {

        if (!isQuiz)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadSceneAsync(mainLevel);
                //Kill the voiceover
                Destroy(FindObjectOfType<AudioSource>());
            }
        }
	
	}

    void Load()
    {
        SceneManager.LoadSceneAsync(scene);
    }

}
