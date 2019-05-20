using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotHskElevator2Console : MonoBehaviour
{

    [HideInInspector]
    public DotHskElevator2 elevatorBase = null;

    // thisFloor: -9 - unattached console (default), -1 - platform console, 0..floors.Count() - corresponding floor 
    [HideInInspector]
    public int thisFloor = -9;

    private bool touch;
    private bool allowCall = true;

    void Update()
    {
        int _floor;
        if (touch && (thisFloor >= -1))
        {
            if (elevatorBase != null)
            {
                if (thisFloor < 0)
                {
                    // This is platfom console
                    if ((_floor = getInputFloor(elevatorBase.hKeys)) > -1)
                    {
                        if (elevatorBase.call(_floor) && !elevatorBase.multiCallMode)
                        {
                            allowCall = false;
                        };
                    }
                }
                else
                {
                    // This is floor console
                    if (Input.GetKey(KeyCode.E))
                    {
                        elevatorBase.call(thisFloor);
                    }
                }
            }
        }
    }

    public void OnStop()
    {
        allowCall = true;
    }

    public void OnConsoleEnter(Collider other)
    {
        touch = true;
        if ((elevatorBase != null) && (elevatorBase.elevatorEvents != null))
        {
            elevatorBase.elevatorEvents.OnActivateConsole(thisFloor);
        };
    }

    public void OnConsoleExit(Collider other)
    {
        touch = false;
        if ((elevatorBase != null) && (elevatorBase.elevatorEvents != null))
        {
            elevatorBase.elevatorEvents.OnDeactivateConsole(thisFloor);
        };
    }

    void OnGUI()
    {
        if (allowCall && touch && (thisFloor >= -1))
        {
            Texture2D tip = (thisFloor < 0) ? elevatorBase.enterFloorTip : elevatorBase.callElevatorTip;
            if (tip == null) { return; }
            float _tw = tip.width;
            float _th = tip.height;
            GUI.DrawTexture(new Rect((Screen.width - _tw) / 2, Screen.height - 36 - _th, _tw, _th), tip, ScaleMode.ScaleToFit, true);
        }
    }

    private int getInputFloor(Dictionary<int, int> hkeys)
    {
        int result = -1;
        if ((hkeys == null) || (hkeys.Count < 1)) { return result; }
        int sign = (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) ? -1 : 1;
        int zeroCode = (int)KeyCode.Alpha0;
        foreach (KeyValuePair<int, int> kv in hkeys)
        {
            if (kv.Key * sign < 0) { continue; }
            if (Input.GetKeyDown((KeyCode)(kv.Key * sign + zeroCode)))
            {
                result = kv.Value; break;
            };
        }
        return result;
    }


}
