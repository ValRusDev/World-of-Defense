using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleyaArrow : MonoBehaviour
{
    public Transform saleya;

    float lifeTime;
    float maxLifeTime = 1f;
    float speed;
    short damage;
    Vector2 endPosition;
    Vector3 normalizeDirection;
    List<Transform> targets = new List<Transform>();

    void Start()
    {
        var saleyaComponent = saleya.GetComponent<Saleya>();
        speed = saleyaComponent.arrowSpeed;
        damage = saleyaComponent.spellDmg;
        endPosition = saleyaComponent.arrowEndPosition;
        normalizeDirection = ((Vector3)endPosition - transform.position).normalized;
    }

    void Update()
    {
        if (lifeTime < maxLifeTime)
        {
            transform.position += normalizeDirection * speed * Time.deltaTime;
            //transform.Translate(endPosition * speed * Time.deltaTime);
            //transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            lifeTime += Time.deltaTime;
        }
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Transform targetTransform = collision.transform;
        if (targetTransform != null && !targets.Contains(targetTransform))
        {
            /*var enemyComponent = targetTransform.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RemoveHealth(damage);
                targets.Add(targetTransform);
            }*/

			var health = targetTransform.GetComponent<Health>();
			if (health != null)
			{
				health.GetDamage(damage);
				targets.Add(targetTransform);
			}
		}
    }
}
