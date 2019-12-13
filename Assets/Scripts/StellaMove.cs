using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellaMove : MonoBehaviour
{
    [Tooltip("移動速度(秒速)"), SerializeField]
    float moveSpeed = 3f;
    [Tooltip("重力加速度(速度/秒)"), SerializeField]
    float gravityAdd = 20f;
    [Tooltip("ステラの横向きの角度"), SerializeField]
    float rotateY = 40f;

    /// <summary>
    /// アニメのStateに設定する値
    /// </summary>
    public enum AnimType
    {
        Start,
        Walk,
        Jump
    }

    CharacterController chrController = null;
    Vector3 myVelocity = Vector3.zero;
    Animator anim;

    void Awake()
    {
        chrController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        anim.SetInteger("State", (int)AnimType.Walk);
    }

    void FixedUpdate()
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

        // 落下
        if (chrController.isGrounded)
        {
            myVelocity.y = 0f;
            anim.SetInteger("State", (int)AnimType.Walk);
        }
        else
        {
            anim.SetInteger("State", (int)AnimType.Jump);
        }
        myVelocity.y += -gravityAdd * Time.fixedDeltaTime;

        anim.SetFloat("VelX", Mathf.Abs(myVelocity.x));
        anim.SetFloat("VelY", myVelocity.y);

        chrController.Move(myVelocity * Time.fixedDeltaTime);
    }
}
