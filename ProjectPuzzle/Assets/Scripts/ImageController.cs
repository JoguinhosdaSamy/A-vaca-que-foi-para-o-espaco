using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    public float fillSpeed = 0.2f;
    private RectTransform imageRectTransform;
    private Transform target;

    public void SetTarget(Transform targetTransform, Image image)
    {
        target = targetTransform;

        if (image != null)
        {
            imageRectTransform = image.GetComponent<RectTransform>();
        }
        else
        {
            imageRectTransform = null;
        }
    }

    private void LateUpdate()
    {
        if (target != null && imageRectTransform != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position);
            imageRectTransform.position = screenPosition;

            DecreaseFillAmount();
        }
    }

    public void DecreaseFillAmount()
    {
        Image image = imageRectTransform.GetComponent<Image>();

        if (image != null)
        {
            float fillAmount = image.fillAmount;
            fillAmount -= fillSpeed * Time.deltaTime;
            fillAmount = Mathf.Clamp01(fillAmount);
            image.fillAmount = fillAmount;

            Vector2 sizeDelta = imageRectTransform.sizeDelta;
            sizeDelta.x = fillAmount * image.rectTransform.rect.width;
            imageRectTransform.sizeDelta = sizeDelta;
        }
    }
}
