 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class SaveData : MonoBehaviour {

	public static EntityContainer entityContainer = new EntityContainer();
	public static VariableContainer variableContainer = new VariableContainer();

	public delegate void SerializeAction();
	public static event SerializeAction onLoaded;
	public static event SerializeAction onBeforeSave;

	public static void Load_e(string path) {
		entityContainer = LoadEntities(path);

		foreach (EntityData data in entityContainer.entities) {
			Vector3 position = new Vector3 (data.posX,data.posY,data.posZ);
			EnvironmentController.createEntity (data, EnvironmentController.playerPath, position, data.rotation); 
		}
		if (onLoaded != null) {
			onLoaded ();
		}
	}

	public static void Load_v(string path) {
		variableContainer = LoadVariables(path);

		if (onLoaded != null) {
			onLoaded ();
		}
	}

	public static void Save(string path, EntityContainer entities) {
		if (onBeforeSave != null) {
			onBeforeSave ();
		}

		SaveEntities(path, entities);

		ClearEntities();
	}

	public static void Save(string path, VariableContainer variables) {
		if (onBeforeSave != null) {
			onBeforeSave ();
		}

		SaveVariables(path, variables);

		ClearVariables();
	}

	public static void AddEntityData(EntityData data) {
		entityContainer.entities.Add(data);
	}

	public static void ClearEntities() {
		entityContainer.entities.Clear();
	}

	public static void AddVariableData(VariableData data) {
		variableContainer.variables.Add(data);
	}

	public static void ClearVariables() {
		variableContainer.variables.Clear();
	}

	private static EntityContainer LoadEntities(string path) {
		XmlSerializer serializer = new XmlSerializer (typeof(EntityContainer));
		FileStream stream = new FileStream (path, FileMode.Open);

		EntityContainer entities = serializer.Deserialize (stream) as EntityContainer;
		stream.Close();

		return entities;
	}

	private static void SaveEntities(string path, EntityContainer entities) {
		XmlSerializer serializer = new XmlSerializer (typeof(EntityContainer));
		FileStream stream = new FileStream (path, FileMode.Truncate);

		serializer.Serialize(stream, entities);
		stream.Close();
	}

	private static VariableContainer LoadVariables(string path) {
		XmlSerializer serializer = new XmlSerializer (typeof(VariableContainer));
		FileStream stream = new FileStream (path, FileMode.Open);

		VariableContainer variables = serializer.Deserialize (stream) as VariableContainer;
		stream.Close();

		return variables;
	}

	private static void SaveVariables(string path, VariableContainer variables) {
		XmlSerializer serializer = new XmlSerializer (typeof(VariableContainer));
		FileStream stream = new FileStream (path, FileMode.Truncate);

		serializer.Serialize(stream, variables);
		stream.Close();
	}
}
