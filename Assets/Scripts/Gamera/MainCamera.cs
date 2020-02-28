using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
	[HideInInspector]
	public Transform ownTransform;

	[HideInInspector]
	public Camera ownCamera;

	/// <summary>
	/// Игровой менеджер
	/// </summary>
	[Tooltip("Менеджер уровня")]
	public LevelManager levelManager;

	/// <summary>
	/// Выбранный объект
	/// </summary>
	SelectingObject selectedObject => levelManager.SelectedObject;

	/// <summary>
	/// Пересечения с лучом
	/// </summary>
	RaycastHit[] hits3D = new RaycastHit[0];

	/// <summary>
	/// Пересечения с лучом 2D
	/// </summary>
	RaycastHit2D[] hits2D = new RaycastHit2D[0];

	Vector3 _lastPosition;
	float _cameraToWorldRatio = 0.1f;

	void Awake()
	{
		ownTransform = transform;
		ownCamera = GetComponent<Camera>();
	}

	void Start()
	{
		levelManager = LevelManager.Instance;
	}

	void Update()
	{
		MouseControl();
	}

	/// <summary>
	/// Управление мышью
	/// </summary>
	void MouseControl()
	{
		if (Input.GetMouseButtonDown(0))
		{
			GetHits();

			if (hits2D.Any(/*h => h.transform != null && h.transform.GetComponent<Button>() != null*/))
				return;

			if (selectedObject == null)
				SelectObject();
			else
				ActionWithSelectedObject();
		}
		if (Input.GetMouseButton(0))
		{

		}
		if (Input.GetMouseButtonUp(0))
		{

		}
	}

	/// <summary>
	/// Выбрать объект
	/// </summary>
	void SelectObject()
	{
		for (int i = 0; i < hits3D.Length; i++)
		{
			RaycastHit hit = hits3D[i];
			Transform hitTransform = hit.transform;
			if (hitTransform == null)
				continue;

			SelectingObject selectingObject = hitTransform.GetComponent<SelectingObject>();
			if (selectingObject == null)
				continue;

			var hero = hitTransform.GetComponent<Hero>();
			var tower = hitTransform.GetComponent<Tower>();
			var buildPlace = hitTransform.GetComponent<BuildPlace>();
			if (hero != null)
			{
				levelManager.SelectedObject = hero;
				break;
			}
			else if (tower != null)
			{
				levelManager.SelectedObject = tower;
				break;
			}
			else if (buildPlace != null)
			{
				levelManager.SelectedObject = buildPlace;
				break;
			}
		}
	}

	/// <summary>
	/// Действие с выбранным объектом
	/// </summary>
	void ActionWithSelectedObject()
	{
		switch (selectedObject)
		{
			case Hero hero:
				ActionWithHero(hero);
				break;
			case Tower tower:
				ActionWithTower(tower);
				break;
			case BuildPlace buildPlace:
				ActionWithBuildPlace(buildPlace);
				break;
		}
	}

	/// <summary>
	/// Действие с героем
	/// </summary>
	/// <param name="hero">Герой</param>
	void ActionWithHero(Hero hero)
	{

	}

	/// <summary>
	/// Действие с вышкой
	/// </summary>
	/// <param name="tower">Вышка</param>
	void ActionWithTower(Tower tower)
	{
		switch (tower)
		{
			case WarTower warTower:
				ActionWithWarTower(warTower);
				break;

		}
	}

	/// <summary>
	/// Действие с местом постройки
	/// </summary>
	/// <param name="buildPlace">Место постройки</param>
	void ActionWithBuildPlace(BuildPlace buildPlace)
	{
		//levelManager.SelectedObject = null;
	}

	/// <summary>
	/// Действие с вышкой воинов
	/// </summary>
	/// <param name="warTower">Вышка воинов</param>
	void ActionWithWarTower(WarTower warTower)
	{
		MoveArea moveArea = warTower.moveArea;
		for (int i = 0; i < hits3D.Length; i++)
		{
			RaycastHit hit = hits3D[i];
			Transform hitTransform = hit.transform;
			if (hitTransform == null)
				continue;

			MoveArea hitMoveArea = hitTransform.GetComponent<MoveArea>();
			if (hitMoveArea == null)
				continue;

			if (hitMoveArea == moveArea)
			{
				Vector3 point = hit.point;
				warTower.SetPlacePoint(point);
				break;
			}
		}
		levelManager.SelectedObject = null;
	}

	/// <summary>
	/// Получить пересечения
	/// </summary>
	void GetHits()
	{
		var position = Input.mousePosition;
		var ray = Camera.main.ScreenPointToRay(position);
		Vector2 mousePos2D = new Vector2(position.x, position.y);

		//RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
		//hits2D = Physics2D.RaycastAll(ownTransform.position, position);
		hits2D = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
		hits3D = Physics.RaycastAll(ray);
	}

	Vector3 GetWorldPoint(Vector3 screenPoint)
	{
		RaycastHit hit;
		Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit);
		return hit.point;
	}
}
