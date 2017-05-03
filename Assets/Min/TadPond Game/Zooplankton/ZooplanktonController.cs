using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZooplanktonController : MonoBehaviour, OrganismInterface  {

    private GameObject ZOOPLANKTON_GOD;
    //MOVEMENT//    moveTimer & pauseDelay used to pause zooplankton at intervals
    public List<float> boundary_LRUD;
    private float moveTimer = 0f;
    private float pauseDelay = 1f;
    private float speed = 0.05f;
    private bool pause = false;
    private Vector3 originalPosition;
    private Vector3 destination;
    //HEALTH//      healthTimer & decayRate used to decay health if hasn't eaten
    public List<string> prey;
    public List<string> predators;
    private float health = 3;
    private float healthToMultiply = 6;
    private float healthTimer = 0f; 
    private float decayRate = 5f;
   

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        NewRandomPosition();
        if (boundary_LRUD.Count < 4)
        {
            Debug.Log("Boundary LRUD for ZooPlanktonController is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<float>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }
	public void SetGod(GameObject god)
    {
        ZOOPLANKTON_GOD = god;
    }
	// Update is called once per frame
	void Update () {
        healthTimer += Time.deltaTime;
        Move();
        UpdateHealth();
	}
    public void UpdateHealth(float health)
    {
        this.health = health;
    }
    void UpdateHealth()
    {
        if (healthTimer > decayRate)
        {
            health--;
            healthTimer = 0;
        }
        if(health == 0)
        {
            Die();
        }
        if(health >= healthToMultiply)
        {
            Reproduce();
            health = 3;
        }
    }

    void Eat(Collider2D coll)
    {
        health += 2;
        healthTimer = 0;
    }

    void Reproduce()
    {
        if (ZOOPLANKTON_GOD != null)
        {
            ZOOPLANKTON_GOD.GetComponent<ZooplanktonGod>().Spawn(2, transform.position);
        }
    }

    void Move() {
        if (Magnitude(destination.x - transform.position.x) < 0.5f && !pause) {
            NewRandomPosition();
            pause = true;
            pauseDelay = Random.Range(0.5f, 1.0f);
        }
        if (pause)
        {
            moveTimer += Time.deltaTime;
            if(moveTimer > pauseDelay)
            {
                moveTimer = 0;
                pause = false;
            }
        }
        else if(!pause){
            Vector2 difference = -transform.position + destination;
            difference.Normalize();
            
            transform.position = new Vector3(transform.position.x + difference.x * speed, transform.position.y + difference.y * speed);

            Vector3 vectorToTarget = destination - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (prey.Contains(coll.tag))
        {
            Eat(coll);
        }
        if (predators.Contains(coll.tag))
        {
            Die();
        }
    }
    
    public void Die()
    {
        if (ZOOPLANKTON_GOD != null)
        {
            ZOOPLANKTON_GOD.GetComponent<ZooplanktonGod>().Kill(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    void NewRandomPosition()
    {
        
        float x = Random.Range(-5, 5);
        float y = Random.Range(-5, 5);
        while (originalPosition.x + x < boundary_LRUD[0] || originalPosition.x + x > boundary_LRUD[1])
        {
            x = Random.Range(-5, 5);
        }
        while (originalPosition.y + y < boundary_LRUD[3] || (originalPosition.y + y > (boundary_LRUD[2] - (boundary_LRUD[2] - boundary_LRUD[3])*0.6)))
        {
            y = Random.Range(-5, 5);
        }
        speed = Random.Range(0.03f, 0.05f);
        destination = new Vector3(originalPosition.x + x, originalPosition.y + y);
        originalPosition = destination;
    }

    float Magnitude(float v)
    {
        if (v < 0)
            return -v;
        else
            return v;
    }
}
