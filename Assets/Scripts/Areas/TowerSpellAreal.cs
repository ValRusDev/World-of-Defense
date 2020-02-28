using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpellAreal : GeneralSpellArea
{
    bool onTriggerStayFinished;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (onTriggerStayFinished)
            return;

        /*Transform collsisionTransform = collision.transform;
        List<Transform> innerTransforms = new List<Transform>();
        innerTransforms = parentComponent.spellInnerTransforms;

        if (!innerTransforms.Contains(collsisionTransform))
        {
            if (collsisionTransform.GetComponent<Unit>() != null)
            {
                innerTransforms.Add(collsisionTransform);
                parentComponent.spellInnerTransforms = innerTransforms;

            }
        }*/
        onTriggerStayFinished = true;
    }
}
