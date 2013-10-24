// Kerbal Space Program Launcher countdown plug-in by Athlonic
// Licensed under CC BY 3.0 terms: http://creativecommons.org/licenses/by-nc-sa/3.0/
// v 1.3


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LaunchCountDown.Extensions;
using UnityEngine;

namespace LaunchCountDown
{
    class LaunchCountDown : PartModule
    {
        public FXGroup countdownApolloFx = null; // Make sure this is public so it can be initialised internally.
        public FXGroup LaunchAbortedApolloFx = null;
        public FXGroup countdownFx = null;
        public FXGroup LaunchAbortedFx = null;
        private float countDownStarted;
        private float countDownDuration = 39.0f;

        /// <summary>
        /// Called during the Part startup.
        /// StartState gives flag values of initial state
        /// </summary>
        public override void OnStart(PartModule.StartState state)
        {
            if (state == StartState.Editor || state == StartState.None) return; // Don't play sounds in the editor view.

            //GameSettings.SHIP_VOLUME = 0.25f;
                        
            countdownApolloFx.audio = gameObject.AddComponent<AudioSource>();
            countdownApolloFx.audio.volume = GameSettings.VOICE_VOLUME;
            countdownApolloFx.audio.panLevel = 0;
            countdownApolloFx.audio.Stop();
            // Be sure not to include the file extension of the sound here.
            // Unity can play WAV and OGG, possibly some others, but not MP3.
            countdownApolloFx.audio.clip = GameDatabase.Instance.GetAudioClip("LaunchCountDown/Sounds/Apollo_11_CountDown");
            countdownApolloFx.audio.loop = false;

            LaunchAbortedApolloFx.audio = gameObject.AddComponent<AudioSource>();
            LaunchAbortedApolloFx.audio.volume = GameSettings.VOICE_VOLUME;
            LaunchAbortedApolloFx.audio.panLevel = 0;
            LaunchAbortedApolloFx.audio.Stop();
            LaunchAbortedApolloFx.audio.clip = GameDatabase.Instance.GetAudioClip("LaunchCountDown/Sounds/Apollo_13_WeHaveAProblem");
            LaunchAbortedApolloFx.audio.loop = false;

            countdownFx.audio = gameObject.AddComponent<AudioSource>();
            countdownFx.audio.volume = GameSettings.VOICE_VOLUME;
            countdownFx.audio.panLevel = 0;
            countdownFx.audio.Stop();
            countdownFx.audio.clip = GameDatabase.Instance.GetAudioClip("LaunchCountDown/Sounds/LaunchCountDown");
            countdownFx.audio.loop = false;

            LaunchAbortedFx.audio = gameObject.AddComponent<AudioSource>();
            LaunchAbortedFx.audio.volume = GameSettings.VOICE_VOLUME;
            LaunchAbortedFx.audio.panLevel = 0;
            LaunchAbortedFx.audio.Stop();
            LaunchAbortedFx.audio.clip = GameDatabase.Instance.GetAudioClip("LaunchCountDown/Sounds/LaunchAborted");
            LaunchAbortedFx.audio.loop = false;

            base.OnStart(state); // Allow OnStart to do what it usually does.
        }

        public override void OnUpdate()
        {
            if (LaunchUI._buttonPushed == true && LaunchUI._launchSequenceIsActive == false)
            {
                StartLaunchSequence();
            }

            if (LaunchUI._buttonPushed2 == true && LaunchUI._launchSequenceIsActive == true)
            {
                AbortLaunchSequence();
            }
        }

        // Editor description
        public override string GetInfo()
        {
            return "Launch CountDown by Athlonic Electronics™";
        }

        private IEnumerator<WaitForSeconds> StartCountDown()
        {
            ScreenMessages.PostScreenMessage("Launch in : ", countDownDuration, ScreenMessageStyle.UPPER_CENTER);
                        
            while ((Time.time - countDownStarted) < countDownDuration)
            {
                float remaining = (countDownDuration - 2.0f) - (Time.time - countDownStarted);

                if (remaining > 0)
                {
                    ScreenMessages.PostScreenMessage(remaining.ToString("#0"), 1.0f, ScreenMessageStyle.UPPER_CENTER);
                }
                else
                {
                    ScreenMessages.PostScreenMessage("", 1.0f, ScreenMessageStyle.UPPER_CENTER);
                }

                yield return new WaitForSeconds(1.0f);
            }

            ScreenMessages.PostScreenMessage("LIFTOFF !", 2.0f, ScreenMessageStyle.UPPER_CENTER);
            Staging.ActivateNextStage();
        }

        // Add part action group in the editor
        [KSPAction("Start Countdown")]
        public void CountDownAction(KSPActionParam param)
        {
            if (LaunchUI._launchSequenceIsActive == false)
            {
                StartLaunchSequence();
            }
        }

        // Add part action group in the editor
        [KSPAction("Abort Launch !")]
        public void AbortLaunchAction(KSPActionParam param)
        {
            if (LaunchUI._launchSequenceIsActive == true)
            {
                AbortLaunchSequence();
            }
        }

        public void StartLaunchSequence()
        {
            if (LaunchUI._audioSet == 0)
            {
                countDownDuration = 39.0f;
                countdownApolloFx.audio.Play();
            }
            else
            {
                countDownDuration = 17.0f;
                countdownFx.audio.Play();
            }
            
            LaunchAbortedApolloFx.audio.Stop();
            LaunchAbortedFx.audio.Stop();

            LaunchUI._buttonPushed2 = false;
            LaunchUI._launchSequenceIsActive = true;
            
            countDownStarted = Time.time;
            StartCoroutine(StartCountDown());
        }

        public void AbortLaunchSequence()
        {
            if (LaunchUI._audioSet == 0)
            {
                LaunchAbortedApolloFx.audio.Play();
            }
            else
            {
                LaunchAbortedFx.audio.Play();
            }

            countdownApolloFx.audio.Stop();
            countdownFx.audio.Stop();

            LaunchUI._buttonPushed = false;
            LaunchUI._launchSequenceIsActive = false;
            
            StopAllCoroutines();
            ScreenMessages.PostScreenMessage("LAUNCH ABORTED !!!", 6, ScreenMessageStyle.UPPER_CENTER);
        }
    }
}