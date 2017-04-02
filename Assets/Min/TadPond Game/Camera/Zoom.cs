using UnityEngine;
using System.Collections;

public class Zoom : MonoBehaviour
{

    public float zoomSpeed = 1;
    public float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 20.0f;
    public float scroll;

    void Start()
    {
        targetOrtho = Camera.main.orthographicSize;
    }

    void LateUpdate()
    {

        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            scroll = Input.GetAxis("Mouse ScrollWheel");
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            scroll = 0.5f;
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            scroll = -0.5f;
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
        }
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }
}
