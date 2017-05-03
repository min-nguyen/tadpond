using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckDetector : MonoBehaviour
{

    private DuckController DUCK;
    private List<string> preyToDetect;
    private float timer = 0f;
    private OrganismInterface destroytarget;
    // Use this for initialization
    void Start()
    {
        DUCK = transform.parent.gameObject.GetComponent<DuckController>();
        preyToDetect = DUCK.prey;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f && destroytarget != null)
        {
            DUCK.SetTarget(null);
            destroytarget.Die();
            destroytarget = null;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (preyToDetect.Contains(coll.tag) && DUCK.GetTarget() == null)
        {
            DUCK.SetTarget(coll.transform);
            if (DUCK.GetComponent<DuckController>().IsEating()) 
            {
                timer = 0f;
                destroytarget = coll.gameObject.GetComponent<OrganismInterface>();
            }
        }
    }
}