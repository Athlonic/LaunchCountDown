// Kerbal Space Program Launcher countdown plug-in by Athlonic
// Licensed under CC BY 3.0 terms: http://creativecommons.org/licenses/by-nc-sa/3.0/
// v 1.6


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LaunchCountDown.Extensions;
using UnityEngine;

namespace LaunchCountDown
{
    // Class to manage audio clips
    public class ClipSource
    {
        public GameObject clip_player;
        public string clip_name;
        public AudioSource audiosource;
        public string current_clip;
    }

    // MAIN
    public class LaunchCountDown : PartModule
    {
        private List<ClipSource> clipsource_list = new List<ClipSource>();
        private Dictionary<string, AudioClip> dict_clip_samples = new Dictionary<string, AudioClip>();
        private Dictionary<AudioClip, string> dict_clip_samples2 = new Dictionary<AudioClip, string>();
        
        // Audio files folders
        private string dir_apollo11_countdown = "LaunchCountDown/Sounds/Apollo_11/CountDown/";
        //private string dir_apollo11_events = "LaunchCountDown/Sounds/Apollo_11/Events/";

        // Time managers
        private int clip_counter;
        //private float countDownStarted;
        //private float countDownDuration;
        //private float countDownFineTune;

        /// <summary>
        /// Called during the Part startup.
        /// StartState gives flag values of initial state
        /// </summary>
        public override void OnStart(PartModule.StartState state)
        {
            if (state == StartState.Editor || state == StartState.None || this.vessel.isActiveVessel == false) return; // Don't do audio thing in the editor view or if part is not on active vessel

            Debug.Log("[LCD]: OnStart ...");

            LoadAudioClips();

            SetAudioClips();
            
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

            base.OnUpdate();
        }

