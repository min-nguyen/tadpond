using UnityEngine;
using System.Collections;

public class ChaseState : FishBehaviourInterface
{

    FishController FishStateController;
    private float chaseSpeed;
    public ChaseState(FishController controller)
    {
        this.FishStateController = controller;
        this.chaseSpeed = 0.03f;
    }
    public void updateState()
    {
        if (FishStateController.getTarget() == null)
        {
            toIdleState();
        }
        else
        {
            swim();
            rotate();
        }
    }
    public void onReturn()
    {

    }
    //Translates fish relative to local axis (direction faced)
    public void swim()
    {
        if (FishStateController.getFacingRight())
            FishStateController.transform.Translate(chaseSpeed, 0, 0);
        else
            FishStateController.transform.Translate(-chaseSpeed, 0, 0);
    }
    //Rotates towards target
    public void rotate()
    {
        Vector3 direction = (FishStateController.getTarget().position - FishStateController.transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(direction);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //If target is on LHS
        if ( angle < -90 || angle > 90)
        {
            if (FishStateController.getFacingRight())
            {
                FishStateController.setFacingLeft();
            }
            FishStateController.transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
        }
        //If target is on RHS
        else 
        {
            if (!FishStateController.getFacingRight())
            {
                FishStateController.setFacingRight();
            }
            FishStateController.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    public void switchDirection()
    {

    }
    public void onTrigger(Collider2D other)
    {

    }
    public void toIdleState()
    {
        FishStateController.setToIdleState();
    }
    public void toMoveState()
    {
        FishStateController.setToSwimState();
    }
    public void toChaseState()
    {
        
    }
}
