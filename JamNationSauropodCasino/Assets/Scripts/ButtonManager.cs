using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ButtonManager
    {
        public static bool GetButtonDown(Button b, Player p)
        {
            if (p == Player.Any)
            {
                switch (b)
                {
                    case Button.A :
                        return Input.GetButtonDown("ButtonA_All");
                    case Button.B:
                        return Input.GetButtonDown("ButtonB_All");
                    case Button.X:
                        return Input.GetButtonDown("ButtonX_All");
                    case Button.Y:
                        return Input.GetButtonDown("ButtonY_All");
                    case Button.L1:
                        return Input.GetButtonDown("ButtonL_All");
                    case Button.R1:
                        return Input.GetButtonDown("ButtonR_All");
                    case Button.R2:
                        return R2Pressed("TriggerZAxis_All");
                    case Button.L2:
                        return L2Pressed("TriggerZAxis_All");
                    case Button.Up:
                        return UpPressed("DPadAxisY_All");
                    case Button.Down:
                        return DownPressed("DPadAxisY_All");
                    case Button.Left:
                        return LeftPressed("DPadAxisX_All");
                    case Button.Right:
                        return RightPressed("DPadAxisX_All");
                    default :
                        return false;
                }
            }
            else if (p == Player.P1)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonDown("ButtonA_P1");
                    case Button.B:
                        return Input.GetButtonDown("ButtonB_P1");
                    case Button.X:
                        return Input.GetButtonDown("ButtonX_P1");
                    case Button.Y:
                        return Input.GetButtonDown("ButtonY_P1");
                    case Button.L1:
                        return Input.GetButtonDown("ButtonL_P1");
                    case Button.R1:
                        return Input.GetButtonDown("ButtonR_P1");
                    case Button.R2:
                        return R2Pressed("TriggerZAxis_P1");
                    case Button.L2:
                        return L2Pressed("TriggerZAxis_P1");
                    case Button.Up:
                        return UpPressed("DPadAxisY_P1");
                    case Button.Down:
                        return DownPressed("DPadAxisY_P1");
                    case Button.Left:
                        return LeftPressed("DPadAxisX_P1");
                    case Button.Right:
                        return RightPressed("DPadAxisX_P1");
                    default:
                        return false;
                }
            }
            else if (p == Player.P2)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonDown("ButtonA_P2");
                    case Button.B:
                        return Input.GetButtonDown("ButtonB_P2");
                    case Button.X:
                        return Input.GetButtonDown("ButtonX_P2");
                    case Button.Y:
                        return Input.GetButtonDown("ButtonY_P2");
                    case Button.L1:
                        return Input.GetButtonDown("ButtonL_P2");
                    case Button.R1:
                        return Input.GetButtonDown("ButtonR_P2");
                    case Button.R2:
                        return R2Pressed("TriggerZAxis_P2");
                    case Button.L2:
                        return L2Pressed("TriggerZAxis_P2");
                    case Button.Up:
                        return UpPressed("DPadAxisY_P2");
                    case Button.Down:
                        return DownPressed("DPadAxisY_P2");
                    case Button.Left:
                        return LeftPressed("DPadAxisX_P2");
                    case Button.Right:
                        return RightPressed("DPadAxisX_P2");
                    default:
                        return false;
                }
            }
            else if (p == Player.P3)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonDown("ButtonA_P3");
                    case Button.B:
                        return Input.GetButtonDown("ButtonB_P3");
                    case Button.X:
                        return Input.GetButtonDown("ButtonX_P3");
                    case Button.Y:
                        return Input.GetButtonDown("ButtonY_P3");
                    case Button.L1:
                        return Input.GetButtonDown("ButtonL_P3");
                    case Button.R1:
                        return Input.GetButtonDown("ButtonR_P3");
                    case Button.R2:
                        return R2Pressed("TriggerZAxis_P3");
                    case Button.L2:
                        return L2Pressed("TriggerZAxis_P3");
                    case Button.Up:
                        return UpPressed("DPadAxisY_P3");
                    case Button.Down:
                        return DownPressed("DPadAxisY_P3");
                    case Button.Left:
                        return LeftPressed("DPadAxisX_P3");
                    case Button.Right:
                        return RightPressed("DPadAxisX_P3");
                    default:
                        return false;
                }
            }
            else if (p == Player.P4)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonDown("ButtonA_P4");
                    case Button.B:
                        return Input.GetButtonDown("ButtonB_P4");
                    case Button.X:
                        return Input.GetButtonDown("ButtonX_P4");
                    case Button.Y:
                        return Input.GetButtonDown("ButtonY_P4");
                    case Button.L1:
                        return Input.GetButtonDown("ButtonL_P4");
                    case Button.R1:
                        return Input.GetButtonDown("ButtonR_P4");
                    case Button.R2:
                        return R2Pressed("TriggerZAxis_P4");
                    case Button.L2:
                        return L2Pressed("TriggerZAxis_P4");
                    case Button.Up:
                        return UpPressed("DPadAxisY_P4");
                    case Button.Down:
                        return DownPressed("DPadAxisY_P4");
                    case Button.Left:
                        return LeftPressed("DPadAxisX_P4");
                    case Button.Right:
                        return RightPressed("DPadAxisX_P4");
                    default:
                        return false;
                }
            }
            return false;
        }

        public static bool GetButtonUp(Button b, Player p)
        {
            if (p == Player.Any)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonUp("ButtonA_All");
                    case Button.B:
                        return Input.GetButtonUp("ButtonB_All");
                    case Button.X:
                        return Input.GetButtonUp("ButtonX_All");
                    case Button.Y:
                        return Input.GetButtonUp("ButtonY_All");
                    case Button.L1:
                        return Input.GetButtonUp("ButtonL_All");
                    case Button.R1:
                        return Input.GetButtonUp("ButtonR_All");
                    case Button.R2:
                        return !R2Pressed("TriggerZAxis_All");
                    case Button.L2:
                        return !L2Pressed("TriggerZAxis_All");
                    case Button.Up:
                        return !UpPressed("DPadAxisY_All");
                    case Button.Down:
                        return !DownPressed("DPadAxisY_All");
                    case Button.Left:
                        return !LeftPressed("DPadAxisX_All");
                    case Button.Right:
                        return !RightPressed("DPadAxisX_All");
                    default:
                        return false;
                }
            }
            else if (p == Player.P1)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonUp("ButtonA_P1");
                    case Button.B:
                        return Input.GetButtonUp("ButtonB_P1");
                    case Button.X:
                        return Input.GetButtonUp("ButtonX_P1");
                    case Button.Y:
                        return Input.GetButtonUp("ButtonY_P1");
                    case Button.L1:
                        return Input.GetButtonUp("ButtonL_P1");
                    case Button.R1:
                        return Input.GetButtonUp("ButtonR_P1");
                    case Button.R2:
                        return !R2Pressed("TriggerZAxis_P1");
                    case Button.L2:
                        return !L2Pressed("TriggerZAxis_P1");
                    case Button.Up:
                        return !UpPressed("DPadAxisY_P1");
                    case Button.Down:
                        return !DownPressed("DPadAxisY_P1");
                    case Button.Left:
                        return !LeftPressed("DPadAxisX_P1");
                    case Button.Right:
                        return !RightPressed("DPadAxisX_P1");
                    default:
                        return false;
                }
            }
            else if (p == Player.P2)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonUp("ButtonA_P2");
                    case Button.B:
                        return Input.GetButtonUp("ButtonB_P2");
                    case Button.X:
                        return Input.GetButtonUp("ButtonX_P2");
                    case Button.Y:
                        return Input.GetButtonUp("ButtonY_P2");
                    case Button.L1:
                        return Input.GetButtonUp("ButtonL_P2");
                    case Button.R1:
                        return Input.GetButtonUp("ButtonR_P2");
                    case Button.R2:
                        return !R2Pressed("TriggerZAxis_P2");
                    case Button.L2:
                        return !L2Pressed("TriggerZAxis_P2");
                    case Button.Up:
                        return !UpPressed("DPadAxisY_P2");
                    case Button.Down:
                        return !DownPressed("DPadAxisY_P2");
                    case Button.Left:
                        return !LeftPressed("DPadAxisX_P2");
                    case Button.Right:
                        return !RightPressed("DPadAxisX_P2");
                    default:
                        return false;
                }
            }
            else if (p == Player.P3)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonUp("ButtonA_P3");
                    case Button.B:
                        return Input.GetButtonUp("ButtonB_P3");
                    case Button.X:
                        return Input.GetButtonUp("ButtonX_P3");
                    case Button.Y:
                        return Input.GetButtonUp("ButtonY_P3");
                    case Button.L1:
                        return Input.GetButtonUp("ButtonL_P3");
                    case Button.R1:
                        return Input.GetButtonUp("ButtonR_P3");
                    case Button.R2:
                        return !R2Pressed("TriggerZAxis_P3");
                    case Button.L2:
                        return !L2Pressed("TriggerZAxis_P3");
                    case Button.Up:
                        return !UpPressed("DPadAxisY_P3");
                    case Button.Down:
                        return !DownPressed("DPadAxisY_P3");
                    case Button.Left:
                        return !LeftPressed("DPadAxisX_P3");
                    case Button.Right:
                        return !RightPressed("DPadAxisX_P3");
                    default:
                        return false;
                }
            }
            else if (p == Player.P4)
            {
                switch (b)
                {
                    case Button.A:
                        return Input.GetButtonUp("ButtonA_P4");
                    case Button.B:
                        return Input.GetButtonUp("ButtonB_P4");
                    case Button.X:
                        return Input.GetButtonUp("ButtonX_P4");
                    case Button.Y:
                        return Input.GetButtonUp("ButtonY_P4");
                    case Button.L1:
                        return Input.GetButtonUp("ButtonL_P4");
                    case Button.R1:
                        return Input.GetButtonUp("ButtonR_P4");
                    case Button.R2:
                        return !R2Pressed("TriggerZAxis_P4");
                    case Button.L2:
                        return !L2Pressed("TriggerZAxis_P4");
                    case Button.Up:
                        return !UpPressed("DPadAxisY_P4");
                    case Button.Down:
                        return !DownPressed("DPadAxisY_P4");
                    case Button.Left:
                        return !LeftPressed("DPadAxisX_P4");
                    case Button.Right:
                        return !RightPressed("DPadAxisX_P4");
                    default:
                        return false;
                }
            }
            return false;
        }

        private static bool R2Pressed(string axisName) { return Input.GetAxis(axisName) <= -0.8; }
        private static bool L2Pressed(string axisName) { return Input.GetAxis(axisName) >= 0.8; }
        private static bool UpPressed(string axisName) { return Input.GetAxis(axisName) >= 0.8; }
        private static bool DownPressed(string axisName) { return Input.GetAxis(axisName) <= -0.8; }
        private static bool LeftPressed(string axisName) { return Input.GetAxis(axisName) <= -0.8; }
        private static bool RightPressed(string axisName) { return Input.GetAxis(axisName) >= 0.8; }
    }

    public enum Button
    {
        A,
        B,
        X,
        Y,
        L1,
        R1,
        L2,
        R2,
        Up,
        Down,
        Left,
        Right,
    }

    public enum Player
    {
        Any,
        P1,
        P2,
        P3,
        P4,
    }
}
