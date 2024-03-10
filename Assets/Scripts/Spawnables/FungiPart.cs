using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiPart : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (HUD.Instance.toolsOpened)
        {
            GetComponentInParent<Fungi>().UseTool();
        }
        else if (!HUD.Instance.encyOpened && !HUD.Instance.gubermentOpened && !HUD.Instance.resultsOpened)
        {
            if (GetComponentInParent<Fungi>().isZoomed)
            {
                GetComponentInParent<Fungi>().ZoomOut();
            }
            else { GetComponentInParent<Fungi>().ZoomIn(); }
        }

    }
}
