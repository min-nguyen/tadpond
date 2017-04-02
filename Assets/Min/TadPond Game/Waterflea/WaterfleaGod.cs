using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfleaGod : MonoBehaviour, OrganismGodInterface{

    public GameObject WATERFLEA;
    public List<int> boundary_LRUD;
    private List<GameObject> waterfleas = new List<GameObject>();
    private float POPULATION = 0;
    private float timer = 0f;

    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        if (boundary_LRUD.Count < 4)
        {
           // Debug.Log("Boundary LRUD for WaterfleaGod is not initialised in inspector with 4 values - creating default boundaries");
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

    }

    void Update()
    {
        
    }

    public void Kill(GameObject waterflea_)
    {
        waterfleas.Remove(waterflea_);
        Destroy(waterflea_);
        POPULATION--;
    }

}
