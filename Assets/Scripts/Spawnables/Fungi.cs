using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Fungi : MonoBehaviour
{
    public int ID;
    public GameObject attachedOrgan;
    public List<GameObject> matureParts = new List<GameObject>();
    public List<GameObject> immatureParts = new List<GameObject>();
    public List<GameObject> ailments = new List<GameObject>();
    //public List<GameObject> bloodParticles = new List<GameObject>();
    public List<organType> acceptedOrgans = new List<organType>();

    public List<Tool> strongAgainst;
    public List<Tool> weakAgainst;
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
    public float sporeProb;

    public float rotationLimit;

    private bool spores;
    private List<GameObject> spawnedParts = new List<GameObject>();
    private Color originalColoring;

    //TODO Spred types
    public void Start()
    {
        spores = (Random.value < sporeProb);
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

        if (spores ) { activeSprites = matureParts; }
        else{ activeSprites = immatureParts; }
        foreach (Vector3 p in fungiPoint)
        {
            
            qr = Quaternion.Euler(0, 0, Random.Range(0, 90) - 45);
            var t1 = Instantiate(activeSprites[Random.Range(0, activeSprites.Count - 1)], p, qr);
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
            UVLightOff();
        }
        {
            if (PlayerState.Instance.currentMoney > activeTool.price + DayManager.Instance.priceIncrease)
            {
                if (Random.value < activeTool.dependency)
                {
                    ToolSelection(activeTool);
                }
                else { Debug.Log("Skill issue!"); }
            }
            else { Debug.Log("Sorry u broke!"); }
        }


    }

    private void ToolSelection(Tool activeTool)
    {
        switch (activeTool.toolName)
        {
            case toolType.scalpel:
                Debug.Log(depthMycelium);
                break;
            case toolType.syringeCheap:
                Debug.Log("Cheap blood!");
                break;
            case toolType.syringeExpansive:
                Debug.Log("Expansice blood!");
                break;
            case toolType.UVlight:
                Debug.Log("Change lights");
                UVLightActive();
                break;
            case toolType.fastGrowth:
                Debug.Log("Show growth!");
                break;
            case toolType.sporeDetector:
                Debug.Log(spores);
                break;
            default: return;
        }
    }

    private void UVLightActive()
    {
        foreach (GameObject fungiParts in spawnedParts)
        {
            fungiParts.GetComponent<SpriteRenderer>().color = uvColoring;
        }
    }

    private void UVLightOff()
    {
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


}


public enum SpreadType
{
    Normal,
    Center,
    Islands,
    Outer
}

