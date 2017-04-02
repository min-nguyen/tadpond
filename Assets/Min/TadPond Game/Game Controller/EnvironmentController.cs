using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnvironmentController : MonoBehaviour {

    /*      Core Components     */
    public GameObject Environment;
    private GameObject Water;
    private WaterController WaterController;
    private OrganismController OrganismController;

    /*      GUI                     */
    public Button saveButton;
	public Button loadButton;
	private static string dataPath_e = string.Empty;
	private static string dataPath_v = string.Empty;
    public static GameObject instance;
    public const string playerPath = "Tadpole";
    private bool spawned;
    public float health;
    public Text healthText;
    private GameObject[] creatures;
    /*      Dropdown                */
    public Dropdown dropdown;
    public Text selectedOrganism;
    List<string> organisms = new List<string> { "Select", "Tadpole", "Carp", "Bitterling", "Waterflea", "Zoo Plankton", "Duck" };

    /*      Slider Variables        */
    public float sunlightSlider;
	public float nutrientsSlider;
	public float rainSlider;
    /*      Environment Variables   */
    public float sunlight;
    public float nutrients;
    public float rain;
    /*      Pond Variables          */
    public float algaeHealth;
	public float waterTemp;
	public float airTemp;
	public float pH;
	public float oxygen;

	/*      Day Cycle Variables     */
	private int dayLength;
	public int totalTime;
	private int currentTime;
	public float cycleSpeed;
	public Light sun;

	void Awake() {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			dataPath_e = System.IO.Path.Combine (Application.persistentDataPath, "Resources/entities.xml");
			dataPath_v = System.IO.Path.Combine (Application.persistentDataPath, "Resources/variables.xml");
		} else {
			dataPath_e = System.IO.Path.Combine (Application.dataPath, "Resources/entities.xml");
			dataPath_v = System.IO.Path.Combine (Application.dataPath, "Resources/variables.xml");
		}
	}

	void Start () {
        algaeHealth = 3f;
        waterTemp = 12.0f;
		airTemp = 15.0f;
		pH = 6.7f;
		oxygen = 10.0f;
		sunlightSlider = 0.5f;
		nutrientsSlider = 0.5f;
		rainSlider = 0.5f;
		dayLength = 24 * 60;
		currentTime = 12 * 60;
        PopulateDropdown();
        
        spawned = false;
		health = 0.0f;
		StartCoroutine(TimeOfDay ());
        OrganismController = GetComponent<OrganismController>();
        
        for (int i = 0; i < Environment.transform.childCount; i++)
        {
            if (Environment.transform.GetChild(i).tag == "Water")
            {
                Water = Environment.transform.GetChild(i).gameObject;
                WaterController = Water.GetComponent<WaterController>();
                break;
            }
        }

    }

	void Update() {
		updateSliders ();
		calculateHealth ();
		if (spawned == false && totalTime > 1000 && health > 66) {
			spawned = true;
			spawnEvent();
		}
		healthText.text = "Health: " + (((int) health).ToString());
	}

	//environmental controllers.
	public void changeSunlight (float newSunlight) {
		sunlightSlider = newSunlight;
	}
	public void changeNutrients (float newNutrients) {
		nutrientsSlider = newNutrients;
	}
	public void changeRain (float newRain) {
		rainSlider = newRain;
	}

	public void organismSelected (int index) {
		selectedOrganism.text = "Spawn  : " + organisms [index];
	}

	public void Spawn () {
		Debug.Log (selectedOrganism.text);
		health += 10;
        switch (selectedOrganism.text)
        {
            case "Tadpole":
                OrganismController.SpawnTadpole();
                break;
            case "Carp":
                OrganismController.SpawnCarp();
                break;
            case "Bitterling":
                OrganismController.SpawnBitterling();
                break;
            case "Waterflea":
                OrganismController.SpawnWaterflea();
                break;
            case "Zoo Plankton":
                OrganismController.SpawnZooPlankton();
                break;
            case "Duck":
                OrganismController.SpawnDuck();
                break;
            default:
                break;
        }
    }

	void PopulateDropdown () {
		dropdown.AddOptions(organisms);

	}

	IEnumerator TimeOfDay() {
		while (true) {
			currentTime += 1;
			totalTime += 1;
			currentTime = currentTime % dayLength;
			//int hours = Mathf.RoundToInt(currentTime / 60) % 24;
			//int minutes = currentTime % 60;
			//Debug.Log (hours + ":" + minutes);
			yield return new WaitForSeconds(1F/cycleSpeed);
		}
	}

	public static Entity createEntity(EntityData data, string path, Vector3 position, Quaternion rotation) {

		GameObject prefab = Resources.Load<GameObject>(path);
		GameObject go = GameObject.Instantiate(prefab, position, rotation) as GameObject;

		Entity entity = go.GetComponent<Entity>() ?? go.AddComponent<Entity>();
		entity.data = data;

		return entity;
	}

	public void OnEnable() {
		saveButton.onClick.AddListener (delegate { SaveData.Save(dataPath_e, SaveData.entityContainer); });
		saveButton.onClick.AddListener (delegate { SaveData.Save(dataPath_v, SaveData.variableContainer); });
		loadButton.onClick.AddListener (delegate { SaveData.Load_e(dataPath_e); });
		loadButton.onClick.AddListener (delegate { SaveData.Load_v(dataPath_v); });
	}

	public void OnDisable() {
		saveButton.onClick.RemoveListener (delegate { SaveData.Save(dataPath_e, SaveData.entityContainer); });
		saveButton.onClick.RemoveListener (delegate { SaveData.Save(dataPath_v, SaveData.variableContainer); });
		loadButton.onClick.RemoveListener (delegate { SaveData.Load_e(dataPath_e); });
		loadButton.onClick.RemoveListener (delegate { SaveData.Load_v(dataPath_v); });
	}

	private void updateSliders() {
	/*
	 * 
	 * oxygen ~ sun . nutrients . #org
	 * pH ~ rain . nutrients
	 * waterTemp ~ sun . rain . # #org
	 * airTemp ~ sun . rain
	 * 
	 */
		//int numOrg = creatures.Length;

		float currentTimeF = currentTime;
		if (currentTime < 12 * 60) {
			sunlight = ((currentTimeF / (12*60)) * sunlightSlider);
		}
		else {
			sunlight = (((2 - (currentTimeF / (12*60)))) * sunlightSlider);
		}
		sun.intensity = sunlight;
		rain = rainSlider;
		nutrients = nutrientsSlider;

		oxygen = 6 + (6 * Mathf.Abs(nutrients - sunlight));
		pH = 5 + (5 * Mathf.Abs(nutrients - rain));
		waterTemp = 8 + (8 * Mathf.Abs(sunlight - rain));
		airTemp = 10 + (10 * Mathf.Abs(sunlight - rain));

        //Update algae
        if (Mathf.Abs(7 - pH) > 1)
            algaeHealth = 2 + (6 * Mathf.Abs(nutrients - sunlight)) / (Mathf.Abs(7 - pH));
        else
            algaeHealth = 2 + (6 * Mathf.Abs(nutrients - sunlight));
        WaterController.SetAlgaeHealth(algaeHealth);
    }

	private void calculateHealth() {
		float healthPH = 0.0f;
		float healthO2 = 0.0f;
		float healthWT = 0.0f;
		float healthAT = 0.0f;

		if (oxygen <= 10 && oxygen >= 8) {
			healthO2 = ((oxygen * oxygen) -64) / 144;
		}
		if (pH <= 7.5 && pH >= 6) {
			healthPH = (pH - 6) / 6;
		}
		if (waterTemp <= 16 && waterTemp >= 8) {
			healthWT = (Mathf.Sqrt((waterTemp * waterTemp) - 64) - 2) / 48;
		}
		if (airTemp <= 20 && airTemp >= 10) {
			healthAT = (airTemp - 10) / 40;
		}

		health = (healthAT + healthO2 + healthPH + healthWT + algaeHealth/7) * 100;
		
	}

	private void kill(int number) {
		if (creatures.Length > 10) {
			for (int i = 0; i <= number; i++) {
				int j = Mathf.RoundToInt(creatures.Length * Random.value);
				Destroy (creatures[j]);
			}
		}
	}
    
	void spawnEvent() {
        
		foreach (GameObject frogspawn in creatures) {
			/*GameObject.Instantiate (Tadpole, frogspaw.transform.position, frogspaw.transform.rotation) as GameObject;
			GameObject.Destroy(frogspawn);*/
		}
	}
}