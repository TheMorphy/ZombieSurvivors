using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BaseScriptableObject : ScriptableObject
{

	//------------------ Not Sure What To Do With This -----------
	public class Property
	{
		public string name;
		public string value;
	}

	public virtual Dictionary<string, object> GetProperties()
	{
		Dictionary<string, object> properties = new Dictionary<string, object>();
		Type type = GetType();
		var propertyInfos = type.GetFields();
		foreach (var property in propertyInfos)
		{
			properties.Add(property.Name, property.GetValue(this));
		}
		return properties;
	}
	//------------------------------------------------------------
}
