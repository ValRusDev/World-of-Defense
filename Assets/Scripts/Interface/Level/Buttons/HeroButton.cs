using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour
{
    public Hero hero;

    void Start()
    {
        Button button = transform.GetComponent<Button>();
        button.onClick.AddListener(HeroButtonClick);
    }

    public void HeroButtonClick()
    {
        Camera mainCamera = Camera.main;
        var cameraComponent = mainCamera.GetComponent<TitleScript>();
        cameraComponent.buttonPressed = true;
        var selectedObject = cameraComponent.selectedObject;
        if (selectedObject == null)
            SelectHero(cameraComponent);
        else
        {
            if (selectedObject == hero)
                selectedObject.GetComponent<SelectingObject>().ChangeSelect();
            else
            {
                // снимаем выделение
                selectedObject.GetComponent<SelectingObject>().ChangeSelect();
                // выбираем другого
                SelectHero(cameraComponent);
            }
        }
    }

    void SelectHero(TitleScript cameraComponent)
    {
        cameraComponent.selectedObject = hero.ownTransform;
        cameraComponent.selectedObject.GetComponent<SelectingObject>().ChangeSelect();
    }
}
