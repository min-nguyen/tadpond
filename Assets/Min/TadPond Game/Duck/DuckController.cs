using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour, OrganismInterface {

    //Movement Related Variables
    public List<int> boundary_LRUD;
    private bool facingRight;
    private bool returningFromEat;
    private float moveTimer = 0f;
    private float originalY;
    private float originalX;
    private float hoverScale;
    private float swimSpeed = 2f;
    //Eating Related Variables
    public float health = 0f;
    public List<string> prey;
    public List<string> predators;
    public AnimationClip EatAnimationClip;
    private float eatAnimationLength;
    private float eatTimer = 0f;
    private Transform target;
    //Core Components
    private GameObject DETECTOR;
    private GameObject DUCK_GOD;
    private DuckState state;
    private Animator animator;

    public enum DuckState{
        Swim, Idle, Eat
    }

	// Use this for initialization
	void Start () {
        state = DuckState.Swim;
        animator = GetComponent<Animator>();
        animator.SetBool("Eating", false);
        eatAnimationLength = EatAnimationClip.length;
        facingRight = false;
        returningFromEat = false;
        originalY = transform.position.y;
        originalX = transform.position.x;
        hoverScale = transform.localScale.y * 0.3f;
        if (boundary_LRUD.Count < 4)
        {
            // Debug.Log("Boundary LRUD for DuckController is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<int>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }
	
    public void SetGod(GameObject god)
    {
        this.DUCK_GOD = god;
    }
    public void UpdateHealth(float health)
    {
        this.health = health;
    }
    public void Die()
    {
        if (DUCK_GOD != null)
            DUCK_GOD.GetComponent<DuckGod>().Kill(this.gameObject);
        else
            Destroy(this.gameObject);
    }

  

	// Update is called once per frame
	void Update () {
		if(state == DuckState.Swim)
        {
            animator.SetBool("Eating", false);
            Swim();
        }
        else if(state == DuckState.Eat && target != null)
        {
            animator.SetBool("Eating", true);
            Eat();
        }
	}

    void Swim()
    {
        moveTimer += Time.deltaTime;
        if (transform.position.x < boundary_LRUD[0] && !facingRight)
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (transform.position.x > boundary_LRUD[1] && facingRight)
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        int direction = -1;
        if (facingRight)
            direction = 1;

        //Move & Hover
        if (returningFromEat)
        {
            moveTimer = 0f;
            transform.position = new Vector2(transform.position.x + (swimSpeed * Time.deltaTime * direction), Mathf.Lerp(transform.position.y, originalY, Time.deltaTime));
            if (Magnitude(transform.position.y - originalY) < 0.05){
                returningFromEat = false;
            }
        }
        else {
            transform.position = new Vector2(transform.position.x + (swimSpeed * Time.deltaTime * direction),
                originalY + ((float)Math.Sin(moveTimer * 2) * hoverScale));
        }
    }

    void Eat()
    {
        eatTimer += Time.deltaTime;
        float period = 2 * Mathf.PI / eatAnimationLength;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.05f * Mathf.Sin(Mathf.PI + period * eatTimer));
        //Return from eating
        if(eatTimer > eatAnimationLength)
        {
            returningFromEat = true;
            eatTimer = 0f;
            state = DuckState.Swim;
            target = null;
        }
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetTarget(Transform targetTransform)
    {
        if (!returningFromEat)
        {
            state = DuckState.Eat;
            target = targetTransform;
        }
    }

    public DuckState GetState()
    {
        return state;
    }

    public float Magnitude(float n)
    {
        if (n < 0)
            return -n;
        else
            return n;
    }
}
