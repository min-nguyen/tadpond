using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tadpole_Age2_CircleDetector : MonoBehaviour {

    private GameObject tadpoleMid;
    private Tadpole_Age2_Controller tadpoleMidScript;
    private List<string> food;
    private float visionRangeAngle = 30;

	// Use this for initialization
	void Start () {
        tadpoleMid = transform.parent.gameObject;
        tadpoleMidScript = tadpoleMid.GetComponent<Tadpole_Age2_Controller>();
        food = tadpoleMidScript.prey;
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {

        Vector3 facingDirection = tadpoleMid.transform.right;

        Vector3 colliderPosition = coll.transform.position;
        Vector3 tadpoleMidPosition = tadpoleMid.transform.position;

        Vector3 betweenVector = colliderPosition - tadpoleMidPosition;

        //Angle between tadpole facing direction (up y axis neutrally) and collider
        float angleBetween = Vector3.Angle(facingDirection, betweenVector);


        if (angleBetween < visionRangeAngle)
        {
            if (food.Contains(coll.tag))
                tadpoleMidScript.DetectFood(coll.gameObject);
        }
        
    }

    public float customConvertAngle(float angle)
    {
        angle += 90;
        if (angle > 360)
            return mod(angle, 360);
        else
            return angle;
    }

    public float mod(float x, float m)
    {
        return (x % m + m) % m;
    }
}
