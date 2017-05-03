using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgaeController : MonoBehaviour {

    private ParticleSystem ParticleSys;
    private Transform Trans;
    //Health 0-10. 
    private float Health = 0;
    //Particle Emission Rate = Health * 50
    private int EmissionScale = 50;

    void Start () {
        ParticleSys = GetComponent<ParticleSystem>();
        Trans = GetComponent<Transform>();
        ParticleSys.emissionRate = 0;
    }

	void Update () {

	}

    public void IncreaseHealth(){
        if (Health < 20){
            Health++;
            ParticleSys.emissionRate = Health * EmissionScale;
        }
    }

    public void DecreaseHealth()
    {
        if(Health > 0)
        {
            Health--;
            ParticleSys.emissionRate = Health * EmissionScale;
        }
    }

    public void SetHealth(float h)
    {
        if (h >= 0 && h <= 20) { 
            Health = h;
            ParticleSys.emissionRate = Health * EmissionScale;
        }
    }
}
