import face_recognition

photo = face_recognition.load_image_file("photo.png")
photo_encoding = face_recognition.face_encodings(photo)[0]
player = face_recognition.load_image_file("player.png")
player_encoding = face_recognition.face_encodings(player)[0]
results = face_recognition.compare_faces([photo_encoding], player_encoding)
if results[0] == True:
    print("The same person!")
else:
    print("Different person!")
