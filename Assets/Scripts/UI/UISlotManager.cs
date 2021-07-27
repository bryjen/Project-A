using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlotManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Movement Easing Curve")]
    [SerializeField] private AnimationCurve easeInCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    [SerializeField] private AnimationCurve easeOutCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    [Header("Associated Sentinel Settings")]
    [SerializeField] private GameObject sentinelPrefab;
    [SerializeField] private GameObject sentinelPreviewPrefab;
    
    private static readonly Color defaultColor = new Color(0.53f, 0.53f, 0.47f);
    private static readonly Color selectedColor = new Color(0.69f, 0.69f, 0.44f);
    
    private IEnumerator currentCoroutine;
    private bool isEnterExitDisabled;
    private bool isSelected;

    #region OnMouseOver

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEnterExitDisabled) return;
        
        StartNewCoroutine(MoveSlot());
    }
    
    private IEnumerator MoveSlot()
    {
        float t = 0;
        var duration = 0.5f;
        float s = t / duration;
        
        var destination = new Vector3(50, rectTransform.position.y, rectTransform.position.z);
        
        while (true)
        {
            t += Time.deltaTime;
            s = t / duration;

            rectTransform.position = Vector3.Lerp(rectTransform.position, 
                destination, easeInCurve.Evaluate(s));
            
            if (rectTransform.position == destination) 
                break;

            yield return null;
        }

        currentCoroutine = null;
    }

    #endregion

    #region OnPointerExit

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEnterExitDisabled) return;
        
        StartNewCoroutine(ReturnSlot());
    }
    
    private IEnumerator ReturnSlot()
    {
        float t = 0;
        var duration = 0.5f;
        float s = t / duration;
        
        var destination = new Vector3(0, rectTransform.position.y, rectTransform.position.z);
        
        while (true)
        {
            t += Time.deltaTime;
            s = t / duration;

            rectTransform.position = Vector3.Lerp(rectTransform.position, 
                destination, easeOutCurve.Evaluate(s));
            
            if (rectTransform.position == destination) 
                break;

            yield return null;
        }
        
        currentCoroutine = null;
    }

    #endregion

    #region OnPointerClick

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(OnPointerClickCoroutine());
    }

    private IEnumerator OnPointerClickCoroutine()
    {
        while (currentCoroutine != null)
            yield return null;

        if (rectTransform.position.x != 50)
            yield return StartCoroutine(MoveSlot());

        if (isSelected)
        {
            isEnterExitDisabled = false;
            StartCoroutine(ColorChanger(defaultColor));
            isSelected = false;
            
            yield break;
        }
        
        isEnterExitDisabled = true;
        StartCoroutine(ColorChanger(selectedColor));
        isSelected = true;
    }

    private IEnumerator ColorChanger(Color newColor)
    {
        var image = GetComponent<Image>();
        var t = 0f;
        var duration = .15f;

        while (t < 1)
        {
            image.color = Color.Lerp(image.color, newColor, t);

            t += Time.deltaTime / duration;
            yield return null;
        }
    }

    #endregion

    /// <summary>Stops the current coroutine (if any) then runs another coroutine</summary>
    private void StartNewCoroutine(IEnumerator coroutine)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        currentCoroutine = coroutine;
        StartCoroutine(currentCoroutine);
    }
}
