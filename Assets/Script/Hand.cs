using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public ClimbTest climbTest = null; // 參考ClimbTest腳本
    public OVRInput.Controller controller = OVRInput.Controller.None; // 左手或右手控制器
    public Vector3 Delta { private set; get; } = Vector3.zero; // 手的移動量

    private Vector3 lastPosition = Vector3.zero;
    private GameObject currentClimbable = null;
    private List<GameObject> contactClimbables = new List<GameObject>();

    private void Start()
    {
        lastPosition = transform.position; // 初始化手的位置
    }

    private void Update()
    {
        // 當按下抓取按鍵時，嘗試抓住攀爬點
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            GrabClimbable();
        }
        // 當放開按鍵時，放開攀爬點
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            ReleaseClimbable();
        }
    }

    private void FixedUpdate()
    {
        // 更新上一次手的位置
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        // 計算手的移動量
        Delta = lastPosition - transform.position;
    }

    public void GrabClimbable()
    {
        // 找到最近的攀爬點
        currentClimbable = GetNearestClimbable();
        if (currentClimbable != null)
        {
            climbTest.StartClimbing(); // 通知ClimbTest開始攀爬
        }
    }

    public void ReleaseClimbable()
    {
        if (currentClimbable != null)
        {
            climbTest.StopClimbing(); // 通知ClimbTest停止攀爬
            currentClimbable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 當碰到帶有Climbable標籤的物體時，加入攀爬點列表
        if (other.CompareTag("Climbable"))
        {
            contactClimbables.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 當離開攀爬點時，從列表中移除
        if (other.CompareTag("Climbable"))
        {
            contactClimbables.Remove(other.gameObject);
        }
    }

    private GameObject GetNearestClimbable()
    {
        // 找到距離手最近的攀爬點
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
