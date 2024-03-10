using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Fungi : MonoBehaviour
{
    public int ID;
    public GameObject attachedOrgan;
    public List<GameObject> matureParts = new List<GameObject>();
    public List<GameObject> immatureParts = new List<GameObject>();
    //public List<GameObject> bloodParticles = new List<GameObject>();
    public List<organType> acceptedOrgans = new List<organType>();

    public List<toolType> strongAgainst;
    public List<toolType> weakAgainst;
    public float depthMycelium;
    public int minCount;
    public int maxCount;

    public int minClusterS;
    public int maxClusterS;

    public int minClusterCount;
    public int maxClusterCount;

    public int spreadSize;

    public SpreadType spread;

    public bool uvSensitive;
    public Color uvColoring;
    public Color originalColor;
    public Sprite bloodEffect;
    public Sprite myceliumRoots;
    public Sprite sporeDish;
    public Sprite uvSample;

    public float sporeProb;
    public float matureProb;

    public float weaknesMod;

    [HideInInspector]
    public bool spores;
    [HideInInspector]
    public bool isZoomed = false;
    private bool isMature;
    private List<GameObject> spawnedParts = new List<GameObject>();

    //TODO Spred types
    public void Start()
    {
        spores = (Random.value < sporeProb);
        isMature = (Random.value < matureProb);
    }

    public void Infect(GameObject Organ)
    {
        attachedOrgan = Organ;
        Spread();
        StartColor();
    }


    public void Spread()
    {
        //Quaternion qr = Random.rotation;
        Quaternion qr = Quaternion.identity;
        qr.x = 0;
        qr.y = 0;
        List<Vector3> fungiPoint = GenerateSpread();
        List<GameObject> activeSprites;

        if (isMature) { activeSprites = matureParts; }
        else{ activeSprites = immatureParts; }
        foreach (Vector3 p in fungiPoint)
        {
            
            qr = Quaternion.Euler(0, 0, Random.Range(0, 90) - 45);
            var t1 = Instantiate(activeSprites[Random.Range(0, activeSprites.Count - 1)], p, qr);
            t1.transform.SetParent(this.transform,false);
            t1.transform.localScale = t1.transform.localScale * ((float)Random.Range(70, 100) / 100);
            spawnedParts.Add(t1);
        }


    }


    private List<Vector3> GenerateSpread()
    {
        int count = Random.Range(minCount, maxCount);
        int clusterSize = Random.Range(minClusterS, maxClusterS);
        int clusterCount = Random.Range(minClusterCount, maxClusterCount);
        


        switch (spread)
        {
            case SpreadType.Normal: return attachedOrgan.transform.Find("FungiZonePrefab").GetComponent<fungiZone>().NormalDistribution(count);
            case SpreadType.Islands: return attachedOrgan.transform.Find("FungiZonePrefab").GetComponent<fungiZone>().GenerateMultipleClusters(clusterCount, clusterSize, spreadSize);
            case SpreadType.Center: return attachedOrgan.transform.Find("FungiZonePrefab").GetComponent<fungiZone>().GenerateCluster(clusterSize, spreadSize);
            default: return new List<Vector3>();
        }
    }
    
    public void Die()
    {
        foreach (var part in spawnedParts)
        {
            Destroy(part);
        }
        Destroy(this);
    }

    public void ScrapeRandom()
    {
        int index = Random.Range(0, spawnedParts.Count - 1);
        var td = spawnedParts[index];
        spawnedParts.RemoveAt(index);
        Destroy(td);
    }
    

    public void UseTool()
    {
        
        Tool activeTool = DayManager.Instance.currentSelectedTool;
        if (activeTool.toolName == toolType.UVlight && DayManager.Instance.UVOn)
        {
            DayManager.Instance.UVOn = false;
            UVLightOff();
        }
        else
        {
            if (PlayerState.Instance.currentMoney > (activeTool.price + DayManager.Instance.priceIncrease))
            {
                if (Random.value < activeTool.dependency)
                {
                    ToolSelection(activeTool);
                }
                else { Debug.Log("Skill issue!"); }
            }
            else {
                Debug.Log(activeTool.price + " " + DayManager.Instance.priceIncrease);
                Debug.Log(PlayerState.Instance.currentMoney);
                Debug.Log("Sorry u broke!"); }
        }


    }

    private void ToolSelection(Tool activeTool)
    {
        if (strongAgainst.Contains(activeTool.toolName))
        {
            if (Random.value < weaknesMod) {
                Debug.Log("Shroom too stronk");
                return; }
        }
        Debug.Log(spores);
        switch (activeTool.toolName)
        {
            case toolType.scalpel:
                if (spores) {
                    attachedOrgan.transform.Find("Spores 1").GetComponent<ParticleSystem>().Play(); }
                else { HUD.Instance.ShowToolResults(activeTool.toolName, activeTool.GetComponent<IToolResultImage>()); }
                break;
            case toolType.syringeCheap:
                HUD.Instance.ShowToolResults(activeTool.toolName, activeTool.GetComponent<IToolResultImage>());
                break;
            case toolType.syringeExpansive:
                HUD.Instance.ShowToolResults(activeTool.toolName, activeTool.GetComponent<IToolResultImage>());
                break;
            case toolType.UVlight:
                HUD.Instance.ShowToolResults(activeTool.toolName, activeTool.GetComponent<IToolResultImage>());
                DayManager.Instance.UVOn = true;
                UVLightActive();
                break;
            case toolType.fastGrowth:
                HUD.Instance.ShowToolResults(activeTool.toolName, activeTool.GetComponent<IToolResultImage>());
                break;
            case toolType.sporeDetector:
                HUD.Instance.ShowToolResults(activeTool.toolName, activeTool.GetComponent<IToolResultImage>());
                break;
            default: return;
        }
    }

    private void UVLightActive()
    {
        HUD.Instance.TurnUVLightOn();
        foreach (GameObject fungiParts in spawnedParts)
        {
            fungiParts.GetComponent<SpriteRenderer>().color = uvColoring;
        }
    }

    private void UVLightOff()
    {
        HUD.Instance.TurnUVLightOff();
        foreach (GameObject fungiParts in spawnedParts)
        {
            fungiParts.GetComponent<SpriteRenderer>().color = originalColor;
        }
    }

    private void StartColor()
    {
        foreach (GameObject fungiParts in spawnedParts)
        {
            fungiParts.GetComponent<SpriteRenderer>().color = originalColor;
        }
    }


    public void ZoomIn()
    {
        isZoomed = true;
        Camera t = Camera.main;
        t.DOOrthoSize(3, 1);
        attachedOrgan.transform.Find("Container").GetComponent<SpriteRenderer>().DOFade(0,0.5f);
    }

    public void ZoomOut()
    {
        isZoomed = false;
        attachedOrgan.transform.Find("Container").GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        Camera t = Camera.main;
        t.DOOrthoSize(5,1);

    }


    public void ScaleCase(float scale)
    {
        transform.localScale = Vector3.one*scale;
        attachedOrgan.transform.localScale = Vector3.one*scale;
    }

}


public enum SpreadType
{
    Normal,
    Center,
    Islands,
    Outer
}

