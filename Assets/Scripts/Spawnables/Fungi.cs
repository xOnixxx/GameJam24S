using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEditor.UI;
using UnityEngine;

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
    public float sporeProb;
    public float matureProb;

    public float weaknesMod;

    private bool spores;
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
        switch (activeTool.toolName)
        {
            case toolType.scalpel:
                if (spores) { Debug.Log("BOOOM!"); }
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
                DayManager.Instance.UVOn = true;
                UVLightActive();
                break;
            case toolType.fastGrowth:
                Debug.Log("Show growth!");
                Sprite toShow = matureParts[Random.Range(0, matureParts.Count -1)].GetComponent<SpriteRenderer>().sprite;
                //TODO SHOW WINDOW WITH MEAT AND MATURED SHROOM/JUST MATURED SHROOM
                break;
            case toolType.sporeDetector:
                //return spore
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

