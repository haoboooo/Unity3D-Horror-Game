using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyLivingRoomDoor : MonoBehaviour
{
    float startTime;
    float stayTime;
    public Transform player;
    public PlayerControl playerControl;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        startTime = 0;
        // Debug.Log(parent.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player) {
            
            string currentRoom = playerControl.currentRoom;
            // update staytime and starttime
            stayTime = playerControl.secondsElapsed - startTime;
            startTime = playerControl.secondsElapsed;
            if (currentRoom == "BoyLivingRoom") {
                playerControl.currentRoom = "Corridor";
                playerControl.eachRoomStayTime["BoyLivingRoom"] = (float)(playerControl.eachRoomStayTime["BoyLivingRoom"]) + stayTime;
                playerControl.eachRoomEnterTime["Corridor"] = (int)(playerControl.eachRoomEnterTime["Corridor"]) + 1;
                Debug.Log("Enter Corridor, " + "staytime= " + stayTime);
            }
            if (currentRoom == "BathRoom") {
                playerControl.currentRoom = "Corridor";
                playerControl.eachRoomStayTime["BathRoom"] = (float)(playerControl.eachRoomStayTime["BathRoom"]) + stayTime;
                playerControl.eachRoomEnterTime["Corridor"] = (int)(playerControl.eachRoomEnterTime["Corridor"]) + 1;
                Debug.Log("Enter Corridor, " + "staytime= " + stayTime);
            }
            if (currentRoom == "SecretRoom") {
                playerControl.currentRoom = "StudyRoom";
                playerControl.eachRoomStayTime["SecretRoom"] = (float)(playerControl.eachRoomStayTime["SecretRoom"]) + stayTime;
                playerControl.eachRoomEnterTime["StudyRoom"] = (int)(playerControl.eachRoomEnterTime["StudyRoom"]) + 1;
                Debug.Log("Enter StudyRoom, " + "staytime= " + stayTime);
            }
            if (currentRoom == "StudyRoom"&& parent.name == "StudyRoom") {
                playerControl.currentRoom = "Corridor";
                playerControl.eachRoomStayTime["StudyRoom"] = (float)(playerControl.eachRoomStayTime["StudyRoom"]) + stayTime;
                playerControl.eachRoomEnterTime["Corridor"] = (int)(playerControl.eachRoomEnterTime["Corridor"]) + 1;
                Debug.Log("Enter Corridor, " + "staytime= " + stayTime);
            }
            //记得把triggersec放到secretroom下
            if (currentRoom == "StudyRoom"&& parent.name == "SecretRoom") {
                playerControl.currentRoom = "SecretRoom";
                playerControl.eachRoomStayTime["StudyRoom"] = (float)(playerControl.eachRoomStayTime["StudyRoom"]) + stayTime;
                playerControl.eachRoomEnterTime["SecretRoom"] = (int)(playerControl.eachRoomEnterTime["SecretRoom"]) + 1;
                Debug.Log("Enter SecretRoom, " + "staytime= " + stayTime);
            }
            if (currentRoom == "Corridor" && parent.name == "BoyLivingRoom") {
                playerControl.currentRoom = "BoyLivingRoom";
                playerControl.eachRoomStayTime["Corridor"] = (float)(playerControl.eachRoomStayTime["Corridor"]) + stayTime;
                playerControl.eachRoomEnterTime["BoyLivingRoom"] = (int)(playerControl.eachRoomEnterTime["BoyLivingRoom"]) + 1;
                Debug.Log("Enter BoyLivingRoom, " + "staytime= " + stayTime);
            }
            if (currentRoom == "Corridor" && parent.name == "BathRoom") {
                playerControl.currentRoom = "BathRoom";
                playerControl.eachRoomStayTime["Corridor"] = (float)(playerControl.eachRoomStayTime["Corridor"]) + stayTime;
                playerControl.eachRoomEnterTime["BathRoom"] = (int)(playerControl.eachRoomEnterTime["BathRoom"]) + 1;
                Debug.Log("Enter BathRoom, " + "staytime= " + stayTime);
            }
            if (currentRoom == "Corridor" && parent.name == "StudyRoom") {
                playerControl.currentRoom = "StudyRoom";
                playerControl.eachRoomStayTime["Corridor"] = (float)(playerControl.eachRoomStayTime["Corridor"]) + stayTime;
                playerControl.eachRoomEnterTime["StudyRoom"] = (int)(playerControl.eachRoomEnterTime["StudyRoom"]) + 1;
                Debug.Log("Enter StudyRoom, " + "staytime= " + stayTime);
            }
            
        }
    }
}
