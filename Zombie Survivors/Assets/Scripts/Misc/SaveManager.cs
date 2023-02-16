using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SaveManager
{
	public static void InsertToJSON<T>(List<T> toSave, string fileName)
	{
		string content = JsonHelper.ToJson(toSave.ToArray(), true);
		WriteFile(GetPath(fileName), content);
	}

	public static void SaveToJSON<T>(T toSave, string fileName)
	{
		T[] array = { toSave };
		string content = JsonHelper.ToJson<T>(array, true);
		WriteFile(GetPath(fileName), content);
	}

	public static void UpdateInJSON<T>(T item, string trackingKey, string fileName)
	{
		string path = GetPath(fileName);
		if (File.Exists(path))
		{
			// Load the JSON file into a list
			List<T> itemsList = ReadFromJSON<T>(fileName);

			// Find the item with the same trackingKey and update it
			for (int i = 0; i < itemsList.Count; i++)
			{
				var currentItem = itemsList[i];
				var trackingKeyProperty = typeof(T).GetProperty("TrackingCode");
				if (trackingKeyProperty != null && trackingKeyProperty.GetValue(currentItem).ToString() == trackingKey)
				{
					// Update the item and save the updated list to the JSON file
					var properties = typeof(T).GetProperties();
					foreach (var property in properties)
					{
						var newValue = property.GetValue(item);
						property.SetValue(currentItem, newValue);
					}
					SaveToJSON(itemsList, fileName);
					return;
				}
			}
		}
	}


	public static void DeleteFromJSON<T>(T item, string fileName)
	{
		string path = GetPath(fileName);
		if (File.Exists(path))
		{
			// Load the JSON file into a list
			List<T> itemsList = ReadFromJSON<T>(fileName);

			// Remove the item from the list
			itemsList.Remove(item);

			// Save the updated list to the JSON file
			InsertToJSON(itemsList, fileName);
		}
	}

	public static void ClearJSONFile(string fileName)
	{
		string path = GetPath(fileName);
		string emptyContent = "{}";
		WriteFile(path, emptyContent);
	}

	public static List<T> ReadFromJSON<T>(string fileName)
	{
		string contnet = ReadFile(GetPath(fileName));

		if (string.IsNullOrEmpty(contnet) || contnet == "{}")
		{
			return new List<T>();
		}
		
		List<T> result = JsonHelper.FromJson<T>(contnet).ToList();

		return result;
	}

	public static int GetNumSavedItems<T>(string fileName)
	{
		string content = ReadFile(GetPath(fileName));

		if (string.IsNullOrEmpty(content) || content == "{}")
		{
			return 0;
		}

		T[] savedItems = JsonHelper.FromJson<T>(content);
		return savedItems.Length;
	}


	private static string GetPath(string fileName)
	{
		return Application.persistentDataPath + "/" + fileName;
	}

	private static void WriteFile(string path, string content)
	{
		FileStream fileStream = new FileStream(path, FileMode.Create);

		using(StreamWriter writer = new StreamWriter(fileStream))
		{
			writer.Write(content);
		}
	}

	private static string ReadFile(string path)
	{
		if (File.Exists(path))
		{
			using(StreamReader reader = new StreamReader(path))
			{
				string content = reader.ReadToEnd();
				return content;
			}
		}
		return "";
	}

}

public static class JsonHelper
{
	public static T[] FromJson<T>(string json)
	{
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
		return wrapper.Items;
	}

	public static string ToJson<T>(T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.Items = array;
		return JsonUtility.ToJson(wrapper);
	}

	public static string ToJson<T>(T[] array, bool prettyPrint)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.Items = array;
		return JsonUtility.ToJson(wrapper, prettyPrint);
	}

	[Serializable]
	private class Wrapper<T>
	{
		public T[] Items;
	}
}