        // Editor LaunchClamp description
        public override string GetInfo()
        {
            return "Launch CountDown by Athlonic Electronics™";
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

        // Loading audiofiles to collections
        public void LoadAudioClips()
        {
            if (clipsource_list.Count == 0)
            {
                int file_name = 0;
                string file_path = dir_apollo11_countdown + file_name;

                //Debug.Log("[LCD]: Loading clips :" + file_name + " audio clip");
                //Debug.Log("[LCD]: Loading clips :" + (dir_apollo11_countdown + file_name) + " audio clip");

                while (GameDatabase.Instance.ExistsAudioClip(dir_apollo11_countdown + file_name) == true)
                {
                    dict_clip_samples.Add(file_name.ToString(), GameDatabase.Instance.GetAudioClip(file_path));
                    dict_clip_samples2.Add(GameDatabase.Instance.GetAudioClip(file_path), file_name.ToString());

                    file_name++;
                    file_path = dir_apollo11_countdown + file_name;
                    //Debug.Log("[LCD]: Clip Loaded, next:" + file_name + " audio clip");
                    //Debug.Log("[LCD]: Clip Loaded, next:" + file_path + " audio clip");
                }
                Debug.Log("[LCD]: All clips Loaded:" + dict_clip_samples.Count + " audio clips");

                //foreach (string clip in dict_clip_samples.Keys)
                //{
                //    Debug.Log("[LCD]: Clip :" + clip + " in collection");
                //}
             }
        }

        // Setting audiosources parameters
        public void SetAudioClips()
        {
            for (int i = dict_clip_samples.Count - 1; i >= 0; i--)
            {
                clipsource_list.Add(new ClipSource());

                int x = clipsource_list.Count - 1;
                
                clipsource_list[x].clip_player = new GameObject();
                //clipsource_list[x].clip_player.name = "rbr_beep_player_" + clipsource_list.Count;
                //clipsource_list[x].clip_name = clipsource_list.Count.ToString();
                clipsource_list[x].audiosource = clipsource_list[x].clip_player.AddComponent<AudioSource>();
                clipsource_list[x].audiosource.volume = GameSettings.VOICE_VOLUME;
                clipsource_list[x].audiosource.panLevel = 0;
                clipsource_list[x].current_clip = "Default";

                if (dict_clip_samples.Count > 0)
                {
                    set_clip_clip(clipsource_list[x]);  //set clip
                }
            }            

            Debug.Log("[LCD]: SetAudioClips :" + clipsource_list.Count + " clips in clipsource_list");
        }

        // Resgistering audioclips
        private void set_clip_clip(ClipSource clipsource)
        {
            if (clipsource.current_clip == "Default")
            {
                List<AudioClip> val_list = new List<AudioClip>();
                foreach (AudioClip val in dict_clip_samples.Values)
                {
                    val_list.Add(val);
                }
                clipsource.audiosource.clip = val_list[clipsource_list.Count - 1];
                string s = "";
                if (dict_clip_samples2.TryGetValue(clipsource.audiosource.clip, out s))
                {
                    clipsource.current_clip = s;
                    //Debug.Log("[LCD] Default AudioClip set :: current_clip = " + s);
                }
            }
        }

        // Managing launch sequence
        private IEnumerator<WaitForSeconds> StartCountDown()
        {
            while (LaunchUI._launchSequenceIsActive && clip_counter >= 0)
            {                
                if (clip_counter >= 0) // || clipsource_list[clip_counter + 1].clip_player.audio.isPlaying == false)
                {
                    ScreenMessages.PostScreenMessage("Launch in : ", 1.0f, ScreenMessageStyle.UPPER_CENTER);
                    if (clip_counter == 38 || clip_counter >= 36) ScreenMessages.PostScreenMessage("Launch sequence started...", 1.0f, ScreenMessageStyle.UPPER_CENTER);
                    else if (clip_counter == 35 || clip_counter >= 33) ScreenMessages.PostScreenMessage("Now on internal power...", 1.0f, ScreenMessageStyle.UPPER_CENTER);
                    else if (clip_counter == 17 || clip_counter == 16) ScreenMessages.PostScreenMessage("2nd stage tanks now pressurized", 1.0f, ScreenMessageStyle.UPPER_CENTER);
                    else if (clip_counter == 8 || clip_counter == 7) ScreenMessages.PostScreenMessage("Ignition sequence start", 1.0f, ScreenMessageStyle.UPPER_CENTER);
                    else ScreenMessages.PostScreenMessage(clip_counter.ToString("#0"), 1.0f, ScreenMessageStyle.UPPER_CENTER);

                    clip_counter--;
                                        
                    clipsource_list[clip_counter + 1].clip_player.audio.Play();

                    //Debug.Log("[LCD]: StartCountDown(), playing : " + (clip_counter + 1) + "audio clip.");
                }               

                yield return new WaitForSeconds(1.0f);
            }

            if (LaunchUI._launchSequenceIsActive && clip_counter < 0)
            {
                yield return new WaitForSeconds(1.0f);

                ScreenMessages.PostScreenMessage("All engines running...", 1.0f, ScreenMessageStyle.UPPER_CENTER);

                yield return new WaitForSeconds(2.0f);

                ScreenMessages.PostScreenMessage("LIFTOFF !", 2.0f, ScreenMessageStyle.UPPER_CENTER);
                Staging.ActivateNextStage();
            }            
        }

        public void StartLaunchSequence()
        {
            clip_counter = clipsource_list.Count - 1;

            LaunchUI._buttonPushed2 = false;
            LaunchUI._launchSequenceIsActive = true;

            Debug.Log("[LCD]: StartLaunchSequence, is active ? = " + LaunchUI._launchSequenceIsActive.ToString());
            Debug.Log("[LCD]: StartLaunchSequence, clip_counter = " + clip_counter);
                        
            StartCoroutine(StartCountDown());
        }

        public void AbortLaunchSequence()
        {
            LaunchUI._buttonPushed = false;
            LaunchUI._launchSequenceIsActive = false;

            foreach (ClipSource clip in clipsource_list)
            {
                if (clip.audiosource.isPlaying)
                {
                    clip.audiosource.Stop();

                    Debug.Log("[LCD]: Aborting launch sequence ... Stoping :" + clip.ToString());
                }
            }
            
            StopAllCoroutines();
            ScreenMessages.PostScreenMessage("LAUNCH ABORTED !!!", 6, ScreenMessageStyle.UPPER_CENTER);
        }
    }
}