using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : SelectingObject
{
    public Hero hero;
    public Transform circleProgressBar;
    CircleProgressBar circleProgressBarComponent;
    GameObject circleProgressBarGameObject;

    bool needTargetForSpell;
    Button button;

    void Start()
    {
        circleProgressBar.gameObject.SetActive(false);
        circleProgressBarComponent = circleProgressBar.GetComponent<CircleProgressBar>();
        circleProgressBarGameObject = circleProgressBar.gameObject;

        button = transform.GetComponent<Button>();
        button.onClick.AddListener(CastSpell);
    }

    void Update()
    {
        float skillCooldown = hero.skillCooldown;
        float skillCooldownCount = hero.skillCooldownCount;
        if (skillCooldownCount < skillCooldown)
        {
            button.interactable = false;
            if (!circleProgressBarGameObject.activeInHierarchy)
                circleProgressBarGameObject.SetActive(true);
            circleProgressBarComponent.SetSettings(skillCooldown, skillCooldownCount);
        }
        else
        {
            button.interactable = true;
            circleProgressBar.gameObject.SetActive(false);
        }
    }

    void CastSpell()
    {
        Camera mainCamera = Camera.main;
        var cameraComponent = mainCamera.GetComponent<TitleScript>();
        cameraComponent.buttonPressed = true;
        needTargetForSpell = hero.needTargetForSpell;
        if (needTargetForSpell)
        {
            var selectedObject = cameraComponent.selectedObject;
            if (selectedObject == null)
                cameraComponent.selectedObject = transform;
            else if (selectedObject != transform)
            {
                selectedObject.GetComponent<SelectingObject>().ChangeSelect();
                cameraComponent.selectedObject = transform;
            }
        }
        else
			hero.Cast();
    }
}
