using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;



public enum organType
{
    Digestion,
    Circulation,
    Skeletal,
    Occult
}



public class Organ : MonoBehaviour
{
    public Sprite hammeredImage;
    public PolygonCollider2D fungiZone;
    public PolygonCollider2D ailmentZone;
    public float severity;
    public organType organType;

    private void OnMouseEnter()
    {
        Debug.Log("Entered!");
    }

}

//TODO ADD SIGNAL FOR HAMMER


