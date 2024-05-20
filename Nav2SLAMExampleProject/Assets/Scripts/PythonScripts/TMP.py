
# 创建一个字典，用于存储函数名和对应的委托
Dictionary<string, FunctionDelegate> functions = new Dictionary<string, FunctionDelegate> {
    { "GetTemp", GetTemp },
    { "TurnOnLight", TurnOnLight },
    { "PlayMusic", PlayMusic }
}

# 假设 str 是一个参数，表示要调用的函数名
string str = "GetTemp"

# 判断字典中是否包含 str 对应的函数名，如果存在则执行相应的函数
if (functions.ContainsKey(str)) {
    functions[str](); # 调用函数
} else {
    Console.WriteLine("函数 {str} 未找到")
}

{
  "task_sequence": [
    {
      "task_name": "FaceRecognition",
      "api_call": "FaceRecognition",
      "parameters": {
        "imageName": "photo.png"
      },
      "order": 1
    },
    {
      "task_name": "DoorOpen",
      "api_call": "DoorOpen",
      "parameters": {
        "doorName": "frontDoor"
      },
      "order": 2
    },
    {
      "task_name": "TurnOnLights",
      "api_call": "control_lights",
      "parameters": {
        "lightName": "livingRoom",
        "brightness": "50"
      },
      "order": 2
    }
  ]
}