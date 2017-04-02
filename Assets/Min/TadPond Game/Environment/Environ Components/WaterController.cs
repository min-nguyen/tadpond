using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    private SpriteRenderer rend;
    private Transform trans;
    
    /*  Boundary_LRUD
        Tadpond's Boundary_LRUD originates from Water size dimensions
    */
    private List<int> boundary_LRUD = new List<int>();
    /*  Algae Controller Functions
        IncreaseHealth(); DecreaseHealth(); SetHealth(int h);
    */
    private GameObject Algae;
    private AlgaeController AlgaeController;

    void Start () {
        AlgaeController = GetComponentInChildren<AlgaeController>();
        rend = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        boundary_LRUD.Insert(0 , (int) (trans.position.x - (rend.bounds.size.x / 2)));
        boundary_LRUD.Insert(1,  (int) (trans.position.x + (rend.bounds.size.x / 2)));
        boundary_LRUD.Insert(2,  (int) (trans.position.y + (rend.bounds.size.y / 2)));
        boundary_LRUD.Insert(3,  (int) (trans.position.y - (rend.bounds.size.y / 2)));
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

    void Update () {

	}

    public void SetAlgaeHealth(float health)
    {
        AlgaeController.SetHealth(health);
    }

    public void IncreaseAlgae()
    {
        AlgaeController.IncreaseHealth();
    }

    //Cannot be called by EcosystemController until after EcosystemController finishes 'OnEnable()'
    public List<int> GetBoundaryLRUD()
    {
        return boundary_LRUD;
    }
}
