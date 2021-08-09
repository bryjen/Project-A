using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemoveButton : MonoBehaviour, IPointerClickHandler
{
    private GameData gameData;
    private ChangeColor changeColor;
    
    private void Start()
    {
        gameData = GameData.Instance;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        gameData.isRemovalMode = !gameData.isRemovalMode;

        ChangeColor();
        
        UISlotManager.DeselectAll();
    }

    public void Deselect()
    {
        gameData.isRemovalMode = !gameData.isRemovalMode;
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (changeColor != null)
            changeColor.StopAllCoroutines();
        changeColor = this.gameObject.AddComponent<ChangeColor>();
        changeColor.Initialize(GetComponent<Image>(),
            gameData.isRemovalMode ? new Color(0.43f, 0f, 0.03f) : new Color(0.745283f, 0.745283f, 0.6496618f, 1), .25f);
    }
}
