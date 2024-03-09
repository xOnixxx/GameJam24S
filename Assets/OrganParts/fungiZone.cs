using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fungiZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PointInArea()
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
        Debug.Log("Attemps: " + attempt);

        Debug.Log(x + " " + y);
    }

    private void OnMouseEnter()
    {
        PointInArea();
        Debug.Log("Son entered!");
        //GetComponentInParent<SpriteRenderer>().enabled = !GetComponentInParent<SpriteRenderer>().enabled;
    }
}
