using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameSkipper : MonoBehaviour
{
    [SerializeField] private bool _enabled;
    [SerializeField] private GameObject[] sentinels;

    [Header("UI Options")] 
    [SerializeField]
    private GameObject parent;

#if UNITY_EDITOR

    void Start()
    {
        if (!_enabled)
            return;
        Destroy(GameObject.Find("PreGameManager"));
        var rectTransform = parent.GetComponent<RectTransform>();

        for (int i = 0; i < sentinels.Length; i++)
        {
            var slotCopy = (GameObject) Instantiate(sentinels[i].GetComponent<SentinelData>().GetUISlot(),
                new Vector3(0, 336 - (i * 130) + 540, 0),
                Quaternion.identity);
        
            //Initialize and/or destroy other components
            var slotCopyRectTransform = slotCopy.GetComponent<RectTransform>();
        
            Destroy(slotCopy.GetComponent<PreGameSelectedUISlot>());
            Destroy(slotCopy.GetComponent<PreGameUISlotManager>());
            slotCopy.GetComponent<UISlotManager>().enabled = true;

            //Set a new parent for the prefab
            slotCopy.transform.SetParent(parent.transform);

            //Set the anchor + pivot to center left
            slotCopyRectTransform.anchorMin = new Vector2(0, 0.5f);
            slotCopyRectTransform.anchorMax = new Vector2(0, 0.5f);
            slotCopyRectTransform.pivot = new Vector2(0, 0.5f);
        }
        
        //Start the EnemySpawner
        var enemySpawner = GameObject.Find("EnemySpawner");
        enemySpawner.GetComponent<WaveManager>().StartNextWave();
    }

#endif
}
