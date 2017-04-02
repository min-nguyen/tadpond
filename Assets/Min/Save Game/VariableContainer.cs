using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("Variable Collection")]
public class VariableContainer {

	[XmlArray("Variable")]
	[XmlArrayItem("Variable")]
	public List<VariableData> variables = new List<VariableData>();
}
