using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float stepInterval = 0.45f;
    [SerializeField] private float minMoveSpeed = 0.1f;

    private float stepTimer;

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        if (characterController == null)
        {
            return;
        }

        bool isMoving = characterController.velocity.magnitude > minMoveSpeed;
        bool isGrounded = characterController.isGrounded;

        if (!isMoving || !isGrounded)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f;

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SFXType.PlayerWalk);
            }
        }
    }
}
