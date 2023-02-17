using UnityEngine;
using UnityEngine.UI;

public class NavigationTab : Tab
{
	[SerializeField] private Button equipmentBtn; 
	[SerializeField] private Button shopBtn; 
	[SerializeField] private Button playBtn; 

	public override void Initialize(object[] args)
	{
		equipmentBtn.onClick.AddListener(() => {
			CanvasManager.Show<EquipmentTab>();
		});

		shopBtn.onClick.AddListener(() => {
			//CanvasManager.Show<ShopTab>();
		});

		playBtn.onClick.AddListener(() => {
			CanvasManager.Show<PlayTab>();
		});
	}
}
