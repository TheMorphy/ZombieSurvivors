using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class SaveManager
{
	/// <summary>
	/// This will override any data saved inside a file.
	/// </summary>
	public static void SaveToJSON<T>(List<T> toSave, string fileName)
	{
		// Load the existing items from the file, if it exists
		List<T> itemsList = new List<T>();
		if (File.Exists(GetPath(fileName)))
		{
			itemsList = ReadFromJSON<T>(fileName);
		}

		// Replace existing items with new ones
		foreach (var item in toSave)
		{
			int id = (int)item.GetType().GetField("ID").GetValue(item);
			int index = itemsList.FindIndex(i =>
			{
				var property = i.GetType().GetField("ID");
				return property != null && (int)property.GetValue(i) == id;
			});

			if (index >= 0)
			{
				itemsList[index] = item;
			}
			else
			{
				itemsList.Add(item);
			}
		}

		// Save the updated list to the JSON file
		string content = JsonHelper.ToJson(itemsList.ToArray(), true);
		WriteFile(GetPath(fileName), content);
	}

	/// <summary>
	/// This will override any data saved inside a file.
	/// </summary>
	public static void SaveToJSON<T>(T toSave, string fileName)
	{
		List<T> savedItems = ReadFromJSON<T>(fileName);

		if (savedItems != null && savedItems.Count > 0)
		{
			savedItems.Add(toSave);
			string content = JsonHelper.ToJson(savedItems.ToArray(), true);
			WriteFile(GetPath(fileName), content);
		}
		else
		{
			T[] array = { toSave };
			string content = JsonHelper.ToJson<T>(array, true);
			WriteFile(GetPath(fileName), content);
		}
	}

	public static void DeleteFromJSON<T>(int id, string fileName)
	{
		List<T> itemsList = ReadFromJSON<T>(fileName);

		int index = itemsList.FindIndex(item => (int)item.GetType().GetField("ID").GetValue(item) == id);
		if (index >= 0)
		{
			itemsList.RemoveAt(index);
			string content = JsonHelper.ToJson(itemsList.ToArray(), true);
			WriteFile(GetPath(fileName), content);
		}
	}

	public static void CreateJsonFiles(List<string> fileNames)
	{
		foreach (string fileName in fileNames)
		{
			string filePath = GetPath(fileName);
			if (!File.Exists(filePath))
			{
				SaveToJSON(new List<object>(), fileName);
			}
		}
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
