using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
	public float MovementSpeed = 1;
	public GameObject camera;

	private CharacterController characterController;
	private float currentSpeed;

	void Start()
	{
        characterController = GetComponent<CharacterController>();
		currentSpeed = MovementSpeed;
	}

	void Update()
	{
		float horizontal = Input.GetAxis("Horizontal") * currentSpeed;
		float vertical = Input.GetAxis("Vertical") * currentSpeed;
		Vector3 forward = camera.transform.forward;
		forward.y = 0.0f;
		Vector3 right = camera.transform.right;
		right.y = 0.0f;
		characterController.Move((right * horizontal + forward * vertical) * Time.deltaTime);
	}

    public void StopMove()
    {
		currentSpeed = 0.0f;
		camera.GetComponent<CameraRotate>().enabled = false;
    }

	public void StartMove()
	{
		currentSpeed = MovementSpeed;
		camera.GetComponent<CameraRotate>().enabled = true;
	}
}