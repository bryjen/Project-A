using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Canvases")] 
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject pauseCanvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        EnterPauseMode();
    }

    private void EnterPauseMode()
    {
        Time.timeScale = 0;
        
        mainCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }
}
