using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opertoon.Stepwise;

public class InstructionsMenuInteraction : MainMenuRawInteraction
{

    [SerializeField] private GameObject _stepwiseInstructionMenu;
    [SerializeField] private GameObject _instructionMenuCanvas;
    [SerializeField] private GameObject _bButtonOverlay;

    private bool _instructionMenuActive;
    private float _timeLeft;
    private string _selectedTag;

    private Conductor _instructionMenuConductor;

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        _instructionMenuConductor.NextStep();
    }

    private void HandleScorePrepared(Score score)
    {
        Debug.Log("score prepared:" + score);
        int n = score.sequences.Length;
        for (int i = 0; i < n; i++)
        {
            score.sequences[i].repeat = false;
        }

    }

    private IEnumerator DelayedResetAndNextStep()
    {
        if (_selectedTag == "InstructionMenu")
        {
            Debug.Log("DelayedResetAndNextStep: InstructionMenu");
            yield return 0;
            _instructionMenuConductor.Reset();
            yield return 0;
            _instructionMenuConductor.NextStep();
        }
    }

    void Start()
    {
        _instructionMenuConductor = _stepwiseInstructionMenu.GetComponent<Conductor>();
        _instructionMenuConductor.OnScorePrepared += HandleScorePrepared;

        _instructionMenuActive = false;
        _timeLeft = 6f;
        _selectedTag = "";
    }
    
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft < 0)
        {
            _bButtonOverlay.SetActive(false);
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {

            Debug.Log("Interaction menu raw interaction B button pressed!!");
            if (_bButtonOverlay.activeInHierarchy)
            {
                _bButtonOverlay.SetActive(false);
            }
            _instructionMenuCanvas.SetActive(!_instructionMenuActive);
            _instructionMenuActive = !_instructionMenuActive;
            if (_instructionMenuActive) StartCoroutine(AutoStart());
        }
    }

    public override void OnHoverEnter(Transform t)
    {
        _selectedTag = t.gameObject.tag;
    }

    public override void OnHoverExit(Transform t)
    {
        base.OnHoverExit(t);
    }

    public override void OnSelected(Transform t)
    {
        Debug.Log("Selected tag isssss: " + _selectedTag);
      if (_selectedTag == "InstructionMenu")
        {
            if (_instructionMenuCanvas.activeInHierarchy == false)
            {
                Debug.Log("Instruction menu not active");
                _instructionMenuCanvas.SetActive(true);
                StartCoroutine(DelayedResetAndNextStep());
            }
            else
            {
                Debug.Log("Instruction Menu panel already active: next step");
                _instructionMenuConductor.NextStep();
            }
        }
    }
}
