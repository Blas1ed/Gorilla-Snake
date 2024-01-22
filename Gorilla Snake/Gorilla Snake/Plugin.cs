using BepInEx;
using Gorilla_Snake.SnakeUtils;
using System;
using TMPro;
using UnityEngine;
using Utilla;
using ComputerPlusPlus;
using System.Collections.Generic;

namespace Gorilla_Snake
{
   
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInDependency("com.kylethescientist.gorillatag.computerplusplus", "1.0.1")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        public static AssetBundle MainBundle;
        public static GameObject ActiveArcade;
        public static TextMeshProUGUI Version;
        public static Material RedMat;
        public static Material BlueMat;
        public static TextMeshProUGUI Highscore;
        public static TextMeshPro roundScore;
        public static AudioClip ClaimSound;
        public static List<GameObject> EffectObjects = new List<GameObject>();
        public static AudioSource AudioSpot = new AudioSource();

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            if (ActiveArcade != null)
            {
            ActiveArcade.SetActive(true);
            }

        }

        void OnDisable()
        {
            SnakeManager.Main.ResetGame();
            SnakeManager.Main.UnPauseGame();
            SnakeManager.Main.CurrentGameState = GameStates.StartScreen;
            SnakeManager.Main.LastSpawnPos = -1;
            ActiveArcade.SetActive(false);
        }

        public static void KeyFunc(string Key, bool Arrow = false)
        {
            snakeController c = snakeController.Main;
            SnakeManager m = SnakeManager.Main;

            if (Arrow)
            {
            if (c.direction.x != 0f)
            {
                if (Key == "Up")
                {
                    c.direction = Vector3.up;
                }
                else if (Key == "Down")
                {
                    c.direction = Vector3.down;
                }
            }

            else if (c.direction.y != 0f)
            {
                if (Key == "Right")
                {
                    c.direction = Vector3.right;
                }
                else if (Key == "Left")
                {
                    c.direction = Vector3.left;
                }
            }
            }

            if (Key == "Start")
            {
                if (m.CurrentGameState != GameStates.Started)
                {
                m.StartGame();
                }
            }

            if (Key == "Pause")
            {
                if (m.CurrentGameState == GameStates.Started)
                {
                m.PauseGame();
                }
                else if (m.CurrentGameState == GameStates.Paused)
                {
                m.UnPauseGame();
                }
            }
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Main.SetupAssets();
            var Value = Config.Bind("EnabledMods", PluginInfo.Name, true).Value;

            if (!Value)
            {
                SnakeManager.Main.ResetGame();
                SnakeManager.Main.UnPauseGame();
                SnakeManager.Main.CurrentGameState = GameStates.StartScreen;
                SnakeManager.Main.LastSpawnPos = -1;
                ActiveArcade.SetActive(false);
            }
        }

        void Update()
        {
        }
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
        }
    }
}
