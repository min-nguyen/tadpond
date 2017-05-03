using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tadpole_Age2_Controller : MonoBehaviour, OrganismInterface {

    private GameObject MAIN_TADPOLE_CONTROLLER;
    private TadpoleController MAIN_TADPOLE_CONTROLLER_SCRIPT;
    //MOVEMENT RELATED VARIABLES
    public List<float> boundary_LRUD;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private GameObject target;
    private float destAngle = 0f;
    private float timer = 0f;
    private float swimSpeed = 1f;
    private float rotateSpeed;
    private float angle;
    private Vector3 previousThrustDirection;
    //HEALTH RELATED VARIABLES
    public List<string> prey;
    public List<string> predators;
    public float health = 0;
    private float healthLIMIT = 50f;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rotateSpeed = Random.Range(0, 5f);
        animator = GetComponent<Animator>();
        previousThrustDirection = transform.up;
        if (boundary_LRUD.Count < 4)
        {
          //  Debug.Log("Boundary LRUD for TadpoleAge2 is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<float>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        AutomaticBehaviour();
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

    //unused
    void DetectPredator()
    {
        float visionAngle = 20;
        float maxDistance = 2;
        float x = transform.position.x;
        float y = transform.position.y;
        float angle = customConvertAngle(transform.eulerAngles.z);

        Debug.Log("Facing : " + angle + "with current x : " + x);

        float vertexL_x = x + maxDistance * Mathf.Cos(Mathf.Deg2Rad * (angle + visionAngle));
        float vertexL_y = y + maxDistance * Mathf.Sin(Mathf.Deg2Rad * (angle + visionAngle));
        Debug.Log("L x is " + vertexL_x + " y is " + vertexL_y);

        float vertexR_x = x + maxDistance * Mathf.Cos(Mathf.Deg2Rad * (angle - visionAngle));
        float vertexR_y = y + maxDistance * Mathf.Sin(Mathf.Deg2Rad * (angle - visionAngle));
        Debug.Log("R x is " + vertexR_x + " y is " + vertexR_y);

        Vector3 enemy = new Vector3(0, 0, 0);
    }

    //Sets target
    public void DetectFood(GameObject food)
    {
        Debug.Log("FOOD");
        target = food;
    }

    public void UpdateHealth(float health)
    {
        this.health = health;
    }

    void ChaseTarget()
    {

        float speed = rigidBody.velocity.magnitude;
        if (speed < 0.5f)
        {
            rigidBody.AddForce(transform.right * 40);
        }
        if (speed > 1.0f)
            animator.SetBool("Swimming", true);
        else
            animator.SetBool("Swimming", false);

        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2);
        
    }

    void AutomaticBehaviour()
    {
        if (target != null)
        {
            ChaseTarget();
        }
        else {
            ThrustBehaviour();
            RotateBehaviour();
        }
    }

    //Thrusts forward periodically iff speed is below certain value
    void ThrustBehaviour()
    {
        float speed = rigidBody.velocity.magnitude;
        if (speed < 0.3f)
        {
            rigidBody.AddForce(transform.right * 100);
            previousThrustDirection = transform.up;
        }
        if (speed > 1.0f)
            animator.SetBool("Swimming", true);
        else
            animator.SetBool("Swimming", false);
    }


    //Rotates towards point. 
    void RotateBehaviour()
    {
        float speed = rigidBody.velocity.magnitude;
        if (speed > 0.3f)
        {
            float angle = 0f;
            angle = mod(transform.rotation.eulerAngles.z, 360);
            //Updates destination angle when within 3 degrees.
            if (transform.rotation.eulerAngles.z < destAngle + 3 && transform.rotation.eulerAngles.z > destAngle - 3)
                destAngle = mod(Random.Range((transform.eulerAngles.z - 90), (transform.eulerAngles.z + 90)), 360);
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
            //If not reached scope of destAngle, continue rotating
            if (transform.eulerAngles.z != destAngle)
            {
                float _z = Mathf.LerpAngle(transform.eulerAngles.z, destAngle, Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, _z);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (prey.Contains(col.gameObject.tag))
        {
            Eat(col);
        }
    }

    void Eat(Collider2D food)
    {
        OrganismInterface oi = food.gameObject.GetComponent<OrganismInterface>();
        oi.Die();
        health += 10;
        if (health >= healthLIMIT && MAIN_TADPOLE_CONTROLLER != null)
            MAIN_TADPOLE_CONTROLLER_SCRIPT.HatchTadpoleAge3();
    }

    public void Die()
    {
        if (MAIN_TADPOLE_CONTROLLER != null)
        {
            MAIN_TADPOLE_CONTROLLER.GetComponent<TadpoleController>().Destroy_Age2(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public float mod(float x, float m)
    {
        return (x % m + m) % m;
    }

    //Converts angle as if sprite neutrally faces right
    public float customConvertAngle(float angle)
    {
        angle += 90;
        if (angle > 360)
            return mod(angle, 360);
        else
            return angle;
    }


}
