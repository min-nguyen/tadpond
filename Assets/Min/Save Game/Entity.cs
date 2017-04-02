using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class Entity : MonoBehaviour {
	
	public EntityData data = new EntityData();

	public string title = "Entity";

	public void storeData() {
		data.title = title;
		Vector3 pos = transform.position;
		data.posX = pos.x;
		data.posY = pos.y;
		data.posZ = pos.z;
		data.rotation = transform.rotation;
	}

	public void loadData() {
		title = data.title;
		transform.position = new Vector3 (data.posX, data.posY, data.posZ);
		transform.rotation = data.rotation;
	}

	void OnEnable() {
		SaveData.onLoaded += delegate { loadData (); };
		SaveData.onBeforeSave += delegate { storeData (); };
		SaveData.onBeforeSave += delegate { SaveData.AddEntityData(data); };
	}

	void OnDisable() {
		SaveData.onLoaded -= delegate { loadData (); };
		SaveData.onBeforeSave -= delegate { storeData (); };
		SaveData.onBeforeSave -= delegate { SaveData.AddEntityData(data); };
	}
}

public class EntityData {

	[XmlAttribute("title")]
	public string title;

	[XmlElement("PosX")]
	public float posX;

	[XmlElement("PosY")]
	public float posY;

	[XmlElement("PosZ")]
	public float posZ;

	[XmlElement("Rotation")]
	public Quaternion rotation;
}
