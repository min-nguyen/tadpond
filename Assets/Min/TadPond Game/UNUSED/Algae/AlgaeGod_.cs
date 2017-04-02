using UnityEngine;
using System.Collections;

public class AlgaeGod_ : MonoBehaviour {

    public GameObject ALGAE;
    private float SPAWN_RATE = 2f;
    private float POPULATION = 0;
    private float timer = 0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > SPAWN_RATE)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(0f, 5f), Random.Range(0f, 2f), 0);
            Spawn(1, spawnPosition);
            timer = 0f;
        }
    }

    public void Spawn(int num, Vector3 position)
    {
        for (int i = 0; i < num; i++)
        {
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            GameObject algae = Instantiate(ALGAE, position, spawnRotation) as GameObject;
            AlgaeController_ c = algae.GetComponent<AlgaeController_>();
            c.SetGod(this.gameObject);
            ++POPULATION;
        }
    }

    public void Destroy()
    {
        --POPULATION;
    }
}
