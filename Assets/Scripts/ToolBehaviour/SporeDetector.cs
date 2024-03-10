using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeDetector : IToolResultImage
{
    public Sprite basicDish;
    public override Sprite Visualize()
    {
        if (DayManager.Instance.currentCase.Item1.spores)
        {
            return DayManager.Instance.currentCase.Item1.sporeDish;
        }
        else { return GetComponent<SpriteRenderer>().sprite; }
    }
    public override void DeVisualize()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
