using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ComradeMovement))]
[DisallowMultipleComponent]
public class Comrade : MonoBehaviour
{
    private Player player;

	public void SetPlayerReference(Player player)
	{
		this.player = player;
	}

	public Player GetPlayer()
	{
		return player;
	}
}
