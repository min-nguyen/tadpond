using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfleaController : MonoBehaviour, OrganismInterface {

    private GameObject WATERFLEA_GOD;
    //MOVEMENT RELATED VARIABLES
    public List<int> boundary_LRUD;
    private Vector2 size;
    private Vector2 target;
    private bool falling = true;
    private float fallSpeed = 1.75f;
    //HEALTH RELATED VARIABLES
    public List<string> prey;
    public List<string> predators;
    private float health = 3;
    
    
    // Use this for initialization
    void Start () {
        size = GetComponent<SpriteRenderer>().bounds.size;
        target = transform.position;
        if (boundary_LRUD.Count < 4)
        {
           // Debug.Log("Boundary LRUD for WaterFleaController is not initialised in inspector with 4 values - creating default boundaries");
            boundary_LRUD = new List<int>();
            boundary_LRUD.Insert(0, -10);
            boundary_LRUD.Insert(1, 10);
            boundary_LRUD.Insert(2, 10);
            boundary_LRUD.Insert(3, -10);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    public void SetGod(GameObject god)
    {
        WATERFLEA_GOD = god;
    }

    public void UpdateHealth(float health)
    {
        this.health = health;
    }
    public void Die()
    {
        if (WATERFLEA_GOD != null)
        {
            WATERFLEA_GOD.GetComponent<WaterfleaGod>().Kill(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    void Move()
    {
        if ((transform.position.y > boundary_LRUD[2] && !falling) || ((transform.position.y > (target.y - size.y / 2)) && !falling))
        {
            falling = true;
            target = new Vector2(transform.position.x, transform.position.y - size.y * 3);
        }
        else if ((transform.position.y < boundary_LRUD[3] && falling) || ((transform.position.y < (target.y + size.y / 2)) && falling))
        {
            falling = false;
            int direction = (Random.Range(0f, 1f) < 0.5f) ? -1 : 1;
            float newX = size.x * direction;
            if ((transform.position.x < boundary_LRUD[0] && direction < 0) || (transform.position.x > boundary_LRUD[1] && direction > 0))
                newX = -newX * 2;

            target = new Vector2(transform.position.x + newX, transform.position.y + size.y * 3);
        }
        if (!falling)
        {
            float yposition = Mathf.Lerp(transform.position.y, target.y, Time.deltaTime * 5f);
            float xposition = Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * 5f);
            transform.position = new Vector2(xposition, yposition);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - (size.y * Time.deltaTime * fallSpeed));
        }
    }
}
