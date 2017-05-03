using UnityEngine;
using System.Collections;

public class CameraControlKeyboard : MonoBehaviour {

    public GameObject Environment;
    private GameObject Water;
    public int topBound;
	public int bottomBound;
	public int rightBound;
	public int leftBound;
	public float speed;
	public float zoomSpeed = 1.5f;
	public float targetOrtho;
	public float smoothSpeed = 2.0f;
	public float minOrtho = 1.0f;
	public float maxOrtho = 10.0f;
	public float scroll;

	private bool top, bottom, right, left = false;

	void Start() {
		targetOrtho = Camera.main.orthographicSize;
       
        if (Environment != null)
        {
            for (int i = 0; i < Environment.transform.childCount; i++)
            {
                if (Environment.transform.GetChild(i).tag == "Water")
                {
                    Water = Environment.transform.GetChild(i).gameObject;
                    break;
                }
            }
            //Set Boundary_LRUD to Water Size Dimensions manually - Cannot use GetBoundaryLRUD() function in OnEnable().
            leftBound   =  (int)(Water.transform.position.x - (Water.GetComponent<SpriteRenderer>().bounds.size.x / 2));
            rightBound  =  (int)(Water.transform.position.x + (Water.GetComponent<SpriteRenderer>().bounds.size.x / 2));
            topBound    =  (int)(Water.transform.position.y + (Water.GetComponent<SpriteRenderer>().bounds.size.y / 2));
            bottomBound =  (int)(Water.transform.position.y - (Water.GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

	void Update()
	{
		if(Input.GetKey(KeyCode.RightArrow) && !right)
		{
			transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
			left = false;
		}
		if(Input.GetKey(KeyCode.LeftArrow) && !left)
		{
			transform.Translate(new Vector3(-speed * Time.deltaTime,0,0));
			right = false;
		}
		if(Input.GetKey(KeyCode.DownArrow) && !bottom)
		{
			transform.Translate(new Vector3(0,-speed * Time.deltaTime,0));
			top = false;
		}
		if(Input.GetKey(KeyCode.UpArrow) && !top)
		{
			transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
			bottom = false;
		}
		PositionCheck ();
	}

	void LateUpdate () {
		//zoom in
		if (Input.GetAxis ("Mouse ScrollWheel") > 0.0f) {
			scroll = Input.GetAxis ("Mouse ScrollWheel");
			targetOrtho -= scroll * zoomSpeed;
			targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
			top = false;
			bottom = false;
			right = false;
			left = false;
		}
		//zoom out
		if (Input.GetAxis ("Mouse ScrollWheel") < 0.0f && !top && !bottom && !right && !left) {
			scroll = Input.GetAxis ("Mouse ScrollWheel");
			targetOrtho -= scroll * zoomSpeed;
			targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
		}
		if (Input.GetKeyDown(KeyCode.Equals)) {
			scroll = 0.5f;
			targetOrtho -= scroll * zoomSpeed;
			targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
			Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.Minus) && !top && !bottom && !right && !left) {
			scroll = -0.5f;
			targetOrtho -= scroll * zoomSpeed;
			targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
			Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
		}
		Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
	}

	void PositionCheck() {
		float h = Camera.main.orthographicSize;
		float w = Camera.main.orthographicSize * Camera.main.aspect;
		if (transform.position.y + (h/2) >= 19) {
			top = true;
		}
		if (transform.position.y - (h/2) <= -14) {
			bottom = true;
		}
		if (transform.position.x + (w/2) >= 32) {
			right = true;
		}
		if (transform.position.x - (w/2) <= -30) {
			left = true;
		}
	}
}



//top: transform.position.y + h/2 > backgroundHeight/2
//bottom: transform.position.y - h/2 < backgroundHeight/2
//right: transform.position.x + w/2 > backgroundWidth/2
//left: transform.position.x - w/2 < backgroundWidth/2