using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreGameUISlotManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform rectTransform;
    
    [Header("Movement Easing Curve")]
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    
    private static readonly Color defaultColor = new Color(0.53f, 0.53f, 0.47f);

    private List<GameObject> occupiableSpaces;
    private Image imageComponent;
    private Vector3 unselectedPosition;
    private IEnumerator currentCoroutine;
    private GameData gameData;
    private GameObject newParent;
    private bool isClickable;
    private bool isSelected;

    private void Awake()
    {
        unselectedPosition = rectTransform.position;
        isClickable = true;
        
        gameData = GameData.Instance;

        occupiableSpaces = PreGameController.Instance.GetOccupiableSpaces();
        newParent = GameObject.Find("Slots");
        imageComponent = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClickable)
            return;
        isClickable = false;

        var slotCopy = (GameObject) Instantiate(this.gameObject, rectTransform.position, Quaternion.identity);
        slotCopy.transform.SetParent(newParent.transform);
        var slotCopyRectTransform = slotCopy.GetComponent<RectTransform>();
        
        if (slotCopy.TryGetComponent<PreGameUISlotManager>(out PreGameUISlotManager preGameUiSlotManager))
            Destroy(preGameUiSlotManager);
        
        var preGameSelectedUISlot = slotCopy.AddComponent<PreGameSelectedUISlot>();
        preGameSelectedUISlot.Initialize(rectTransform.position, this);

        //Set the anchor + pivot to center left
        slotCopyRectTransform.anchorMin = new Vector2(0, 0.5f);
        slotCopyRectTransform.anchorMax = new Vector2(0, 0.5f);
        slotCopyRectTransform.pivot = new Vector2(0, 0.5f);
        
        currentCoroutine = MoveSlot(preGameSelectedUISlot, slotCopyRectTransform, new Vector3(0, 336 - (occupiableSpaces.Count * 130) + 540, 0));
        occupiableSpaces.Add(this.gameObject);
        
        StartCoroutine(currentCoroutine);
        StartCoroutine(FadeTo(imageComponent, new Color(0.5f, 0.5f, 0.5f, 1)));
        StartCoroutine(FadeTo(GetComponent<Image>(), new Color(0.415f, 0.415f, 0.3f, 1)));
    }

    public void Deselect()
    {
        isClickable = true;
        occupiableSpaces.Remove(this.gameObject);

        StartCoroutine(FadeTo(imageComponent, Color.white));
        StartCoroutine(FadeTo(GetComponent<Image>(), defaultColor));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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
    
    protected IEnumerator FadeTo(Image image, Color newColor)
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
}
