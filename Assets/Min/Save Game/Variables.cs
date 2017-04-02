using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class Variables : MonoBehaviour {


	public VariableData data = new VariableData();
	public EnvironmentController controller;

	void Start() {
		controller = transform.parent.GetComponent<EnvironmentController>();
	}

	public string title = "Variables";

	public void storeData() {
		data.title = title;
		data.waterTemp = controller.waterTemp;
		data.airTemp = controller.airTemp;
		data.pH = controller.pH;
		data.oxygen = controller.oxygen;
		data.sunlightSlider = controller.sunlightSlider;
		data.nutrientsSlider = controller.nutrientsSlider;
		data.rainSlider = controller.rainSlider;
		data.time = controller.totalTime;
		data.health = controller.health;
	}

	public void loadData() {
		title = data.title;
		controller.waterTemp = data.waterTemp;
		controller.airTemp = data.airTemp;
		controller.pH = data.pH;
		controller.oxygen = data.oxygen;
		controller.sunlightSlider = data.sunlightSlider;
		controller.nutrientsSlider = data.nutrientsSlider;
		controller.rainSlider = data.rainSlider;
		controller.totalTime = data.time;
		controller.health = data.health;
	}

	void OnEnable() {
		SaveData.onLoaded += delegate { loadData (); };
		SaveData.onBeforeSave += delegate { storeData (); };
		SaveData.onBeforeSave += delegate { SaveData.AddVariableData(data); };
	}

	void OnDisable() {
		SaveData.onLoaded -= delegate { loadData (); };
		SaveData.onBeforeSave -= delegate { storeData (); };
		SaveData.onBeforeSave -= delegate { SaveData.AddVariableData(data); };
	}
}

public class VariableData {

	[XmlAttribute("Title")]
	public string title;

	[XmlElement("WaterTemp")]
	public float waterTemp;

	[XmlElement("AirTemp")]
	public float airTemp;

	[XmlElement("PH")]
	public float pH;

	[XmlElement("Oxygen")]
	public float oxygen;

	[XmlElement("SunlightSlider")]
	public float sunlightSlider;

	[XmlElement("NutrientSlider")]
	public float nutrientsSlider;

	[XmlElement("RainSLider")]
	public float rainSlider;

	[XmlElement("Time")]
	public int time;

	[XmlElement("Health")]
	public float health;
}
