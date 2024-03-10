using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalpel : IToolResultImage
{
    public override Sprite Visualize()
    {
        return DayManager.Instance.currentCase.Item1.myceliumRoots;
    }

    public override void DeVisualize()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
