using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tadpole_Age3_Controller : MonoBehaviour, OrganismInterface {

    private GameObject MAIN_TADPOLE_CONTROLLER;
    private TadpoleController MAIN_TADPOLE_CONTROLLER_SCRIPT;
    //MOVEMENT RELATED VARIABLES
    public List<int> boundary_LRUD;
    private float timer = 0.25f;
    private float swimAnimationLength = 1f;
    private float swimSpeed = 0.02f;
    private float swimFrequency = 1f;
    private Quaternion targetQuaternion;
    private bool chasing = false;
    Animator animator;
    //HEALTH RELATED VARIABLES
    private float health = 0;
    public List<string> prey;
    public List<string> predators;
    

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == "Swim") {
                swimAnimationLength = ac.animationClips[i].length/animator.speed;
            }
        }
        swimFrequency = 1 / swimAnimationLength;
        float random = Random.Range(0, 360);
        targetQuaternion = Quaternion.Euler(transform.rotation.x, transform.rotation.y, random);
        if (boundary_LRUD.Count < 4)
        {
          //  Debug.Log("Boundary LRUD for TadpoleAge3 is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<int>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
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

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(health >= 5)
        {
            EscapeFromPond();
        }
        else if (!chasing) {
            animator.speed = 0.5f;
            swimFrequency = animator.speed * (1 / swimAnimationLength);
            Swim();
            RotateRandom();
        }
        else
        {
            animator.speed = 1.0f;
            swimFrequency = animator.speed * (1 / swimAnimationLength);
            Chase();
        }
    }

    void RotateRandom()
    {
       
        if (timer > 3)
        {
            timer = 0;
            float destAngle = Random.Range(0, 360);
            
            //Apply movement boundaries
            if (transform.position.x > boundary_LRUD[1])
            {
                destAngle = 180;
                if (transform.position.y > boundary_LRUD[2])
                    destAngle = 225;
                else if (transform.position.y < boundary_LRUD[3])
                    destAngle = 135;
            }
            else if (transform.position.x < boundary_LRUD[0])
            {
                destAngle = 0;
                if (transform.position.y > boundary_LRUD[2])
                    destAngle = 315;
                else if (transform.position.y < boundary_LRUD[3])
                    destAngle = 45;
            }
            else if (transform.position.y > boundary_LRUD[2])
                destAngle = 270;
            else if (transform.position.y < boundary_LRUD[3])
                destAngle = 90;
            
            targetQuaternion = Quaternion.Euler(transform.rotation.x, transform.rotation.y, destAngle);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime);
    }

    void Swim()
    {
        Vector2 facing = transform.right;
        float scale = (float)(Magnitude(Mathf.Sin(ModPI(timer) * (Mathf.PI * swimFrequency))) * swimSpeed);
        //Also swims at constant factor as well as the Sin scale
        transform.position = new Vector3(transform.position.x + facing.x * scale + facing.x * 0.005f, transform.position.y + facing.y * scale + facing.y * 0.005f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (prey.Contains(col.gameObject.tag))
        {
            Eat(col);
        }
        else if (predators.Contains(col.gameObject.tag))
        {
            Die();
        }
    }

    void Eat(Collider2D food)
    {
        Destroy(food.gameObject);
        health++;
    }

    void EscapeFromPond()
    {
        Debug.Log("escape");
        targetQuaternion = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime / 2);
        Swim();
    }

    void Chase()
    {
        Vector2 facing = transform.right;
        float scale = (float)(Magnitude(Mathf.Sin(ModPI(timer) * (Mathf.PI * swimFrequency))) * swimSpeed);
        //Also swims at constant factor as well as the Sin scale
        transform.position = new Vector3(transform.position.x + facing.x * scale + facing.x * 0.01f, transform.position.y + facing.y * scale + facing.y * 0.01f);

    }

    public void Die()
    {
        if (MAIN_TADPOLE_CONTROLLER != null)
        {
            MAIN_TADPOLE_CONTROLLER.GetComponent<TadpoleController>().Destroy_Age3(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    public void UpdateHealth(float health)
    {
        this.health = health;
    }
    public float mod(float x, float m)
    {
        return (x % m + m) % m;
    }

    float ModPI(float timer)
    {
            return (timer % Mathf.PI + Mathf.PI) % Mathf.PI;
    }

    float Magnitude(float val)
    {
        if (val < 0)
            return (-val);
        else
            return val;
    }
}
