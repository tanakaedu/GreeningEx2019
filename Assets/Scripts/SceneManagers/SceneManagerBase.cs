using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreeningEx2019
{
    /// <summary>
    /// シーンを制御するためのマネージャークラスの親クラス。
    /// 読み込み時に自らをリストに登録して、初期化が完了したかを
    /// 判断できるような構造を持つ。
    /// </summary>
    public class SceneManagerBase : MonoBehaviour
    {
        /// <summary>
        /// Awakeを継承する際は、最後にbase.Awake()を呼び出すこと。
        /// </summary>
        public void Awake()
        {
#if UNITY_EDITOR
            if (SceneChanger.IsBooting) return;
#endif
            SceneChanger.AwakeDone(this);
        }

        /// <summary>
        /// 全てのシーンのAwakeが完了した後に処理したい初期化処理。
        /// フェードアウト中に呼び出されます。
        /// </summary>
        public virtual void OnFadeOutDone()
        {
        }

        /// <summary>
        /// ゲームの開始など、フェードインが完了した時に処理したい処理を実行します。
        /// </summary>
        public virtual void OnFadeInDone()
        {
        }

#if DEBUG
        private void OnGUI()
        {
            GUI.Label(new Rect(20, 20, 100, 30), SceneChanger.NowScene.ToString());
        }
#endif

    }
}
