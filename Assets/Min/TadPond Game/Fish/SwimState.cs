using UnityEngine;
using System.Collections;

public class SwimState : FishBehaviourInterface
{

    FishController FishStateController;
    private float swimSpeed = 0.03f;

    public SwimState(FishController controller)
    {
        this.FishStateController = controller;
    }

    public void updateState() {
        if (FishStateController.getFacingRight())
            FishStateController.transform.Translate(swimSpeed, 0, 0);
        else
            FishStateController.transform.Translate(-swimSpeed, 0, 0);
        rotate();
    }
    //Rotate to horizontal
    public void rotate()
    {
        if (FishStateController.transform.rotation.z != 0)
        {
            //Interpolate between current rotation and target rotation
            Vector3 destRotation = new Vector3(0, 0, 0);
            //Vector3 euler = Vector3.Lerp(controller.transform.eulerAngles, destRotation, 1f * Time.deltaTime);
            float _z = Mathf.LerpAngle(FishStateController.transform.eulerAngles.z, 0, 1f * 1f * Time.deltaTime);
            //Apply rotation - Quaternion ensures minimum rotation direction
            FishStateController.transform.rotation = Quaternion.Euler(0, 0, _z);
        }
    }

    public void onReturn()
    {

    }

    public void onTrigger(Collider2D other) { }
    
    public void toIdleState() { }
    public void toMoveState() { }
    public void toChaseState() { }
   
}
