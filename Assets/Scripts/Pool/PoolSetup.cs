using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pool/PoolSetup")]
public class PoolSetup : MonoBehaviour
{
	//обертка для управления статическим классом PoolManager

	#region Unity scene settings
	[SerializeField]
	private PoolManager.PoolPart[] pools; //структуры, где пользователь задает префаб для использования в пуле и инициализируемое количество 
	#endregion

	#region Methods
	void OnValidate()
	{
		if (pools != null)
		{
			for (int i = 0; i < pools.Length; i++)
			{
				pools[i].name = pools[i].prefab.name; //присваиваем имена заранее, до инициализации
			}
		}
	}

	void Awake()
	{
		Initialize();
	}

	void Initialize()
	{
		PoolManager.Initialize(pools); //инициализируем менеджер пулов
	}
	#endregion

}
