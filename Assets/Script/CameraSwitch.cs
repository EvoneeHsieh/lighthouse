using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject camera_1;
    public GameObject camera_2;
    public int manager;

    private void Start()
    {
        // �T�O��l�۾����A
        camera_1.SetActive(true);
        camera_2.SetActive(false);
    }
    private void Update()
    {
        // �˴� E ��
        if (Input.GetKeyDown(KeyCode.E))
        {
            ManageCamera();
        }
    }

    public void ManageCamera()
    {
        if(manager == 0)
        {
            Cam_2();
            manager = 1;
        }
        else
        {
            Cam_1();
            manager = 0;
        }
    }

    void Cam_1()
    {
        camera_1.SetActive(true);
        camera_2.SetActive(false);
    }
    void Cam_2()
    {
        camera_1.SetActive(false);
        camera_2.SetActive(true);
    }
}
