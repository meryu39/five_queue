using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roulette_controller : MonoBehaviour
{
    [SerializeField]
    private float rot = 0f;
    private bool roll = false;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; // ��Ÿ ����
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) // ��Ÿ ����
        {
      
        
          this.rot = 10f;
          this.roll = true;
      
        }

        
        transform.Rotate(0, 0, this.rot); // ��Ÿ ����
        rot *= 0.96f;
    }
}
