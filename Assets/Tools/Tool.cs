using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{

    public toolType toolName;
    public float dependency;
    public float price;
    public Sprite toolSprite;
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
