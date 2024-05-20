import face_recognition
import os

def main(): 
    result = False
    # 指定目录路径
    current_directory = os.getcwd()
    # 遍历指定目录
    for filename in os.listdir(current_directory):
        if filename == "photo.png":
            photo = face_recognition.load_image_file(filename)
            photo_encoding = face_recognition.face_encodings(photo)[0]

    # 遍历指定目录
    for filename in os.listdir(current_directory):
        # 检查文件是否为图片文件（以.jpg、.jpeg、.png、等结尾）
        if filename.lower().endswith(('.jpg', '.jpeg', '.png', '.bmp')):
            # 检查文件是否为 photo.png
            if filename == "photo.png":
                continue
            face = face_recognition.load_image_file(filename)            
            face_encoding = face_recognition.face_encodings(face)[0]
            results = face_recognition.compare_faces([photo_encoding], face_encoding)
            if results[0] == True:
                result = True

    print(result)
            

if __name__ == "__main__":
    main()
