using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiPart : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponentInParent<Fungi>().UseTool();
    }
}
