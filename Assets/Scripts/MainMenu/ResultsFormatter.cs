using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsFormatter : MonoBehaviour
{
    [Header("Text Components")] 
    [SerializeField] private TextMeshProUGUI outcomeText;
    [SerializeField] private TextMeshProUGUI timeElapsed;
    [SerializeField] private TextMeshProUGUI sentinelsPlaced;
    [SerializeField] private TextMeshProUGUI sentinelsKilled;
    [SerializeField] private TextMeshProUGUI sentinelsRemoved;
    [SerializeField] private TextMeshProUGUI energyProduced;
    [SerializeField] private TextMeshProUGUI energyCollected;

    [Header("Debug Options")] 
    [SerializeField, Tooltip("When starting from the scene itself (no data)")] private bool skipFormatting;
    
    private void Awake()
    {
        Time.timeScale = 1;

        if (!skipFormatting)
        {
            var gameData = GameData.Instance;

            outcomeText.text = $"Status: {(gameData.hasWon ? "Won" : "Lost")}";
            timeElapsed.text = $"Time Elapsed: {String.Format("{0:00.0}", gameData.timeElapsed)}";
            sentinelsPlaced.text = $"Sentinels Placed: {gameData.sentinelsPlaced}";
            sentinelsKilled.text = $"Sentinels Killed: {gameData.sentinelsKilled}";
            sentinelsRemoved.text = $"Sentinels Removed: {gameData.sentinelsRemoved}";
            energyProduced.text = $"Energy Produced: {gameData.energyProduced}";
            energyCollected.text = $"Energy Collected: {gameData.energyCollected}";
        }
        
        StartCoroutine(FadeInformationIn());
    }

    private IEnumerator FadeInformationIn()
    {
        timeElapsed.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
        
        sentinelsPlaced.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
        
        sentinelsKilled.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
        
        sentinelsRemoved.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
        
        energyProduced.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
        
        energyCollected.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
    }
}
