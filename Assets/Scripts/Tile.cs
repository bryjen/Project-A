using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    private GameObject currentSentinel;    //the current sentinel on the tile
    private GameObject sentinelPreview;
    private IEnumerator colorChangerCoroutine;
    private GameData gameData;

    private void Start()
    {
        gameData = GameData.Instance;
    }

    private void OnMouseEnter()
    {
        if (currentSentinel != null)
            return;
        
        StartNewCoroutine(ChangeColor(new Color(1, 1, 1, .5f), .5f));

        if (gameData.selectedSentinelPreview is null)
            return;
        sentinelPreview =
            (GameObject) Instantiate(gameData.selectedSentinelPreview, transform.position, Quaternion.identity);
    }

    private void OnMouseExit()
    {
        if (currentSentinel != null)
            return;
        
        StartNewCoroutine(ChangeColor(new Color(1, 1, 1, 0), .5f));
        
        if (sentinelPreview is null) 
            return;
        Destroy(sentinelPreview);
    }

    private void OnMouseDown()
    {
        if (gameData.selectedSentinel is null)
            return;

        if (gameData.GetEnergyCostOfSelectedPrefab() > gameData.GetEnergy())    //if the energy cost of the sentinel is greater than the current energy
        {
            //todo display like an error message saying not enough energy yk
            return;
        }
        
        if (sentinelPreview != null)
            Destroy(sentinelPreview);

        StartNewCoroutine(ChangeColor(new Color(1, 1, 1, 0), .5f));
        currentSentinel = (GameObject) Instantiate(gameData.selectedSentinel, transform.position, Quaternion.identity);
    }

    private IEnumerator ChangeColor(Color color, float duration)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var t = 0f;

        while (t < 1)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, color, t);

            t += Time.deltaTime / duration;
            yield return null;
        }

        colorChangerCoroutine = null;
    }

    private void StartNewCoroutine(IEnumerator coroutine)
    {
        if (colorChangerCoroutine != null)
            StopCoroutine(colorChangerCoroutine);
        
        colorChangerCoroutine = coroutine;
        StartCoroutine(colorChangerCoroutine);
    }
}