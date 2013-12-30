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

    public class LaunchCountDown : PartModule
    {
        private List<ClipSource> clipsource_list = new List<ClipSource>();
        private Dictionary<string, AudioClip> dict_clip_samples = new Dictionary<string, AudioClip>();
        private Dictionary<AudioClip, string> dict_clip_samples2 = new Dictionary<AudioClip, string>();

        
        // Audio files folders
        private string dir_apollo11_countdown = "LaunchCountDown/Sounds/Apollo_11/CountDown/";
        //private string dir_apollo11 = "LaunchCountDown/Sounds/Apollo_11/Events/Apollo_11_Aborted";

        // Time managers
        //private float countDownStarted;
        //private float countDownDuration;
        //private float countDownFineTune;

        /// <summary>
        /// Called during the Part startup.
        /// StartState gives flag values of initial state
        /// </summary>
        public override void OnStart(PartModule.StartState state)
        {
            if (state == StartState.Editor || state == StartState.None) return; // Don't play sounds in the editor view.

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
                //AbortLaunchSequence();
            }
        }

        // Editor description
        public override string GetInfo()
        {
            return "Launch CountDown by Athlonic Electronics™";
        }
        
        // Add part action group in the editor
        [KSPAction("Start Countdown")]
        public void CountDownAction(KSPActionParam param)
        {
            //if (LaunchUI._launchSequenceIsActive == false)
            {
                StartLaunchSequence();
            }
        }

        // Add part action group in the editor
        [KSPAction("Abort Launch !")]
        public void AbortLaunchAction(KSPActionParam param)
        {
            //if (LaunchUI._launchSequenceIsActive == true)
            {
                //AbortLaunchSequence();
                

                foreach (ClipSource clip in clipsource_list)
                {
                    clip.audiosource.Play();
                    
                    Debug.Log("[LCD]: Start CountDown ... playing : " + clip.ToString());
                }
                
            }
        }

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

                foreach (string clip in dict_clip_samples.Keys)
                {
                    Debug.Log("[LCD]: Clip :" + clip + " in collection");
                    AudioClip _audioclip = new AudioClip();

                    //if (dict_clip_samples.TryGetValue(clip, out _audioclip))
                    //{
                        
                    //}
                }

             }
        }

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
                    Debug.Log("[LCD] Default AudioClip set :: current_clip = " + s);
                }
            }
        }



        public void StartLaunchSequence()
        {
            if (LaunchUI._audioSet == 0)
            {
                int k = 0;
                
                for (k = clipsource_list.Count - 1; k >= 0; k--)
                {

                    clipsource_list[k].clip_player.audio.PlayDelayed(1.0f);

                    Debug.Log("[LCD]: Start CountDown ... playing : " + k);
                    
                    //if (k + 1 >= clipsource_list.Count || clipsource_list[k + 1].clip_player.audio.isPlaying == false)
                    //{

                    //    clipsource_list[k].clip_player.audio.Play();

                    //    Debug.Log("[LCD]: Start CountDown ... playing : " + k);

                    //}
                    //else
                    //{
                        
                    //}
                    
                }


            }
            

            LaunchUI._buttonPushed2 = false;
            LaunchUI._launchSequenceIsActive = true;

            //countDownStarted = Time.time;
            //StartCoroutine(StartCountDown());
        }

        //public void AbortLaunchSequence()
        //{
        //    if (LaunchUI._audioSet == 0)
        //    {
        //        LaunchAbortedKerbalizedFx.audio.Play();
        //    }
        //    else if (LaunchUI._audioSet == 1)
        //    {
        //        LaunchAbortedApolloFx.audio.Play();
        //    }
        //    else
        //    {
        //        LaunchAbortedFx.audio.Play();
        //    }

        //    countdownKerbalizedFx.audio.Stop();
        //    countdownApolloFx.audio.Stop();
        //    countdownFx.audio.Stop();

        //    LaunchUI._buttonPushed = false;
        //    LaunchUI._launchSequenceIsActive = false;
            
        //    StopAllCoroutines();
        //    ScreenMessages.PostScreenMessage("LAUNCH ABORTED !!!", 6, ScreenMessageStyle.UPPER_CENTER);
        //}
    }
}