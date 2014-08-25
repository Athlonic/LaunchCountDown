using UnityEngine;
using LaunchCountDown.Extensions;
using KSP.IO;

using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LaunchCountDown
{
    public class LaunchUI : PartModule
    {
        private static Rect _windowsPosition = new Rect();
        private GUIStyle _windowStyle, _settingsWindowStyle, _buttonStyle, _labelStyle, _toggleStyle;
        private bool _hasInitStyles = false;
        private bool _GUIisSticky = false;
        private bool _GUIDocked = false;

        // ApplicationLauncher Stuff
        private ApplicationLauncherButton launcherButton;
        public static LaunchUI Instance;
        public Texture appIconLaunch;
        public Texture appIconGo;
        public Texture appIconAbort;
        public Texture appIconSettings;

        public static bool _buttonPushed = false;
        public static bool _buttonPushed2 = false;
        public static bool _buttonPushed3 = false;
        public static bool _buttonPushed4 = false;
        public static bool _launchSequenceIsActive = false;
        public static int _audioSet;
        public static string _audioSet_name = "";
        public static bool _debug;

        public override void OnStart(PartModule.StartState state)
        {
            if (state == StartState.Editor || state == StartState.None || this.vessel.isActiveVessel == false) return; // Don't do load GUI in the editor view or if part is not on active vessel.

            if (this.part.IsPrimary(this.vessel.parts, this.ClassID)) // Only load GUI for the primary part if other instances are present.
            {
                if (!_hasInitStyles) InitStyles();
                _launchSequenceIsActive = false;
                _buttonPushed = false;
                _buttonPushed2 = false;
                //RenderingManager.AddToPostDrawQueue(0, OnDraw);

                Instance = this;
                
                appIconLaunch = GameDatabase.Instance.GetTexture("LaunchCountDown/Textures/appIconLaunch", false);
                appIconGo = GameDatabase.Instance.GetTexture("LaunchCountDown/Textures/appIconGo", false);
                appIconAbort = GameDatabase.Instance.GetTexture("LaunchCountDown/Textures/appIconAbort", false);
                appIconSettings = GameDatabase.Instance.GetTexture("LaunchCountDown/Textures/appIconSettings", false);

                GameEvents.onGUIApplicationLauncherReady.Add(OnGUIApplicationLauncherReady);
                GameEvents.onGameSceneLoadRequested.Add(OnSceneChangeRequest);
            }
        }

        //void OnGameSceneLoadRequestedForAppLauncher(GameScenes SceneToLoad)
        //{
        //    RemoveLCDFromAppLauncher();
        //}

        private void OnGUIApplicationLauncherReady()
        {
            if (launcherButton == null)
            {
                launcherButton = ApplicationLauncher.Instance.AddModApplication(onAppButtonToggleOn, onAppButtonToggleOff, onAppButtonHoverOn, onAppButtonHoverOut, null, null, ApplicationLauncher.AppScenes.FLIGHT, appIconLaunch);
                //ApplicationLauncher.Instance.EnableMutuallyExclusive(launcherButton);
            }
        }

        private void onAppButtonToggleOn()
        {
            if (_launchSequenceIsActive)
            {
                onButtonPush2();
                launcherButton.SetTexture(appIconLaunch);
            }
            else
            {
                _GUIisSticky = true;
                launcherButton.SetTexture(appIconGo);
            }
        }
        private void onAppButtonToggleOff()
        {
            if (_launchSequenceIsActive)
            {
                onButtonPush2();
            }
            else onButtonPush();

            _GUIisSticky = false;
            hideLCDGUI();
            Debug.Log("[LCD]: AppLauncher : ToggleOff, Hiding GUI, GUIDocked: " + _GUIDocked);
            launcherButton.SetTexture(appIconAbort);
        }
        private void onAppButtonHoverOn()
        {
            if (_GUIisSticky == false && _GUIDocked == false && !_launchSequenceIsActive)
            {
                RenderingManager.AddToPostDrawQueue(0, new Callback(OnDraw));
                Debug.Log("[LCD]: AppLauncher : HoverOn showing GUI");
            }
        }
        private void onAppButtonHoverOut()
        {
            if (_GUIisSticky == false || _launchSequenceIsActive)
            {
                hideLCDGUI();
                _GUIDocked = false;
                Debug.Log("[LCD]: AppLauncher : HoverOut, Hiding GUI & GUIDocked: " + _GUIDocked);
            }
            Debug.Log("[LCD]: AppLauncher : HoverOut");
            Debug.Log("[LCD]: AppLauncher : HoverOut, button state :" + launcherButton.State);
        }

        private void OnSceneChangeRequest(GameScenes scene)
        {
            if (launcherButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(launcherButton);
                Debug.Log("[LCD]: AppLauncher : OnSceneChangeRequest ");
            }            
        }
        
        public override void OnSave(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<LaunchUI>();

            config.SetValue("_audioSet", _audioSet);
            //config.SetValue("Window Position", _windowsPosition);
            config.SetValue("_debug", _debug);
            config.save();
        }

        public override void OnLoad(ConfigNode node)
        {
            PluginConfiguration config = PluginConfiguration.CreateForType<LaunchUI>();

            config.load();
            //_windowsPosition = config.GetValue<Rect>("Window Position");
            _audioSet = config.GetValue("_audioSet", 0);
            _debug = config.GetValue("_debug", false);
        }

        private void OnDraw()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            {
                _windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindow, "Launch Control", _windowStyle);
                //Debug.Log("[LCD]: AppLauncher : OnDraw, ShowingGUI");

                if (_GUIDocked == false || (_windowsPosition.x == 0f && _windowsPosition.y == 0f))
                {
                    _windowsPosition = _windowsPosition.DockScreen();
                    _GUIDocked = true;
                    Debug.Log("[LCD]: AppLauncher : OnDraw, GUIDocked: " + _GUIDocked);
                }

                //if (_windowsPosition.x == 0f && _windowsPosition.y == 0f || _windowsPosition.x > Screen.width || _windowsPosition.y > Screen.height)
                //    _windowsPosition = _windowsPosition.CenterScreen();
            }
        }

        private void OnDraw2()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            {
                _windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindow2, "Launching...", _windowStyle);

                if (_windowsPosition.x == 0f && _windowsPosition.y == 0f || _windowsPosition.x > Screen.width || _windowsPosition.y > Screen.height)
                    _windowsPosition = _windowsPosition.CenterScreen();
            }
        }

        private void OnDraw3()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && this.part.IsPrimary(this.vessel.parts, this.ClassID))
            {
                _windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindow3, "Settings", _settingsWindowStyle);

                if (_windowsPosition.x == 0f && _windowsPosition.y == 0f || _windowsPosition.x > Screen.width || _windowsPosition.y > Screen.height)
                    _windowsPosition = _windowsPosition.CenterScreen();
            }
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            if (GUILayout.Button("Go Flight !", _buttonStyle))
            {
                if (_launchSequenceIsActive == false)
                    onButtonPush();
            }

            if (GUILayout.Button("settings", _buttonStyle))
            {
                if (_launchSequenceIsActive == false)
                    onSettingsPush();
            }
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0f, 0f, _windowStyle.fixedWidth, 30f));
        }

        private void OnWindow2(int windowId)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Abort Launch !", _labelStyle);
            
            //if (GUILayout.Button("Abort Launch !", _buttonStyle))
            //{
            //    if (_launchSequenceIsActive == true)
            //        onButtonPush2();
            //}
            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0f, 0f, _windowStyle.fixedWidth, 30f));
        }

        private void OnWindow3(int windowId)
        {

            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            _debug = GUILayout.Toggle(_debug, "Debug Mode", _toggleStyle);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("◄", _buttonStyle))
            {
                _audioSet--;
                if (_audioSet < 0) _audioSet = 0;
            }
            
            if (_audioSet == 0) _audioSet_name = "Kerbalish";
            if (_audioSet == 1) _audioSet_name = "Apollo";
            //if (_audioSet == 2) _audioSet_name = "English";

            GUILayout.Label(_audioSet_name, _labelStyle);

            if (GUILayout.Button("►", _buttonStyle))
            {
                _audioSet++;
                if (_audioSet > 1) _audioSet = 1;
            }
            
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Back", _buttonStyle))
            {
                onBackPush();
            }
            
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0f, 0f, _settingsWindowStyle.fixedWidth, 30f));
        }

        private void InitStyles()
        {
            _windowStyle = new GUIStyle(HighLogic.Skin.window);
            _windowStyle.fixedHeight = 96f;
            _windowStyle.fixedWidth = 128f;

            _settingsWindowStyle = new GUIStyle(HighLogic.Skin.window);
            _settingsWindowStyle.fixedHeight = 132f;
            _settingsWindowStyle.fixedWidth = 128f;

            _buttonStyle = new GUIStyle(HighLogic.Skin.button);
            _buttonStyle.stretchWidth = true;

            _labelStyle = new GUIStyle(HighLogic.Skin.label);
            _labelStyle.alignment = TextAnchor.MiddleCenter;

            _toggleStyle = new GUIStyle(HighLogic.Skin.toggle);
            
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
        private void onSettingsPush()
        {
            RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw));
            _buttonPushed3 = true;
            launcherButton.SetTexture(appIconSettings);
            RenderingManager.AddToPostDrawQueue(0, OnDraw3);
        }
        private void onBackPush()
        {
            RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw3));
            _buttonPushed4 = true;
            launcherButton.SetTexture(appIconGo);
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }

        private void hideLCDGUI()
        {
            if (_launchSequenceIsActive == false)
            {
                RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw2));
            }
            RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw));
            RenderingManager.RemoveFromPostDrawQueue(0, new Callback(OnDraw3));
        }

        public void OnDestroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIApplicationLauncherReady);
            GameEvents.onGameSceneLoadRequested.Remove(OnSceneChangeRequest);
            
            if (launcherButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(launcherButton);
            }  
        }
    }
}
