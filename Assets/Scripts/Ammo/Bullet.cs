using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : Ammo
{
    public float bulletSpeed = 10;

    short minDmg;
    short maxDmg;
    short currentDmg;
    float boomRadius;

    Vector2 startPosition;
    Vector2 targetPosition;
    Vector2 halfPath;
    bool aim = false;

    private void Start()
    {
        SetDmg();
        startPosition = transform.position;
        targetPosition = target.transform.position;
        SetEndPosition();
    }

    void SetDmg()
    {
        minDmg = attackingObject.GetComponent<BombTower>().minAttack;
        maxDmg = attackingObject.GetComponent<BombTower>().maxAttack;
        currentDmg = (short)Random.Range(minDmg, maxDmg);
        boomRadius = attackingObject.GetComponent<BombTower>().boomRadius;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (target == null)
            return;
        if ((Vector2)transform.position == halfPath)
            aim = true;
        if (aim)
            transform.position = Vector2.MoveTowards(transform.position, target.ownTransform.position, bulletSpeed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, halfPath, bulletSpeed * Time.deltaTime);
        if (transform.position == target.ownTransform.position)
            Damage();
    }

    void Damage()
    {
        if (target == null)
            return;
        // убираем снаряд
        gameObject.SetActive(false);

        // находим объекты в области
        var colliders = Physics2D.OverlapCircleAll(targetPosition, boomRadius).ToList();
        var targets = colliders.Select(c => c.transform).ToList();
        targets.Add(target.ownTransform);
        targets = targets.Distinct().ToList();
        foreach (var item in targets)
        {
            // наносим урон
            //var healthComponent = item.gameObject.GetComponent<Enemy>();
            /*if (healthComponent != null)
                healthComponent.currentHealth -= currentDmg;*/
        }
    }

    private void SetEndPosition()
    {
        if (target == null)
            return;
        var component = target.GetComponent<Enemy>();
        if (component == null)
            return;
        float targetSpeed = component.moveSpeed;
        // путь до цели
        float dist = (startPosition - targetPosition).magnitude;
        // время до цели
        float timeToTarget = dist / bulletSpeed;

        // определяем, где будет будет цель спустя это время
        Waypoint nextWaypoint = target.GetComponent<Enemy>().nextWaypoint;
        Vector2 nextWaypointPosition = nextWaypoint.ownTransfrom.position;
        Vector2 nextTargetPlace = Vector2.MoveTowards(targetPosition, nextWaypointPosition, targetSpeed * timeToTarget);

        // вершина "параболы"
        float xNew = (startPosition.x + nextTargetPlace.x) / 2;
        float yNew = nextTargetPlace.y + 1;
        if (startPosition.y > nextTargetPlace.y)
            yNew = startPosition.y + 1;
        Vector2 halfVector = new Vector2(xNew, yNew);

        halfPath = halfVector;
        /*// длина пути
        float distanceToNextTargetPlace = nextTargetPlace.magnitude;
        // пол пути до цели
        float halfDis = distanceToNextTargetPlace / 2;
        // высота параболы
        float h = halfDis * .75f;
        // четверть
        float fourth = halfDis / 2f;*/

    }
}
