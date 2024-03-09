using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{

    public toolType toolName;
    public int dependency;
    public float price;
    public Sprite toolSprite;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum toolType
{
    scalpel,
    syringeCheap,
    syringeExpansive,
    UVlight,
    fastGrowth,
    sporeDetector
}
