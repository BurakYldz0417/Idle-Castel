using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;
    private Animator plrAnim;
    [SerializeField] private List<Transform> waters = new List<Transform>();
    [SerializeField] private List<Transform> bales = new List<Transform>();
    [SerializeField] private List<Transform> milks = new List<Transform>();
    [SerializeField] private Transform carryPlace;
    private float yAxis, delay;

    [SerializeField] private Joystick joystick;

    private Transform carryContainer;
    public int inwater, outwater;
    public int inbaley, outbaley;
    public int money;

    public TextMeshProUGUI moneytext;
    void Start()
    {
        plrAnim = GetComponent<Animator>();
        waters.Add(carryPlace);
        bales.Add(carryPlace);
        milks.Add(carryPlace);
        moveSpeed = 5f;

        carryContainer = transform.GetChild(3);
        inwater = 0; outwater = 0;
        inbaley = 0; outbaley = 0;
    }

    void Update()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput);

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            plrAnim.SetBool("run", true);
        }
        else if (moveDirection.magnitude < 0.1f)
        {
            plrAnim.SetBool("run", false);
            plrAnim.SetBool("idle", true);
        }

        if (waters.Count > 1)
        {
            for (int i = 1; i < waters.Count; i++)
            {
                var firstwaters = waters[i - 1];
                var secondwaters = waters[i];

                secondwaters.position = new Vector3(
                    Mathf.Lerp(secondwaters.position.x, firstwaters.position.x, Time.deltaTime * 15f),
                    Mathf.Lerp(secondwaters.position.y, firstwaters.position.y + 0.2f, Time.deltaTime * 15f),
                    firstwaters.position.z);
            }

            if (moveDirection.magnitude > 0.1f)
            {
                plrAnim.SetBool("carry", false);
                plrAnim.SetBool("RunWithWeight", true);
                plrAnim.SetBool("run", false);
            }
            else
            {
                plrAnim.SetBool("carry", true);
                plrAnim.SetBool("RunWithWeight", false);
                plrAnim.SetBool("idle", false);
            }
        }
        if (bales.Count > 1)
        {
            for (int i = 1; i < bales.Count; i++)
            {
                var firstbales = bales[i - 1];
                var secondbales = bales[i];

                secondbales.position = new Vector3(
                    Mathf.Lerp(secondbales.position.x, firstbales.position.x, Time.deltaTime * 15f),
                    Mathf.Lerp(secondbales.position.y, firstbales.position.y + 0.17f, Time.deltaTime * 15f),
                    firstbales.position.z);
            }

            if (moveDirection.magnitude > 0.1f)
            {
                plrAnim.SetBool("carry", false);
                plrAnim.SetBool("RunWithWeight", true);
                plrAnim.SetBool("run", false);
            }
            else
            {
                plrAnim.SetBool("carry", true);
                plrAnim.SetBool("RunWithWeight", false);
                plrAnim.SetBool("idle", false);
            }
        }
        if (milks.Count > 1)
        {
            for (int i = 1; i < milks.Count; i++)
            {
                var firstmilks = milks[i - 1];
                var secondmilks =milks[i];

                secondmilks.position = new Vector3(
                    Mathf.Lerp(secondmilks.position.x, firstmilks.position.x, Time.deltaTime * 15f),
                    Mathf.Lerp(secondmilks.position.y, firstmilks.position.y + 0.17f, Time.deltaTime * 15f),
                    firstmilks.position.z);
            }

            if (moveDirection.magnitude > 0.1f)
            {
                plrAnim.SetBool("carry", false);
                plrAnim.SetBool("RunWithWeight", true);
                plrAnim.SetBool("run", false);
            }
            else
            {
                plrAnim.SetBool("carry", true);
                plrAnim.SetBool("RunWithWeight", false);
                plrAnim.SetBool("idle", false);
            }
        }
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {

            if ((hit.collider.CompareTag("well") || hit.collider.CompareTag("farm")|| hit.collider.CompareTag("barn")))
            {
                if (hit.collider.transform.childCount > 1)
                {
                    var collectible = hit.collider.transform.GetChild(1);
                    collectible.rotation = Quaternion.Euler(0f, 0f, 0f);

                    float yIncrease = 50f;

                    if (hit.collider.CompareTag("well"))
                    {
                        waters.Add(collectible);
                        Debug.Log("water eklendi. Toplam water sayýsý: " + waters.Count);
                        inwater = waters.Count;
                    }
                    else if (hit.collider.CompareTag("farm"))
                    {
                        bales.Add(collectible);
                        Debug.Log("bales eklendi. Toplam Bales sayýsý: " + bales.Count);
                        inbaley = bales.Count;
                    }
                    else if (hit.collider.CompareTag("barn"))
                    {
                        milks.Add(collectible);
                        Debug.Log("milk eklendi. Toplam Bales sayýsý: " + milks.Count);
                    }

                    collectible.position += new Vector3(0f, yIncrease, 0f);

                    collectible.parent = carryContainer;
                    plrAnim.SetBool("carry", true);
                    plrAnim.SetBool("run", false);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
            }

            moneytext.text = "Money : " + money.ToString() + " $";
        }

        // Su ve balya sayýsý sýfýr ise animasyonlarý kapat
        if (waters.Count <= 1 && bales.Count <= 1&&milks.Count<=1)
        {
            plrAnim.SetBool("RunWithWeight", false);
            plrAnim.SetBool("carry", false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "farm")
        {

            if (inwater > 1)
            {
                if (waters.Count > 1)
                {
                    Transform lastwaters = waters[waters.Count - 1];
                    waters.RemoveAt(waters.Count - 1);
                    Destroy(lastwaters.gameObject);
                    Debug.Log("Toplam water sayýsý: " + waters.Count);
                    outwater++;
                    inwater--;
                }
            }

        }
        if (other.gameObject.tag == "barn")
        {

            if (inbaley > 1)
            {
                if (bales.Count > 1)
                {
                    Transform lastbales = bales[bales.Count - 1];
                    bales.RemoveAt(bales.Count - 1);
                    Destroy(lastbales.gameObject);
                    Debug.Log("Toplam bales sayýsý: " + bales.Count);
                    outbaley++;
                    inbaley--;
                }
            }

        }
        if (other.CompareTag("sell"))
        {
            if (bales.Count > 1)
            {
                Transform lastStone = bales[bales.Count - 1];
                bales.RemoveAt(bales.Count - 1);
                Destroy(lastStone.gameObject);
                Debug.Log("Stone satýldý. Toplam Stone sayýsý: " + bales.Count);
                money += 5;
            }
            if (milks.Count > 1)
            {
                Transform lastBrick = milks[milks.Count - 1];
                milks.RemoveAt(milks.Count - 1);
                Destroy(lastBrick.gameObject);
                Debug.Log("Brick satýldý. Toplam Brick sayýsý: " + milks.Count);
                money += 10;
            }
        }
    }
}