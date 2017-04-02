using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGod : MonoBehaviour, OrganismGodInterface{

    public GameObject BITTERLING;
    public GameObject CARP;
    public List<int> boundary_LRUD;
    private List<GameObject> fish = new List<GameObject>();
    private float POPULATION = 0;
    private float timer = 0f;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
        if (boundary_LRUD.Count < 4)
        {
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

    }

    public void Spawn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 position = new Vector3(Random.Range(boundary_LRUD[0], boundary_LRUD[1]), boundary_LRUD[3] + (boundary_LRUD[2] - boundary_LRUD[3]) / 2 + Random.Range(-5, 5));
            Spawn(1, position);
        }
    }
    public void Spawn(int num, string fishtype)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 position = new Vector3(Random.Range(boundary_LRUD[0], boundary_LRUD[1]), boundary_LRUD[3] + (boundary_LRUD[2] - boundary_LRUD[3]) / 2 + Random.Range(-5, 5));
            Spawn(1, position, fishtype);
        }
    }
    public void Spawn(int num, Vector3 position, string fishtype)
    {
        if (fishtype == "Bitterling")
        {
            for (int i = 0; i < num; i++)
            {
                Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                GameObject bitterling = Instantiate(BITTERLING, position, spawnRotation) as GameObject;
                FishController c = bitterling.GetComponent<FishController>();
                c.SetGod(this.gameObject);
                c.boundary_LRUD = this.boundary_LRUD;
                fish.Add(bitterling);
                ++POPULATION;
            }
        }
        else if (fishtype == "Carp")
        {
            for (int i = 0; i < num; i++)
            {
                Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                GameObject carp = Instantiate(CARP, position, spawnRotation) as GameObject;
                FishController c = carp.GetComponent<FishController>();
                c.SetGod(this.gameObject);
                c.boundary_LRUD = this.boundary_LRUD;
                fish.Add(carp);
                ++POPULATION;
            }
        }
    }

    public void Spawn(int num, Vector3 position)
    {
        float random = Random.Range(-1, 1);
        if(random < 0)
            for (int i = 0; i < num; i++)
            {
                Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                GameObject bitterling = Instantiate(BITTERLING, position, spawnRotation) as GameObject;
                FishController c = bitterling.GetComponent<FishController>();
                c.SetGod(this.gameObject);
                c.boundary_LRUD = this.boundary_LRUD;
                fish.Add(bitterling);
                ++POPULATION;
            }
        else
            for (int i = 0; i < num; i++)
            {
                Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                GameObject carp = Instantiate(CARP, position, spawnRotation) as GameObject;
                FishController c = carp.GetComponent<FishController>();
                c.SetGod(this.gameObject);
                c.boundary_LRUD = this.boundary_LRUD;
                fish.Add(carp);
                ++POPULATION;
            }
    }
    
    void Update()
    {
        
    }

    public void Kill(GameObject fish_)
    {
        fish.Remove(fish_);
        Destroy(fish_);
        POPULATION--;
    }
}
