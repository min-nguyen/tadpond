using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Tadpole_Age1_Controller : MonoBehaviour, OrganismInterface {
    
    private GameObject MAIN_TADPOLE_CONTROLLER;
    private TadpoleController MAIN_TADPOLE_CONTROLLER_SCRIPT;
    //MOVEMENT RELATED VARIABLES
    public List<float> boundary_LRUD;
    private Vector3 target;
    private float minVelocity;
    private float maxVelocity;
    private float randomness;
    //HEALTH RELATED VARIABLES
    public List<string> prey;
    public List<string> predators;
    public float health;
    private float healthLIMIT = 50f;
    // Use this for initialization
    void Start () {
        InvokeRepeating("FindTarget", 0.0f, 2.0f);
        SetInitialValues();
        if (boundary_LRUD.Count < 4)
        {
           // Debug.Log("Boundary LRUD for TadpoleAge1 is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<float>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }

    void SetInitialValues()
    {
        minVelocity = 0.3f;
        maxVelocity = 1f;
        randomness = 5f;
        health = 0f;
    }

    public void SetGod(GameObject main_tadpole)
    {
        SetMainTadpoleController(main_tadpole);
    }

    public void SetMainTadpoleController(GameObject MainTadpoleController)
    {
        MAIN_TADPOLE_CONTROLLER = MainTadpoleController;
        MAIN_TADPOLE_CONTROLLER_SCRIPT = MainTadpoleController.GetComponent<TadpoleController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (prey.Contains(col.gameObject.tag))
        {
            Eat(col);
            OrganismInterface oi = col.gameObject.GetComponent<OrganismInterface>();
            oi.Die();
        }
    }

    void Eat(Collider2D food)
    {
        health += 10;
        transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f);
        if (health >= healthLIMIT && MAIN_TADPOLE_CONTROLLER != null)
            MAIN_TADPOLE_CONTROLLER_SCRIPT.HatchTadpoleAge2();
    }
	
	// Update is called once per frame
	void Update () {
        //EnableManualControl();
        UpdateMovement();
    }

    public void UpdateHealth(float health)
    {
        this.health = health;
    }

    void UpdateMovement()
    {
        float velocity = Random.Range(minVelocity, maxVelocity);
        //Calculate direction.
        Vector3 delta = target - transform.position;
        delta.Normalize();
        float rotation = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        //Calculate distance
        float deltaX = (target.x - transform.position.x) * Time.deltaTime * velocity;
        float deltaY = (target.y - transform.position.y) * Time.deltaTime * velocity;

        //Rotate and move.
        transform.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        transform.position += new Vector3(deltaX, deltaY, 0);
    }

    void FindTarget()
    {
        float offsetX = Random.Range(-randomness, randomness);
        float offsetY = Random.Range(-randomness, randomness);
        //Make a random target to swim towards.
        float newX = 0f;
        float newY = 0f;
        //Apply movement boundaries
        newX = transform.position.x + offsetX;
        newY = transform.position.y + offsetY;
        if (newX > boundary_LRUD[1])
            newX = transform.position.x - randomness;
        else if (newX < boundary_LRUD[0])
            newX = transform.position.x + randomness;
        if (newY > boundary_LRUD[2])
            newY = transform.position.y - randomness;
        else if (newY < boundary_LRUD[3])
            newY = transform.position.y + randomness;


        target = new Vector3(newX, newY, 0);    
    }


    void EnableManualControl()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
        }
    }

    public void Die()
    {
        if (MAIN_TADPOLE_CONTROLLER != null)
        {
            MAIN_TADPOLE_CONTROLLER.GetComponent<TadpoleController>().Destroy_Age1(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
}
