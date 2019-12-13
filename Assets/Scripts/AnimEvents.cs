using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// アニメーションイベントを受け取って、
/// 設定されているイベントを実行します。
/// </summary>
public class AnimEvents : MonoBehaviour
{
    [Tooltip("呼び出した時に実行するイベント"), SerializeField]
    UnityEvent onCallFromAnimation;

    public void Proc()
    {
        onCallFromAnimation?.Invoke();
    }
}
