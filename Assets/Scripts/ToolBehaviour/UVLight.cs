using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVLight : IToolResultImage
{
    public override Sprite Visualize()
    {
        return DayManager.Instance.currentCase.Item1.uvSample;
    }
    public override void DeVisualize() { }
}
