using UnityEngine;

/// <summary>
/// Displays a configurable health bar for any object with a Damageable as a parent
/// </summary>
public class HealthBar : MonoBehaviour
{
	MaterialPropertyBlock matBlock;
	MeshRenderer meshRenderer;
	Camera mainCamera;
	Unit unit;
	Health health;
	bool BarShowed => meshRenderer.enabled;

	void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		matBlock = new MaterialPropertyBlock();
	}

	void Start()
	{
		mainCamera = Camera.main;

		unit = GetComponentInParent<Unit>();
		health = unit.health;
		health.OnHealthChange += CheckHealth;
		CheckHealth();
	}

	void LateUpdate()
	{
		if (BarShowed)
			AlignCamera();
	}

	void CheckHealth()
	{
		if (health.CurrentHealth < health.maxHealth)
		{
			meshRenderer.enabled = true;
			AlignCamera();
			UpdateParams();
		}
		else
		{
			meshRenderer.enabled = false;
		}
	}

	void UpdateParams()
	{
		meshRenderer.GetPropertyBlock(matBlock);
		matBlock.SetFloat("_Fill", unit.health.CurrentHealth / unit.health.maxHealth);
		meshRenderer.SetPropertyBlock(matBlock);
	}

	void AlignCamera()
	{
		if (mainCamera != null)
		{
			var camXform = mainCamera.transform;
			var forward = transform.position - camXform.position;
			forward.Normalize();
			var up = Vector3.Cross(forward, camXform.right);
			transform.rotation = Quaternion.LookRotation(forward, up);
		}
	}
}