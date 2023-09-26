using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wellManager : MonoBehaviour
{
    [SerializeField] private Transform[] wellplace = new Transform[10];
    [SerializeField] private GameObject well;
    public float BucketDeliveryTime, YAxis;
    public int Countbales;
    void Start()
    {
        for (int i = 0; i < wellplace.Length; i++)
        {
            wellplace[i] = transform.GetChild(0).GetChild(i);
        }

        StartCoroutine(PrintBales(BucketDeliveryTime));
    }

    public IEnumerator PrintBales(float Time)
    {
        var BP_index = 0;

        while (Countbales < 10)
        {
            GameObject Newbuckets = Instantiate(well, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity, transform.GetChild(1));

            Newbuckets.transform.DOJump(new Vector3(wellplace[BP_index].position.x, wellplace[BP_index].position.y + YAxis, wellplace[BP_index].position.z), 2f, 1, 0.5f).SetEase(Ease.OutQuad);

            if (BP_index < 9)
                BP_index++;
            else
            {
                BP_index = 0;
                YAxis += 0.17f;
            }

            yield return new WaitForSecondsRealtime(5f);

        }
    }
}
