using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Map_Unit : MonoBehaviour
{
    public int floor;
    //�������� 
    public GameObject Unit5F;
    public GameObject Unit4F;
    public GameObject Unit3F;
    public GameObject Unit2F;
    public GameObject Unit1F;

    // Start is called before the first frame update
    void Start()
    {
        Unit5F.SetActive(false);
        Unit4F.SetActive(false);
        Unit3F.SetActive(false);
        Unit2F.SetActive(false);
        Unit1F.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (floor) {
            case 1:
                Unit1F.SetActive(true);
                break;
            case 2:
                Unit2F.SetActive(true);
                break;
            case 3:
                Unit3F.SetActive(true);
                break;
            case 4:
                Unit4F.SetActive(true);
                break;
            case 5:
                Unit5F.SetActive(true);
                break;

        }
    }
}