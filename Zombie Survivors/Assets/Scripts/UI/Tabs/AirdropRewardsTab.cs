using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AirdropRewardsTab : Tab
{
	[SerializeField] private GameObject rewardsGrid;
	[SerializeField] private Image openedAirdropImage;
	[SerializeField] private GameObject rewardCardReference;

	private void Update()
	{
		if(rewardsGrid.transform.childCount > 0 && Input.GetMouseButtonDown(0))
		{
			CanvasManager.ShowLast();
		}
	}

	public override void Initialize()
	{
		
	}

	public override void InitializeWithArgs(object[] args)
	{
		AirdropDetails airdropDetails = (AirdropDetails)args[0];
		List<Reward> rewards = (List<Reward>)args[1];

		openedAirdropImage.sprite = airdropDetails.AirdropSprite;

		for (int i = 0; i < rewards.Count; i++)
		{
			RewardCard rewardCard = Instantiate(rewardCardReference, rewardsGrid.transform).GetComponent<RewardCard>();
			rewardCard.gameObject.SetActive(true);
			rewardCard.InitializeRewardCard(rewards[i]);
		}
	}
}
