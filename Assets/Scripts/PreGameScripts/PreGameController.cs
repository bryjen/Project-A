using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreGameController : MonoBehaviour
{
    [SerializeField] private GameObject slotSelectionPanel;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject countdownText;
    [SerializeField] private GameObject infoPanel;
    
    private static PreGameController _instance;
    public static PreGameController Instance { get { return _instance;  } }
    
    private CameraMovementController cameraMovementController;
    private AnimationCurve defaultEasingCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    private static List<GameObject> slotInstances;
    private TextMeshProUGUI textMeshProUgui;

    public List<GameObject> GetSlotInstances() => slotInstances;

    public void StartGame() => StartCoroutine(StartGameCoroutine());

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        slotInstances = new List<GameObject>();
        
        cameraMovementController = GetComponent<CameraMovementController>();
        textMeshProUgui = countdownText.GetComponent<TextMeshProUGUI>();

        StartCoroutine(FirstCameraMovement());
    }
    
    private IEnumerator StartGameCoroutine()
    {
        MoveSelectionPanel(new Vector3(730, 1750, 0));
        MoveContinueButton(new Vector3(1737, -83, 0));
        StartCoroutine(cameraMovementController.GetSecondMovementCoroutine());
        
        UISlotManager.ClearSlotManagers();
        foreach (var slot in slotInstances)
        {
            slot.GetComponent<UISlotManager>().AddToSlotManagers();

            var preGameSelectedUiSlot = slot.GetComponent<PreGameSelectedUISlot>();
            preGameSelectedUiSlot.GetPreGameUiSlotManager().StopColorChange();
            Destroy(preGameSelectedUiSlot);
        }
        UISlotManager.DisableAllSlots();

        
        yield return StartCoroutine(StartCountdown());
        
        infoPanel.SetActive(true);
        UISlotManager.EnableAllSlots();
        foreach (var slot in slotInstances)
        {
            slot.GetComponent<UISlotManager>().enabled = true;
        }

        Destroy(this.gameObject);
    }

    private IEnumerator FirstCameraMovement()
    {
        yield return new WaitForSeconds(cameraMovementController.GetInitialDelay());

        yield return StartCoroutine(
            cameraMovementController.GetFirstMovementCoroutine());
        
        MoveSelectionPanel(new Vector3(730, 510, 0));
        MoveContinueButton(new Vector3(1737, 83, 0));
    }

    private IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(2.5f);
        countdownText.SetActive(true);
        
        for (int i = 0; i < 3; i++)
        {
            textMeshProUgui.text = (3 - i).ToString();
            yield return new WaitForSeconds(1);
        }

        textMeshProUgui.text = "Start!";
        yield return new WaitForSeconds(1);
        
        countdownText.SetActive(false);
    }

    private void MoveSelectionPanel(Vector3 destination)
    {
        if (slotSelectionPanel.TryGetComponent<MoveTowardsDestination>(out MoveTowardsDestination moveTowardsDestination))
            Destroy(moveTowardsDestination);
        
        slotSelectionPanel.AddComponent<MoveTowardsDestination>()
            .Initialize(defaultEasingCurve, slotSelectionPanel.GetComponent<RectTransform>(), destination, 10);
    }

    private void MoveContinueButton(Vector3 destination)
    {
        if (continueButton.TryGetComponent<MoveTowardsDestination>(out MoveTowardsDestination moveTowardsDestination))
            Destroy(moveTowardsDestination);
        
        continueButton.AddComponent<MoveTowardsDestination>()
            .Initialize(defaultEasingCurve, continueButton.GetComponent<RectTransform>(), destination, 10);
    }
}
