using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testOutsideAdd : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject attachedOrgan;
    private Organ selfOrgan;

    public void Start()
    {
        selfOrgan = attachedOrgan.GetComponent<Organ>();
        Debug.Log(selfOrgan.name);
        Debug.Log("Hello");
    }


    private void OnMouseDown()
    {
        Debug.Log("Hello");
        test();
    }

    void test()
    {
        Debug.Log(selfOrgan.severity);
        Debug.Log("HelloNOOO");
    }
}
