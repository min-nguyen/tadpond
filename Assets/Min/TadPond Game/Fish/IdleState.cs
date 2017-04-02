using UnityEngine;
using System;
using System.Collections;

public class IdleState : FishBehaviourInterface
{

    FishController controller;
    private float hoverScale = 0.5f;
    private float returnSpeed = 1f;
    private float timer;
    private float originalY;
    private bool returningToOriginalPosition;
    

    public IdleState(FishController controller)
    {
        this.controller = controller;
        this.timer = 0f;
        originalY = controller.transform.position.y;
    }

    public void updateState()
    {
        if (returningToOriginalPosition)
        {
            Vector3 destination = new Vector3(controller.transform.position.x, originalY, 0);
            controller.transform.position = Vector3.MoveTowards(controller.transform.position, destination, Time.deltaTime * returnSpeed);
            if(controller.transform.position.y == originalY)
                returningToOriginalPosition = false;
        }
        else
        {
            timer += Time.deltaTime;
            hover();
        }
        rotate();
    }

    //Resets timer used for hover for sin function
    public void onReturn()
    {
        returningToOriginalPosition = true;
        timer = 0f;
    }
    //Hovers fish
    public void hover()
    {
        controller.transform.position = new Vector3(controller.transform.position.x,
           originalY + ((float)Math.Sin(timer) * hoverScale),
           controller.transform.position.z);
    }
    //Rotate to horizontal
    public void rotate()
    {
        if (controller.transform.rotation.z != 0)
        {
            //Interpolate between current rotation and target rotation
            Vector3 destRotation = new Vector3(0, 0, 0);
            //Vector3 euler = Vector3.Lerp(controller.transform.eulerAngles, destRotation, 1f * Time.deltaTime);
            float _z = Mathf.LerpAngle(controller.transform.eulerAngles.z, 0, 1f * 1f * Time.deltaTime);
            //Apply rotation - Quaternion ensures minimum rotation direction
            controller.transform.rotation = Quaternion.Euler(0, 0, _z);
        }
    }
    public void onTrigger(Collider2D other)
    {

    }
    public void toIdleState()
    {

    }
    public void toMoveState()
    {

    }
    public void toChaseState()
    {

    }
}
