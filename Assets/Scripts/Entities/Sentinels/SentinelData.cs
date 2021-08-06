using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelData : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] private GameObject preview;
    [SerializeField] private GameObject UISlot;
    [SerializeField] private bool isUnlocked;

    private UISlotManagerData uiSlotManagerData;

    public GameObject GetPreview() => preview;

    public GameObject GetUISlot() => UISlot;

    public bool IsUnlocked() => isUnlocked;
    
    private void Awake()
    {
        if (UISlot.TryGetComponent<UISlotManager>(out UISlotManager uiSlotManager))
        {
            uiSlotManagerData = uiSlotManager.GetUISlotManagerData();
            Destroy(uiSlotManager);
        }
    }
}
