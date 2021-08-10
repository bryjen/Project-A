using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        if (gameData.isRemovalMode)
        {
            StartNewCoroutine(ChangeColor(new Color(0.43f, 0f, 0.03f, 0.25f), .5f));
            return;
        }
        
        if (currentSentinel != null)
            return;
        
        StartNewCoroutine(ChangeColor(new Color(1, 1, 1, .5f), .5f));

        if (gameData.selectedSentinelPreview is null)
            return;
        sentinelPreview =
            (GameObject) Instantiate(gameData.selectedSentinelPreview, transform.position, Quaternion.identity);
        sentinelPreview.GetComponent<SortingGroup>().sortingOrder = GetOrderInSortingLayer();
    }

    private void OnMouseExit()
    {
        if (gameData.isRemovalMode)
            StartNewCoroutine(ChangeColor(new Color(0.43f, 0f, 0.03f, 0f), .5f));
        else
            StartNewCoroutine(ChangeColor(new Color(1, 1, 1, 0), .5f));
        
        if (sentinelPreview is null) 
            return;
        Destroy(sentinelPreview);
    }

    private void OnMouseDown()
    {
        if (gameData.isRemovalMode && IsRemovable())
            Remove();
        
        if (gameData.selectedSentinel is null)
            return;

        if (gameData.GetEnergyCostOfSelectedPrefab() > gameData.GetEnergy())    //if the energy cost of the sentinel is greater than the current energy
        {
            //todo display like an error message saying not enough energy yk
            return;
        }
        
        if (sentinelPreview != null)
            Destroy(sentinelPreview);

        var offset = gameData.selectedSentinel.GetComponent<EntityBehavior>().PlacementOffset;
        var spawnPosition = (Vector2) transform.position + offset;
        
        StartNewCoroutine(ChangeColor(new Color(1, 1, 1, 0), .5f));
        currentSentinel = (GameObject) Instantiate(gameData.selectedSentinel, spawnPosition, Quaternion.identity);
        currentSentinel.GetComponent<SortingGroup>().sortingOrder = GetOrderInSortingLayer();
        
        //UISlotManager.DeselectAll();
    }

    private bool IsRemovable()
        => currentSentinel != null;

    private void Remove()
    {
        Destroy(currentSentinel.gameObject);
        currentSentinel = null;
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

    private int GetOrderInSortingLayer()
    {
        var row = Convert.ToInt32(gameObject.name.Substring(0, 1));
        return row * 10;
    }
}
