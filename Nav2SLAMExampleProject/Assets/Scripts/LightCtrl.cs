using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCtrl : MonoBehaviour
{
    // 二维 List 用于存储所有灯泡对象名称
    private List<List<string>> objectNames = new List<List<string>>();

    public float interactionDistance = 5f; // 交互距离
    public KeyCode interactKey = KeyCode.F; // 交互键

    void Start()
    {
        // 初始化 DoorGroup 下的直接子对象
        InitializeLightsNames(transform);

        // 打印二维 List 内容（可选）
        // PrintObjectNames();
    }

    void Update()
    {
        // 检测 F 键是否被按下
        if (Input.GetKeyDown(interactKey)) {
            Debug.Log("Button F is pressed");
            // 射线检测，从摄像机发出一条射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 射线检测
            if (Physics.Raycast(ray, out hit, interactionDistance)) {
                Debug.Log("Within Distance");

                // 检测到射线击中了灯泡开关
                if (hit.collider.CompareTag("Switch")) {
                    Debug.Log("Hit Switch Collider");

                    // 获取灯泡对象
                    GameObject lightBulb = GetCorrespondingLightBulb(hit.collider.gameObject);

                    if (lightBulb != null) {
                        // 调用开关灯函数
                        ToggleLight(lightBulb);
                    }
                }
            }
        }
    }

    void InitializeLightsNames(Transform transform)
    {
        // 获取LightGroup下的所有直接子对象
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // 遍历直接子对象，获取其子对象的名称
        for (int i = 0; i < children.Length; i++)
        {
            List<string> childNames = new List<string>();
            childNames.Add(children[i].name); // 添加父对象的名称

            Transform childTransform = children[i];
            for (int j = 0; j < childTransform.childCount; j++)
            {
                childNames.Add(childTransform.GetChild(j).name); // 添加子对象的名称
            }

            // 将子对象名称存入 List 中
            objectNames.Add(childNames);
        }
    }

    GameObject GetCorrespondingLightBulb(GameObject switchObject)
    {
        // 根据 Switch 对象获取对应的灯泡对象
        string lightBulbName = switchObject.name.Replace("Switch", "Light");
        return GameObject.Find(lightBulbName);
    }

    void ToggleLight(GameObject lightBulb)
    {
        GameObject lightSquareObject = lightBulb.transform.Find("LightSquare").gameObject;
        MeshRenderer lightSquareRenderer = lightSquareObject.GetComponent<MeshRenderer>();

        Light spotLight = lightBulb.GetComponentInChildren<Light>();

        if (lightSquareRenderer != null && spotLight != null)
        {
            if (!spotLight.enabled)
            {
                TurnOnLight(lightSquareRenderer, spotLight);
            }
            else
            {
                TurnOffLight(lightSquareRenderer, spotLight);
            }
        }
    }

    void TurnOnLight(MeshRenderer lightSquareRenderer, Light spotLight)
    {
        Debug.Log("Turning on Light");
        // 修改材质
        // lightSquareRenderer.material = yourOnMaterial;
        spotLight.enabled = true;
    }

    void TurnOffLight(MeshRenderer lightSquareRenderer, Light spotLight)
    {
        Debug.Log("Turning off Light");
        // 修改材质
        // lightSquareRenderer.material = yourOffMaterial;
        spotLight.enabled = false;
    }

    // 辅助方法，用于打印二维 List 内容（可选）
    private void PrintObjectNames()
    {
        for (int i = 0; i < objectNames.Count; i++)
        {
            Debug.Log("LightGroup: " + objectNames[i][0]);
            for (int j = 1; j < objectNames[i].Count; j++)
            {
                Debug.Log("    " + objectNames[i][j]);
            }
        }
    }
}
