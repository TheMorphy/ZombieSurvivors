using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	private Player player;
	private bool isPlayerMovementDisabled = false;
	private Quaternion currentRotation;

	private FloatingJoystick joystick;

	Vector3 moveDirection;
	private bool IsPlayerDead = false;
	
	private void Awake()
	{
		joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FloatingJoystick>(); // I no like dis, but it works for now

		rb = GetComponentInParent<Rigidbody>();
		player = GetComponent<Player>();
	}

	private void FixedUpdate()
	{
		if (isPlayerMovementDisabled || IsPlayerDead)
			return;

		HandleMovement();
	}

	private void HandleMovement()
	{
		moveDirection = new Vector3(joystick.Horizontal * player.PlayerDetails.MoveSpeed, 0, joystick.Vertical * player.PlayerDetails.MoveSpeed);
		rb.MovePosition(transform.position + moveDirection * Time.deltaTime * GetMoveSpeed());
	}

	public void StopPlayer()
	{
		joystick.ResetJoystick();

		transform.GetChild(0).gameObject.SetActive(false);

		joystick.gameObject.SetActive(false);

		player.PlayerController.enabled = false;

		moveDirection = Vector3.zero;
	}

	public Vector3 GetMoveDirection()
	{
		return moveDirection;
	}

	public Quaternion GetLookDirection()
	{
		return currentRotation;
	}

	public float GetMoveSpeed()
	{
		return moveDirection.magnitude;
	}

	public void EnablePlayerMovement()
	{
		joystick.gameObject.SetActive(true);

		transform.rotation = currentRotation;

		isPlayerMovementDisabled = false;

		Time.timeScale = 1;
	}

	public void DisablePlayerMovement()
	{
		joystick.transform.GetChild(0).gameObject.SetActive(false);

		currentRotation = transform.rotation;

		joystick.gameObject.SetActive(false);

		isPlayerMovementDisabled = true;

		Time.timeScale = 0;

		joystick.ResetJoystick();
	}

	public void UpgradeMoveSpeed(float value, UpgradeAction upgradeAction)
	{
		if (upgradeAction == UpgradeAction.Add)
		{
			player.PlayerDetails.MoveSpeed += value;
		}
		else if (upgradeAction == UpgradeAction.Multiply)
		{
			player.PlayerDetails.MoveSpeed *= value;
		}
		else if (upgradeAction == UpgradeAction.Increase_Percentage)
		{
			player.PlayerDetails.MoveSpeed = Utilities.ApplyPercentage(value, player.PlayerDetails.MoveSpeed);
		}
	}

	public void SetPlayerToDead()
	{
		IsPlayerDead = true;
	}

	public void SetPlayerToAlive()
	{
		IsPlayerDead = false;
	}
}
