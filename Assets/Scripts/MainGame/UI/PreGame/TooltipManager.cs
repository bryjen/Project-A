using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Spawning Options")]
    [SerializeField] private bool spawnOnCursorLocation;
    [SerializeField, Header("Only if the bool above is set to false")] private Vector3 spawnLocation;

    [Space(10)] 
    
    [SerializeField] private GameObject uiPrefab;

    [Header("Other")] 
    [SerializeField] private float hoverTimeRequired;

    private PreGameUISlotManager preGameUiSlotManager;
    private GameObject uiObject;

    private void Start()
        => preGameUiSlotManager = GetComponent<PreGameUISlotManager>();

    public void OnPointerEnter(PointerEventData eventData)
        => StartCoroutine(OnPointerEnterCoroutine());

    private IEnumerator OnPointerEnterCoroutine()
    {
        if (preGameUiSlotManager.isSelected)
            yield break;
        
        yield return new WaitForSeconds(hoverTimeRequired);
        
        SpawnTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        
        DestroyTooltip();
    }
    
    public void SpawnTooltip()
    {
        uiObject = (GameObject) Instantiate(uiPrefab,
            spawnOnCursorLocation ? Input.mousePosition : spawnLocation, Quaternion.identity);
        uiObject.transform.SetParent(this.transform.GetChild(0).GetChild(0));
    }

    public void DestroyTooltip()
    {
        if (uiObject != null)
            Destroy(uiObject);
    }
}
