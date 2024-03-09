using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilmentZone : MonoBehaviour
{

    public GameObject ailment;
    private List<GameObject> ailmentList = new List<GameObject>();

    public Vector3 PointInArea()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();
        Bounds boxBounds = box.bounds;


        float x = 0;
        float y = 0;
        int attempt = 0;
        do
        {
            x = Random.Range(boxBounds.min.x, boxBounds.max.x);
            y = Random.Range(boxBounds.min.y, boxBounds.max.y);
            attempt++;
        } while (!polygon.OverlapPoint(new Vector2(x, y)) || attempt <= 100);

        return new Vector3(x, y, 0);
    }

    public void AddAilment()
    {
        Quaternion qr = Random.rotation;
        qr.x = 0;
        qr.y = 0;
        ailmentList.Add(Instantiate(ailment, position: PointInArea(), rotation: qr));
    }

    public void RemoveAilment()
    {
        foreach (GameObject fungi in ailmentList)
        {
            Destroy(fungi);
        }
    }

    private void OnMouseEnter()
    {
        AddAilment();
    }

    private void OnMouseDown()
    {
        RemoveAilment();
    }
}
