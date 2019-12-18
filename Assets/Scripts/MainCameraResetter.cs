using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreeningEx2019
{
    public class MainCameraResetter : MonoBehaviour
    {
        private void OnDestroy()
        {
            SceneChanger.SetSubCamera();
        }
    }
}