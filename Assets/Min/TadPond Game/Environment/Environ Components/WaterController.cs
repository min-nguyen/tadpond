using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    private SpriteRenderer rend;
    private Transform trans;
    private EnvironmentController environmentController;
    private float sunlight  = 0.5f;
    private float rain = 0.5f;
    private float originalY;
    public float maxShift;
    public float shiftSpeed = 0.15f;

    /*  Boundary_LRUD
        Tadpond's Boundary_LRUD originates from Water size dimensions
    */
    private List<float> boundary_LRUD = new List<float>();
    /*  Algae Controller Functions
        IncreaseHealth(); DecreaseHealth(); SetHealth(int h);
    */
    private GameObject Algae;
    private AlgaeController AlgaeController;

    void Start () {
        AlgaeController = GetComponentInChildren<AlgaeController>();
        rend = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        boundary_LRUD.Insert(0 , (trans.position.x - (rend.bounds.size.x / 2)));
        boundary_LRUD.Insert(1,  (trans.position.x + (rend.bounds.size.x / 2)));
        boundary_LRUD.Insert(2,  (trans.position.y + (rend.bounds.size.y / 2)));
        boundary_LRUD.Insert(3,  (trans.position.y - (rend.bounds.size.y / 2)));
        originalY = trans.position.y;
        maxShift = rend.bounds.size.y / 20;
    }

    public void SetEnvironmentController(EnvironmentController envcont)
    {
        environmentController = envcont;
    }

    void Update () {
        if (sunlight > 0.7f && transform.position.y > originalY - maxShift )
        {
            float y = Time.deltaTime * shiftSpeed;
            rend.transform.position = new Vector3(rend.transform.position.x, rend.transform.position.y - y, rend.transform.position.z);
            boundary_LRUD[2] = (trans.position.y + (rend.bounds.size.y / 2));
            boundary_LRUD[3] = (trans.position.y - (rend.bounds.size.y / 2));
            environmentController.SetBoundaryLRUD(boundary_LRUD);
        }
        if (rain > 0.7f && transform.position.y < originalY + maxShift)
        {
            float y = Time.deltaTime * shiftSpeed;
            rend.transform.position = new Vector3(rend.transform.position.x, rend.transform.position.y + y, rend.transform.position.z);
            boundary_LRUD[2] = (trans.position.y + (rend.bounds.size.y / 2));
            boundary_LRUD[3] = (trans.position.y - (rend.bounds.size.y / 2));
            environmentController.SetBoundaryLRUD(boundary_LRUD);
        }
    }

    public void SetAlgaeHealth(float health)
    {
        AlgaeController.SetHealth(health);
    }
    public void SetSunlight(float sunlight)
    {
        this.sunlight = sunlight;
    }
    public void SetRain(float rain)
    {
        this.rain = rain;
    }
    public void IncreaseAlgae()
    {
        AlgaeController.IncreaseHealth();
    }

    //Cannot be called by EcosystemController until after EcosystemController finishes 'OnEnable()'
    public List<float> GetBoundaryLRUD()
    {
        return boundary_LRUD;
    }
}
