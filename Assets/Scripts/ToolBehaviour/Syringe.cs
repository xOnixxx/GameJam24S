using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : IToolResultImage
{
    public override Sprite Visualize()
    {
        return DayManager.Instance.currentCase.Item1.bloodEffect;
    }
    public override void DeVisualize()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("BloodEffect").GetComponent<ParticleSystem>().Stop();
    }
}
