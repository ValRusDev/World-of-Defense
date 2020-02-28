using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerLevel", menuName = "TowerLevel")]
public class TowerLevel : ScriptableObject
{
	/// <summary>
	/// Цена
	/// </summary>
	[Tooltip("Цена")]
	public short price;

	/// <summary>
	/// Префаб вышки
	/// </summary>
	[Tooltip("Префаб вышки")]
	public Transform towerPrefab;
}
