using BepInEx;
using BepInEx.Configuration;
using GameNetcodeStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomRadarSounds
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<int> Audio;
        Plugin() => Begin();

        void Begin()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Audio = Config.Bind("Settings", "AudioClip", 0, "Pick what number clip you would like to use (Rejoin a lobby to apply)");
        }
    }
    public class AudioThing : MonoBehaviour
    {
        public static AudioThing thing;
        string AudioPath = Path.Combine(Paths.PluginPath, PluginInfo.Name.ToString(), "Audio");
        List<string> audioNames = new List<string>();
        AudioClip audio;
        public void Getaudio()
        {
            if (!Directory.Exists(AudioPath))
            {
                Directory.CreateDirectory(AudioPath);
            }
            string[] audio = Directory.GetFiles(AudioPath);
            string[] audioname = new string[audio.Length];
            for (int i = 0; i < audioname.Length; i++)
            {
                audioname[i] = Path.GetFileName(audio[i]);
                audioNames.Add(audioname[i]);
            }
            StartCoroutine(LoadAudioCoroutine());
        }
        IEnumerator LoadAudioCoroutine()
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(Path.Combine(AudioPath , audioNames[Plugin.Audio.Value]), AudioType.UNKNOWN))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                    audio = audioClip;
                }
                else
                {
                    Debug.LogError("Failed to load audio: " + www.error);
                }
            }
        }
        void Update()
        {
            if (audio != null)
            {
                HUDManager.Instance.scanSFX = audio;
                Destroy(this);
            }
        }
    }
}