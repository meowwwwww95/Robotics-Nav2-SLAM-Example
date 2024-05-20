using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{
    // 一维 List用于存储所有门对象名称
    private List<string> doorNames = new List<string>();

    // 一维 List用于存储所有门的开关状态
    private List<bool> doorStatus = new List<bool>();

    public float interactionDistance = 5f; // 交互距离
    public KeyCode interactKey = KeyCode.F; // 交互键

    void Start()
    {
        // 初始化 DoorGroup 下的直接子对象
        InitializeDoorNames(transform);

        // 打印一维 List 内容（可选）
        // PrintObjectNames();
    }

    void Update()
    {
        // 检测 F 键是否被按下
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("Button F is pressed");
            // 射线检测，从摄像机发出一条射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 射线检测
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                Debug.Log("Within Distance");

                // 检测到射线击中了门把
                if (hit.collider.CompareTag("Door"))
                {
                    Debug.Log("Hit Door Collider");

                    // 获取门对象
                    GameObject door = hit.collider.gameObject;

                    if (door != null)
                    {
                        // 调用开关门函数
                        ToggleDoor(door);
                    }
                }
            }
        }
    }

    // 初始化 DoorGroup 下的直接子对象
    void InitializeDoorNames(Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            // 添加子对象名称到 List 中
            doorNames.Add(child.name);
            doorStatus.Add(false);
        }
    }

    void ToggleDoor(GameObject door)
    {
        if (door != null) {
            Transform doorShift = door.transform.parent;
            string doorName = doorShift.transform.parent.name;
            // 如果该门的状态是关闭的，则打开该门；反之亦然
            if (!doorStatus[doorNames.IndexOf(doorName)]) {
                UnityEngine.Debug.Log("Opening Door");
                OpenDoor(doorShift.gameObject);
                doorStatus[doorNames.IndexOf(doorName)] = true;
            }
            else {
                UnityEngine.Debug.Log("Closing Door");
                CloseDoor(doorShift.gameObject);
                doorStatus[doorNames.IndexOf(doorName)] = false;
            }
        }
    }

    // 执行开门动画函数
    void OpenDoor(GameObject doorShift)
    {
        doorShift.GetComponent<Animator>().SetBool("IsDoorOpen", true);
    }

    // 执行关门动画函数
    void CloseDoor(GameObject doorShift)
    {
        doorShift.GetComponent<Animator>().SetBool("IsDoorOpen", false);
    }

    // 辅助方法，用于打印二维 List 内容（可选）
    private void PrintObjectNames()
    {
        Debug.Log("Door Names:");
        foreach (string name in doorNames)
        {
            Debug.Log(name);
        }
    }

}
