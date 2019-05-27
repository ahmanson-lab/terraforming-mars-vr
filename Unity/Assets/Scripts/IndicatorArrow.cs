using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorArrow : MonoBehaviour
{
    [SerializeField] private Transform _arrow;
    [SerializeField] private float _arrowMoveDistance = 0.1f;
    [SerializeField] private float _arrowMoveSpeed = 6;
    
    // Update is called once per frame
    void Update()
    {
        _arrow.localPosition = new Vector3(0,Mathf.Sin(_arrowMoveSpeed * Time.time) * _arrowMoveDistance, 0);
    }
}
