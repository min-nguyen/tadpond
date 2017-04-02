using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleGod : MonoBehaviour, OrganismGodInterface{

    public GameObject FULL_TADPOLE;
    public List<int> boundary_LRUD;
    public int flocksize = 5;
    private List<GameObject> tadpoles = new List<GameObject>();
    private float POPULATION = 0;
    private float timer = 0f;
    private float GLOBAL_HEALTH = 0f;
    float nutrients, sunlight, rain, watertemp, airtemp, pH, oxygen, algaeHealth;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
        InitialiseEggs();
        if (boundary_LRUD.Count < 4)
        {
           // Debug.Log("Boundary LRUD for TadpoleGod is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<int>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }


    public void SetBoundaryLRUD(List<int> LRUD)
    {
        boundary_LRUD = new List<int>();
        boundary_LRUD.Insert(0, LRUD[0]);
        boundary_LRUD.Insert(1, LRUD[1]);
        boundary_LRUD.Insert(2, LRUD[2]);
        boundary_LRUD.Insert(3, LRUD[3]);
  
    }

    public void UpdateEnvironmentalValues(float nutrients,
                                    float sunlight,
                                    float rain,
                                    float watertemp,
                                    float airtemp,
                                    float pH,
                                    float oxygen,
                                    float algaeHealth)
    {
        this.nutrients = nutrients;
        this.sunlight = sunlight;
        this.rain = rain;
        this.watertemp = watertemp;
        this.airtemp = airtemp;
        this.pH = pH;
        this.oxygen = oxygen;
        this.algaeHealth = algaeHealth;
        CalculateHealth();
    }

    void CalculateHealth()
    {
        GLOBAL_HEALTH = Mathf.Log10((algaeHealth + nutrients) * 50);
    }

    // Update is called once per frame
    void Update()
    {
        TestingMode();

        /* *
        * 
        * DO SOMETHING WITH GLOBAL_HEALTH
        * 
        * */
    }

    //Spawn an existing egg
    public void Spawn(int num)
    {
        Spawn(num, new Vector3());
    }
    public void Spawn(int num, Vector3 position)
    {
        int n = num;
        for (int i = 0; i < tadpoles.Count; i++)
        {
            if (n <= 0)
                break;

            TadpoleController tadpoleScript = tadpoles[i].GetComponent<TadpoleController>();
            if(tadpoleScript.STATE == TadpoleController.TADPOLE_STATE.EGG)
            {
                tadpoleScript.HatchTadpoleAge1();
                n--;
            }
        }
    }

    void InitialiseEggs()
    {
        //Spawns flocksize number of eggs randomly around positioned God prefab
        Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        for (int i = 0; i < flocksize; i++)
        {
            GameObject tadpole = Instantiate(FULL_TADPOLE, new Vector2(Random.Range(transform.position.x - 1.5f, transform.position.x + 1.5f), Random.Range(transform.position.y - 0.2f, transform.position.y + 0.2f)),
                spawnRotation) as GameObject;
            tadpole.GetComponent<TadpoleController>().SetGod(this.gameObject);
            tadpole.GetComponent<TadpoleController>().boundary_LRUD = this.boundary_LRUD;
            tadpoles.Add(tadpole);
        }
        POPULATION = flocksize;
    }

    public void Kill(GameObject tadpole)
    {
        tadpoles.Remove(tadpole);
        Destroy(tadpole);
        POPULATION--;
    }
    
    public void HatchEgg()
    {
        GameObject tadpole = null;
        for(int i = 0; i < tadpoles.Count; i++)
        {
            if (tadpoles[i].GetComponent<TadpoleController>().STATE == TadpoleController.TADPOLE_STATE.EGG)
            {
                tadpole = tadpoles[i];
                break;
            }
        }
        if (tadpole != null)
        {
            tadpole.GetComponent<TadpoleController>().HatchTadpoleAge1();
        }
    }
	
	

    void TestingMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            HatchEgg();
    }
}
