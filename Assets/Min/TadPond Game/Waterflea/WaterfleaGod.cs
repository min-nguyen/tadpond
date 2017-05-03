using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfleaGod : MonoBehaviour, OrganismGodInterface{

    public GameObject WATERFLEA;
    public List<float> boundary_LRUD;
    private List<GameObject> waterfleas = new List<GameObject>();
    private float POPULATION = 0;
    private float timer = 0f;
    private float GLOBAL_HEALTH = 0f;
    float nutrients, sunlight, rain, watertemp, airtemp, pH, oxygen, algaeHealth;

    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        if (boundary_LRUD.Count < 4)
        {
           // Debug.Log("Boundary LRUD for WaterfleaGod is not initialised in inspector with 4 values - creating default boundaries");
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
        for (int i = 0; i < waterfleas.Count; i++)
        {
            waterfleas[i].GetComponent<WaterfleaController>().boundary_LRUD = boundary_LRUD;
        }
    }
    public void Spawn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            for (int j = 1; j < 10; j++)
            {
                Vector3 position = new Vector3((boundary_LRUD[0] + (boundary_LRUD[1] - boundary_LRUD[0])/j), Random.Range(boundary_LRUD[3], boundary_LRUD[3] + 5f));
                Spawn(2, position);
            }
        }
    }
    public void Spawn(int num, Vector3 position)
    {
        for (int i = 0; i < num; i++)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            GameObject waterflea = Instantiate(WATERFLEA, position, spawnRotation) as GameObject;
            WaterfleaController c = waterflea.GetComponent<WaterfleaController>();
            c.boundary_LRUD = this.boundary_LRUD;
            c.SetGod(this.gameObject);
            waterfleas.Add(waterflea);
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
        if (waterfleas.Count > 0 && waterfleas.Count < 30)
        {
            timer += Time.deltaTime;
            int p = (int)Random.Range(0, waterfleas.Count);

            if (timer * GLOBAL_HEALTH > 1)
            {
                Vector3 spawn_pos = waterfleas[p].GetComponent<Transform>().position;
                Spawn(1, spawn_pos);
                timer = 0f;
            }
        }
    }

    public void Kill(GameObject waterflea_)
    {
        waterfleas.Remove(waterflea_);
        Destroy(waterflea_);
        POPULATION--;
    }

}
