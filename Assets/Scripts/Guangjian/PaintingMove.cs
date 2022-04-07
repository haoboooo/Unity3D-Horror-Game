using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingMove : MonoBehaviour
{
    public Transform end; 
    public Transform start; 
    public float speed; 

    // start point
    //float xStart = -0.4141073f;
    //float yStart = 1.3f;
    //float zStart = 3.257f;

    // destination
    //float xDest = -0.4141073f;    
    //float yDest = 1.3f;
    //float zDest = 0;

    public GameObject cube;
    
    // If trigged, the item begins to move
    bool isTriggered = false;


    void Awake() {
	cube = GameObject.Find("Nun Painting");
    }

    void Update()
    {
	if(Input.GetKeyDown(KeyCode.Q)) {
	    isTriggered = true;
	}

	if(isTriggered) {
	    cube.GetComponent<Transform>().position = Vector3.MoveTowards(start.position, end.position ,speed*Time.deltaTime);
	}

  	if(start.position.z == end.position.z) {
	    isTriggered = false;
	}
    }

}
