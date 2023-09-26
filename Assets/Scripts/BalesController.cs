using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalesController : MonoBehaviour
{
    [SerializeField] private Transform[] BalesPlace = new Transform[10];
    [SerializeField] private GameObject Bales;
    public float BucketDeliveryTime, YAxis;
    public int CountWater;

    PlayerManager playerManager;

    private int a = 0; // Baþlatma iþlemi için bir deðiþken

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        for (int i = 0; i < BalesPlace.Length; i++)
        {
            BalesPlace[i] = transform.GetChild(0).GetChild(i);
        }

        if (a > 0)
        {
            StartBalesProduction();
        }
    }

    void Update()
    {
        if (playerManager.outwater > 0)
        {
            a = 1;
            StartBalesProduction();
        }

        if (playerManager.outwater <= 0)
        {
            a = 0;
        }
    }

    // Yeni bir bale üretme fonksiyonu
    private void ProduceBale()
    {
        var BP_index = 0;

        GameObject Newbuckets = Instantiate(Bales, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity, transform.GetChild(1));

        Newbuckets.transform.DOJump(new Vector3(BalesPlace[BP_index].position.x, BalesPlace[BP_index].position.y + YAxis, BalesPlace[BP_index].position.z), 2f, 1, 0.5f).SetEase(Ease.OutQuad);

        if (BP_index < 9)
            BP_index++;
        else
        {
            BP_index = 0;
            YAxis += 0.17f;
        }
    }

    // Bale üretimini baþlatan fonksiyon
    private void StartBalesProduction()
    {
        if (CountWater < 10)
        {
            ProduceBale();
            CountWater++;
            playerManager.outwater--;
        }
    }
}
