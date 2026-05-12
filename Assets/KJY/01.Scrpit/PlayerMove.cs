using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); // A, D Ű
        float z = Input.GetAxis("Vertical");   // W, S Ű

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}
