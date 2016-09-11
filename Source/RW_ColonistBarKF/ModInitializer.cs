﻿using System.IO;
using RimWorld;
using RW_ColonistBarKF;
using UnityEngine;
using Verse;

namespace ColonistBarKF
{
    public class ModInitializer : ITab
    {
        protected GameObject modInitializerControllerObject;

        public ModInitializer()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                modInitializerControllerObject = new GameObject("Colonist Bar KF");
                modInitializerControllerObject.AddComponent<CBKF>();
                Object.DontDestroyOnLoad(modInitializerControllerObject);
                Log.Message("Colonist Bar KF Initialized");
            });
        }

        protected override void FillTab() { }
    }

    internal class CBKF : MonoBehaviour
    {
        public static SettingsColBar ColSettingsColBar = new SettingsColBar();
        public static SettingsPSI SettingsPsi= new SettingsPSI();

        public static SettingsColBar LoadBarSettings(string path = "ColonistBarKF.xml")
        {
            var configFolder = Path.GetDirectoryName(GenFilePaths.ModsConfigFilePath);
            SettingsColBar result = XmlLoader.ItemFromXmlFile<SettingsColBar>(configFolder + "/" + path);
            return result;
        }
        public static void SaveBarSettings(string path = "ColonistBarKF.xml")
        {
            if (ColSettingsColBar.UseGender)
                ColonistBarTextures.BGTex = ColonistBarTextures.BGTexGrey;
            else
            {
                ColonistBarTextures.BGTex = ColonistBarTextures.BGTexVanilla;
            }
            var configFolder = Path.GetDirectoryName(GenFilePaths.ModsConfigFilePath);
            XmlSaver.SaveDataObject(ColSettingsColBar, configFolder + "/" + path);
        }
        public static SettingsPSI LoadPsiSettings(string path = "ColonistBarPSIKF.xml")
        {
            var configFolder = Path.GetDirectoryName(GenFilePaths.ModsConfigFilePath);
            SettingsPSI result = XmlLoader.ItemFromXmlFile<SettingsPSI>(configFolder + "/" + path);
            return result;
        }
        public static void SavePsiSettings(string path = "ColonistBarPSIKF.xml")
        {
            var configFolder = Path.GetDirectoryName(GenFilePaths.ModsConfigFilePath);
            XmlSaver.SaveDataObject(SettingsPsi, configFolder + "/" + path);
        }
        private int _lastStatUpdate;

        public void FixedUpdate()
        {
            if (Current.ProgramState != ProgramState.MapPlaying)
                return;

            if (Find.TickManager.TicksGame - _lastStatUpdate > 1900)
            {
                ((UIRootMap)Find.UIRoot).colonistBar.MarkColonistsListDirty();
                _lastStatUpdate = Find.TickManager.TicksGame;
            }

        }

        public void Start()
        {
            ColSettingsColBar = LoadBarSettings();
            SettingsPsi = LoadPsiSettings();
            _lastStatUpdate = -5000;
        }
    }
}
