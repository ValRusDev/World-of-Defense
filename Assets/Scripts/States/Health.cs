using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : State
{
	[HideInInspector]
	public Unit unit;

	[Tooltip("Здоровье")]
	public float maxHealth;

	/// <summary>
	/// Текущее здоровье
	/// </summary>
	[SerializeField, Tooltip("Текущее здоровье")]
	float currentHealth;

	public float CurrentHealth
	{
		get
		{
			return currentHealth;
		}
		set
		{
			currentHealth = value;
		}
	}

	public delegate void CurrentHealthChangeHandler();
	public event CurrentHealthChangeHandler OnHealthChange;

	void Awake()
	{
		unit = GetComponent<Unit>();
		unit.health = this;
	}

	/// <summary>
	/// Задать начальные свойства
	/// </summary>
	public void SetStartStats()
	{
		CurrentHealth = maxHealth;
	}

	/// <summary>
	/// Получить урон
	/// </summary>
	public void GetDamage(float amount)
	{
		CurrentHealth -= amount;
		OnHealthChange.Invoke();
	}

	/// <summary>
	/// Отнять здоровье
	/// </summary>
	public void RemoveHealth(short value)
	{
		CurrentHealth -= value;
		OnHealthChange.Invoke();
	}

	/// <summary>
	/// Полностью вылечить
	/// </summary>
	public void FullHeal()
	{
		CurrentHealth = maxHealth;
		OnHealthChange.Invoke();
	}
}
