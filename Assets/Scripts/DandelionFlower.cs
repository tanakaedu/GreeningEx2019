using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionFlower : MonoBehaviour
{
    Animator animator = null;
    int state = 0;
    [SerializeField]
    Vector3 fluffOffset = new Vector3(0f, 0.5f, 0);
    [SerializeField]
    GameObject Fluff = null;
    [SerializeField]
    float insTime = 2;
    float lastTime;
    [SerializeField]
    Vector2 direction = new Vector2(0.3f, 0.3f);
    [SerializeField]
    float FluffLifeTime = 5f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") || state == 0)
        {
            animator.SetTrigger("bloom");
            state = 1;
        }
    }
    public void ToFluff()
    {
        state = 2;
        lastTime = Time.time;
    }
    private void FixedUpdate()
    {
        if (state == 2)
        {
            if (Time.time - lastTime > insTime)
            {
                GameObject Go = Instantiate(Fluff, transform.position + fluffOffset, Quaternion.identity);
                lastTime = Time.time;
                Go.GetComponent<Fluff>().init(direction, FluffLifeTime);
            }
        }
    }
}
