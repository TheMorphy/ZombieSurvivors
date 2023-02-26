using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
	private static CanvasManager instance;

	[SerializeField] private Tab startingTab;
	[SerializeField] private Tab[] tabs;

	private Tab currentTab;

	private readonly Stack<Tab> history = new Stack<Tab>();

	private void Awake()
	{
		if(instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		for (int i = 0; i < tabs.Length; i++)
		{
			tabs[i].Initialize(null);

			if (tabs[i].GetType() == typeof(NavigationTab))
			{
				tabs[i].Show();
				continue;
			}

			tabs[i].Hide();
		}

		if(startingTab != null)
		{
			Show(startingTab, true);
		}
	}

	public static T GetTab<T>() where T : Tab
	{
		for (int i = 0; i < instance.tabs.Length; i++)
		{
			if (instance.tabs[i] is T tTab)
			{
				return tTab;
			}
		}
		return null;
	}

	public static void Show<T>(bool remember = true, object[] args = null) where T : Tab
	{
		for (int i = 0; i < instance.tabs.Length; i++)
		{
			if (instance.tabs[i] is T)
			{
				if (instance.currentTab != null)
				{
					if (remember)
					{
						instance.history.Push(instance.currentTab);
					}
					
					instance.currentTab.Hide();
				}

				instance.tabs[i].Show();

				if (args != null)
				{
					instance.tabs[i].Initialize(args);
				}

				instance.currentTab = instance.tabs[i];
			}
		}
	}

	public static void Show(Tab tab, bool remember = true)
	{
		if (instance.currentTab != null)
		{
			if (remember)
			{
				instance.history.Push(instance.currentTab);
			}

			instance.currentTab.Hide();
		}
		tab.Show();

		instance.currentTab = tab;
	}

	public static void ShowLast()
	{
		if (instance.history.Count != 0)
		{
			Show(instance.history.Pop(), false);
		}
	}
}
