using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TadpoleEggController : MonoBehaviour, OrganismInterface {

    Vector3 originalPosition;
    public List<string> prey;
    public List<string> predators;
    private GameObject MAIN_TADPOLE_CONTROLLER;
    private float timer;
    public float health = 0f;
    private float vibrateScale;


	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        //Start timer at random value to ensure asynchronous vibrations with other tadpole eggs
        timer = UnityEngine.Random.Range(0f, 5f);
        vibrateScale = 0.1f;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (prey.Contains(col.gameObject.tag))
        {
            
        }
    }

    public void SetGod(GameObject main_tadpole)
    {
        SetMainTadpoleController(main_tadpole);
    }

    public void SetMainTadpoleController(GameObject MainTadpoleController)
    {
        MAIN_TADPOLE_CONTROLLER = MainTadpoleController;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        vibrate();
	}

    //Vibrates tadpole eggs
    public void vibrate()
    {
        transform.position = new 
            Vector3(originalPosition.x + (float)Math.Sin(timer/2) * vibrateScale,
           originalPosition.y + ((float)Math.Sin(timer) * vibrateScale),
           transform.position.z);
    }

    public void UpdateHealth(float health)
    {
        this.health = health;
    }

    public void Die()
    {
        if (MAIN_TADPOLE_CONTROLLER != null)
        {
            MAIN_TADPOLE_CONTROLLER.GetComponent<TadpoleController>().Destroy_Egg(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
}
