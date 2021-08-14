using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private WaveManager waveManager;
    public List<Group> groups { get; private set; }

    void Awake()
    {
        groups = new List<Group>();
        
        foreach (Transform child in transform)
        {
            if (!child.gameObject.TryGetComponent<Group>(out Group _group))
                throw new Exception($"The gameobject {child.name} does not contain a Group script!");
            
            groups.Add(_group);
        }
        
        waveManager = transform.parent.GetComponent<WaveManager>();
    }

    

    public void StartNextGroup()
    {
        if (groups.Count == 0)
        {
            waveManager.StartNextWave();
            return;
        }

        if (groups.Count == 1)
        {
            StartCoroutine(DisplayWaveMessage());
            return;
        }
        
        groups[0].StartGroup();
        groups.RemoveAt(0);
    }

    private IEnumerator DisplayWaveMessage()
    {
        var waveText = waveManager.GetWaveText();
        waveText.SetActive(true);

        var animator = waveText
            .GetComponent<Animator>();
        
        animator.Play("Start");
        yield return new WaitForSeconds(2f);
        animator.Play("Exit");
        yield return new WaitForSeconds(1f);

        if (waveManager.waves.Count == 0)
        {
            waveText.GetComponent<TextMeshProUGUI>()
                .text = "Final Wave!";
            animator.Play("Start");
            yield return new WaitForSeconds(2f);
            animator.Play("Exit");
            yield return new WaitForSeconds(1f);
        }
        
        waveText.SetActive(false);
        groups[0].StartGroup();
        groups.RemoveAt(0);
    }
}
