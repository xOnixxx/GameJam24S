using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

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
        PlaceOrgan(organ, fungi);
        fungi.GetComponent<Fungi>().Infect(organ);
        (Fungi,Organ) temp1 = (fungi.GetComponent<Fungi>(), organ.GetComponent<Organ>());

        return temp1;
    }

    public void PlaceOrgan(GameObject organ, GameObject fungi)
    {
        organ.transform.position = new Vector3(-10, -2.7f, 2);
        fungi.transform.position = new Vector3(-10, -2.7f, 1);
        Transform tO = organ.transform;
        Transform tF = fungi.transform;



        tO.DOMove(new Vector3(0, -1f, 2), 1).SetEase(Ease.InCubic);
        tF.DOMove(new Vector3(0, -1f, 1), 1).SetEase(Ease.InCubic);

        tO.DOScale(new Vector3(2, 2, 1), 1);
        tF.DOScale(new Vector3(2, 2, 1), 1);

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
