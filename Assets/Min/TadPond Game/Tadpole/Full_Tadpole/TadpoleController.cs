using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TadpoleController : MonoBehaviour
{
    public enum TADPOLE_STATE{
        EGG, AGE1, AGE2, AGE3
    };

    public GameObject TADPOLE_EGG, TADPOLE_AGE1, TADPOLE_AGE2, TADPOLE_AGE3;
    public TADPOLE_STATE STATE;
    public List<int> boundary_LRUD;
    private GameObject TADPOLE_GOD;
    private GameObject currentTadpole;
    private Renderer rend;


    // Use this for initialization
    void Start()
    {
        setInitialValues();
        SpawnTadpoleEgg();
        rend.enabled = false;
        if (boundary_LRUD.Count < 4)
        {
          //  Debug.Log("Boundary LRUD for TadpoleController is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<int>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }

    void setInitialValues()
    {
        rend = GetComponent<Renderer>();

        //PREFABS 
        if (TADPOLE_EGG == null)
            Debug.Log("PLEASE ATTACH PREFAB NAMED 'TADPOLE_EGG'");
        if (TADPOLE_AGE1 == null)
            Debug.Log("PLEASE ATTACH PREFAB NAMED 'TADPOLE_AGE1'");
        if (TADPOLE_AGE2 == null)
            Debug.Log("PLEASE ATTACH PREFAB NAMED 'TADPOLE_AGE2'");
        if (TADPOLE_AGE3 == null)
            Debug.Log("PLEASE ATTACH PREFAB NAMED 'TADPOLE_AGE3'");
        if (TADPOLE_GOD == null)
            Debug.Log("NO TADPOLE GOD SET");
    }

    //Call Destroy functions only from the actual game object
    public void Destroy_Egg(GameObject egg)
    {
        Destroy(egg);
        if (TADPOLE_GOD != null)
        {
            TADPOLE_GOD.GetComponent<TadpoleGod>().Kill(this.gameObject);
        }
    }

    public void Destroy_Age1(GameObject age1)
    {
        Destroy(age1);
        if (TADPOLE_GOD != null)
        {
            TADPOLE_GOD.GetComponent<TadpoleGod>().Kill(this.gameObject);
        }
    }

    public void Destroy_Age2(GameObject age2)
    {
        Destroy(age2);
        if (TADPOLE_GOD != null)
        {
            TADPOLE_GOD.GetComponent<TadpoleGod>().Kill(this.gameObject);
        }
    }

    public void Destroy_Age3(GameObject age3)
    {
        Destroy(age3);
        if (TADPOLE_GOD != null)
        {
            TADPOLE_GOD.GetComponent<TadpoleGod>().Kill(this.gameObject);
        }
    }

    public void SetGod(GameObject god)
    {
        TADPOLE_GOD = god;
    }

    void SpawnTadpoleEgg()
    {
        STATE = TADPOLE_STATE.EGG;
        Vector3 spawnPosition = new Vector3(transform.position.x + UnityEngine.Random.Range(-0.3f, 0.3f),
                transform.position.y + UnityEngine.Random.Range(-0.3f, 0.3f));
        Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        GameObject tadpoleEgg = Instantiate(TADPOLE_EGG, spawnPosition, spawnRotation) as GameObject;
        tadpoleEgg.GetComponent<TadpoleEggController>().SetMainTadpoleController(this.gameObject);
        currentTadpole = tadpoleEgg;
       
    }
    
    public void HatchTadpoleAge1()
    {
        if (STATE == TADPOLE_STATE.EGG) { 
            Vector2 spawnPosition = new Vector2(currentTadpole.transform.position.x, currentTadpole.transform.position.y);
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            Destroy(currentTadpole);
            GameObject tadpoleAge1 = Instantiate(TADPOLE_AGE1, spawnPosition, spawnRotation) as GameObject;
            tadpoleAge1.GetComponent<Tadpole_Age1_Controller>().SetMainTadpoleController(this.gameObject);
            tadpoleAge1.GetComponent<Tadpole_Age1_Controller>().boundary_LRUD = this.boundary_LRUD;
            currentTadpole = tadpoleAge1;
            STATE = TADPOLE_STATE.AGE1;
            
        }
    }

    public void HatchTadpoleAge2()
    {
        if (STATE == TADPOLE_STATE.AGE1)
        {
            Vector2 spawnPosition = new Vector2(currentTadpole.transform.position.x, currentTadpole.transform.position.y);
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            Destroy(currentTadpole);
            GameObject tadpoleAge2 = Instantiate(TADPOLE_AGE2, spawnPosition, spawnRotation) as GameObject;
            tadpoleAge2.GetComponent<Tadpole_Age2_Controller>().SetMainTadpoleController(this.gameObject);
            tadpoleAge2.GetComponent<Tadpole_Age2_Controller>().boundary_LRUD = this.boundary_LRUD;
            currentTadpole = tadpoleAge2;
            STATE = TADPOLE_STATE.AGE2;
        }
    }

    public void HatchTadpoleAge3()
    {
        if (STATE == TADPOLE_STATE.AGE2)
        {
            Vector2 spawnPosition = new Vector2(currentTadpole.transform.position.x, currentTadpole.transform.position.y);
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            Destroy(currentTadpole);
            GameObject tadpoleAge3 = Instantiate(TADPOLE_AGE3, spawnPosition, spawnRotation) as GameObject;
            tadpoleAge3.GetComponent<Tadpole_Age3_Controller>().SetMainTadpoleController(this.gameObject);
            tadpoleAge3.GetComponent<Tadpole_Age3_Controller>().boundary_LRUD = this.boundary_LRUD;
            currentTadpole = tadpoleAge3;
            STATE = TADPOLE_STATE.AGE3;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseHealth(float h)
    {
        if (STATE == TADPOLE_STATE.EGG)
        {
            currentTadpole.GetComponent<TadpoleEggController>().health += h;
        }
        else if (STATE == TADPOLE_STATE.AGE1)
        {
            currentTadpole.GetComponent<Tadpole_Age1_Controller>().health += h;
        }
        else if (STATE == TADPOLE_STATE.AGE2)
        {
            currentTadpole.GetComponent<Tadpole_Age2_Controller>().health += h;
        }
        else if (STATE == TADPOLE_STATE.AGE3)
        {
            currentTadpole.GetComponent<Tadpole_Age3_Controller>().health += h;
        }
    }

    void TestingMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HatchTadpoleAge2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HatchTadpoleAge3();
        }
    } 
}
