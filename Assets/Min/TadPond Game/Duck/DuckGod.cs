using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckGod : MonoBehaviour, OrganismGodInterface {

    public GameObject DUCK;
    public List<float> boundary_LRUD;
    private List<GameObject> ducks = new List<GameObject>();
    private int POPULATION = 0;
    private float timer = 0f;
    private float GLOBAL_HEALTH = 0f;
    float nutrients, sunlight, rain, watertemp, airtemp, pH, oxygen, algaeHealth;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
        if (boundary_LRUD.Count < 4)
        {
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
        for (int i = 0; i < ducks.Count; i++)
        {
            ducks[i].GetComponent<DuckController>().boundary_LRUD = boundary_LRUD;
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

    // Update is called once per frame
    void Update () {
        /* *
        * 
        * DO SOMETHING WITH GLOBAL_HEALTH
        * 
        * */
    }

    public void Kill(GameObject duck_)
    {
        ducks.Remove(duck_);
        Destroy(duck_);
        POPULATION--;
    }

    public void Spawn(int num, Vector3 position)
    {
        Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        GameObject duck = Instantiate(DUCK, position, spawnRotation) as GameObject;
        DuckController d = duck.GetComponent<DuckController>();
        d.SetGod(this.gameObject);
        d.boundary_LRUD = this.boundary_LRUD;
        ducks.Add(duck);
        ++POPULATION;
    }
    public void Spawn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 position = new Vector3(Random.Range(boundary_LRUD[0], boundary_LRUD[1]), boundary_LRUD[2] + 0.5f);
            Spawn(1, position);
        }
    }
}
