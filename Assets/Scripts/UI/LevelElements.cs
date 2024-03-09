using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelElements : MonoBehaviour
{
    public CanvasGroup interactableFreezer;
    public static LevelElements Instance;
    public Vector2 startOfLine = new(0, 1.25f);
    public Vector2 endOfLine = new(0, 1f);
    public Vector2 sizeOfFreezerMovement = new(0, -0.25f);
    public Vector2 freezerHorizontalSize = new(0.1f, 0);
    public float speedOfFreezerMovement = 0.5f;
    public List<RectTransform> freezerLine;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void MoveFreezerLine()
    {
        if(DayManager.Instance.hasActiveCase)
        {
            return;
        }
        interactableFreezer.interactable = false;
        interactableFreezer.blocksRaycasts = false;
        DayManager.Instance.hasActiveCase = true;
        DayManager.Instance.GetNewOrgan();
        HUD.Instance.CheckConfirmEligibility();
        //Move whole line
        foreach (var freezer in freezerLine)
        {
            freezer.DOAnchorMin(freezer.anchorMin + sizeOfFreezerMovement, speedOfFreezerMovement);
            freezer.DOAnchorMax(freezer.anchorMax + sizeOfFreezerMovement, speedOfFreezerMovement);
        }
        //Afterwards move the last freezer to the top
        StartCoroutine(ReturnFreezerToTop());
    }

    private IEnumerator ReturnFreezerToTop()
    {
        yield return new WaitForSeconds(speedOfFreezerMovement);
        var lastFreezer = freezerLine[freezerLine.Count - 1];
        lastFreezer.anchorMax = startOfLine + freezerHorizontalSize;
        lastFreezer.anchorMin = startOfLine + sizeOfFreezerMovement;
        freezerLine.RemoveAt(freezerLine.Count - 1);
        freezerLine.Insert(0, lastFreezer);
        interactableFreezer.interactable = true;
        interactableFreezer.blocksRaycasts = true;
    }
}
