using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Genetic : MonoBehaviour {

    public int perGeneration = 20;
    public int timeLimit = 5;

    //First Population Bounds
    public Vector2 bounds_DropX;
    public Vector2 bounds_DropY;
    public Vector2 bounds_Mass;

    public Vector2 bounds_WheelRadius;
    public Vector2 bounds_WheelPosition;
    public Vector2 bounds_WheelSpeed;
    public int wheelLimit = 5;
       
    public Vector2 bounds_ShooterAmmo;
    public Vector2 bounds_ShooterForce;
    public Vector2 bounds_ShooterPosition;
    public Vector2 bounds_ShooterDelay;
    public int shooterLimit = 3;

    //Stuff to make individuals out of
    public GameObject baseObject;
    public GameObject wheelObject;
    public GameObject shooterObject;

    //UI Stuff
    public Text infoText;
    
    //Private stuff
    private Manager manager;
    private float counter;
    private int currentGeneration;
    private int currentMember;
    private GameObject currentBeing; //the current gameobject (so we can destroy it)

    #region Unity Stuff

    // Use this for initialization
    void Start () {

        manager = FindObjectOfType<Manager>();
        infoText = GameObject.Find("GenerationInfo").GetComponent<Text>();
        currentGeneration = 1;
        currentMember = 0;

	}
	
	// Update is called once per frame
	void Update () {

        if (manager.playing)
        {

            perGeneration = manager.quizAnswers.population;

            //Update Timer
            counter += Time.deltaTime;

            if (counter > timeLimit)
            {
                counter = 0;
                manager.Reset();

                //End of generation - reset stuff and sort the list to cull the shit ones
                if (currentMember == perGeneration)
                {
                    currentGeneration++;
                    currentMember = 0;
                    manager.SortPerformance();
                    manager.CullBottom();
                }

                currentMember++;
                if (currentBeing)
                    Destroy(currentBeing);

                if (currentGeneration == 1)
                {
                    //Generate a random population
                    GeneData newGenes = GenerateRandomGeneData();
                    ParseGeneData(newGenes);
                    manager.currentGenes = newGenes;
                }
                else
                {
                    //Breed them randomly
                    GeneData newGenes = Breed(manager.performance[Random.Range(0, manager.performance.Count)].genes, manager.performance[Random.Range(0, manager.performance.Count)].genes);
                    //25% chance of mutation
                    ParseGeneData(ROTF ? Mutate(newGenes) : newGenes);
                    manager.currentGenes = newGenes;
                }
            }

            //Update UI
            infoText.text = "Generation: " + currentGeneration + "\n" + "Member: " + currentMember;

        }
	}

    #endregion

    #region Genes

    GeneData GenerateRandomGeneData()
    {

        GeneData genes = new GeneData();

        genes.dropX = Random.Range(bounds_DropX.x, bounds_DropX.y);
        genes.dropY = Random.Range(bounds_DropY.x, bounds_DropY.y);

        genes.mass = Random.Range(bounds_Mass.x, bounds_Mass.y);

        int numberOfWheels = Mathf.Clamp (Random.Range(-wheelLimit, wheelLimit), 0, wheelLimit);
        for (int i = 0; i < numberOfWheels; i++)
        {
            Wheel wheel = new Wheel();
            wheel.radius = Random.Range(bounds_WheelRadius.x, bounds_WheelRadius.y);
            wheel.position = new Vector3(Random.Range(bounds_WheelPosition.x, bounds_WheelPosition.y), Random.Range(bounds_WheelPosition.x, bounds_WheelPosition.y), transform.position.z);
            wheel.spinSpeed = Random.Range(bounds_WheelSpeed.x, bounds_WheelSpeed.y);
            genes.wheels.Add(wheel);
        }


        int numberOfShooters = Mathf.Clamp(Random.Range(-shooterLimit, shooterLimit), 0, shooterLimit);
        for (int i = 0; i < numberOfShooters; i++)
        {
            Shooter shooter = new Shooter();
            //We could make this point away using the dot product (do this if you want) but I want it randomererererer(TM)
            shooter.rotation = Quaternion.Euler (new Vector3(0,0, Random.Range (0,359)));
            shooter.position = new Vector3(Random.Range(bounds_ShooterPosition.x, bounds_ShooterPosition.y), Random.Range(bounds_ShooterPosition.x, bounds_ShooterPosition.y), transform.position.z);
            shooter.shootForce = Random.Range(bounds_ShooterForce.x, bounds_ShooterForce.y);
            shooter.shootDelay = Random.Range(bounds_ShooterDelay.x, bounds_ShooterDelay.y);
            shooter.ammo = (int)Random.Range(bounds_ShooterAmmo.x, bounds_ShooterAmmo.y);
            shooter.ammoType = Random.Range(0, 100);
            genes.shooters.Add(shooter);
        }

        return genes;

    }

    GeneData Breed (GeneData genes1, GeneData genes2)
    {
        GeneData child = new GeneData();

        //TODO: Optimize with reflection
        child.dropX = ROOZ ? genes1.dropX : genes2.dropX;
        child.dropY = ROOZ ? genes1.dropY : genes2.dropY;
        child.mass = ROOZ ? genes1.mass : genes2.mass;
        child.wheels = ROOZ ? genes1.wheels : genes2.wheels;
        child.shooters = ROOZ ? genes1.shooters : genes2.shooters;

        return child;

    }

    GeneData Mutate(GeneData genes)
    {

        GeneData mutant = genes;

        //TODO: Optimize with reflection

        float dropXMutation = (ROOZ ? -1 : 1) * Random.Range(0, (bounds_DropX.y - bounds_DropX.x)) / 10;
        mutant.dropX += (ROTF ? dropXMutation : 0);

        float dropYMutation = (ROOZ ? -1 : 1) * Random.Range(0, (bounds_DropY.y - bounds_DropY.x)) / 10;
        mutant.dropY += (ROTF ? dropYMutation : 0);

        float massMutation = (ROOZ ? -1 : 1) * Random.Range(0, (bounds_Mass.y - bounds_Mass.x)) / 100;
        mutant.mass += (ROTF ? massMutation : 0);

        //Wheels  =====================
        //We add or remove wheels randomly 25%

        if (ROTF) {

            int wheelNumber = mutant.wheels.Count + (ROOZ ? -1 : 1);

            if (wheelNumber > mutant.wheels.Count)
            {
                //If there are now more wheels add a wheel
                Wheel wheel = new Wheel();
                wheel.radius = Random.Range(bounds_WheelRadius.x, bounds_WheelRadius.y);
                wheel.position = new Vector3(Random.Range(bounds_WheelPosition.x, bounds_WheelPosition.y), Random.Range(bounds_WheelPosition.x, bounds_WheelPosition.y), transform.position.z);
                wheel.spinSpeed = Random.Range(bounds_WheelSpeed.x, bounds_WheelSpeed.y);
                mutant.wheels.Add(wheel);
            }
            else
            {
                //Otherwise remove a random wheel
                mutant.wheels.RemoveAt(Random.Range(0, mutant.wheels.Count));
            }
        }

        //Mutate wheel settings randomly
        foreach (Wheel wheel in mutant.wheels) {

            float wheelRadiusMutation = (ROOZ ? -1 : 1) * Random.Range(0, (bounds_WheelRadius.y - bounds_WheelRadius.x)) / 10;
            if (wheel.radius + wheelRadiusMutation > 0)
            {
                wheel.radius += (ROTN ? wheelRadiusMutation : 0);
            }

            Vector3 wheelPositionMutation = new Vector3 ((ROOZ ? -1 : 1) * Random.Range(0, (bounds_WheelPosition.y - bounds_WheelPosition.x)) / 10, (ROTF ? -1 : 1) * Random.Range(0, (bounds_WheelPosition.y - bounds_WheelPosition.x)) / 10, 0);
            wheel.position += (ROTN ? wheelPositionMutation : Vector3.zero);

            float wheelSpeedMutation = (ROOZ ? -1 : 1) * Random.Range(0, (bounds_WheelSpeed.y - bounds_WheelSpeed.x)) / 10;
            wheel.spinSpeed += (ROTN ? wheelSpeedMutation : 0);

        }

        foreach (Shooter shooter in mutant.shooters)
        {

            float shooterShootForceMutation = (ROOZ ? -1 : 1) * Random.Range(0, (bounds_ShooterForce.y - bounds_ShooterForce.x)) / 10;
            shooter.shootForce += (ROTN ? shooterShootForceMutation : 0);

            float shooterShootDelayMutation = (ROOZ ? -1 : 1) * Random.Range(0, (bounds_ShooterDelay.y - bounds_ShooterDelay.x)) / 10;
            shooter.shootDelay += (ROTN ? shooterShootDelayMutation : 0);

            Vector3 shooterPositionMutation = new Vector3((ROOZ ? -1 : 1) * Random.Range(0, (bounds_ShooterPosition.y - bounds_ShooterPosition.x)) / 10, (ROTF ? -1 : 1) * Random.Range(0, (bounds_ShooterPosition.y - bounds_ShooterPosition.x)) / 10, 0);
            shooter.position += (ROTN ? shooterPositionMutation : Vector3.zero);

            int shooterAmmoMutation = (ROOZ ? -1 : 1) * (int)Random.Range(0, (bounds_ShooterAmmo.y - bounds_ShooterAmmo.x)) / 10;
            shooter.ammo += (ROTN ? shooterAmmoMutation : 0);

            int shooterAmmoTypeMutation = Random.Range (1, 100);
            shooter.ammoType += (ROTN ? shooterAmmoTypeMutation : 0);

            //This is special bc literally no one thinks in quaternions (except for hamilton). Note how we are simply assigning directly instead of adding anything
            Quaternion shooterRotationMutation = Quaternion.Euler (shooter.rotation.eulerAngles + new Vector3(0, 0, Random.Range(0, 359)));
            shooter.rotation = (ROTN ? shooterRotationMutation :  shooter.rotation);

        }


        return mutant;

    }

#region Random Stuff
    
    //These provide just enough randomness for stuff to happen

    //Random one or zero (50% chance)
    bool ROOZ
    {
        get { return Random.Range(0, 1) == 1; }
    }

    //Random 0 to 3 (25% chance)
    bool ROTF
    {
        get { return Random.Range(0, 3) == 1; }
    }

    //Random 0 to N (???% chance)
    bool ROTN
    {
        get { return Random.Range(0, manager.quizAnswers.mutationChance) == 1; }
    }

    #endregion

    void ParseGeneData(GeneData geneData)
    {

        GameObject container = new GameObject("BeingContainer");

        GameObject baseGO = Instantiate(baseObject, new Vector2(geneData.dropX, geneData.dropY), Quaternion.identity, container.transform) as GameObject;
        baseGO.GetComponent<Rigidbody2D>().mass = geneData.mass;

        foreach (Wheel wheel in geneData.wheels)
        {
            GameObject wheelGO = Instantiate(wheelObject, baseGO.transform.position + wheel.position, wheel.rotation, container.transform) as GameObject;
            wheelGO.GetComponent<WheelJoint2D>().connectedBody = baseGO.GetComponent<Rigidbody2D>();
            wheelGO.transform.localScale = new Vector3(wheel.radius, wheel.radius, 1);
            WheelJoint2D joint = wheelGO.GetComponent<WheelJoint2D>();
            JointMotor2D motor = joint.motor;

            wheelGO.GetComponent<Rigidbody2D>().mass = baseGO.GetComponent<Rigidbody2D>().mass / geneData.wheels.Count;
            float mass = wheelGO.GetComponent<Rigidbody2D>().mass;
            motor.motorSpeed = Random.Range(bounds_WheelSpeed.x, bounds_WheelSpeed.y) * mass;
            motor.maxMotorTorque = motor.motorSpeed * mass / 10000000;
            joint.motor = motor;
        }
        
        foreach (Shooter shooter in geneData.shooters)
        {
            GameObject shooterGO = Instantiate(shooterObject, baseGO.transform.position + shooter.position, shooter.rotation, baseGO.transform) as GameObject;
            ShooterLogic sl = shooterGO.GetComponent<ShooterLogic>();
            sl.shootForce = shooter.shootForce;
            sl.shootDelay = shooter.shootDelay;
            sl.ammo = shooter.ammo;
            sl.ammoType = shooter.ammoType;
                        
        }

        currentBeing = container;

    }

    #endregion

}

#region Gene Classes

[System.Serializable]
public class GeneData
{
    public float dropX;
    public float dropY;
    public float mass;
    public List<Wheel> wheels = new List<Wheel>();
    public List<Shooter> shooters = new List<Shooter>();

}

[System.Serializable]
public class Wheel
{
    public float radius;
    public Vector3 position;
    public Quaternion rotation;
    public float spinSpeed;
}

[System.Serializable]
public class Shooter
{
    public Vector3 position;
    public Quaternion rotation;
    public float shootForce;
    public float shootDelay;
    public int ammo;
    public int ammoType;
}

#endregion