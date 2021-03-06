using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreGameUISlotManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Movement Easing Curve")]
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    
    private static readonly Color defaultColor = new Color(0.53f, 0.53f, 0.47f);
    
    public bool isSelected { get; private set; }
    
    private List<GameObject> slotInstances;
    private Image imageComponent;
    private Vector3 unselectedPosition;
    private List<IEnumerator> currentCoroutines;
    private GameData gameData;
    private GameObject newParent;
    private bool isClickable;

    private bool stopColorChange;

    private void Awake()
    {
        unselectedPosition = rectTransform.position;
        isClickable = true;
        
        currentCoroutines = new List<IEnumerator>();
        gameData = GameData.Instance;
        slotInstances = PreGameController.Instance.GetSlotInstances();
        newParent = GameObject.Find("Slots");
        imageComponent = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void StopColorChange() => stopColorChange = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClickable)
            return;
        isClickable = false;
        isSelected = true;
        
        var slotCopy = (GameObject) Instantiate(this.gameObject, rectTransform.position, Quaternion.identity);
        
        //Initialize and/or destroy other components
        var slotCopyRectTransform = slotCopy.GetComponent<RectTransform>();
        
        var preGameSelectedUISlot = slotCopy.AddComponent<PreGameSelectedUISlot>();
        preGameSelectedUISlot.Initialize(rectTransform.position, this);

        Destroy(slotCopy.GetComponent<PreGameUISlotManager>());

        //Set a new parent for the prefab
        slotCopy.transform.SetParent(newParent.transform);

        //Set the anchor + pivot to center left
        slotCopyRectTransform.anchorMin = new Vector2(0, 0.5f);
        slotCopyRectTransform.anchorMax = new Vector2(0, 0.5f);
        slotCopyRectTransform.pivot = new Vector2(0, 0.5f);
        
        //Start moving the prefab + darken the color
        StartCoroutine(MoveSlot(preGameSelectedUISlot, slotCopyRectTransform, new Vector3(0, 336 - (slotInstances.Count * 130) + 540, 0)));
        
        //Destroy tooltip (if any)
        try
        {
            var tooltip = slotCopy.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
            Destroy(tooltip);
        }
        catch {} //ignored
        
        ChangeColors(slotCopy);
        
        slotInstances.Add(slotCopy);
    }

    public void Deselect()
    {
        isClickable = true;
        isSelected = false;

        StartCoroutine(FadeTo(imageComponent, Color.white, .15f));
        StartCoroutine(FadeTo(GetComponent<Image>(), defaultColor, .15f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isSelected)
            return;
        
        if (currentCoroutines.Count > 0)
            currentCoroutines.ForEach(coroutine => StopCoroutine(coroutine));
        
        currentCoroutines.Add(FadeTo(GetComponent<Image>(), new Color(0.415f, 0.415f, 0.3f, 1), .25f));
        currentCoroutines.Add(FadeTo(imageComponent, new Color(0.5f, 0.5f, 0.5f, 1), .25f));
        
        currentCoroutines.ForEach(coroutine => StartCoroutine(coroutine));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isSelected)
            return;
        
        if (currentCoroutines.Count > 0)
            currentCoroutines.ForEach(coroutine => StopCoroutine(coroutine));

        currentCoroutines.Add(FadeTo(GetComponent<Image>(), defaultColor, .25f));
        currentCoroutines.Add(FadeTo(imageComponent, Color.white, .25f));
        
        currentCoroutines.ForEach(coroutine => StartCoroutine(coroutine));
    }

    private void ChangeColors(GameObject slotCopy)
    {
        var blackPanel = slotCopy.transform.GetChild(0).GetChild(0);
        var blackPanelImage = blackPanel.GetComponent<Image>();
        var imageImage = blackPanel.GetChild(0).GetComponent<Image>();
        
        slotCopy.GetComponent<Image>().color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
        blackPanelImage.color = new Color(0, 0, 0, 0);
        imageImage.color = new Color(1, 1, 1, 0);

        //The slotCopy fades back to regular opacity
        StartCoroutine(FadeTo(slotCopy.GetComponent<Image>(), defaultColor, 1.5f));
        StartCoroutine(FadeTo(blackPanelImage, new Color(0, 0, 0, .5f), 1.5f));
        StartCoroutine(FadeTo(imageImage, Color.white, 1.5f));
        
        //This gameObject fades to a darker tone
        StartCoroutine(FadeTo(GetComponent<Image>(), new Color(0.415f, 0.415f, 0.3f, 1), .15f));
        StartCoroutine(FadeTo(imageComponent, new Color(0.5f, 0.5f, 0.5f, 1), .15f));
    }
    
    private IEnumerator MoveSlot(PreGameSelectedUISlot preGameSelectedUiSlot, RectTransform rectTransform, Vector3 destination)
    {
        float t = 0;
        var duration = 0.5f;
        float s = t / duration;

        while (true)
        {
            t += Time.deltaTime;
            s = t / duration;

            rectTransform.position = Vector3.Lerp(this.rectTransform.position, 
                destination, easingCurve.Evaluate(s));
            
            if (rectTransform.position == destination) 
                break;
            
            yield return null;
        }
        
        preGameSelectedUiSlot.MakeClickable();
    }
    
    private IEnumerator FadeTo(Image image, Color newColor, float duration)
    {
        var t = 0f;

        while (t < 1)
        {
            try
            {
                image.color = Color.Lerp(image.color, newColor, t);
            }
            catch
            {
                yield break;
            }
            
            if (stopColorChange) 
                yield break;
            
            t += Time.deltaTime / duration;
            yield return null;
        }
    }
}
