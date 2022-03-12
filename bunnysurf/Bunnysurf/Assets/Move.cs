using UnityEngine;

public class Move : MonoBehaviour
{
    private Transform _transform;
    private CharacterController _controller;

    public float moveSpeed = 2;
    public float mouseSensivity = 2;

    public Transform cameraTransform;

    private float _rot;

    private Vector3 _prevVelocity = Vector3.zero;

    [Header("Controller Settings")] 
    public float friction = 0.3f;

    public float ground_accelerate = 2f;
    public float air_accelerate = 1f;
    public float max_velocity_ground = 10f;
    public float max_velocity_air = 10f;
    public float gravity = 0.5f;

    public float jumpHeight = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        _transform = transform;
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");
        
        var tempVect = moveX * _transform.right + moveY * _transform.forward;
        tempVect *= moveSpeed * Time.deltaTime;
        tempVect.y -= gravity * Time.deltaTime;
        Vector3 currVelocity;
        if (_controller.isGrounded)
        {
            _prevVelocity.y = 0;
            if (Input.GetButton("Jump"))
            {
                tempVect = _transform.up * GetVelocityForHeight(jumpHeight, gravity);
            }
            currVelocity = MoveGround(tempVect, _prevVelocity);
        }
        else
        {
            currVelocity = MoveAir(tempVect, _prevVelocity);
        }
        
        _controller.Move(currVelocity);
        
        _prevVelocity = currVelocity;

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        
        _transform.Rotate(0, mouseX * mouseSensivity, 0, Space.Self);
        _rot -= mouseY * mouseSensivity;
        _rot = Mathf.Clamp(_rot, -90, 90);
        var rot = cameraTransform.localEulerAngles;
        rot.x = _rot;
        cameraTransform.localEulerAngles = rot;
    }
    // accelDir: normalized direction that the player has requested to move (taking into account the movement keys and look direction)
    // prevVelocity: The current velocity of the player, before any additional calculations
    // accelerate: The server-defined player acceleration value
    // max_velocity: The server-defined maximum player velocity (this is not strictly adhered to due to strafejumping)
    private Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float max_velocity)
    {
        float projVel = Vector3.Dot(prevVelocity, accelDir); // Vector projection of Current velocity onto accelDir.
        float accelVel = accelerate * Time.deltaTime; // Accelerated velocity in direction of movment

        // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
        if(projVel + accelVel > max_velocity)
            accelVel = max_velocity - projVel;

        return prevVelocity + accelDir * accelVel;
    }

    private static float GetVelocityForHeight(float height, float gravity)
    {
        return Mathf.Sqrt(2 * height * gravity);
    }

    private Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
    {
        // Apply Friction
        float speed = prevVelocity.magnitude;
        if (speed != 0) // To avoid divide by zero errors
        {
            float drop = speed * friction * Time.deltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
        }

        // ground_accelerate and max_velocity_ground are server-defined movement variables
        return Accelerate(accelDir, prevVelocity, ground_accelerate, max_velocity_ground);
    }

    private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity)
    {
        // air_accelerate and max_velocity_air are server-defined movement variables
        return Accelerate(accelDir, prevVelocity, air_accelerate, max_velocity_air);
    }
}
