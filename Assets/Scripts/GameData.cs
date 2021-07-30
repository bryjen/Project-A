using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData _instance;
    public static GameData Instance { get { return _instance;  } }
    
    [HideInInspector] public GameObject selectedSentinel { get; set; }
    [HideInInspector] public GameObject selectedSentinelPreview { get; set; }
    [SerializeField] private TextMeshProUGUI textMeshPro;
    
    private int energy;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        energy = 125124809;
        textMeshPro.text = energy.ToString();
    }

    public int GetEnergy() => energy;

    public int GetEnergyCostOfSelectedPrefab()
    {
        var sentinelBehavior = selectedSentinel.GetComponent<SentinelBehavior>();
        return sentinelBehavior.GetEnergyCost();
    }

    public void ChangeEnergy(int newValue)
    {
        energy = newValue;
        textMeshPro.text = energy.ToString();
    }
}
