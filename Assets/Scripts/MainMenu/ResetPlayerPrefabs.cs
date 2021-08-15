using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefabs : MonoBehaviour
{
    private void Awake()
        => PlayerPrefs.SetString("canvas", "");
    
}
