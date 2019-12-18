using UnityEngine;

namespace GreeningEx2019
{
    [CreateAssetMenu(menuName = "Greening/Stella Actions/Create Base", fileName = "StellaAction")]
    public class StellaActionScriptableObject : ScriptableObject
    {
        /// <summary>
        /// 動作を開始する時に必要な処理があったら、overrideして実装します。
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// 更新処理です。前の処理からの経過秒数を引数に渡します。
        /// </summary>
        /// <param name="tick">経過秒数。Time.deltaTimeか、Time.fixedDeltaTimeの値</param>
        public virtual void UpdateAction(float tick) { }

        /// <summary>
        /// 終了時に必要な処理があったら、overrideして実装します。
        /// </summary>
        public virtual void End() { }

        /// <summary>
        /// 接触時の処理のうち、必要なものをoverrideします。
        /// </summary>
        /// <param name="other"></param>
        public virtual void OnTriggerEnter(Collider other) { }
        public virtual void OnTriggerStay(Collider other) { }
        public virtual void OnTriggerExit(Collider other) { }

        public virtual void OnCollisionEnter(Collision col) { }
        public virtual void OnCollisionStay(Collision col) { }
        public virtual void OnCollisionExit(Collision col) { }

    }
}