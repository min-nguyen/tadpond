using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OrganismGodInterface {

    void Spawn(int num, Vector3 position);  //Spawns 'num' amount of objects at 'position'
    void Spawn(int num);
    void UpdateEnvironmentalValues( float nutrients, 
                                    float sunlight, 
                                    float rain, 
                                    float watertemp, 
                                    float airtemp, 
                                    float pH, 
                                    float oxygen, 
                                    float algaeHealth);
    void SetBoundaryLRUD(List<int> LRUD);
    void Kill(GameObject organism);
}

