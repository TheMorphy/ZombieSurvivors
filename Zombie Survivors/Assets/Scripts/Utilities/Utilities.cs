using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class Utilities
{
	public static string GetRandomEnumName<T>() where T : Enum
	{
		// Get an array of the names of the values in the enum
		string[] names = Enum.GetNames(typeof(T));

		// Get a random index
		int index = UnityEngine.Random.Range(0, names.Length);

		// Return the name at the random index
		return names[index];
	}

	public static T GetRandomEnumValue<T>() where T : Enum
	{
		// Get an array of the values in the enum
		T[] values = (T[])Enum.GetValues(typeof(T));

		// Get a random index
		int index = UnityEngine.Random.Range(0, values.Length);

		// Return the value at the random index
		return values[index];
	}

	public static T GetEnumValue<T>(string value) where T : Enum, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("T must be an enumerated type");
		}

		return (T)Enum.Parse(typeof(T), value);
	}

	public static string GetDescription(this Enum value)
	{
		DescriptionAttribute attribute = value.GetAttribute<DescriptionAttribute>();
		return attribute == null ? value.ToString() : attribute.Description;
	}

	private static T GetAttribute<T>(this Enum value) where T : Attribute
	{
		FieldInfo field = value.GetType().GetField(value.ToString());
		T attribute = Attribute.GetCustomAttribute(field, typeof(T)) as T;
		return attribute;
	}

	public static string GetDescription<T>(string name)
	{
		Type enumType = typeof(T);
		FieldInfo field = enumType.GetField(name);
		DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
		return attribute == null ? name : attribute.Description;
	}

	public static float ApplyPercentage(float percentage, float total)
	{
		total += (percentage * total) / 100;
		return total;
	}

	public static T GetComponentInChildrenButNotParent<T>(GameObject parent) where T : UnityEngine.Component
	{
		// Get all of the children of the given GameObject
		Transform[] children = parent.GetComponentsInChildren<Transform>(true);

		// Iterate through each child
		foreach (Transform child in children)
		{
			// Skip the parent
			if (child == parent.transform)
			{
				continue;
			}

			// Try to get the component of type T in the child
			T component = child.GetComponent<T>();

			// If the component was found, return it
			if (component != null)
			{
				return component;
			}
		}

		// If the component was not found in any of the children, return null
		return null;
	}
}
