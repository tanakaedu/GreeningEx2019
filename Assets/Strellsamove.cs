using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strellsamove : MonoBehaviour
{
    [Tooltip("移動速度"), SerializeField]
    float moveSpeed = 5f;

    [Tooltip("重力加速度"),SerializeField]
    float graviryAdd=9.81f;
    [Tooltip("ステラの向き"), SerializeField]
    float rotateY = 50f;
    Vector3 myVelocity = Vector3.zero;

    CharacterController chrController=null;
    Animator anim=null;

    public enum A
    {
        Steat,
        Walk,
        Jump,
    }

    // Start is called before the first frame update
    void Awake()
    {
        chrController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        anim.SetInteger("State", (int)A.Steat);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        float vx = h * moveSpeed;

        myVelocity.x = vx;
        Vector3 e = transform.eulerAngles;
        if (h < -0.5f)
        {
            e.y = rotateY;

        }
        else if (h < 0.5f)

        {
            e.y = -rotateY;

        }
            transform.eulerAngles = e;
        anim.SetFloat("VelX", Mathf.Abs(myVelocity.x));
        anim.SetFloat("VelX", myVelocity.y);



        if (chrController.isGrounded)
        {
            myVelocity.y = 0f;
            anim.SetInteger("State", (int)A.Walk);
        }
        else
        {
            anim.SetInteger("State", (int)A.Jump);
        }
        myVelocity.y -= graviryAdd * Time.fixedUnscaledDeltaTime;
        chrController.Move(myVelocity * Time.fixedUnscaledDeltaTime);
    }
}
