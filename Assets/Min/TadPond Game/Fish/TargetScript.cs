using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {

    Transform trans;
	// Use this for initialization
	void Start () {
        trans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        trans.Translate(new Vector3(0.02f, 0, 0));
	}
}
