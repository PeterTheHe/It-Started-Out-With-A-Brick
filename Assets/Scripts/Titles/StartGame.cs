using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartGame : MonoBehaviour {

    //Devlog: as you may have noticed, there's a negative correlation between code quality and tiredless 
    
    public Text text1;
    public Text text2;
    public Text text3;
    public Rigidbody2D startBrick;

    private Manager manager;
    private InfiniteSong music;
    private bool canClick;

	// Use this for initialization
	IEnumerator Start () {

        canClick = false;

        manager = FindObjectOfType<Manager>();
        music = FindObjectOfType<InfiniteSong>();

        text1 = GameObject.Find("IntroText1").GetComponent<Text>();
        text2 = GameObject.Find("IntroText2").GetComponent<Text>();
        text3 = GameObject.Find("IntroText3").GetComponent<Text>();
        startBrick = GameObject.Find("StartBase").GetComponent<Rigidbody2D>();

        //Make them invisible

        text1.color = new Color(255, 255, 255, 0);
        text2.color = new Color(255, 255, 255, 0);
        text3.color = new Color(255, 255, 255, 0);

        //Fade them in

        while (text1.color.a < Color.white.a - 0.01f) //0.01 is close enough
        {
            text1.color = Color.Lerp(text1.color, Color.white, Mathf.PingPong(Time.timeSinceLevelLoad/50, 1));
            yield return new WaitForSeconds(Time.deltaTime / 3);
        }

        yield return new WaitForSeconds(2f);

        while (text2.color.a < Color.white.a - 0.03f) //0.01 is close enough
        {
            text2.color = Color.Lerp(text2.color, Color.white, Mathf.PingPong(Time.timeSinceLevelLoad / 50, 1));
            yield return new WaitForSeconds(Time.deltaTime / 3);
        }

        yield return new WaitForSeconds(2f);

        while (text3.color.a < Color.white.a - 0.03f) //0.01 is close enough
        {
            text3.color = Color.Lerp(text2.color, Color.white, Mathf.PingPong(Time.timeSinceLevelLoad / 50, 1));
            yield return new WaitForSeconds(Time.deltaTime / 3);
        }

        canClick = true;

    }
	
	// Update is called once per frame
	void Update () {


        if (canClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                canClick = false;
                StartCoroutine(Begin());
            }
        }

    }

    IEnumerator Begin()
    {

        //Fade them out
        while (text1.color.a > 0.01f) //0.01 is close enough
        {
            text1.color = Color.Lerp(text1.color, new Color(255, 255, 255, 0), Mathf.PingPong(Time.timeSinceLevelLoad / 50, 1));  
            text2.color = Color.Lerp(text2.color, new Color(255, 255, 255, 0), Mathf.PingPong(Time.timeSinceLevelLoad / 50, 1));
            text3.color = Color.Lerp(text2.color, new Color(255, 255, 255, 0), Mathf.PingPong(Time.timeSinceLevelLoad / 50, 1));
            yield return new WaitForSeconds(Time.deltaTime / 3);
        }

        startBrick.isKinematic = false;

        yield return new WaitForSeconds(6);

        manager.Reset();
        manager.playing = true;
        music.Play();

    }
}
