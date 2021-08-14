using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData _instance;
    public static GameData Instance { get { return _instance;  } }
    
    [HideInInspector] public bool isRemovalMode { get; set; }
    [HideInInspector] public GameObject selectedSentinel { get; set; }
    [HideInInspector] public GameObject selectedSentinelPreview { get; set; }
    
    [SerializeField] private TextMeshProUGUI textMeshPro;
    
    //Post game information
    [HideInInspector] public float timeElapsed;
    [HideInInspector] public bool hasWon;
    [HideInInspector] public int sentinelsPlaced;
    [HideInInspector] public int sentinelsKilled;
    [HideInInspector] public int sentinelsRemoved;
    [HideInInspector] public int energyProduced;
    [HideInInspector] public int energyCollected;
    
    private int energy;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        energy = 5000;
        textMeshPro.text = energy.ToString();
    }

    public int GetEnergy() => energy;

    private void Update()
        => timeElapsed += Time.deltaTime;

    public int GetEnergyCostOfSelectedPrefab()
    {
        var sentinelBehavior = selectedSentinel.GetComponent<EntityBehavior>();
        return (int) sentinelBehavior.EnergyCost;
    }

    public void AddEnergy(int energy)
    {
        this.energy += energy;
        energyCollected += energy;
        textMeshPro.text = energy.ToString();
    }
    
    public void SubtractEnergy(int energy)
    {
        this.energy -= energy;
        textMeshPro.text = energy.ToString();
    }
}
