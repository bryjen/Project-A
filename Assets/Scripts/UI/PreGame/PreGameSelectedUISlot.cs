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

    private List<GameObject> slotInstances;
    private PreGameUISlotManager preGameUiSlotManager;
    private bool isClickable;

    public void MakeClickable() => isClickable = true;

    public PreGameUISlotManager GetPreGameUiSlotManager() => preGameUiSlotManager;

    public void Initialize(Vector3 initialCoordinates, PreGameUISlotManager preGameUiSlotManager)
    {
        this.initialCoordinates = initialCoordinates;
        this.preGameUiSlotManager = preGameUiSlotManager;

        slotInstances = PreGameController.Instance.GetSlotInstances();
        blackPanelImage = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        image = blackPanelImage.transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClickable)
            return;
        
        preGameUiSlotManager.Deselect();
        slotInstances.Remove(this.gameObject);

        AdjustSlotInstances();

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

    /// <summary> Adjusts the positions of each slot when a slot has been deselected </summary>
    private void AdjustSlotInstances()
    {
        for (int i = 0; i < slotInstances.Count; i++)
        {
            if (slotInstances[i] == this.gameObject)
                continue;

            slotInstances[i].GetComponent<RectTransform>().position = new Vector3(0, 336 - (i * 130) + 540);
        }
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
    
    private IEnumerator FadeTo(Image image, Color newColor)
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
