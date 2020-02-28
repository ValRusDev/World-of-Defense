using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCamRef : MonoBehaviour
{
    Transform ownTransform;
    Camera mainCamera;
    float maxCameraSize;
    float currentCameraSize;
    Vector3 startScale;

    void Start()
    {
        ownTransform = transform;
        mainCamera = Camera.main;
        TitleScript mainCameraComponent = mainCamera.GetComponent<TitleScript>();
        maxCameraSize = mainCameraComponent.maxCameraSize;
        currentCameraSize = mainCamera.orthographicSize;
        startScale = ownTransform.localScale;
    }

    void Update()
    {
        var newCameraSize = mainCamera.orthographicSize;
        if (newCameraSize != currentCameraSize)
        {
            float ratio = newCameraSize * 100 / maxCameraSize;
            ownTransform.localScale = startScale * (ratio / 100);

            currentCameraSize = newCameraSize;
        }
    }
}
