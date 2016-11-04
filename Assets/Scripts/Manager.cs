using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour {

    [System.Serializable]
    public class DeathStat : IComparable <DeathStat>
    {
        public GeneData genes;
        public int numberOfDead;
        public int numberOfInjured;
        public float timeToKill;

        public static Comparison<DeathStat> compareInjured = delegate (DeathStat object1, DeathStat object2)
        {
            return object1.numberOfInjured.CompareTo(object2.numberOfInjured);
        };

        public static Comparison<DeathStat> compareDead = delegate (DeathStat object1, DeathStat object2)
        {
            return object1.numberOfDead.CompareTo(object2.numberOfDead);
        };

        public static Comparison<DeathStat> compareTimes = delegate (DeathStat object1, DeathStat object2)
        {
            return object1.timeToKill.CompareTo(object2.timeToKill);
        };

        public int CompareTo(DeathStat other)
        {
            throw new NotImplementedException();
        }
    }

    public bool playing; //Is the game playing?

    public List<DeathStat> performance = new List<DeathStat>(); //List of all the population's performance
    public GeneData currentGenes; //Genes atm

    public int numberOfDead; 
    public int numberOfInjured;
    public float timeToKill;

    public GameObject people; //The object holding the people to instantiate

    private GameObject currentPeople; //The current object holding the people (so we can destroy it when we wish)
    private List<Transform> injuredPeople = new List<Transform>(); //list of people who have been injured
    private bool timingDeath; //Are we timing?

    public QuizAnswers quizAnswers;

    // Use this for initialization
    void Start () {

        quizAnswers = FindObjectOfType<QuizAnswers>();
        if (!quizAnswers)
            quizAnswers = gameObject.AddComponent<QuizAnswers>();

    }
	
	// Update is called once per frame
	void Update () {

        if (timingDeath)
        {
            timeToKill += Time.deltaTime;
            if (numberOfDead == 10) //If everyone's dead
            {
                timingDeath = false;
            }
        }

	}

    public void AddDeath(Transform person)
    {
        numberOfDead++;
        if (numberOfDead == 1) //This is the first death 
            timingDeath = true;
        if (injuredPeople.Contains(person))
            numberOfInjured--;
        injuredPeople.Add(person);
    }

    public void AddInjury(Transform person)
    {
        //Prevent duplicates
        if (!injuredPeople.Contains (person))
            numberOfInjured++;
        injuredPeople.Add(person);
    }

    public void Reset()
    {

        //Save performance data ==========================================
        DeathStat deathStat = new DeathStat();

        //Death timing
        if (timingDeath)
        {
            timingDeath = false;
            deathStat.timeToKill = timeToKill;
        }

        //Other stuff
        deathStat.numberOfDead = numberOfDead;
        deathStat.numberOfInjured = numberOfInjured;
        deathStat.genes = currentGenes;

        //Save Performance
        performance.Add(deathStat);

        //Reset ==========================================================
        numberOfDead = 0;
        timeToKill = 0;

        //Get rid of the current people
        if (currentPeople)
            Destroy(currentPeople);
        
        //Get rid of any ammo lying around
        foreach (GameObject ammo in GameObject.FindGameObjectsWithTag("Ammo"))
        {
            Destroy(ammo);
        }

        //New people!
        currentPeople = Instantiate(people, transform.position, Quaternion.identity) as GameObject;
        injuredPeople.Clear();

        //Reset walls
        foreach (WallGenerator wg in FindObjectsOfType<WallGenerator>())
        {
            wg.Reset();
        }

    }

    public void SortPerformance()
    {

        //First sort by time taken, then by injuries, then  by deaths
        performance.Sort(DeathStat.compareTimes);
        //As big times are considered better by the sorting thing, reverse the list bc we want the most efficient killing machines ever!!!(TM)
        performance.Reverse();

        performance.Sort(DeathStat.compareInjured);
        performance.Sort(DeathStat.compareDead);

    }

    public void CullBottom()
    {
        //Culls n/m of the list of stuff
        performance.RemoveRange(0, (int)Mathf.Floor(performance.Count * ((float)quizAnswers.percentageCulled/100f)));

    }

}
