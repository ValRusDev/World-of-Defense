using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : Hero
{
    public override void Cast()
    {
        ScareEnemies();
        ClearTarget();
        SetToZeroSkillCooldownCount();
    }

    void ScareEnemies()
    {
        foreach (var innerTransform in spellUnits)
        {
            var enemyCompopent = innerTransform as Enemy;
            if (enemyCompopent != null)
                enemyCompopent.GetScared(3f);

			var health = innerTransform.GetComponent<Health>();
			if (health != null)
				health.GetDamage(spellDmg);
		}
    }

    void ClearTarget()
    {
        if (meleeTarget != null)
        {
            meleeTarget.GetComponent<Enemy>().target = null;
            meleeTarget = null;
        }
    }
}
