using UnityEngine;
using System.Collections;

public interface FishBehaviourInterface{

    void updateState();
    void onTrigger(Collider2D other);
    void toIdleState();
    void toMoveState();
    void toChaseState();
    void onReturn();
}
