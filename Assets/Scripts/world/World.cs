using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Survival Game/World/World")]
public class World : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<Vector3Int, Chunk> chunks { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
