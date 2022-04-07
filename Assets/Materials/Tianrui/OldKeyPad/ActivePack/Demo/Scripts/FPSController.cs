using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BigBlit.ActivePack.Buttons
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        [SerializeField] Camera playerCamera = null;

        [Range(0.01f, 1.0f)]
        [SerializeField] float _smoothTime = 0.25f;
        [SerializeField] float walkingSpeed = 7.5f;
        [SerializeField] float runningSpeed = 11.5f;

        [SerializeField] float lookSpeed = 2.0f;
        [SerializeField] float lookXLimit = 45.0f;

        [SerializeField] bool _pointerEnable = true;
        [SerializeField] bool _controllerEnable = false;
        [SerializeField] bool _run = false;
        [SerializeField] bool softwareCursor = true;

        [SerializeField] Texture2D _cursorTexture = null;
        [SerializeField] Texture2D _cursorTextureDown = null;


        Vector2 _mouseRotValues;
        Vector2 _mouseSmootingVel;
        CharacterController characterController;
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        private bool canMove = true;

        void Start() {
            characterController = GetComponent<CharacterController>();
        }

        private void OnEnable() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (softwareCursor)
                Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void OnDisable() {
            Cursor.lockState = CursorLockMode.None;
           Cursor.visible = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
 
        void Update() {
            if(Input.GetMouseButtonDown(0)) {
                if (softwareCursor)
                    Cursor.SetCursor(_cursorTextureDown, Vector2.zero, CursorMode.ForceSoftware);
        
            } else if(Input.GetMouseButtonUp(0)) {
                if (softwareCursor)
                    Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            }

            if (Input.GetKey(KeyCode.LeftShift) == true) {
                _run = true;
            }
            else
                _run = false; 

            if (Input.GetKeyDown(KeyCode.Space) == true) {
                if (_pointerEnable) {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                _pointerEnable = !_pointerEnable;
            }

            if (Input.GetKeyDown(KeyCode.Tab) == true) {
                _controllerEnable = !_controllerEnable;
            }

            if (!_controllerEnable)
                return;

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            _mouseRotValues = Vector2.SmoothDamp(
                _mouseRotValues, new Vector3(Input.GetAxis("Mouse X") * lookSpeed,
                Input.GetAxis("Mouse Y") * lookSpeed), ref _mouseSmootingVel, _smoothTime);

            float curSpeedX = canMove ? (_run ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (_run ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove) {
                rotationX += -_mouseRotValues.y;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, _mouseRotValues.x, 0);
            }
        }
    }
}