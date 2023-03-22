using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[DisallowMultipleComponent]
public class ExpDrop : Collectable
{
	[SerializeField] private float absorbTime = 1.5f;

	private Rigidbody rb;

	private int expAmmount;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	private void Start()
	{
		// Moves a bit up, instead of just being on the ground
		transform.position += Vector3.up * 2f;

		// Adds velocity to the random direction to create falling effect
		rb.velocity = new Vector2(Random.onUnitSphere.x, Random.onUnitSphere.z) * 4f;
	}

	private void Update()
	{
		if(GameManager.Instance.GameState == GameState.Evacuating)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			if (SquadControl.Comrades.Count > 0)
			{
				StartCoroutine(Collect(other.transform, absorbTime));
			}
		}
	}

	public void SetExpValue(int expAmmount)
	{
		this.expAmmount = expAmmount;
	}

	public int CollectExp()
	{
		return expAmmount;
	}

	protected override void OnCollected()
	{
		AudioManager.Instance.PlaySFX(SoundTitle.EXP_Pickup);
		GameManager.Instance.GetLevelSystem().AddExperience(expAmmount);
		Destroy(gameObject);
	}
}
