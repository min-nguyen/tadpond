using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Parallaxing : MonoBehaviour {

    private List<ParallaxObject> parallaxingObjects;
    public List<string> tags;           //Tags to recognise objects that should be parallaxed
    public float smoothing = 1f;
    private Hashtable objectNumbers;    
    private Transform cam;
    private Vector3 prevCamPosition;

    void Awake()
    {
        cam = Camera.main.transform;
    }

	void Start () {
        prevCamPosition = cam.position;
        tags = new List<string>();
        objectNumbers = new Hashtable();
        List<GameObject> pObjects = new List<GameObject>();
        //Add to this when further prefabs are created which need parallaxing
        tags.Add("FarCloud");
        tags.Add("CloseCloud");
        //For each Tag, find all objects and add to list pObjects
        for(int i = 0; i < tags.Count; i++)
        {
            GameObject[] tempParallaxObjects = GameObject.FindGameObjectsWithTag(tags[i]);
            objectNumbers.Add(tags[i], tempParallaxObjects.Length);
            pObjects.AddRange(tempParallaxObjects);
        }
        //Condense each Object into the custom type ParallaxObject 
        parallaxingObjects = new List<ParallaxObject>();
        for(int i = 0; i < pObjects.Count; i++)
        {
            ParallaxObject obj = new ParallaxObject(pObjects[i], pObjects[i].tag, pObjects[i].transform.position.z * -1);
            parallaxingObjects.Add(obj);
        }
	}
	
	// Update is called once per frame
	void Update () {
//      Debug.Log(parallaxingObjects.Count);
        //Check if Parallaxed Objects have been Destroyed
        for (int i = 0; i < parallaxingObjects.Count; i++)
        {
            ParallaxObject pObj = parallaxingObjects[i];
            if (pObj.gameObject == null)
            {
                string tag = pObj.tag;
                objectNumbers[tag] = (int) objectNumbers[tag] - 1;
                parallaxingObjects.RemoveAt(i);
            }
        }
        //Check if Parallaxed Objects have been Introduced
        for(int i = 0; i < tags.Count; i++)
        {
            GameObject[] go = GameObject.FindGameObjectsWithTag(tags[i]);
            if ((int) objectNumbers[tags[i]] < go.Length)       // New object of relevant tag detected
            {
                //Identify game object that Parallaxing does not yet contain
                for(int j = 0; j < go.Length; j++)
                {
                    GameObject obj = go[j];
                    bool found = false;
                    for(int k = 0; k < parallaxingObjects.Count; k++)
                    {
                        if (parallaxingObjects[k].gameObject == obj)
                            found = true;
                    }
                    if (!found)
                    {
                        ParallaxObject pObj = new ParallaxObject(obj, tags[i], obj.transform.position.z * -1);
                        parallaxingObjects.Add(pObj);
                    }
                }
            }
        }
        //Update Parallaxing
        for (int i = 0; i < parallaxingObjects.Count; i++)
        {
            ParallaxObject pObj = parallaxingObjects[i];
            float parallax_x = (prevCamPosition.x - cam.position.x) * pObj.parallaxScale;
            float parallax_y = (prevCamPosition.y - cam.position.y) * pObj.parallaxScale;
            float backgroundtargetposx = pObj.gameObject.transform.position.x + parallax_x;
            float backgroundtargetposy = pObj.gameObject.transform.position.y + parallax_y;
            Vector3 backgroundtargetpos = new Vector3(backgroundtargetposx, backgroundtargetposy, pObj.gameObject.transform.position.z);
            pObj.gameObject.transform.position = Vector3.Lerp(pObj.gameObject.transform.position, backgroundtargetpos, smoothing * Time.deltaTime);
        }

        prevCamPosition = cam.position;

    }
}

class ParallaxObject
{
    public GameObject gameObject;
    public string tag;
    public float parallaxScale;
    public ParallaxObject(GameObject gameObject, string tag, float parallaxScale)
    {
        this.gameObject = gameObject;
        this.tag = tag;
        this.parallaxScale = parallaxScale;
    }
}
// ctrl kc, ctrl ku