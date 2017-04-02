using UnityEngine;
using System.Collections;

public class CloseCloudController : MonoBehaviour
{

    public Sprite[] cloud_sprites;
    private float velocity;
    // Use this for initialization
    void Start()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.sprite = cloud_sprites[(Random.Range(0, cloud_sprites.Length))];
        velocity = Random.Range(0.6f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + Time.deltaTime * velocity, transform.position.y, transform.position.z);
    }
}
