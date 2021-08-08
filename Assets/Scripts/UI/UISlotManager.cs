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
    
    private static readonly HashSet<UISlotManager> slotManagers = new HashSet<UISlotManager>();
    private static readonly Color defaultColor = new Color(0.53f, 0.53f, 0.47f);
    private static readonly Color selectedColor = new Color(0.69f, 0.69f, 0.44f);
    private static bool isDisabled;
    
    private IEnumerator currentCoroutine;
    private GameData gameData;
    private bool isEnterExitDisabled;
    private bool isSelected;

    private void Awake()
    {
        gameData = GameData.Instance;
        slotManagers.Add(gameObject.GetComponent<UISlotManager>());
    }

    public static void DisableAllSlots()
    {
        isDisabled = true;

        foreach (var uiSlotManager in slotManagers)
        {
            uiSlotManager.ChangeColor(new Color(0.3113208f, 0.3113208f, 0.2596298f, 1));
            uiSlotManager.ChangeColor(uiSlotManager.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>(), 
                new Color(.5f, .5f, .5f, 1));
        }
    }

    public static void EnableAllSlots()
    {
        isDisabled = false;

        foreach (var uiSlotManager in slotManagers)
        {
            uiSlotManager.ChangeColor(defaultColor);
            uiSlotManager.ChangeColor(uiSlotManager.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>(), 
               Color.white);
        }
    }

    public static void ClearSlotManagers() => slotManagers.Clear();

    public void AddToSlotManagers() => slotManagers.Add(this);

    public IEnumerator Deselect()
    {
        while (currentCoroutine != null)
            yield return null;

        isEnterExitDisabled = false;
        StartCoroutine(ColorChanger(GetComponent<Image>(), defaultColor));
        StartCoroutine(ReturnSlot());
        isSelected = false;
            
        yield break;
    }

    #region OnMouseOver

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEnterExitDisabled || isDisabled) return;
        
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
        if (isEnterExitDisabled || isDisabled) return;
        
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
        if (isDisabled)
            return;
        
        foreach (var slotManager in slotManagers)
        {
            if (slotManager == this) continue;

            if (slotManager.isSelected)
                StartCoroutine(slotManager.Deselect());
        }
        
        StartCoroutine(OnPointerClickCoroutine());
    }

    private IEnumerator OnPointerClickCoroutine()
    {
        if (isSelected)
        {
            gameData.selectedSentinel = null;
            gameData.selectedSentinelPreview = null;
            
            isEnterExitDisabled = false;
            StartCoroutine(ColorChanger(GetComponent<Image>(), defaultColor));
            isSelected = false;
            
            yield break;
        }
        gameData.selectedSentinel = sentinelPrefab;
        gameData.selectedSentinelPreview = sentinelPreviewPrefab;
        
        isEnterExitDisabled = true;
        StartCoroutine(ColorChanger(GetComponent<Image>(), selectedColor));
        isSelected = true;
        
        while (currentCoroutine != null)
            yield return null;
        
        if (rectTransform.position.x != 50)
            yield return StartCoroutine(MoveSlot());
    }

    private IEnumerator ColorChanger(Image image, Color newColor)
    {
        var t = 0f;
        var duration = .15f;

        while (t < 1)
        {
            image.color = Color.Lerp(image.color, newColor, t);

            t += Time.deltaTime / duration;
            yield return null;
        }
    }

    private void ChangeColor(Color newColor) => StartCoroutine(ColorChanger(GetComponent<Image>(), newColor));
    private void ChangeColor(Image image, Color newColor) => StartCoroutine(ColorChanger(image, newColor));

    #endregion

    public static void DeselectAll()
    {
        foreach (var slotManager in slotManagers)
        {
            if (slotManager.isSelected)
                slotManager.StartDeselect();
        }

        var gameData = GameData.Instance;
        gameData.selectedSentinel = null;
        gameData.selectedSentinelPreview = null;
    }

    private void StartDeselect()
    {
        StartCoroutine(Deselect());
    }

    /// <summary>Stops the current coroutine (if any) then runs another coroutine</summary>
    private void StartNewCoroutine(IEnumerator coroutine)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        currentCoroutine = coroutine;
        StartCoroutine(currentCoroutine);
    }
}