using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellaWater : MonoBehaviour
{
    [Tooltip("水プレハブ"),SerializeField]
    GameObject waterPrefab = null;

    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mpos = Input.mousePosition;
            mpos.z = 0f - mainCamera.transform.position.z;

            Vector3 wpos = mainCamera.ScreenToWorldPoint(mpos);

            Instantiate(waterPrefab, wpos, Quaternion.identity);
        }
    }
}
