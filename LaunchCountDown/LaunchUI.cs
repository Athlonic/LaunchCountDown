using UnityEngine;
using LaunchCountDown.Extensions;
using KSP.IO;

namespace LaunchCountDown
{
    public class LaunchUI : PartModule
    {
        private static Rect _windowsPosition = new Rect();
        private GUIStyle _windowStyle, _buttonStyle;
        private bool _hasInitStyles = false;
        public static bool _buttonPushed = false;
        public static bool _buttonPushed2 = false;
        public static bool _launchSequenceIsActive = false;

        public override void OnStart(PartModule.StartState state)
        {
            if (state != StartState.Editor)
            {
                if (!_hasInitStyles) InitStyles();
                _launchSequenceIsActive = false;
                _buttonPushed = false;
                _buttonPushed2 = false;
                RenderingManager.AddToPostDrawQueue(0, OnDraw);
            }
        }

        public override void OnSave(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<LaunchUI>();

            config.SetValue("Window Position", _windowsPosition);
            config.save();
        }

        public override void OnLoad(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<LaunchUI>();

            config.load();
            _windowsPosition = config.GetValue<Rect>("Window Position");
        }

        private void OnDraw()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            {
                _windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindow, "Launch Control", _windowStyle);

                if (_windowsPosition.x == 0f && _windowsPosition.y == 0f || _windowsPosition.x > Screen.width || _windowsPosition.y > Screen.height)
                    _windowsPosition = _windowsPosition.CenterScreen();
            }
        }

        private void OnDraw2()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            {
                _windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindow2, "Launch Control", _windowStyle);

                if (_windowsPosition.x == 0f && _windowsPosition.y == 0f || _windowsPosition.x > Screen.width || _windowsPosition.y > Screen.height)
                    _windowsPosition = _windowsPosition.CenterScreen();
            }
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Go Flight !", _buttonStyle))
            {
                if (_launchSequenceIsActive == false)
                    onButtonPush();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0f, 0f, _windowStyle.fixedWidth, 30f));
        }

        private void OnWindow2(int windowId)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Abort Launch !", _buttonStyle))
            {
                if (_launchSequenceIsActive == true)
                    onButtonPush2();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0f, 0f, _windowStyle.fixedWidth, 30f));
        }

        private void InitStyles()
        {
            _windowStyle = new GUIStyle(HighLogic.Skin.window);
            _windowStyle.fixedWidth = 128f;

            _buttonStyle = new GUIStyle(HighLogic.Skin.button);
            _buttonStyle.stretchWidth = true;

            _hasInitStyles = true;
        }

        private void onButtonPush()
        {
            RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw));
            _buttonPushed = true;
            RenderingManager.AddToPostDrawQueue(0, OnDraw2);
        }

        private void onButtonPush2()
        {
            RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw2));
            _buttonPushed2 = true;
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }
    }
}
