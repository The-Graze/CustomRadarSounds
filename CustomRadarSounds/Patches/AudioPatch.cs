using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomRadarSounds.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class AudioPatch
    {
        private static void Postfix(HUDManager __instance)
        {
            AudioThing.thing = __instance.gameObject.AddComponent<AudioThing>();
            AudioThing.thing.Getaudio();
        }
    }
}
