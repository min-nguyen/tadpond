using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ZooplanktonGod : MonoBehaviour, OrganismGodInterface {

    public GameObject ZOOPLANKTON;
    public List<int> boundary_LRUD;
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

    public void Spawn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 position = new Vector3(Random.Range(boundary_LRUD[0], boundary_LRUD[1]), Random.Range(boundary_LRUD[3], boundary_LRUD[3] + 5f));
            Spawn(1, position);
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

    }

    void CalculateHealth()
    {
        GLOBAL_HEALTH = ((algaeHealth + nutrients) * 50) ;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (sunlight > 0.7f)
        {
            int p = (int)Random.Range(0, zooplankton.Count);
            CalculateHealth();
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
