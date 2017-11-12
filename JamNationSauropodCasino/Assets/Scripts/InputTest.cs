using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InputTest : MonoBehaviour
    {
        void Update()
        {
            if (ButtonManager.GetButtonDown(Button.A, Player.Any))
                Debug.Log("A down");
            if (ButtonManager.GetButtonUp(Button.A, Player.Any))
                Debug.Log("A up");
            if (ButtonManager.GetButtonDown(Button.B, Player.P1))
                Debug.Log("B P1");
            if (ButtonManager.GetButtonDown(Button.B, Player.P2))
                Debug.Log("B P2");
            if (ButtonManager.GetButtonDown(Button.R2, Player.Any))
                Debug.Log("R2 down");
            //if (ButtonManager.GetButtonUp(Button.R2, Player.Any))
            //    Debug.Log("R2 up");

            if (ButtonManager.GetButtonDown(Button.A, Player.Any))
            {
                float debugZ = Input.GetAxis("TriggerZAxis_All");
                Debug.Log("Z : " + debugZ);
                float debugX = Input.GetAxis("DPadAxisX_All");
                Debug.Log("X : " + debugX);
                float debugY = Input.GetAxis("DPadAxisY_All");
                Debug.Log("Y : " + debugY);
            }
        }
    }
}
