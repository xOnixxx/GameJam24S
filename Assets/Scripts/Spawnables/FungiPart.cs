using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiPart : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponentInParent<Fungi>().ScaleCase(0.5f);
        GetComponentInParent<Fungi>().UseTool();
    }
}
