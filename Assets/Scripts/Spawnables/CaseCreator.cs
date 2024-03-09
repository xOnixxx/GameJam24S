using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CaseCreator : MonoBehaviour
{

    public List<GameObject> fungiList;
    public List<GameObject> organList;



    public (Fungi, Organ) GenerateCase()
    {
        int organID = UnityEngine.Random.Range(0, organList.Count - 1);
        int fungiID = UnityEngine.Random.Range(0, fungiList.Count - 1);
        var fungi = Instantiate<GameObject>(fungiList[fungiID]);
        var organ = Instantiate<GameObject>(organList[organID]);
        PlaceOrgan(organ);
        fungi.GetComponent<Fungi>().Infect(organ);
        (Fungi,Organ) temp1 = (fungi.GetComponent<Fungi>(), organ.GetComponent<Organ>());

        return temp1;
    }

    public void PlaceOrgan(GameObject organ)
    {
        organ.transform.position = Vector3.one;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
