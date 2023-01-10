using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
	[SerializeField] private Transform weaponShootPositionTransform;

	private SetActiveWeaponEvent setWeaponEvent;
	private Weapon currentWeapon;

	private void Awake()
	{
		// Load components
		setWeaponEvent = GetComponent<SetActiveWeaponEvent>();
	}

	private void OnEnable()
	{
		setWeaponEvent.OnSetActiveWeapon += SetWeaponEvent_OnSetActiveWeapon;
	}

	private void OnDisable()
	{
		setWeaponEvent.OnSetActiveWeapon -= SetWeaponEvent_OnSetActiveWeapon;
	}

	private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
	{
		SetWeapon(setActiveWeaponEventArgs.weapon);
	}

	private void SetWeapon(Weapon weapon)
	{
		currentWeapon = weapon;

		currentWeapon.weaponDetails.AmmoDetails = weapon.weaponDetails.AmmoDetails;

		// Set weapon shoot position
		//weaponShootPositionTransform.localPosition = currentWeapon.weaponDetails.WeaponShootPosition;
	}

	public Weapon GetCurrentWeapon()
	{
		return currentWeapon;
	}

	public AmmoDetailsSO GetCurrentAmmo()
	{
		return currentWeapon.weaponDetails.AmmoDetails;
	}

	public Vector3 GetShootPosition()
	{
		return weaponShootPositionTransform.position;
	}

	public Transform GetShootFirePointTransform()
	{
		return weaponShootPositionTransform;
	}

	public void RemoveCurrentWeapon()
	{
		currentWeapon = null;
	}
}
