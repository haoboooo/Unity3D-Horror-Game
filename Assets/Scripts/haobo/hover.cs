using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover : MonoBehaviour
{
    private Color CubeColor;
    
    private GameObject Obj;
    // Start is called before the first frame update
    void Start()
    {
        Obj = GameObject.Find("ca");
        CubeColor = Obj.GetComponent<Renderer>().material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseOver()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    // 鼠标点击
    void OnMouseDown()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.green;
    }


    //鼠标离开
    void OnMouseUp()
    {
        transform.GetComponent<MeshRenderer>().material.color = CubeColor;
    }

}
