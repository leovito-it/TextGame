using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Range(0f, 10f)]
    public float delay = 3;

    // Start is called before the first frame update
    void Start()
    {
        DestroyMe();
    }

    void DestroyMe()
    {
        Destroy(gameObject,delay);
    }
    
}
