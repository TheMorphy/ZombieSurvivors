using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	public static PoolManager Instance;

	[System.Serializable]
	public class Pool
	{
		public string name;
		public GameObject prefab;
		public int size;
	}

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		GameObject ammo = new GameObject("Ammo");

		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		for (int i = 0; i < pools.Count; i++)
		{

			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int j = 0; j < pools[i].size; j++)
			{
				GameObject obj = Instantiate(pools[i].prefab, ammo.transform.position, Quaternion.identity, ammo.transform);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pools[i].name, objectPool);

		}
	}

	public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(poolName))
		{
			Debug.Log("Error: Pool with tag: " + poolName + "doesn't exist");
			return null;
		}

		GameObject objectToSpawn = poolDictionary[poolName].Dequeue();

		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;

		poolDictionary[poolName].Enqueue(objectToSpawn);

		return objectToSpawn;
	}
}

