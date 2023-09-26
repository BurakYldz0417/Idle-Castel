using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barnManager : MonoBehaviour
{[SerializeField] private Transform[] MilksPlace = new Transform[10];
    [SerializeField] private GameObject Milks;
    public float BucketDeliveryTime, YAxis;
    public int CountWater;

    PlayerManager playerManager;

    private int a = 0; // Ba�latma i�lemi i�in bir de�i�ken

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        for (int i = 0; i < MilksPlace.Length; i++)
        {
            MilksPlace[i] = transform.GetChild(0).GetChild(i);
        }

        if (a > 0)
        {
            StartMilksProduction();
        }
    }

    void Update()
    {
        if (playerManager.outbaley > 0)
        {
            a = 1;
            StartMilksProduction();
        }

        if (playerManager.outbaley <= 0)
        {
            a = 0;
        }
    }

    // Yeni bir bale �retme fonksiyonu
    private void ProduceBale()
    {
        var BP_index = 0;

        GameObject Newbuckets = Instantiate(Milks, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity, transform.GetChild(1));

        Newbuckets.transform.DOJump(new Vector3(MilksPlace[BP_index].position.x, MilksPlace[BP_index].position.y + YAxis, MilksPlace[BP_index].position.z), 2f, 1, 0.5f).SetEase(Ease.OutQuad);

        if (BP_index < 9)
            BP_index++;
        else
        {
            BP_index = 0;
            YAxis += 0.17f;
        }
    }

    // Bale �retimini ba�latan fonksiyon
    private void StartMilksProduction()
    {
        if (CountWater < 10)
        {
            ProduceBale();
            CountWater++;
            playerManager.outbaley--;
        }
    }
}
