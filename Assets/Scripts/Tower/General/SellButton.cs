using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
	public Tower tower;

	[HideInInspector]
	public Transform ownTransform;

	Button button;
	LevelManager levelManager;

	void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(SellTower);

		ownTransform = transform;

		tower = ownTransform.GetComponentInParent<Tower>();
	}

	void Start()
	{
		levelManager = LevelManager.Instance;
	}

	public void SellTower()
    {
        tower.buildPlace.gameObject.SetActive(true);
		tower.buildPlace.ShowHideMeshs(false);
		tower.ownTransform.gameObject.SetActive(false);

		WarTower warTower = tower as WarTower;
		if (warTower != null)
		{
			var warriors = warTower.warriors;
			foreach (var warrior in warriors)
			{
				warrior.isActive = false;
				warrior.ClearTarget();
			}
		}

		int priceToReturn = tower.priceToReturn;
		levelManager.AddGold(priceToReturn);
		levelManager.SelectedObject = null;
	}
}
