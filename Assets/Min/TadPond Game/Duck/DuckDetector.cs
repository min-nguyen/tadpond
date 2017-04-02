using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckDetector : MonoBehaviour
{

    private DuckController DUCK;
    private List<string> preyToDetect;

    // Use this for initialization
    void Start()
    {
        DUCK = transform.parent.gameObject.GetComponent<DuckController>();
        preyToDetect = DUCK.prey;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (preyToDetect.Contains(coll.tag) && DUCK.GetTarget() == null)
        {
            DUCK.SetTarget(coll.transform);
        }
    }
}