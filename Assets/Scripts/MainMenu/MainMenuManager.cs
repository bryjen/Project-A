using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Results")] 
    [SerializeField] private GameObject resultsCanvas;
    [SerializeField] private GameObject infoPanel;

    [Header("Main")] 
    [SerializeField] private GameObject mainCanvas;

    private List<GameObject> canvases;
    
    private void Awake()
    {
        canvases = new List<GameObject>(
            new []{resultsCanvas, infoPanel});
        
        SetCanvas(PlayerPrefs.GetString("canvas"));
    }

    public void SetCanvas(string canvas)
    {
        canvases.ForEach(canvas => canvas.SetActive(false));
        
        switch (canvas.ToLower())
        {
            case "results":
                SetResultsCanvas();
                return;
        }
    } 
    
    public void SetCanvas(Canvas canvas)
    {
        canvases.ForEach(canvas => canvas.SetActive(false));
        
        switch (canvas)
        {
            case Canvas.RESULTS:
                SetResultsCanvas();
                return;
        }
    }

    public void SwitchToGame()
    {
        SceneManager.LoadScene("Default Scene");
    }

    private void SetResultsCanvas()
    {
        resultsCanvas.SetActive(true);
        infoPanel.SetActive(true);
    }
}

public enum Canvas
{
    RESULTS
}
