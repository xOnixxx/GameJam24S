using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fungi : MonoBehaviour
{
    public int ID;
    public GameObject attachedOrgan;
    public List<GameObject> matureParts = new List<GameObject>();
    public List<GameObject> immatureParts = new List<GameObject>();
    public List<GameObject> ailments = new List<GameObject>();
    //public List<GameObject> bloodParticles = new List<GameObject>();
    public List<Organ> acceptedOrgans = new List<Organ>();

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
    public float sporeProb;

    public float rotationLimit;

    private bool spores;
    private List<GameObject> spawnedParts = new List<GameObject>();


    //TODO Spred types
    public void Start()
    {
        spores = (Random.value < sporeProb);
        Spread();
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
            qr = Random.rotation;
            qr.x = 0;
            qr.y = 0;
            spawnedParts.Add(Instantiate(activeSprites[0], p, qr));
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
    

}


public enum SpreadType
{
    Normal,
    Center,
    Islands,
    Outer
}

