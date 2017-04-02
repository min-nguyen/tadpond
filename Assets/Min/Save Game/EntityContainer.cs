using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("Entity Collection")]
public class EntityContainer {

	[XmlArray("Entity")]
	[XmlArrayItem("Entity")]
	public List<EntityData> entities = new List<EntityData>();
}
