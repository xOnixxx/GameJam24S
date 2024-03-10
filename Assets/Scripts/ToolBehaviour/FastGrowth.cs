using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastGrowth : IToolResultImage
{
    public override Sprite Visualize()
    {
        return DayManager.Instance.currentCase.Item1.matureParts[Random.Range(0, DayManager.Instance.currentCase.Item1.matureParts.Count - 1)].GetComponent<SpriteRenderer>().sprite;
    }
    public override void DeVisualize()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
