using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Gorilla_Snake.SnakeUtils
{
    public class snakeButton : MonoBehaviour
    {
        public string Function = "None";
        public string ArrowKey = "Left";
        public InputDevice leftController, rightController;

        void Start()
        {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "RightHandTriggerCollider" || other.gameObject.name == "LeftHandTriggerCollider")
            {

                if (SnakeManager.Main.CurrentGameState == GameStates.Started)
                {
                if (Function.Contains("Arrow"))
                {
                    Plugin.KeyFunc(ArrowKey, true);
                }
                else
                    {
                      if (Function != "Start")
                        {
                            Plugin.KeyFunc(Function, false);
                        }
                    }
                }
                else
                {
                    if (Function != "Arrow")
                    {
                    Plugin.KeyFunc(Function, false);
                    }

                }

                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, other.gameObject.name == "LeftHandTriggerCollider" ? true : false, 0.09f);

                var hand = other.gameObject.name == "LeftHandTriggerCollider" ? leftController : rightController;
                hand.SendHapticImpulse(0u, .5f, .05f);

                if (Function.Contains("Arrow"))
                {
                    RedButton(true);
                }
            }

        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "RightHandTriggerCollider" || other.gameObject.name == "LeftHandTriggerCollider")
            {
                if (Function.Contains("Arrow"))
                {
                    RedButton(false);
                }
            }
        }

        public void RedButton(bool on)
        {
            switch (on) 
            {
                case true:
                    gameObject.GetComponent<Renderer>().material = Plugin.RedMat; 
                    break;
                case false:
                    gameObject.GetComponent<Renderer>().material = Plugin.BlueMat;
                    break;
            }
        }
    }


}
