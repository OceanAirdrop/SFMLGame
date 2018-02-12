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
        public static CurrentButtonState JoyState = CurrentButtonState.None;

        public static string RefreshButtonPressed()
        {
            var helperText = "";

            Joystick.Update();

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.A))
            {
                SetJoystikFlag(CurrentButtonState.A_PRESSED);
                helperText = "Button Pressed: A";
            }
            else
                ClearJoystikFlag(CurrentButtonState.A_PRESSED);

            // Check for buttons
            if (Joystick.IsButtonPressed(0, (int)AvailableButton.B))
            {
                SetJoystikFlag(CurrentButtonState.B_PRESSED);
                helperText = "Button Pressed: B";
            }
            else
                ClearJoystikFlag(CurrentButtonState.B_PRESSED);


            // Check for buttons
            if (Joystick.IsButtonPressed(0, (int)AvailableButton.X))
            {
                SetJoystikFlag(CurrentButtonState.X_PRESSED);
                helperText = "Button Pressed: X";
            }
            else
                ClearJoystikFlag(CurrentButtonState.X_PRESSED);

            // Check for buttons
            if (Joystick.IsButtonPressed(0, (int)AvailableButton.Y))
            {
                SetJoystikFlag(CurrentButtonState.Y_PRESSED);
                helperText = "Button Pressed: Y";
            }
            else
                ClearJoystikFlag(CurrentButtonState.Y_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.R_BUTT))
            {
                SetJoystikFlag(CurrentButtonState.R_PRESSED);
                helperText = "Button Pressed: R_BUTT";
            }
            else
                ClearJoystikFlag(CurrentButtonState.R_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.L_BUTT))
            {
                SetJoystikFlag(CurrentButtonState.L_PRESSED);
                helperText = "Button Pressed: L_BUTT";
            }
            else
                ClearJoystikFlag(CurrentButtonState.L_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.BACK))
            {
                SetJoystikFlag(CurrentButtonState.BACK_PRESSED);
                helperText = "Button Pressed: BACK";
            }
            else
                ClearJoystikFlag(CurrentButtonState.BACK_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.START))
            {
                SetJoystikFlag(CurrentButtonState.START_PRESSED);
                helperText = "Button Pressed: START";
            }
            else
                ClearJoystikFlag(CurrentButtonState.START_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.LEFT_THUMB_DOWN))
            {
                SetJoystikFlag(CurrentButtonState.LEFT_THUMB_DOWN_PRESSED);
                helperText = "Button Pressed: LEFT_THUMB_DOWN";
            }
            else
                ClearJoystikFlag(CurrentButtonState.LEFT_THUMB_DOWN_PRESSED);

            if (Joystick.IsButtonPressed(0, (int)AvailableButton.RIGHT_THUMB_DOWN))
            {
                SetJoystikFlag(CurrentButtonState.RIGHT_THUMB_DOWN_PRESSED);
                helperText = "Button Pressed: RIGHT_THUMB_DOWN";
            }
            else
                ClearJoystikFlag(CurrentButtonState.RIGHT_THUMB_DOWN_PRESSED);

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

                ClearJoystikFlag(CurrentButtonState.DPAD_LEFT_PRESSED);
                ClearJoystikFlag(CurrentButtonState.DPAD_RIGHT_PRESSED);
                ClearJoystikFlag(CurrentButtonState.DPAD_DOWN_PRESSED);
                ClearJoystikFlag(CurrentButtonState.DPAD_UP_PRESSED);

                return helperText; // no button pressed!
            }

            if (joyY == -100)
            {
                SetJoystikFlag(CurrentButtonState.DPAD_LEFT_PRESSED);
                helperText = string.Format("Axis Pressed: DPAD_LEFT {0}, {1}", joyX, joyY);
            }
            else
                ClearJoystikFlag(CurrentButtonState.DPAD_LEFT_PRESSED);

            if (joyY == 100)
            {
                SetJoystikFlag(CurrentButtonState.DPAD_RIGHT_PRESSED);
                helperText = string.Format("Axis Pressed: DPAD_RIGHT {0}, {1}", joyX, joyY);
            }
            else
                ClearJoystikFlag(CurrentButtonState.DPAD_RIGHT_PRESSED);

            if (joyX == -100 || joyX == -70)
            {
                SetJoystikFlag(CurrentButtonState.DPAD_DOWN_PRESSED);
                helperText = string.Format("Axis Pressed: DPAD_DOWN {0}, {1}", joyX, joyY);
            }
            else
                ClearJoystikFlag(CurrentButtonState.DPAD_DOWN_PRESSED);

            if (joyX == 100)
            {
                SetJoystikFlag(CurrentButtonState.DPAD_UP_PRESSED);
                helperText = string.Format("Axis Pressed: DPAD_UP {0}, {1}", joyX, joyY);
            }
            else
                ClearJoystikFlag(CurrentButtonState.DPAD_UP_PRESSED);

            return helperText;
        }

        private static void SetJoystikFlag(CurrentButtonState state)
        {
            JoyState = EnumUtil.SetFlag(JoyState, state);
        }

        private static void ClearJoystikFlag(CurrentButtonState state)
        {
            JoyState = EnumUtil.UnsetFlag(JoyState, state);
        }



        enum AvailableButton
        {
            A = 0,
            B = 1,
            X = 2,
            Y = 3,
            L_BUTT = 4,
            R_BUTT = 5,
            BACK = 6,
            START = 7,
            LEFT_THUMB_DOWN = 8,
            RIGHT_THUMB_DOWN = 9,
            DPAD_UP = 20,
            DPAD_DOWN = 21,
            DPAD_LEFT = 22,
            DPAD_RIGHT = 23
        };

        [Flags]
        public enum CurrentButtonState
        {
            None = 0,
            A_PRESSED = 1,
            B_PRESSED = 2,
            X_PRESSED = 4,
            Y_PRESSED = 8,
            L_PRESSED = 16,
            R_PRESSED = 32,
            BACK_PRESSED = 64,
            START_PRESSED = 128,
            LEFT_THUMB_DOWN_PRESSED = 256,
            RIGHT_THUMB_DOWN_PRESSED = 512,
            DPAD_UP_PRESSED = 1024,
            DPAD_DOWN_PRESSED = 2048,
            DPAD_LEFT_PRESSED = 4096,
            DPAD_RIGHT_PRESSED = 8192
        }
    }
}
