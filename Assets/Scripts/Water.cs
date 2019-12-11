using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [Tooltip("生存秒数"), SerializeField]
    float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);        
    }
}
