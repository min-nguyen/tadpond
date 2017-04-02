using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishDetectorCircleScript : MonoBehaviour {

    private GameObject Fish;
    private FishController FishStateController;
    private Transform target;
    private List<string> preyToDetect;

	void Start () {
        Fish =  transform.parent.gameObject;
        FishStateController = transform.parent.gameObject.GetComponent<FishController>();
        target = FishStateController.getTarget();
        preyToDetect = FishStateController.prey;

    }
	
	void Update () {

    }

    void setTarget(Transform trans)
    {
        FishStateController.setTarget(trans);
        FishStateController.setToChaseState();
    }

    public void IncreaseSize(float growthFactor)
    {
        transform.localScale = new Vector3(transform.localScale.x + growthFactor, transform.localScale.y + growthFactor, 0);
    }

    void clearTarget()
    {
        FishStateController.setTarget(null);
    }

    //Sets a chase target only if health < 4 and not in idle state (hovering)
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (preyToDetect.Contains(coll.tag) && FishStateController.currentState.ToString() != "IdleState" /*&& FishStateController.GetHealth() < 4*/)
        {
            FishStateController.setTarget(coll.transform);
            FishStateController.setToChaseState();
        }
    }

}
