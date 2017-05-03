using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismController : MonoBehaviour {

    /*      Organism Gods           */
    public GameObject TadpoleGodPrefab, PondWeedGodPrefab, WaterfleaGodPrefab, ZooplanktonGodPrefab, FishGodPrefab, DuckGodPrefab;
    private GameObject TadpoleGod, PondWeedGod, WaterfleaGod, ZooplanktonGod, FishGod, DuckGod;
    private TadpoleGod TadpoleGodScript;
    private PondWeedGod PondWeedGodScript;
    private WaterfleaGod WaterfleaGodScript;
    private ZooplanktonGod ZooplanktonGodScript;
    private DuckGod DuckGodScript;
    private FishGod FishGodScript;

    /*      SpawnRate Variables     */
    private float pondweed_spawnrate;
    private float waterflea_spawnrate;
    private float zooplankton_spawnrate;
    private float bitterling_spawnrate;
    private float carp_spawnrate;
    private float timer = 0f;

    /*      Core Components         */
    private GameObject Environment;
    private EnvironmentController EnvironmentController;
    private GameObject Water;
    public List<float> boundary_LRUD; // Optional water boundaries

    /*      Slider Variables        */
    private float nutrients = 0f, sunlight = 0f, rain = 0f;
    /*      Pond Variables          */
    private float waterTemp = 0f, airTemp = 0f, pH = 0f, 
                  oxygen = 0f, algaeHealth = 0f;


    void OnEnable()
    {   
        EnvironmentController = GetComponent<EnvironmentController>();
        if (EnvironmentController == null)
            Debug.Log("'OrganismController' script not attached to 'GameController' object - can't find 'EnvironmentController' script");
        else
        {
            Environment = EnvironmentController.Environment;
            InvokeRepeating("UpdateEnvironmentalValues", 1f, 1f);
        }

        if (boundary_LRUD.Count < 4)
        {
            boundary_LRUD = new List<float>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
        if (Environment == null)
            Debug.Log("'Environment object' not attached to 'EnvironmentController' script - can't find set environment boundaries");
        if (Environment != null)
        {
            for (int i = 0; i < Environment.transform.childCount; i++)
            {
                if (Environment.transform.GetChild(i).tag == "Water")
                {
                    Water = Environment.transform.GetChild(i).gameObject;
                    break;
                }
            }
            //Set Boundary_LRUD to Water Size Dimensions manually - Cannot use GetBoundaryLRUD() function in OnEnable().
            boundary_LRUD = new List<float>();
            boundary_LRUD.Insert(0, (Water.transform.position.x - (Water.GetComponent<SpriteRenderer>().bounds.size.x / 2)));
            boundary_LRUD.Insert(1, (Water.transform.position.x + (Water.GetComponent<SpriteRenderer>().bounds.size.x / 2)));
            boundary_LRUD.Insert(2, (Water.transform.position.y + (Water.GetComponent<SpriteRenderer>().bounds.size.y / 2)));
            boundary_LRUD.Insert(3, (Water.transform.position.y - (Water.GetComponent<SpriteRenderer>().bounds.size.y / 2)));
        }
        if (TadpoleGodPrefab != null)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            TadpoleGod = Instantiate(TadpoleGodPrefab, new Vector2(), spawnRotation) as GameObject;
            TadpoleGodScript = TadpoleGod.GetComponent<TadpoleGod>();
            TadpoleGodScript.SetBoundaryLRUD(boundary_LRUD);
        }
        if (PondWeedGodPrefab != null)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            PondWeedGod = Instantiate(PondWeedGodPrefab, new Vector2(), spawnRotation) as GameObject;
            PondWeedGodScript = PondWeedGod.GetComponent<PondWeedGod>();
            PondWeedGodScript.SetBoundaryLRUD(boundary_LRUD);
        }
        if (WaterfleaGodPrefab != null)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            WaterfleaGod = Instantiate(WaterfleaGodPrefab, new Vector2(), spawnRotation) as GameObject;
            WaterfleaGodScript = WaterfleaGod.GetComponent<WaterfleaGod>();
            WaterfleaGodScript.SetBoundaryLRUD(boundary_LRUD);
        }
        if (ZooplanktonGodPrefab != null)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            ZooplanktonGod = Instantiate(ZooplanktonGodPrefab, new Vector2(), spawnRotation) as GameObject;
            ZooplanktonGodScript = ZooplanktonGod.GetComponent<ZooplanktonGod>();
            ZooplanktonGodScript.SetBoundaryLRUD(boundary_LRUD);
        }
        if (FishGodPrefab != null)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            FishGod = Instantiate(FishGodPrefab, new Vector2(), spawnRotation) as GameObject;
            FishGodScript = FishGod.GetComponent<FishGod>();
            FishGodScript.SetBoundaryLRUD(boundary_LRUD);
        }
        if (DuckGodPrefab != null)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            DuckGod = Instantiate(DuckGodPrefab, new Vector2(), spawnRotation) as GameObject;
            DuckGodScript = DuckGod.GetComponent<DuckGod>();
            DuckGodScript.SetBoundaryLRUD(boundary_LRUD);
        }
    }

    public void SetBoundaryLRUD(List<float> boundLRUD)
    {
        boundary_LRUD = boundLRUD;
        TadpoleGodScript.SetBoundaryLRUD(boundary_LRUD);
        PondWeedGodScript.SetBoundaryLRUD(boundary_LRUD);
        WaterfleaGodScript.SetBoundaryLRUD(boundary_LRUD);
        FishGodScript.SetBoundaryLRUD(boundary_LRUD);
        DuckGodScript.SetBoundaryLRUD(boundary_LRUD);
    }

    void Start()
    {


    }

    void UpdateEnvironmentalValues()
    {
        nutrients = EnvironmentController.nutrients;
        sunlight = EnvironmentController.sunlight;
        rain = EnvironmentController.rain;
        waterTemp = EnvironmentController.waterTemp;
        airTemp = EnvironmentController.airTemp;
        pH = EnvironmentController.pH;
        oxygen = EnvironmentController.oxygen;
        algaeHealth = EnvironmentController.algaeHealth;

        TadpoleGodScript.UpdateEnvironmentalValues(nutrients, sunlight, rain, waterTemp, airTemp, pH, oxygen, algaeHealth);
        PondWeedGodScript.UpdateEnvironmentalValues(nutrients, sunlight, rain, waterTemp, airTemp, pH, oxygen, algaeHealth);
        ZooplanktonGodScript.UpdateEnvironmentalValues(nutrients, sunlight, rain, waterTemp, airTemp, pH, oxygen, algaeHealth);
        WaterfleaGodScript.UpdateEnvironmentalValues(nutrients, sunlight, rain, waterTemp, airTemp, pH, oxygen, algaeHealth);
        FishGodScript.UpdateEnvironmentalValues(nutrients, sunlight, rain, waterTemp, airTemp, pH, oxygen, algaeHealth);
        DuckGodScript.UpdateEnvironmentalValues(nutrients, sunlight, rain, waterTemp, airTemp, pH, oxygen, algaeHealth);
    }

	void Update () {
       
    }

    public void SpawnTadpole()
    {
        TadpoleGodScript.Spawn(1);
    }
    public void SpawnPondWeed()
    {
        PondWeedGodScript.Spawn(1);
    }
    public void SpawnWaterflea()
    {
        WaterfleaGodScript.Spawn(1);
    }
    public void SpawnZooPlankton()
    {
        ZooplanktonGodScript.Spawn(1);
    }
    public void SpawnCarp()
    {
        FishGodScript.Spawn(1, "Carp");
    }
    public void SpawnBitterling()
    {
        FishGodScript.Spawn(1, "Bitterling");
    }
    public void SpawnDuck()
    {
        DuckGodScript.Spawn(1);
    }

}
