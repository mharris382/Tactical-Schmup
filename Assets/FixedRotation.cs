﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class FixedRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
