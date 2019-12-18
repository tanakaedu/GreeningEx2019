using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Tooltip("プレイヤーの画面左端"), SerializeField]
    float viewPointMin = 0.2f;
    [Tooltip("プレイヤーの画面右端"), SerializeField]
    float viewPointMax = 0.6f;

    Transform playerTransform = null;
    Vector3 camToPlayer;

    /// <summary>
    /// ターゲットを設定
    /// </summary>
    /// <param name="tg">目的のオブジェクトのトランスフォーム</param>
    public void SetTarget(Transform tg)
    {
        playerTransform = tg;
        camToPlayer = playerTransform.position - transform.position;
    }

    private void Awake()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go == null)
        {
            return;
        }
        SetTarget(go.transform);
    }

    private void LateUpdate()
    {
        if (!playerTransform) return;

        Vector3 next = playerTransform.position - camToPlayer;
        transform.position = next;
    }
}
