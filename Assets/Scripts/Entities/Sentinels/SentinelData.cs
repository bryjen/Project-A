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

    public GameObject GetPreview() => preview;

    public GameObject GetUISlot() => UISlot;

    public bool IsUnlocked() => isUnlocked;
}
