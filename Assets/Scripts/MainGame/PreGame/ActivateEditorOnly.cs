using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEditorOnly : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    
#if UNITY_EDITOR

    private void Awake()
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }
    }

#endif
}
