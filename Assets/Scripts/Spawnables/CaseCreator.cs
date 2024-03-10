using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Windows;

public class CaseCreator : MonoBehaviour
{

    public List<GameObject> fungiList;
    public List<GameObject> organList;


    (Fungi, Organ) activeCase;

    private void OnMouseDown()
    {
        Debug.Log("DELETING");
        DeleteCase(activeCase);
    }

    public (Fungi, Organ) GenerateCase()
    {
        Debug.Log("Spawning");
        int organID = UnityEngine.Random.Range(0, organList.Count - 1);
        int fungiID = UnityEngine.Random.Range(0, fungiList.Count - 1);
        var fungi = Instantiate<GameObject>(fungiList[fungiID]);
        var organ = Instantiate<GameObject>(organList[organID]);
        PlaceOrgan(organ, fungi);
        fungi.GetComponent<Fungi>().Infect(organ);
        (Fungi,Organ) temp1 = (fungi.GetComponent<Fungi>(), organ.GetComponent<Organ>());
        activeCase = temp1;

        return temp1;
    }

    public void PlaceOrgan(GameObject organ, GameObject fungi)
    {
        Debug.Log("Placing");
        organ.transform.position = new Vector3(-10, -2.7f, 1);
        fungi.transform.position = new Vector3(-10, -2.7f, -2);
        Transform tO = organ.transform;
        Transform tF = fungi.transform;

        tO.DOMove(new Vector3(0, -1f, 1), 1).SetEase(Ease.InCubic);
        tF.DOMove(new Vector3(0, -1f, -2), 1).SetEase(Ease.InCubic);

        tO.DOScale(new Vector3(1.5f, 1.5f, 1), 1);
        tF.DOScale(new Vector3(1.5f, 1.5f, 1), 1);
        Debug.Log(tO.transform.position);
    }



    public void DeleteCase((Fungi, Organ) activeCase)
    {
        activeCase.Item2.transform.Find("VacuumEffect").GetComponent<ParticleSystem>().Play();
        var sequence = DOTween.Sequence();
        activeCase.Item1.transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        sequence.Append(activeCase.Item2.transform.DOScale(new Vector3(0, 0, 0), 0.5f));
        //deleteParticle.Play();
        sequence.AppendCallback(() =>
        {
            //activeCase.Item1.Die();
            Destroy(activeCase.Item1.transform.GameObject());
            Destroy(activeCase.Item2.transform.GameObject());
        });
        sequence.Play();

    }


}
