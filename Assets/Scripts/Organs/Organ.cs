using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum organType
{
    Stomach,
    Eyes,
    Heart,
    Brain,
    Intestine
}



public class Organ : MonoBehaviour
{
    public Sprite hammeredImage;
    public Sprite organSelf;
    public Sprite container;
    public PolygonCollider2D fungiZone;
    public List<PolygonCollider2D> ailmentZone;
    public float severity;
    public organType organType;
    
    public void ZoomIn()
    {
        transform.Find("Container").GetComponent<SpriteRenderer>().enabled = false;
    }

}

//TODO ADD SIGNAL FOR HAMMER


