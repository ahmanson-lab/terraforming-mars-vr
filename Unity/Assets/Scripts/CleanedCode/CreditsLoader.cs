using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opertoon.Stepwise;

/** This script automatically loads the credits 
 * into the credit panel. There is no player interaction 
 * supported in this script! */
public class CreditsLoader : MonoBehaviour
{
    private Conductor _creditsConductor;
    [SerializeField] private GameObject _creditsStepwise;

    private void HandleScorePrepared(Score score)
    {
        Debug.Log("score prepared:" + score);
        int n = score.sequences.Length;
        for (int i = 0; i < n; i++)
        {
            score.sequences[i].repeat = false;
        }

    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        _creditsConductor.NextStep();
    }

    void Start()
    {
        _creditsConductor = _creditsStepwise.GetComponent<Conductor>();
        _creditsConductor.OnScorePrepared += HandleScorePrepared;

        StartCoroutine(AutoStart());
    }
}
