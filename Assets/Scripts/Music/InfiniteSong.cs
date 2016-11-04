using UnityEngine;
using System.Collections;

public class InfiniteSong : MonoBehaviour {

    public AudioClip theme;
    public AudioClip[] variations;
    public AudioClip countermelody;
    public bool playOnStart;

    public AudioSource source;
    private bool wasCountermelody;
    private bool playing;

	// Use this for initialization
	void Start () {

        if (!source)
            source = GetComponent<AudioSource>();

        if (playOnStart)
            Play();
       
	}

    public void Play()
    {
        source.PlayOneShot(theme);
        wasCountermelody = false;
        playing = true;
    }

    // Update is called once per frame
    void Update () {

        //Have we been turned on yet?
        if (playing)
        {
            //If there's nothing playing, pick something!
            if (!source.isPlaying)
            {
                //We want the music in the form theme, vairation, countermelody, var, var, count, var...

                if (wasCountermelody)
                {
                    //play a random variation
                    source.PlayOneShot(variations[Random.Range(0, variations.Length)]);
                    wasCountermelody = false;
                }
                else
                {
                    //either play the countermelody or another variation
                    if (Random.Range(0, 5) > 2)
                    {
                        source.PlayOneShot(countermelody);
                        wasCountermelody = true;
                    }
                    else
                    {
                        source.PlayOneShot(variations[Random.Range(0, variations.Length)]);
                        wasCountermelody = false;
                    }
                }

            }
        }

    }
}
