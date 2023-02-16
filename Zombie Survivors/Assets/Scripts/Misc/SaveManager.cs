using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class SaveManager
{
	/// <summary>
	/// This will override any data saved inside a file.
	/// </summary>
	public static void SaveToJSON<T>(List<T> toSave, string fileName)
	{
		string content = JsonHelper.ToJson(toSave.ToArray(), true);
		WriteFile(GetPath(fileName), content);
	}

	/// <summary>
	/// This will override any data saved inside a file.
	/// </summary>
	public static void SaveToJSON<T>(T toSave, string fileName)
	{
		T[] array = { toSave };
		string content = JsonHelper.ToJson<T>(array, true);
		WriteFile(GetPath(fileName), content);
	}

	/// <summary>
	/// This will add a new list of items to the file
	/// </summary>
	public static void AppendToJSON<T>(List<T> toSave, string fileName)
	{
		// Load the existing JSON data into a list
		List<T> existingData = ReadFromJSON<T>(fileName);

		// Add the new items to the list
		existingData.AddRange(toSave);

		// Save the updated list back to the file
		string content = JsonHelper.ToJson(existingData.ToArray());
		WriteFile(GetPath(fileName), content);
	}

	/// <summary>
	/// This will add a new item to the file
	/// </summary>
	public static void AppendToJSON<T>(T toSave, string fileName)
	{
		string filePath = GetPath(fileName);

		// Check if the file already exists
		if (File.Exists(filePath))
		{
			// Read the existing contents of the file into a list
			List<T> existingItems = ReadFromJSON<T>(fileName);

			// Add the new item to the list
			existingItems.Add(toSave);

			// Serialize the updated list to the file
			SaveToJSON(existingItems, fileName);
		}
		else
		{
			// If the file doesn't exist, create a new list with the item and serialize it to the file
			T[] array = { toSave };
			string content = JsonHelper.ToJson<T>(array);
			WriteFile(filePath, content);
		}
	}

	public static async Task AppendToJsonAsync<T>(T toSave, string fileName)
	{
		string filePath = GetPath(fileName);

		await Task.Run(() => {
			// Check if the file already exists
			if (File.Exists(filePath))
			{
				// Read the existing contents of the file into a list
				List<T> existingItems = ReadFromJSON<T>(fileName);

				// Add the new item to the list
				existingItems.Add(toSave);

				// Serialize the updated list to the file
				SaveToJSON(existingItems, fileName);
			}
			else
			{
				// If the file doesn't exist, create a new list with the item and serialize it to the file
				T[] array = { toSave };
				string content = JsonHelper.ToJson<T>(array);
				WriteFile(filePath, content);
			}
		});
	}

	/// <summary>
	/// This will append new item, but only works for Trackable objects
	/// </summary>
	public static void UpdateTrackingInJSON<T>(T item, string trackingKey, string fileName)
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
					SaveToJSON<List<T>>(itemsList, fileName);
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
			SaveToJSON(itemsList, fileName);
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
