using EnumUtilities;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanAirdrop.SharedLib
{
    public class XboxController
    {
        public static ControllerState JoyState = ControllerState.None;

        public static string RefreshButtonPressed()
        {
            var helperText = "";

            Joystick.Update();

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.A))
            {
                SetJoystikFlag(ControllerState.A_PRESSED);
                helperText = "Button Pressed: A";
            }
            else
                ClearJoystikFlag(ControllerState.A_PRESSED);

            // Check for buttons
            if (Joystick.IsButtonPressed(0, (int)AvailableButton.B))
            {
                SetJoystikFlag(ControllerState.B_PRESSED);
                helperText = "Button Pressed: B";
            }
            else
                ClearJoystikFlag(ControllerState.B_PRESSED);


            // Check for buttons
            if (Joystick.IsButtonPressed(0, (int)AvailableButton.X))
            {
                SetJoystikFlag(ControllerState.X_PRESSED);
                helperText = "Button Pressed: X";
            }
            else
                ClearJoystikFlag(ControllerState.X_PRESSED);

            // Check for buttons
            if (Joystick.IsButtonPressed(0, (int)AvailableButton.Y))
            {
                SetJoystikFlag(ControllerState.Y_PRESSED);
                helperText = "Button Pressed: Y";
            }
            else
                ClearJoystikFlag(ControllerState.Y_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.R_BUTT))
            {
                SetJoystikFlag(ControllerState.R_PRESSED);
                helperText = "Button Pressed: R_BUTT";
            }
            else
                ClearJoystikFlag(ControllerState.R_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.L_BUTT))
            {
                SetJoystikFlag(ControllerState.L_PRESSED);
                helperText = "Button Pressed: L_BUTT";
            }
            else
                ClearJoystikFlag(ControllerState.L_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.BACK))
            {
                SetJoystikFlag(ControllerState.BACK_PRESSED);
                helperText = "Button Pressed: BACK";
            }
            else
                ClearJoystikFlag(ControllerState.BACK_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.START))
            {
                SetJoystikFlag(ControllerState.START_PRESSED);
                helperText = "Button Pressed: START";
            }
            else
                ClearJoystikFlag(ControllerState.START_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.LEFT_THUMB_DOWN))
            {
                SetJoystikFlag(ControllerState.LEFT_THUMB_DOWN_PRESSED);
                helperText = "Button Pressed: LEFT_THUMB_DOWN";
            }
            else
                ClearJoystikFlag(ControllerState.LEFT_THUMB_DOWN_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.RIGHT_THUMB_DOWN))
            {
                SetJoystikFlag(ControllerState.RIGHT_THUMB_DOWN_PRESSED);
                helperText = "Button Pressed: RIGHT_THUMB_DOWN";
            }
            else
                ClearJoystikFlag(ControllerState.RIGHT_THUMB_DOWN_PRESSED);

            return helperText;
        }


        public static string RefreshAxisPressed()
        {
            var helperText = "";

            Joystick.Update();

            var joyX = Joystick.GetAxisPosition(0, Joystick.Axis.PovX);
            var joyY = Joystick.GetAxisPosition(0, Joystick.Axis.PovY);

            if (joyX == 0 && joyY == 0)
            {
                helperText = "no directional button pressed";

                ClearJoystikFlag(ControllerState.DPAD_LEFT_PRESSED);
                ClearJoystikFlag(ControllerState.DPAD_RIGHT_PRESSED);
                ClearJoystikFlag(ControllerState.DPAD_DOWN_PRESSED);
                ClearJoystikFlag(ControllerState.DPAD_UP_PRESSED);

                return helperText; // no button pressed!
            }

            if (joyY == -100)
            {
                SetJoystikFlag(ControllerState.DPAD_LEFT_PRESSED);
                helperText = string.Format("Axis Pressed: DPAD_LEFT {0}, {1}", joyX, joyY);
            }
            else
                ClearJoystikFlag(ControllerState.DPAD_LEFT_PRESSED);

            if (joyY == 100)
            {
                SetJoystikFlag(ControllerState.DPAD_RIGHT_PRESSED);
                helperText = string.Format("Axis Pressed: DPAD_RIGHT {0}, {1}", joyX, joyY);
            }
            else
                ClearJoystikFlag(ControllerState.DPAD_RIGHT_PRESSED);

            if (joyX == -100 || joyX == -70)
            {
                SetJoystikFlag(ControllerState.DPAD_DOWN_PRESSED);
                helperText = string.Format("Axis Pressed: DPAD_DOWN {0}, {1}", joyX, joyY);
            }
            else
                ClearJoystikFlag(ControllerState.DPAD_DOWN_PRESSED);

            if (joyX == 100)
            {
                SetJoystikFlag(ControllerState.DPAD_UP_PRESSED);
                helperText = string.Format("Axis Pressed: DPAD_UP {0}, {1}", joyX, joyY);
            }
            else
                ClearJoystikFlag(ControllerState.DPAD_UP_PRESSED);

            return helperText;
        }

        private static void SetJoystikFlag(ControllerState state)
        {
            JoyState = EnumUtil.SetFlag(JoyState, state);
        }

        private static void ClearJoystikFlag(ControllerState state)
        {
            JoyState = EnumUtil.UnsetFlag(JoyState, state);
        }



        enum AvailableButton
        {
            A                = 0,
            B                = 1,
            X                = 2,
            Y                = 3,
            L_BUTT           = 4,
            R_BUTT           = 5,
            BACK             = 6,
            START            = 7,
            LEFT_THUMB_DOWN  = 8,
            RIGHT_THUMB_DOWN = 9,
            DPAD_UP          = 20,
            DPAD_DOWN        = 21,
            DPAD_LEFT        = 22,
            DPAD_RIGHT       = 23
        };

        [Flags]
        public enum ControllerState
        {
            None                     = 0,
            A_PRESSED                = 1,
            B_PRESSED                = 2,
            X_PRESSED                = 4,
            Y_PRESSED                = 8,
            L_PRESSED                = 16,
            R_PRESSED                = 32,
            BACK_PRESSED             = 64,
            START_PRESSED            = 128,
            LEFT_THUMB_DOWN_PRESSED  = 256,
            RIGHT_THUMB_DOWN_PRESSED = 512,
            DPAD_UP_PRESSED          = 1024,
            DPAD_DOWN_PRESSED        = 2048,
            DPAD_LEFT_PRESSED        = 4096,
            DPAD_RIGHT_PRESSED       = 8192
        }
    }
}
