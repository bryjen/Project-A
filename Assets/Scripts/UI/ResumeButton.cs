using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResumeButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Canvases")] 
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject pauseCanvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        ExitPauseMode();
    }

    private void ExitPauseMode()
    {
        Time.timeScale = 1;

        mainCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }
}
