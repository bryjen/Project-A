using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnGameCompleted : MonoBehaviour
{
    [Header("UI Components")] 
    [SerializeField] private GameObject onCompletedCanvas;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject continueButton;

    /// <summary>
    /// - Slows the timescale down to 0
    /// </summary>
    public void FinishGame(bool hasWon)
    {
        GameData.Instance.hasWon = hasWon;
        StartCoroutine(ShowEndGameMenu(hasWon));
    }
    
    private IEnumerator ShowEndGameMenu(bool hasWon)
    {
        StartCoroutine(StopTime());
        yield return new WaitForSecondsRealtime(1);
        
        onCompletedCanvas.SetActive(true);

        Tile.SetEnabled(false);   //disables all tiles from being interacted with

        if (!hasWon)
            text.GetComponent<TextMeshProUGUI>().text = "You lose!";
        
        mainPanel.GetComponent<Animator>().Play("FadeIn");
        
        if (hasWon)
            text.GetComponent<Animator>().Play("OnWin");
        else
            text.GetComponent<Animator>().Play("OnLose");
        
        yield return new WaitForSecondsRealtime(1.5f);

        continueButton.SetActive(true);
        continueButton.GetComponent<Animator>().Play("FadeIn");
    }

    private IEnumerator StopTime()
    {
        do
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0, 0.01f);

            if (Time.timeScale < 0.15)
                yield break;

            yield return new WaitForSeconds(Time.deltaTime);
        } while (true);
    }
}