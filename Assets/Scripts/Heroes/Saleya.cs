using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saleya : Hero
{
    public Transform spellArrow;
    public float arrowSpeed;
    public Vector2 arrowEndPosition;

    public override void Cast()
    {
        SetArrowEndPosition();
        LetTheArrow();
        ClearSelectedObject();
        SetToZeroSkillCooldownCount();
    }

    void SetArrowEndPosition()
    {
        arrowEndPosition = positionForSpell;
        //arrowEndPosition = Vector2.MoveTowards(transform.position, positionForSpell, 10);
        //arrowEndPosition = arrowEndPosition * 3;
    }

    void ClearSelectedObject()
    {
        Camera mainCamera = Camera.main;
        var cameraComponent = mainCamera.GetComponent<TitleScript>();
        //var selectedObject = cameraComponent.selectedObject;
        //cameraComponent.selectedObject.GetComponent<SelectingObject>().ChangeSelect();
        cameraComponent.selectedObject = null;
    }

    // выстрел
    public Transform LetTheArrow()
    {
        Transform ammoInst = Instantiate(spellArrow, transform.position, Quaternion.identity);
        var angle = Vector2.Angle(Vector2.right, arrowEndPosition - (Vector2)transform.position);//угол между вектором от объекта к мыше и осью х
        ammoInst.eulerAngles = new Vector3(0f, 0f, transform.position.y < arrowEndPosition.y ? angle : -angle);//немного магии на последок
        ammoInst.GetComponent<SaleyaArrow>().saleya = transform;

        return ammoInst;
    }
}
