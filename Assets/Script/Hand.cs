using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public ClimbTest climbTest = null; // �Ѧ�ClimbTest�}��
    public OVRInput.Controller controller = OVRInput.Controller.None; // ����Υk�ⱱ�
    public Vector3 Delta { private set; get; } = Vector3.zero; // �⪺���ʶq

    private Vector3 lastPosition = Vector3.zero;
    private GameObject currentClimbable = null;
    private List<GameObject> contactClimbables = new List<GameObject>();

    private void Start()
    {
        lastPosition = transform.position; // ��l�Ƥ⪺��m
    }

    private void Update()
    {
        // ����U�������ɡA���է���k���I
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            GrabClimbable();
        }
        // ���}����ɡA��}�k���I
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            ReleaseClimbable();
        }
    }

    private void FixedUpdate()
    {
        // ��s�W�@���⪺��m
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        // �p��⪺���ʶq
        Delta = lastPosition - transform.position;
    }

    public void GrabClimbable()
    {
        // ���̪��k���I
        currentClimbable = GetNearestClimbable();
        if (currentClimbable != null)
        {
            climbTest.StartClimbing(); // �q��ClimbTest�}�l�k��
        }
    }

    public void ReleaseClimbable()
    {
        if (currentClimbable != null)
        {
            climbTest.StopClimbing(); // �q��ClimbTest�����k��
            currentClimbable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ��I��a��Climbable���Ҫ�����ɡA�[�J�k���I�C��
        if (other.CompareTag("Climbable"))
        {
            contactClimbables.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �����}�k���I�ɡA�q�C������
        if (other.CompareTag("Climbable"))
        {
            contactClimbables.Remove(other.gameObject);
        }
    }

    private GameObject GetNearestClimbable()
    {
        // ���Z����̪��k���I
        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (var point in contactClimbables)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = point;
            }
        }
        return nearest;
    }
}
