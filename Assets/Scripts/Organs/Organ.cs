using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;



public enum organType
{
    Stomach,
    Eyes,
    Heart,
    Brain
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

    private void OnMouseEnter()
    {
        Debug.Log("Entered!");
    }

}

//TODO ADD SIGNAL FOR HAMMER


