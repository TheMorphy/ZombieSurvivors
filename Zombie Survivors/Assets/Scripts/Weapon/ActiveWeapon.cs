using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
	[SerializeField] private Transform weaponShootPositionTransform;

	private SetActiveWeaponEvent setWeaponEvent;
	private static Weapon CurrentWeapon;

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

	public void SetWeapon(Weapon weapon)
	{
		CurrentWeapon = weapon;

		CurrentWeapon.weaponDetails.AmmoDetails = weapon.weaponDetails.AmmoDetails;
	}

	public Weapon GetCurrentWeapon()
	{
		return CurrentWeapon;
	}

	public AmmoDetailsSO GetCurrentAmmo()
	{
		return CurrentWeapon.weaponDetails.AmmoDetails;
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
		CurrentWeapon = null;
	}
}
