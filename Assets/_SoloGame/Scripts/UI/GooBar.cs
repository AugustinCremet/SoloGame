using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GooBar : MonoBehaviour
{
    private RectTransform _rect;
    private const float _MAX_RECT_HEIGHT = 0f;
    private const float _MIN_RECT_HEIGHT = -16f;
    private float _maxGoo;
    private float _currentGoo;
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }
    public void ChangeCurrentGoo(float currentGoo)
    {
        float desiredHeightFromMin = currentGoo / _maxGoo * -_MIN_RECT_HEIGHT;
        desiredHeightFromMin = Mathf.Clamp(desiredHeightFromMin, 0f, -_MIN_RECT_HEIGHT);
        _rect.anchoredPosition = new Vector3(0f, _MIN_RECT_HEIGHT + desiredHeightFromMin, 0f);
    }

    public void ChangeMaxGoo(float maxGoo)
    {
        _maxGoo = maxGoo;
    }
}
