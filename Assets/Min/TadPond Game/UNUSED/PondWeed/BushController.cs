using UnityEngine;
using System.Collections;

public class BushController : MonoBehaviour {

    SpriteRenderer rend;
    Transform trans;
    Vector3 originalPosition;
    Vector2 originalSize;
    float maxHeight = 1;
    float growthRate = 0.005f;
    bool growing = true;
    bool beingEaten;
    //Grow, Be Eaten, Life


    bool x = false;

    // Use this for initialization
    void Start () {
        rend = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        originalPosition = trans.position;
        originalSize = new Vector2(rend.bounds.size.x, rend.bounds.size.y);
        trans.localScale = new Vector3(0.1f, 0.1f);
        
    }
	
	// Update is called once per frame
	void Update () {

        
        
    }

    void growshinkexperiment()
    {
        if (!x)
        {
            grow();
            fadeIn();
            if (trans.localScale.x > 1f)
                x = true;
            Debug.Log(trans.localScale.x);
        }
        else
        {
            Debug.Log("SHRINK");
            shrink();
            fadeOut();
            if (trans.localScale.x < 0.1f)
                x = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLIDED");
    }

    void grow()
    {
        trans.localScale += new Vector3(growthRate, growthRate, 0);
        float y = rend.bounds.size.y / 2 + originalPosition.y - originalSize.y / 2;
        trans.position = new Vector3(originalPosition.x, y, 0);
    }
    void fadeIn()
    {
        Color color = rend.material.color;
        if (color.a < 1.5) { 
            color.a += 0.4f * Time.deltaTime;
            rend.material.color = color;
        }
    }

    void shrink()
    {
        trans.localScale -= new Vector3(growthRate, growthRate, 0);
        float y = (rend.bounds.size.y / 2 + originalPosition.y - originalSize.y / 2);
        trans.position = new Vector3(originalPosition.x, y, 0);
    }
    void fadeOut()
    {
        Color color = rend.material.color;
        if (color.a > 0) { 
            color.a -= 0.4f * Time.deltaTime;
            rend.material.color = color;
        }
    }

    void enableManualControl()
    {
        while (Input.GetKeyDown(KeyCode.DownArrow))
        { 
            shrink();
            fadeIn();
        }
        while (Input.GetKeyDown(KeyCode.DownArrow))
        {
            grow();
            fadeOut();
        }
    }
}
