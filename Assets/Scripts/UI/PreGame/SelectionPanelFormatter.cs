using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPanelFormatter : MonoBehaviour
{
    [SerializeField] private GameObject[] availableUnits;

    [SerializeField] private GameObject selectionPanel;

    private RectTransform rectTransform;
    private int placedSlots;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        foreach (var unit in availableUnits)
        {
            if (!unit.TryGetComponent<SentinelData>(out SentinelData sentinelData))
                return;
            
            var slotPrefab =
                (GameObject) Instantiate(sentinelData.GetUISlot(), GetNextAvailablePosition(), Quaternion.identity);
            var slotPrefabRectTransform = slotPrefab.GetComponent<RectTransform>();
            
            //Set the anchor + pivot to center
            slotPrefabRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            slotPrefabRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            slotPrefabRectTransform.pivot = new Vector2(0.5f, 0.5f);

            //Disable the unrequired scripts
            slotPrefab.GetComponent<UISlotManager>().enabled = false;

            slotPrefab.transform.SetParent(selectionPanel.transform, false);
        }
    }

    private Vector3 GetNextAvailablePosition()
    {
        var row = placedSlots / 3;
        var column = placedSlots % 3;

        var position = new Vector3();
        switch (column)
        {
            case 0 :
                position.x = -298;
                break;
            case 1 : 
                position.x = -9;
                break;
            case 2 : 
                position.x = 292;
                break;
        }

        position.y = 322 - (row * 153);

        placedSlots++;
        return position;
    }
}
