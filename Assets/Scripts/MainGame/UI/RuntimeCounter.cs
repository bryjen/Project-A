using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuntimeCounter : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private float time;
    
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        textMeshPro.text = String.Format("{0:00.0}", time);
    }
}
