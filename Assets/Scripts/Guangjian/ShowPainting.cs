using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPainting : MonoBehaviour
{
    public GameObject obj;
    
    public float x = 3.229163f;
    public float y = 0.1f;
    public float z = 0.91f; 
    

    // Update is called once per frame
    void Update()
    {
	if(Input.GetKeyDown(KeyCode.R)) {
            Instantiate(obj, new Vector3(x,y,z), Quaternion.Euler(-90,0,0));
        }
    }
}
