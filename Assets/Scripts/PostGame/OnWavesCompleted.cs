using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnWavesCompleted : MonoBehaviour
{
    [Header("UI Components")] 
    [SerializeField] private GameObject onCompletedCanvas;
    [SerializeField] private GameObject mainPanel;

    /// <summary>
    /// - Slows the timescale down to 0
    /// </summary>
    public void OnCompleted()
    {
        StartCoroutine(OnCompletedCoroutine());
    }

    private IEnumerator OnCompletedCoroutine()
    {
        StartCoroutine(StopTime());
        yield return new WaitForSecondsRealtime(1);

        onCompletedCanvas.SetActive(true);
        StartCoroutine(ChangeColor(mainPanel.GetComponent<Image>(), new Color(0, 0, 0, 0.4f), 10));
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

    private IEnumerator ChangeColor(Image image, Color newColor, float duration)
    {
        var t = 0f;

        while (t < 1)
        {
            image.color = Color.Lerp(image.color, newColor, t);

            t += Time.deltaTime / duration;
            yield return null;
        }
    }
}