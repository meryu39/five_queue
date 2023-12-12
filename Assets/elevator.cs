using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{


    public GameObject floor1to2;
    public GameObject floor2to3;
    public GameObject floor3to4;


    public GameObject floor4to3;
    public GameObject floor3to2;
    public GameObject floor2to1;
    public GameObject floor1to0;


    public Vector3 floor1to2_position = new Vector3(-108.1226f, -48f, 0f);
    public Vector3 floor2to3_position = new Vector3(65.94f, 30.11f, 0f);
    public Vector3 floor3to4_position = new Vector3(-11f, -0.48f, 0f);

    public Vector3 floor4to3_position = new Vector3(139.61f, 28.44f, 0f);
    public Vector3 floor3to2_position = new Vector3(-44f, -66f, 0);
    public Vector3 floor2to1_position = new Vector3(298.2f, 53f, 0);
    public Vector3 floor1to0_position = new Vector3(-130.99f, 18.55f, 0);



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}