using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectingObject : MonoBehaviour
{
	[Header("Настройки SelectingObject")]
	/// <summary>
	/// Объекты выбран или нет
	/// </summary>
	[Tooltip("Объект выбран или нет")]
	public bool isSelected;

	/// <summary>
	/// Объекты для отображения
	/// </summary>
	[Tooltip("Объекты для отображения")]
	public Transform[] toShowObjects;

	/// <summary>
	/// Рендер объекта
	/// </summary>
	[HideInInspector, Tooltip("Рендер объекта")]
	public Renderer ownMeshRenderer;

	[HideInInspector]
	public Transform ownTransform;
	[HideInInspector]
	public Collider2D ownCollider;

	/// <summary>
	/// Изменить "выбранность"
	/// </summary>
	public void ChangeSelect()
	{
		isSelected = !isSelected;
		ShowHideMeshs(isSelected);
	}

	/// <summary>
	/// Показать/скрыть спрайты
	/// </summary>
	/// <param name="show"></param>
	public void ShowHideMeshs(bool show)
	{
		foreach (var showObject in toShowObjects)
		{
			// основной спрайт
			var spRender = showObject.GetComponent<SpriteRenderer>();
			if (spRender != null)
				spRender.enabled = show;

			var meshRenderer = showObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null)
				meshRenderer.enabled = show;

			var canvas = showObject.GetComponent<Canvas>();
			if (canvas != null)
				canvas.gameObject.SetActive(show);

			/*// дочерние объекты
            int childCount = showObject.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = showObject.GetChild(i);
                child.gameObject.SetActive(show);
            }*/
		}
	}
}
