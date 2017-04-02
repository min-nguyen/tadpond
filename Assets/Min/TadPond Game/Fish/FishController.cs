using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//

public class FishController : MonoBehaviour, OrganismInterface{

    public List<int> boundary_LRUD;
    public List<string> prey;
    public List<string> predators;
    private GameObject FISH_GOD;
    private GameObject DETECTOR_CIRCLE;
    private Animator animator;
    private float health = 6f;
    private float maxSizeScale = 1.6f;
    private float growthFactor = 0.05f;
    private float swimDelay = 0f;
    private float pauseDelay = 0f;
    private float moveTimer = 0f;
    private float healthTimer = 0f;
    private float decayRate = 2.5f;
    private Transform target;
    private bool facingRight;
    private int life;

    public FishBehaviourInterface currentState;
    ChaseState chase;
    IdleState idle;
    SwimState swim;

    void Awake()
    {
        DETECTOR_CIRCLE = transform.FindChild("FISH_DETECTOR_CIRCLE").gameObject;
        chase = new ChaseState(this);
        idle = new IdleState(this);
        swim = new SwimState(this);
        animator = GetComponent<Animator>();
        currentState = idle;
        setFacingRight();
    }

    void Start()
    {
        if (boundary_LRUD.Count < 4)
        {
            // Debug.Log("Boundary LRUD for FishController is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<int>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }

    public void SetGod(GameObject god)
    {
        FISH_GOD = god;
    }

    public void setTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public Transform getTarget()
    {
        return target;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (prey.Contains(coll.tag) && currentState.ToString() == "ChaseState")
        {
            Eat(coll.gameObject);
            Destroy(coll.gameObject);
        }
        //else if (predators.Contains(coll.tag))
        //{
        //   FISH_GOD.GetComponent<FishGod>().Destroy();
        //   Destroy(this.gameObject);
        //}
    }

    public void Eat(GameObject coll)
    {
        //Increase size of object and detector circle
        if (transform.localScale.x < maxSizeScale)
        {
            transform.localScale = new Vector3(transform.localScale.x + growthFactor, transform.localScale.y + growthFactor, 0);
            DETECTOR_CIRCLE.GetComponent<FishDetectorCircleScript>().IncreaseSize(growthFactor);
        }
        health += 2;
        healthTimer = 0;
        setToIdleState();
    }

    public void Die()
    {
        FISH_GOD.GetComponent<FishGod>().Kill(this.gameObject);
    }

    public void UpdateHealth(float health)
    {
        this.health = health;
    }

    // Update is called once per frame
    void Update()
    {
        EnableAutomaticBehaviour();
    }

    public float GetHealth()
    {
        return health;
    }

    //Automatic movement pattern
    void EnableAutomaticBehaviour()
    {
        //Apply movement boundaries
        if ((transform.position.x < boundary_LRUD[0] && !facingRight))
            setFacingRight();
        else if ((transform.position.x > boundary_LRUD[1] && facingRight))
            setFacingLeft();
        if (transform.position.y > boundary_LRUD[2] || transform.position.y < boundary_LRUD[3])
            setToIdleState();   //Idle applies returning to original position


        moveTimer += Time.deltaTime;
        healthTimer += Time.deltaTime;
        // Switch between move & idle states periodically
        if (currentState != chase)
        {
            if (moveTimer > pauseDelay && currentState == idle)
            {
                swimDelay = Random.Range(5f, 8f);
                moveTimer = 0;
                setToSwimState();
            }
            else if (moveTimer > swimDelay && currentState == swim)
            {
                pauseDelay = Random.Range(2f, 3f);
                if (pauseDelay < 2.5f)
                    ChangeDirection();
                setToIdleState();
                moveTimer = 0;

            }
        }
        //Decay health
        if (healthTimer > decayRate)
        {
            --health;
            healthTimer = 0;
        }

        currentState.updateState();
    }

    public void setFacingRight()
    {
        facingRight = true;
        animator.SetBool("facingRight", true);
    }
    public void setFacingLeft()
    {
        facingRight = false;
        animator.SetBool("facingRight", false);
    }
    public bool getFacingRight()
    {
        if (facingRight)
            return true;
        else
            return false;
    }
    public void ChangeDirection()
    {
        if (facingRight)
            setFacingLeft();
        else
            setFacingRight();
        currentState.onReturn();
    }

    public void setToChaseState()
    {
        if (target == null)
        {
            Debug.Log("No target assigned to Fish - can't chase");
            return;
        }
        animator.SetBool("moving", true);
        currentState = chase;
        currentState.onReturn();
    }

    public void setToIdleState()
    {
        animator.SetBool("moving", false);
        currentState = idle;
        currentState.onReturn();
    }

    public void setToSwimState()
    {
        animator.SetBool("moving", true);
        currentState = swim;
        currentState.onReturn();
    }

    //Key input controlled movement
    void EnableManualControl()
    {
        /*
        //CHASE
        if (Input.GetKeyDown(KeyCode.A))
        {
            setToChaseState();
        }//IDLE
        else if (Input.GetKeyDown(KeyCode.S)) {
            setToIdleState();
        }//SWIM
        else if (Input.GetKeyDown(KeyCode.D))
        {
            setToSwimState();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            changeDirection();
        }
        */
    }




}
