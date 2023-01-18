using System;
using UnityEngine;

public static class StaticEvents
{
	// Multiplication Circle Spawned
	public static event Action<CircleSpawnedEventArgs> OnCircleSpawned;

	public static void CallCircleSpawnedEvent(Vector3 spawnPosition)
	{
		OnCircleSpawned?.Invoke(new CircleSpawnedEventArgs() { spawnPosition = spawnPosition });
	}

	// Multiplication Circle Spawned
	public static event Action<CircleDespawnedEventArgs> OnCircleDespawned;

	public static void CallCircleDespawnedEvent(Vector3 spawnPosition)
	{
		OnCircleDespawned?.Invoke(new CircleDespawnedEventArgs() { despawnPosition = spawnPosition });
	}
}

public class CircleSpawnedEventArgs : EventArgs
{
	public Vector3 spawnPosition;
}

public class CircleDespawnedEventArgs : EventArgs
{
	public Vector3 despawnPosition;
}
