using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

	public static GameObject instance;
	GameObject musicToggle, volumeSlider;

	void Start() {
		musicToggle = GameObject.Find ("Music Toggle");
		volumeSlider = GameObject.Find("Volume Slider");
		AudioListener.pause = false;
		AudioListener.volume = 0.5f;
		//createEntity (playerPath, new Vector3(0,1.6f,0), Quaternion.identity);
	}

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = gameObject;
		} else {
			Destroy (gameObject);
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.M)) {
			if (AudioListener.pause) {
				AudioListener.pause = false;
			} else {
				AudioListener.pause = true;
			}
		}
		UIUpdate ();
	}

	void UIUpdate () {
		musicToggle = GameObject.FindWithTag("MusicToggle");
		volumeSlider = GameObject.FindWithTag("VolumeSlider");
		if (musicToggle != null && volumeSlider != null) {
            AudioListener.pause = musicToggle.GetComponent<UnityEngine.UI.Toggle> ().isOn;
			AudioListener.volume = volumeSlider.GetComponent<UnityEngine.UI.Slider>().value;
		}
	}

	public void changeToScene (string sceneToChangeTo) {
		SceneManager.LoadScene (sceneToChangeTo);
	}

	public void changeVolume (float newVolume) {
		AudioListener.volume = newVolume;
	}

	public void mute (bool isMuted) {
		AudioListener.pause = isMuted;
	}


}

