using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTowerButton : MonoBehaviour
{
	public Tower tower;
	Button button;
	string currentGold;
	LevelManager levelManager;

	void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(BuildTower);
	}

	void Start()
	{
		levelManager = LevelManager.Instance;
		levelManager.OnGoldChange += CheckGoldChange;
	}

	void CheckGoldChange()
	{
		if (tower == null)
			return;

		TowerLevel towerData = tower.towerData;
		int price = towerData.price;
		string decodingGold = B64X.Decode(levelManager.currentGold);
		int existsGold = int.Parse(decodingGold);
		if (price > existsGold && button.interactable)
			button.interactable = false;
		else if (price <= existsGold && !button.interactable)
			button.interactable = true;

		currentGold = levelManager.currentGold;
	}

	public void BuildTower()
	{
		BuildPlace buildPlace = GetComponentInParent<BuildPlace>();
		Transform towerInst = Instantiate(tower.transform, buildPlace.ownTransform.position, tower.transform.rotation);
		towerInst.parent = buildPlace.ownTransform.parent;
		Tower newTower = towerInst.GetComponent<Tower>();
		newTower.buildPlace = buildPlace;

		buildPlace.gameObject.SetActive(false);

		TowerLevel towerData = newTower.towerData;
		int price = towerData.price;
		newTower.priceToReturn = price;

		levelManager.AddGold(price * (-1));
		levelManager.SelectedObject = null;
	}
}
