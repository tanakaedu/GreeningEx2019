using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreeningEx2019
{
    /// <summary>
    /// ステラを制御するためのスクリプト。
    /// このスクリプトには以下を実装します。
    /// <list type="bullet">
    /// <item>ステラの状態管理</item>
    /// <item>各状態のためのスクリプタブルオブジェクトの登録と呼び出し</item>
    /// <item>MonoBehaviourを制御するためのサービスメソッド</item>
    /// </list>
    /// </summary>
    public class StellaMove : MonoBehaviour
    {
        public static StellaMove instance = null;

        [Tooltip("移動速度(秒速)"), SerializeField]
        float moveSpeed = 3f;
        [Tooltip("重力加速度(速度/秒)"), SerializeField]
        float gravityAdd = 20f;
        [Tooltip("ステラの横向きの角度"), SerializeField]
        float rotateY = 40f;
        [Tooltip("ステラの各状態を制御するためのStellaActionアセット。Create > Greening > StellaActionsから、必要なものを作成"), SerializeField]
        StellaActionScriptableObject[] stellaActionScriptableObjects = null;
        [Tooltip("歩く際のデフォルトの落下距離"), SerializeField]
        float walkDownY = 0.1f;

        /// <summary>
        /// アニメのStateに設定する値
        /// </summary>
        public enum AnimType
        {
            Start,
            Walk,
            Jump,
        }

        /// <summary>
        /// ステラの行動定義
        /// </summary>
        public enum ActionType
        {
            Start,  // 開始時の演出
            Walk,   // 立ち、歩き
            Air,    // 空中。ジャンプ、落下
            Water,  // 水まき
            Nae,    // 苗運び
        }

        public static CharacterController chrController { get; private set; }
        public static Vector3 myVelocity = Vector3.zero;
        static Animator anim;
        static ActionType nowAction;

        void Awake()
        {
            instance = this;
            chrController = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
            anim.SetInteger("State", (int)AnimType.Walk);
            nowAction = ActionType.Walk;
        }

        void FixedUpdate()
        {
            if (!StageManager.CanMove) return;

            stellaActionScriptableObjects[(int)nowAction]?.UpdateAction(Time.fixedDeltaTime);
        }

        /// <summary>
        /// 設定した動きをキャラクターコントローラーに反映。
        /// </summary>
        public void Move()
        {
            // 設定された速度を反映
            anim.SetFloat("VelX", Mathf.Abs(myVelocity.x));
            anim.SetFloat("VelY", myVelocity.y);

            Vector3 move = myVelocity * Time.fixedDeltaTime;
            if (chrController.isGrounded)
            {
                move.y -= walkDownY;
            }
            chrController.Move(move);
        }

        /// <summary>
        /// 歩いたり止まったりします。
        /// </summary>
        public void Walk()
        {
            // キーの入力を調べる
            float h = Input.GetAxisRaw("Horizontal");

            // 左右の移動速度(秒速)を求める
            float vx = h * moveSpeed;

            // 動かす
            myVelocity.x = vx;

            Vector3 e = transform.eulerAngles;
            if (h < -0.5f)
            {
                e.y = rotateY;
            }
            else if (h > 0.5f)
            {
                e.y = -rotateY;
            }
            transform.eulerAngles = e;
        }

        /// <summary>
        /// 重力加速を処理します。
        /// </summary>
        public void Gravity()
        {
            // 落下
            if (chrController.isGrounded)
            {
                myVelocity.y = 0f;
            }

            myVelocity.y += -gravityAdd * Time.fixedDeltaTime;
        }

        /// <summary>
        /// アニメーターのStateを指定の状態にします。
        /// </summary>
        /// <param name="type">StellaMove.AnimTypeで指定</param>
        public void SetAnimState(AnimType type)
        {
            anim.SetInteger("State", (int)type);
        }

        /// <summary>
        /// 違う行動に切り替えます。
        /// </summary>
        /// <param name="type">StellaMove.ActionTypeで次の動作を指定します。</param>
        public void ChangeAction(ActionType type)
        {
            int idx = (int)type;
            stellaActionScriptableObjects[idx].End();
            nowAction = type;
            stellaActionScriptableObjects[idx].Init();
        }
    }
}