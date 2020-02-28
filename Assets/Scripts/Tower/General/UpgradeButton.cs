using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
	public Tower currentTower;
	public TowerLevel towerLevel;

	Transform towerPrefab;
	short price;

	string currentGold;
	Button button;

	LevelManager levelManager;

	void Awake()
	{
		currentTower = transform.parent.parent.parent.GetComponent<Tower>();

		button = GetComponent<Button>();
		button.onClick.AddListener(UpdateTower);

		towerPrefab = towerLevel.towerPrefab;
		price = towerLevel.price;
	}

	void Start()
	{
		levelManager = LevelManager.Instance;
	}

	void Update()
	{
		if (currentGold != TitleScript.gold)
		{
			short price = towerLevel.price;
			string decodingGold = B64X.Decode(TitleScript.gold);
			short existsGold = short.Parse(decodingGold);
			if (price > existsGold && button.interactable)
				button.interactable = false;
			else if (price <= existsGold && !button.interactable)
				button.interactable = true;

			currentGold = TitleScript.gold;
		}
	}

	public void UpdateTower()
	{
		BuildPlace buildPlace = currentTower.buildPlace;
		Destroy(currentTower.gameObject);

		/*Transform towerInst = GameObject.Instantiate(towerPrefab, currentTower.ownTransform.position, Quaternion.identity);
		Tower tower = towerInst.GetComponent<Tower>();
		tower.buildPlace = buildPlace;

		TowerLevel towerData = tower.towerData;
		int towerPrice = towerData.price;
		tower.priceToReturn = towerPrice + price;

		levelManager.AddGold()*/
	}
}
