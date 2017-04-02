using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkyController : MonoBehaviour {
    Renderer rend;
    public List<GameObject> clouds; //Not used at the moment
    public GameObject CLOSE_CLOUD;
    public GameObject FAR_CLOUD;
    float timer;
    float cloudSpawnInterval;
    //Sky should cont
    void Start()
    {
        rend = GetComponent<Renderer>();
        clouds = new List<GameObject>();
        timer = 0f;
        cloudSpawnInterval = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        EnableManualControl();

        timer += Time.deltaTime;
        if(timer >= cloudSpawnInterval)
        {
            int x = (int) Random.Range(0, 10);
            if (x % 2 == 0)
            {
                SpawnCloseCloud();
            }
            else if (x % 2 == 1)
            {
                SpawnFarCloud();
            }
            timer = 0;
        }
    }

    void SpawnFarCloud()
    {
        float skyHeight = rend.bounds.size.y;
        float skyWidth = rend.bounds.size.x;
        float cloudWidth = FAR_CLOUD.GetComponent<Renderer>().bounds.size.x;

        Vector3 position = new Vector3(transform.position.x - (skyWidth/2 + cloudWidth/2), 
                                            transform.position.y + Random.Range(0, skyHeight/2), 30);
        Quaternion spawnRotation = Quaternion.Euler(0,0,0);
        GameObject gobj = Instantiate(FAR_CLOUD, position, spawnRotation) as GameObject;
        clouds.Add(gobj);
    }

    void SpawnCloseCloud()
    {
        float skyHeight = rend.bounds.size.y;
        float skyWidth = rend.bounds.size.x;
        float cloudWidth = CLOSE_CLOUD.GetComponent<Renderer>().bounds.size.x;

        Vector3 position = new Vector3(transform.position.x - (skyWidth / 2 + cloudWidth / 2),
                                            transform.position.y - Random.Range(0, skyHeight / 2), 30);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
        GameObject gobj = Instantiate(CLOSE_CLOUD, position, spawnRotation) as GameObject;
        clouds.Add(gobj);
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

    void EnableManualControl()
    {
        //Change brightness
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeBrightness(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeBrightness(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnFarCloud();

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnCloseCloud();
        }
    }
}