using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GreeningEx2019 {
    public class DebugStageNumberText : MonoBehaviour
    {
        Text debugText = null;
        string debugMessage = "";

        void Awake()
        {
#if UNITY_EDITOR
            debugText = GetComponent<Text>();
            debugMessage = debugText.text;
#else
        Destroy(gameObject);
#endif

        }

        void FixedUpdate()
        {
            debugText.text = debugMessage + " " + (GameParams.SelectedStage+1);
        }
    }
}