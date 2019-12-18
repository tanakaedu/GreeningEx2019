using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellaWater : MonoBehaviour
{
    // 水プレハブを受け取るための変数の定義
    [Tooltip("水のプレハブ"), SerializeField]
    GameObject waterPrefab = null;

    // メインカメラのインスタンスの記録
    Camera mainCamera;

    void Start()
    {
        // メインカメラのインスタンスを取得してキャッシュしておく
        mainCamera = Camera.main;
    }

    void Update()
    {
        // マウスがクリックされたかを判定
        if (Input.GetMouseButtonDown(0))
        {
            // マウスの座標を、Z座標が0の場所のワールド座標に変換
            Vector3 mpos = Input.mousePosition;
            mpos.z = 0f - mainCamera.transform.position.z;

            // 変換した座標に、水をInstantiateする
            Vector3 wpos = mainCamera.ScreenToWorldPoint(mpos);

            // wposの場所にwaterPrefabを生成
            Instantiate(waterPrefab, wpos, Quaternion.identity);
        }
    }
}
