using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Transform _transform;

    public float speed = 1;

    public float rotateSpeed = 1;

    public float mouseSensitivity = 5;
    // Start is called before the first frame update
    private void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * rotateSpeed;
        float moveY = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        
        _transform.Rotate(new Vector3(0, -moveX, 0), Space.Self);
        
        _transform.Translate(-_transform.up * moveY, Space.World);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        _transform.Rotate(new Vector3(mouseX, 0, mouseY), Space.Self);
    }
}
