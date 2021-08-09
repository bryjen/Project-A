using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public void Initialize(Image image, Color newColor, float duration)
    {
        StartCoroutine(FadeTo(image, newColor, duration));
    }

    private IEnumerator FadeTo(Image image, Color newColor, float duration)
    {
        var t = 0f;

        while (t < 1)
        {
            image.color = Color.Lerp(image.color, newColor, t);

            t += Time.deltaTime / duration;
            yield return null;
        }
        
        Destroy(this);
    }
}
