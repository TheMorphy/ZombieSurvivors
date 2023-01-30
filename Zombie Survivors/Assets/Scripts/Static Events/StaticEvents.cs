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

	// Collected object event
	public static event Action<CollectedEventArgs> OnCollected;
	public static void CallCollectedEvent(Vector3 collectedPosition)
	{
		OnCollected?.Invoke(new CollectedEventArgs() { collectedPosition = collectedPosition });
	}

	// Player Initialized
	public static event Action<ComradeBoardedEventArgs> OnPlayerInitialized;
	public static void CallPlayerInitializedEvent(Transform playerTransform)
	{
		OnPlayerInitialized?.Invoke(new ComradeBoardedEventArgs() { playerTransform = playerTransform });
	}

	// Comrade boarded to helicopter
	public static event Action OnComradeBoarded;
	public static void CallComradeBoardedEvent()
	{
		OnComradeBoarded?.Invoke();
	}

	// Airdrop spawned
	public static event Action<Vector3> OnAirdropSpawned;
	public static void CallAirdropSpawnedEvent(Vector3 spawnPos)
	{
		OnAirdropSpawned?.Invoke(spawnPos);
	}
}

public class ComradeBoardedEventArgs : EventArgs
{
	public Transform playerTransform;
}

public class CircleSpawnedEventArgs : EventArgs
{
	public Vector3 spawnPosition;
}

public class CollectedEventArgs : EventArgs
{
	public Vector3 collectedPosition;
}
