using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluff : MonoBehaviour
{
    [SerializeField]
    float removeHight = 5;
    Rigidbody rb;
    float lifeTime;
    float startY;
    public void init(Vector2 vel,float lf)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = vel;
        lifeTime = lf;
        startY = transform.position.y;
    }


    // Update is called once per frame
    void Update()
    {
        if(transform.position.y-startY>removeHight)
        {
            Destroy(gameObject);
        }
    }
}
