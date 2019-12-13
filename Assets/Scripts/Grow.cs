using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物(苗)の親クラス。
/// 水がかかったら、アニメのGrowをトリガーして、
/// アニメが完了した時のパラメーターを受け取る。
/// </summary>
public class Grow : MonoBehaviour
{
    /// <summary>
    /// 汎用の状態
    /// </summary>
    public enum StateType
    {
        Nae,
        Growing,
        Growed,
    }

    /// <summary>
    /// 現在の状態を表します。
    /// </summary>
    public StateType state = StateType.Nae;

    Animator anim;

    private void Awake()
    {
        state = StateType.Nae;
        anim = GetComponent<Animator>();
        if (!anim)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((state == StateType.Nae) && other.CompareTag("Water"))
        {
            state = StateType.Growing;
            anim.SetTrigger("Grow");
        }
    }

    /// <summary>
    /// アニメーションの最後のフレームのイベントから
    /// 呼び出します。
    /// </summary>
    public void GrowDone()
    {
        state = StateType.Growed;
    }
}


