using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCanvas : MonoBehaviour
{
	RectTransform rectTransform;
	BoxCollider2D boxCollider;

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	void Start()
	{
		boxCollider.size = rectTransform.sizeDelta;

	}
}
