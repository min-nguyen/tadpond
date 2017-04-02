using UnityEngine;
using System.Collections.Generic;
using System;

public class AlgaeController_ : MonoBehaviour {
   
    public List<string> predators;
    private GameObject ALGAE_GOD;
    private Vector3 originalPosition;

    // Use this for initialization
    void Start () {
        originalPosition = transform.position;
    }

    public void SetGod(GameObject god)
    {
        ALGAE_GOD = god;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new
               Vector3(originalPosition.x + (float)Math.Sin(Time.deltaTime / 2) ,
              originalPosition.y + ((float)Math.Sin(Time.deltaTime) ),
              transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (predators.Contains(coll.tag))
        {
            if (ALGAE_GOD != null)
            {
                ALGAE_GOD.GetComponent<AlgaeGod_>().Destroy();
            }
            Destroy(this.gameObject);
        }
    }
}
