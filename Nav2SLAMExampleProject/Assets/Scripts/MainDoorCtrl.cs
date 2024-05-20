using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PythonScriptManager;
using System.Threading;
using System.Threading.Tasks;

public class MainDoorCtrl : MonoBehaviour
{
    // 设置 Python 脚本路径
    string pythonScriptName = "face_recognition_script.py";

    // 设置 Python 脚本所在目录
    string scriptDirectory = "D:\\Projects\\Unity\\Robotics-Nav2-SLAM-Example\\Robotics-Nav2-SLAM-Example\\Nav2SLAMExampleProject\\Assets\\Scripts\\PythonScripts";

    private int maxLoopCount = 3;
    private bool isMainDoorOpen = false;
    private bool isPlayerInArea = false;
    
    public bool autoCloseFunction = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Someone's outside the maindoor.");

            isPlayerInArea = true;
            if (transform.Find("DoorCamera") != null)
                TakePhotos();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = false;
            if (autoCloseFunction)
                CloseDoor();
        }
    }

    private async void TakePhotos()
    {
        int loopCount = 0;
        bool isPassed = false;

        while (isPlayerInArea && !isPassed && loopCount < maxLoopCount)
        {
            string pythonScriptResult = "";

            // 创建一个线程来调用 CallPythonScript 方法
            Thread thread = new Thread(() =>
            {
                pythonScriptResult = PythonScriptManager.CallPythonScript(pythonScriptName);
                Debug.Log("Python script result: " + pythonScriptResult);
            });

            // 启动线程
            thread.Start();

            // 等待线程执行完毕，并继续主线程的执行
            await Task.Run(() => thread.Join());

            // 根据结果执行相应操作
            isPassed = bool.Parse(pythonScriptResult);

            // 如果满足条件则退出循环
            if (isPassed)
            {
                OpenDoor();
                break;
            }

            // 计数器增加
            loopCount++;
        }

        Debug.Log("Background thread finished.");
    }

    private void TakePhoto()
    {
        // 拍照逻辑
        Camera doorCamera = transform.GetComponent<Camera>();
        doorCamera.Render();
        Texture2D photo = new Texture2D(doorCamera.pixelWidth, doorCamera.pixelHeight);
        photo.ReadPixels(new Rect(0, 0, doorCamera.pixelWidth, doorCamera.pixelHeight), 0, 0);
        photo.Apply();

        // 保存照片
        byte[] bytes = photo.EncodeToPNG();
        System.IO.File.WriteAllBytes(scriptDirectory + "\\photo.png", bytes);
    }

    // 执行开门动画函数
    void OpenDoor()
    {
        Debug.Log("Opening Main Door");
        Transform doorShift = transform.Find("DoorShift");
        doorShift.GetComponent<Animator>().SetBool("IsDoorOpen", true);
        isMainDoorOpen = true;
    }

    // 执行关门动画函数
    void CloseDoor()
    {
        Debug.Log("Closing Main Door");
        Transform doorShift = transform.Find("DoorShift");
        doorShift.GetComponent<Animator>().SetBool("IsDoorOpen", false);
        isMainDoorOpen = false;
    }
}
