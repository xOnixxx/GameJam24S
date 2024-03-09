using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fungi : MonoBehaviour
{
    public GameObject attachedOrgan;
    public List<GameObject> matureParts = new List<GameObject>();
    public List<GameObject> immatureParts = new List<GameObject>();
    public List<GameObject> ailments = new List<GameObject>();
    //public List<GameObject> bloodParticles = new List<GameObject>();

    public List<GameObject> strongAgainst;
    public List<GameObject> weakAgainst;
    public float depthMycelium;
    public bool spores;
    public bool uvSensitive;
    public float sporeProb;


    private List<GameObject> spawnedParts = new List<GameObject>();


    //TODO Spred types
    public void Start()
    {
        spores = (Random.value < sporeProb);
    }

    public void Spread()
    {
        Quaternion qr = Random.rotation;
        qr.x = 0;
        qr.y = 0;
        Vector3 fungiPoint = attachedOrgan.transform.Find("AilmentZonePrefab").GetComponent<AilmentZone>().PointInArea();
        //TODO IF FOR Mature or Immature
        //spawnedParts.Add(Instantiate(fungiParts[0], fungiPoint, qr));
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
    

    private void OnMouseDown()
    {
        Debug.Log("Click!");
        Spread();
    }

}

