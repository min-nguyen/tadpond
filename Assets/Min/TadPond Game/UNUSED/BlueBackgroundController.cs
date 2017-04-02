using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueBackgroundController : MonoBehaviour {

    Renderer rend;

    //Bubbles
    public GameObject bubblePrefab;
    private List<GameObject> bubbles;
    public float minSize;
    public float maxSize;
    public float minVelocity;
    public float maxVelocity;
    public float randomness;
    ////////////////////////////

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        bubbles = new List<GameObject>();
        minSize = 1f;
        maxSize = 2f;
        minVelocity = 3f;
        maxVelocity = 6f;
	}
	
	// Update is called once per frame
	void Update () {
       // EnableManualControl();
        HandleBubbleDeletion();
	}

    //Spawn single bubble at random x location
    void SpawnBubble()
    {
        bubblePrefab.transform.localScale = Vector3.one * Random.Range(minSize, maxSize); //Set a random size between minSize and maxSize.

        Vector3 spawnPosition = new Vector3(
                Random.Range(transform.position.x - rend.bounds.size.x/2, 
                             transform.position.x + rend.bounds.size.x/2),
                transform.position.y - rend.bounds.size.y / 2); 
        Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f); //Set a random rotation.
        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, spawnRotation) as GameObject;
        bubble.GetComponent<BubbleController>().velocity = Random.Range(minVelocity, maxVelocity);
        bubbles.Add(bubble);
    }

    //Destroy bubble if out of background range
    void HandleBubbleDeletion()
    {
        for(int i = 0; i < bubbles.Count; i++)
        {
            GameObject bubble = bubbles[i];
            if(bubble.transform.position.y > transform.position.y + rend.bounds.size.y / 2)
            {
                Destroy(bubble);
                bubbles.Remove(bubble);
            }
        }
    }

    //Change brightness - true to increase, false to decrease
    void ChangeBrightness(bool brightnessUp)
    {
        if (brightnessUp && rend.material.color.a < 2)
        {
            Color color = rend.material.color;
            color.a += 0.2f;
            rend.material.color = color;
        }
        if (!(brightnessUp) && rend.material.color.a > 0.2)
        {
            Color color = rend.material.color;
            color.a -= 0.2f;
            rend.material.color = color;
        }
    }

    //Enable keyboard input testing
    void EnableManualControl()
    {
        //Change brightness
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeBrightness(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeBrightness(false);
        }
        //Spawn bubble
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnBubble();
        }
    }
}
