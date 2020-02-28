using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : AttackingObject
{
	[Header("Свойства башни")]
	/// <summary>
	/// Место для постройки
	/// </summary>
	[Tooltip("Место для постройки")]
    public BuildPlace buildPlace;

	/// <summary>
	/// Данные башни
	/// </summary>
	[Header("Данные башни"), Tooltip("Данные башни")]
	public TowerLevel towerData;

	/// <summary>
	/// Цена для возврата
	/// </summary>
	[Tooltip("Цена для возврата")]
	public int priceToReturn;

	void Awake()
	{
		ownTransform = transform;
	}

	void Start()
    {
        ShowHideMeshs(false);
    }
}
