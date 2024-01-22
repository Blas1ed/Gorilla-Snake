using GorillaLocomotion.Gameplay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;

namespace Gorilla_Snake.SnakeUtils
{
    public static class Main
    {
        public static GameObject SnakeHead = new GameObject();
        public static void SetupAssets()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Gorilla_Snake.Resources.snakearcade");
            Plugin.MainBundle = AssetBundle.LoadFromStream(stream);
            Plugin.RedMat = Plugin.MainBundle.LoadAsset<Material>("Red");
            Plugin.BlueMat = Plugin.MainBundle.LoadAsset<Material>("Blue");
            Plugin.ClaimSound = Plugin.MainBundle.LoadAsset<AudioClip>("ClaimFoodSoundFX");
            SetupSnakeArcade();
        }

        public static void SetupSnakeArcade()
        {
            GameObject ArcadeObj = GameObject.Instantiate(Plugin.MainBundle.LoadAsset("SnakeArcadeModel") as GameObject);
            Plugin.AudioSpot = ArcadeObj.FindInParent("Audio").GetComponent<AudioSource>();
            GorillaSurfaceOverride gso = ArcadeObj.FindInParent("ArcadeBase").gameObject.AddComponent<GorillaSurfaceOverride>();
            gso.overrideIndex = 0;
            Plugin.ActiveArcade = ArcadeObj;
            GorillaSurfaceOverride gso2 = ArcadeObj.FindInParent("BackPanel").gameObject.AddComponent<GorillaSurfaceOverride>();
            gso2.overrideIndex = 0;
            SnakeHead = Plugin.ActiveArcade.FindInParent("Snake Base").gameObject;
            Plugin.Version = Plugin.ActiveArcade.FindInParent("SnakeVersion").GetComponent<TextMeshProUGUI>();
            Plugin.Version.text = "Gorilla Snake V" + PluginInfo.Version;
            SnakeManager m = Plugin.ActiveArcade.AddComponent<SnakeManager>();
            m.GameStart = Plugin.ActiveArcade.FindInParent("GameStart").gameObject;
            m.GamePause = Plugin.ActiveArcade.FindInParent("GamePaused").gameObject;
            m.SnakeParent = Plugin.ActiveArcade.FindInParent("Snake").gameObject;
            m.SnakeHead = Plugin.ActiveArcade.FindInParent("Snake Base").gameObject;
            m.SpawnableFood.Add(Plugin.ActiveArcade.FindInParent("Apple").gameObject);
            m.SpawnableFood.Add(Plugin.ActiveArcade.FindInParent("Orange").gameObject);
            Plugin.Highscore = Plugin.ActiveArcade.FindInParent("Highscore").gameObject.GetComponent<TextMeshProUGUI>();
            Plugin.roundScore = Plugin.ActiveArcade.FindInParent("RoundScore").gameObject.GetComponent<TextMeshPro>();
            foreach (Transform child in Plugin.ActiveArcade.FindInParent("Spwnposes"))
            {
                m.spawnPos.Add(child);
            }

            Plugin.EffectObjects.Add(ArcadeObj.FindInParent("Rocket Right").gameObject);
            Plugin.EffectObjects.Add(ArcadeObj.FindInParent("Rocket Left").gameObject);
            snakeController c = m.SnakeHead.AddComponent<snakeController>();
            c.speed = 18f;
            c.snakeManager = m;
            c.CollisionSize = 0.01f;
            c.movementInterval = 0.075f;
            c.segprefab = Plugin.ActiveArcade.FindInParent("Snake Seg (Obsticle)");

            SetupButtons();
        }

        public static void SetupButtons()
        {
            List<GameObject> buttons = new List<GameObject>()
            {
                Plugin.ActiveArcade.FindInParent("Left").gameObject,
                Plugin.ActiveArcade.FindInParent("Right").gameObject,
                Plugin.ActiveArcade.FindInParent("Up").gameObject,
                Plugin.ActiveArcade.FindInParent("Down").gameObject,
                Plugin.ActiveArcade.FindInParent("Start").gameObject,
                Plugin.ActiveArcade.FindInParent("Pause").gameObject,
            };

            foreach (GameObject button in buttons) 
            {
                LayerMask layer = LayerMask.NameToLayer("Gorilla Tag Collider");

                button.layer = layer;

                snakeButton btn = button.AddComponent<snakeButton>();

                if (btn.name == "Left" || btn.name == "Right" || btn.name == "Up" || btn.name == "Down")
                {
                    btn.Function = "Arrow";

                    btn.ArrowKey = btn.name;
                }
                else
                {
                    btn.Function = btn.name;
                }
            }
        }

        public static Transform FindInParent(this GameObject Parent, string ChildName)
        {
            List<Transform> Children = new List<Transform>();
            FindAllChildrenRecursive(Parent.transform, ref Children);
            foreach (Transform child in Children)
            {
                if (child.name == ChildName)
                {
                    return child;
                }
            }

            return null;
        }

        public static void FindAllChildrenRecursive(Transform parent, ref List<Transform> childrenList)
        {
            foreach (Transform child in parent)
            {
                childrenList.Add(child);
                FindAllChildrenRecursive(child, ref childrenList);
            }
        }
    }
}
