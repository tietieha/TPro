﻿using UnityEngine;

namespace TEngine
{
    public sealed partial class DebuggerModule : Module
    {
        private sealed class InputGyroscopeInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
#if GAME_DEBUG
                GUILayout.Label("<b>Input Gyroscope Information</b>");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Enable", GUILayout.Height(30f)))
                        {
                            Input.gyro.enabled = true;
                        }
                        if (GUILayout.Button("Disable", GUILayout.Height(30f)))
                        {
                            Input.gyro.enabled = false;
                        }
                    }
                    GUILayout.EndHorizontal();

                    DrawItem("Enabled", Input.gyro.enabled.ToString());
                    if (Input.gyro.enabled)
                    {
                        DrawItem("Update Interval", Input.gyro.updateInterval.ToString());
                        DrawItem("Attitude", Input.gyro.attitude.eulerAngles.ToString());
                        DrawItem("Gravity", Input.gyro.gravity.ToString());
                        DrawItem("Rotation Rate", Input.gyro.rotationRate.ToString());
                        DrawItem("Rotation Rate Unbiased", Input.gyro.rotationRateUnbiased.ToString());
                        DrawItem("User Acceleration", Input.gyro.userAcceleration.ToString());
                    }
                }
                GUILayout.EndVertical();
#endif
            }
        }
    }
}
