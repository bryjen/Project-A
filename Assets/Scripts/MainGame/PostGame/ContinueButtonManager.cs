using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButtonManager : MonoBehaviour
{
    [SerializeField] private Scene scene;

    public void OnContinueButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
        PlayerPrefs.SetString("canvas", "results");
    }
}
