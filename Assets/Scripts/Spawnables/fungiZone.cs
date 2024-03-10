using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Path;
using UnityEngine;

public class fungiZone : MonoBehaviour
{

    public GameObject fungi;
    BoxCollider2D box;
    PolygonCollider2D polygon;
    Bounds boxBounds;


    public List<Vector3> NormalDistribution(int size)
    {
        box = GetComponent<BoxCollider2D>();
        polygon = GetComponent<PolygonCollider2D>();
        boxBounds = box.bounds;
        List<Vector3> points = new List<Vector3>();
        float x = -9999;
        float y = -9999;
        int attempt = 0;

        for (int i = 0; i < size; i++)
        {
            while (!polygon.OverlapPoint(new Vector2(x, y)) || attempt < 100)
            {   
               x = Random.Range(boxBounds.min.x, boxBounds.max.x);
               y = Random.Range(boxBounds.min.y, boxBounds.max.y);
               attempt++; 
            }

            points.Add(new Vector3(x, y, 0));
            attempt = 0;
            x = 0;
            y = 0;
        }
        return points;
    }

    public List<Vector3> GenerateCluster(int size, float range)
    {
        box = GetComponent<BoxCollider2D>();
        polygon = GetComponent<PolygonCollider2D>();
        boxBounds = box.bounds;
        Vector3 center = this.NormalDistribution(1)[0];
        List<Vector3> points = new List<Vector3>();
        int attempt = 0;
        float x = -9999;
        float y = -9999;
        //(x < (center.x - range) && x > (center.x + range) && y < (center.y - range) && y > (center.y + range)
        for (int i = 0; i < size; i++)
        {
            while (!polygon.OverlapPoint(new Vector2(x, y)) || attempt > 10)
            {
                Debug.Log(attempt);
                var temp = (Random.insideUnitCircle + new Vector2(center.x,center.y));
                x = temp.x;
                y = temp.y;
                attempt++;
            }
            attempt = 0;
            
            points.Add(new Vector3(x,y, 0));
            x = 0; y = 0;
        }
        return points;
    }

    public List<Vector3> GenerateMultipleClusters(int clusterNum, int clusterSize, float range) {
        box = GetComponent<BoxCollider2D>();
        polygon = GetComponent<PolygonCollider2D>();
        boxBounds = box.bounds;
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < clusterNum; i++)
        {
            points = points.Concat<Vector3>(GenerateCluster(clusterSize, range)).ToList();
        }
        return points;

    }

}
