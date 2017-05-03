using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ZooplanktonGod : MonoBehaviour, OrganismGodInterface {

    public GameObject ZOOPLANKTON;
    public List<float> boundary_LRUD;
    private List<GameObject> zooplankton = new List<GameObject>();
    private float POPULATION = 0;
    private float timer = 0f;
    private float GLOBAL_HEALTH = 0f;
    float nutrients, sunlight, rain, watertemp, airtemp, pH, oxygen, algaeHealth;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
        if (boundary_LRUD.Count < 4)
        {
           // Debug.Log("Boundary LRUD for ZooPlanktonGod is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<float>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }

    public void SetBoundaryLRUD(List<float> LRUD)
    {
        boundary_LRUD = new List<float>();
        boundary_LRUD.Insert(0, LRUD[0]);
        boundary_LRUD.Insert(1, LRUD[1]);
        boundary_LRUD.Insert(2, LRUD[2]);
        boundary_LRUD.Insert(3, LRUD[3]);
        for (int i = 0; i < zooplankton.Count; i++)
        {
            zooplankton[i].GetComponent<ZooplanktonController>().boundary_LRUD = boundary_LRUD;
        }
    }

    public void Spawn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            for (int j = 1; j < 10; j++)
            {
                Vector3 position = new Vector3((boundary_LRUD[0] + (boundary_LRUD[1] - boundary_LRUD[0]) / j), Random.Range(boundary_LRUD[3], boundary_LRUD[3] + 5f));
                Spawn(2, position);
            }
        }
    }
    public void Spawn(int num, Vector3 position)
    {
        for (int i = 0; i < num; i++)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            GameObject zooplankton = Instantiate(ZOOPLANKTON, position, spawnRotation) as GameObject;
            ZooplanktonController c = zooplankton.GetComponent<ZooplanktonController>();
            c.boundary_LRUD = this.boundary_LRUD;
            c.SetGod(this.gameObject);
            this.zooplankton.Add(zooplankton);
            ++POPULATION;
        }
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

    void Update()
    {
        if (zooplankton.Count > 0 && zooplankton.Count < 30)
        {
            timer += Time.deltaTime;
            int p = (int)Random.Range(0, zooplankton.Count);
            
            if (timer * GLOBAL_HEALTH > 1)
            {
                Vector3 spawn_pos = zooplankton[p].GetComponent<Transform>().position;
                Spawn(1, spawn_pos);
                timer = 0f;
            }
        }
    }

    public void Kill(GameObject zooplankton_)
    {
        zooplankton.Remove(zooplankton_);
        Destroy(zooplankton_);
        POPULATION--;
    }
    
}
