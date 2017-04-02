using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface OrganismInterface
{
    void SetGod(GameObject God);
    void UpdateHealth(float h);
    void Die();
}
