using System.Linq;
using UnityEngine;

public class MainMenuViewController : MonoBehaviour
{
	public static MainMenuViewController Instance;

	[Space]
	[SerializeField] private SlotsController slotsController;
	[SerializeField] private RewardsController rewardsController;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		GetCollectedAirdrops();

		rewardsController.Hide();
	}

	private void GetCollectedAirdrops()
	{
		for (int i = 0; i < Settings.AirdropTypes.Length; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (PlayerPrefs.HasKey($"{Settings.AirdropTypes[i] + "_" + j}"))
				{
					var airdrop = GameResources.Instance.Airdrops.First(x => x.AirdropType.ToString().Equals(Settings.AirdropTypes[i]));
					slotsController.InitializeAirdropSlotIfEmpty(airdrop, j);
				}
			}
		}
	}

	public void ResetPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	public RewardsController GetRewardsController()
	{
		return rewardsController;
	}

	public SlotsController GetSlotsController()
	{
		return slotsController;
	}
}
