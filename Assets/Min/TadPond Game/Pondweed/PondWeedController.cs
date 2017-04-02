using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondWeedController : MonoBehaviour, OrganismInterface {

    public int health = 1; //Health MUST be incremented or decremented by exactly 1. Health must stay in range 0-4.
    private Animator animator;
    private GameObject PONDWEED_GOD;
    private PondWeedGod PONDWEED_GOD_SCRIPT;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        health = 1;
        animator.SetInteger("health", health);
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void SetGod(GameObject god)
    {
        PONDWEED_GOD = god;
        PONDWEED_GOD_SCRIPT = god.GetComponent<PondWeedGod>();
    }

    //ONLY INCREMENT/DECREMENT 1 AT A TIME: Range is from 0-4
    //PASS -1 FOR DECREMENT, PASS +1 FOR INCREMENT
    public void UpdateHealth(float health)
    {
        if (health > 0 && this.health < 4)
        {
            this.health++;
            animator.SetInteger("health", this.health);

        }
        else if (health < 0 && this.health > 0)
        {
            this.health--;
            animator.SetInteger("health", this.health);
            Debug.Log("dec'd health" + this.health);
        }
    }
    public int GetHealth()
    {
        return health;
    }
    public void Die()
    {
        if (PONDWEED_GOD != null)
            PONDWEED_GOD.GetComponent<PondWeedGod>().Kill(this.gameObject);
        else
            Destroy(this.gameObject);
    }
    void EnableManualTesting()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateHealth(1);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            UpdateHealth(-1);
        }
    }
}
