using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameController : MonoBehaviour
{
    [SerializeField] private GameObject slotSelectionPanel;
    
    private static PreGameController _instance;
    public static PreGameController Instance { get { return _instance;  } }
    
    private CameraMovementController cameraMovementController;
    private AnimationCurve defaultEasingCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    private static List<GameObject> occupiableSpaces;

    public List<GameObject> GetOccupiableSpaces() => occupiableSpaces;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        occupiableSpaces = new List<GameObject>();
        
        cameraMovementController = GetComponent<CameraMovementController>();

        StartCoroutine(FirstCameraMovement());
    }

    private IEnumerator FirstCameraMovement()
    {
        yield return new WaitForSeconds(cameraMovementController.GetInitialDelay());

        yield return StartCoroutine(
            cameraMovementController.GetFirstMovementCoroutine());
        
        BringDownSlotSelectionPanel();
    }

    private void BringDownSlotSelectionPanel()
    {
        slotSelectionPanel.AddComponent<MoveTowardsDestination>()
            .Initialize(defaultEasingCurve, slotSelectionPanel.GetComponent<RectTransform>(), new Vector3(730, 510, 0), 10);
    }
}
