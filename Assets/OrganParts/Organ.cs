using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum organType
{
    Digestion,
    Circulation
}



public interface Organ
{
    Sprite selfImage { get; set; }
    PolygonCollider2D fungiZone { get; set;}
    PolygonCollider2D ailmentZone { get; set; }
    float severity { get; set; }



}
