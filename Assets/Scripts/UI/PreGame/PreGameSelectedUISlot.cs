using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreGameSelectedUISlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialCoordinates;

    private Image blackPanelImage;
    private Image image;
    
    private PreGameUISlotManager preGameUiSlotManager;
    private bool isClickable;

    public void MakeClickable() => isClickable = true;

    public void Initialize(Vector3 initialCoordinates, PreGameUISlotManager preGameUiSlotManager)
    {
        this.initialCoordinates = initialCoordinates;
        this.preGameUiSlotManager = preGameUiSlotManager;

        blackPanelImage = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        image = blackPanelImage.transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClickable)
            return;
        
        preGameUiSlotManager.Deselect();
        
        StartCoroutine(MoveSlot(GetComponent<RectTransform>(), initialCoordinates));
        StartCoroutine(FadeTo(GetComponent<Image>(), new Color(1, 1, 1, 0)));
        StartCoroutine(FadeTo(blackPanelImage, new Color(1, 1, 1, 0)));
        StartCoroutine(FadeTo(image, new Color(1, 1, 1, 0)));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
    
    private IEnumerator MoveSlot(RectTransform rectTransform, Vector3 destination)
    {
        float t = 0;
        var duration = 0.5f;
        float s = t / duration;

        var easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        while (true)
        {
            t += Time.deltaTime;
            s = t / duration;

            rectTransform.position = Vector3.Lerp(rectTransform.position, 
                destination, easingCurve.Evaluate(s));
            
            if (rectTransform.position == destination) 
                break;
            
            yield return null;
        }
    }
    
    protected IEnumerator FadeTo(Image image, Color newColor)
    {
        var t = 0f;
        var duration = .5f;

        while (t < 1)
        {
            image.color = Color.Lerp(image.color, newColor, t);

            t += Time.deltaTime / duration;
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
}