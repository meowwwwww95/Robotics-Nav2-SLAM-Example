using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PythonScriptManager;
using System.IO;

public class AudioGPT : MonoBehaviour
{
    private AudioClip recording;
    private string filename = "recording";
    private bool isRecording = false;
    private float recordStartTime;

    // 设置 Python 脚本路径
    string pythonScriptName = "chatgpt_azure_script.py";
    const string scriptDirectory = "D:\\Projects\\Unity\\Robotics-Nav2-SLAM-Example\\Robotics-Nav2-SLAM-Example\\Nav2SLAMExampleProject\\Assets\\Scripts\\PythonScripts";

    void Update()
    {
        // 开始录音
        if (Input.GetKeyDown(KeyCode.T) && !isRecording)
        {
            StartRecording();
        }
        // 停止录音并保存
        else if (Input.GetKeyUp(KeyCode.T) && isRecording)
        {
            StopRecording();
            // 调用Python脚本
            string result = CallPythonScript(pythonScriptName);
            Debug.Log("GPT Result: " + result);
        }
    }

    void StartRecording()
    {
        string microphoneDevice = Microphone.devices[0];
        isRecording = true;
        recording = Microphone.Start(microphoneDevice, false, 300, 44100);

        Debug.Log("Recording started...");
        recordStartTime = Time.time; // 记录开始录音的时间
    }

    void StopRecording()
    {
        isRecording = false;
        Microphone.End(null);
        Debug.Log("Recording stopped");

        // 计算裁剪的样本数
        int samplesToClip = (int)((Time.time - recordStartTime) * recording.frequency);
        if (samplesToClip > 0 && samplesToClip < recording.samples)
        {
            AudioClip clippedRecording = AudioClip.Create("ClippedRecording", samplesToClip, recording.channels, recording.frequency, false);
            float[] data = new float[samplesToClip];
            recording.GetData(data, 0);
            clippedRecording.SetData(data, 0);
            recording = clippedRecording;
        }

        // 保存录音到指定路径
        SavWav.Save(filename, recording);
    }

}

