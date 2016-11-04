using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuizAnswers : MonoBehaviour {

    public int population= 15; //Corresponds to the baby question - the more you like babies, the larger the population
    public int percentageCulled = 50; //Corresponds to the elitist question - the more elitist you are, the more selective the algorithm
    public int mutationChance = 9; //Corresponds to the radioactive question - the more likely you are, the more likely a muation is;

	// Use this for initialization
	void Start () {

        UpdateValues();
        DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void UpdateValues ()
    {
        try
        {

            population = (int)GameObject.Find("PopulationSlider").GetComponent<Slider>().value;
            percentageCulled = (int)GameObject.Find("CullSlider").GetComponent<Slider>().value;
            mutationChance = (int)GameObject.Find("MutationSlider").GetComponent<Slider>().value;
        }
        catch (System.NullReferenceException e)
        {

        }
    }

}
