using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondWeedGod : MonoBehaviour, OrganismGodInterface{

    public GameObject PONDWEED;
    public List<int> boundary_LRUD;
    private List<GameObject> pondweed = new List<GameObject>();
    private float SPAWN_RATE = 5f;
    private float GLOBAL_HEALTH = 0f;
    private float POPULATION = 0;
    private float timer = 0f;
    float nutrients, sunlight, rain, watertemp, airtemp, pH, oxygen, algaeHealth;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
        InitialisePondWeed();
        if (boundary_LRUD.Count < 4)
        {
            // Debug.Log("Boundary LRUD for PondWeedGod is not initialised in inspector with 4 values - creating default boundaries");
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
                                    float waterTemp,
                                    float airTemp,
                                    float pH,
                                    float oxygen,
                                    float algaeHealth)
    {
        this.nutrients = nutrients;
        this.sunlight = sunlight;
        this.rain = rain;
        this.watertemp = waterTemp;
        this.airtemp = airTemp;
        this.pH = pH;
        this.oxygen = oxygen;
        this.algaeHealth = algaeHealth;
    }

    public void Spawn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 position = new Vector3(Random.Range(boundary_LRUD[0], boundary_LRUD[1]), boundary_LRUD[3] + Random.Range(-0.5f, 0.5f));
            Spawn(1, position);
        }
    }
    public void Spawn(int num, Vector3 position)
    {
        for (int i = 0; i < num; i++)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            GameObject pondweed = Instantiate(PONDWEED, position, spawnRotation) as GameObject;
            this.pondweed.Add(pondweed);
            PondWeedController c = pondweed.GetComponent<PondWeedController>();
            c.SetGod(this.gameObject);
            ++POPULATION;
        }
    }
    void InitialisePondWeed()
    {
        //Set up pond weed
        for (int i = boundary_LRUD[0]; i < boundary_LRUD[1]; i += 2)
        {
            float randY = Random.Range(-1, 1);
            Spawn(1, new Vector3(i, boundary_LRUD[3] + randY));
        }
    }

    public void Kill(GameObject pondweed_)
    {
        pondweed.Remove(pondweed_);
        Destroy(pondweed_);
        POPULATION--;
    }

    void CalculateHealth()
    {
        GLOBAL_HEALTH = ((rain + sunlight + nutrients)*50)/pondweed.Count;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (sunlight > 0.7f)
        {
            int p = (int)Random.Range(0, pondweed.Count);
            CalculateHealth();
            if (timer * GLOBAL_HEALTH > 1)
            {
                pondweed[p].GetComponent<PondWeedController>().UpdateHealth(1);
                timer = 0f;
            }
        }

    }
}
