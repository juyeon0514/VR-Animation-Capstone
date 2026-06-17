using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [Header("Footstep")]
    [SerializeField] private float stepInterval = 0.45f;
    [SerializeField] private bool requireGrounded = true;

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
        bool hasMoveInput =
            Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f ||
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f;

        bool isGrounded = true;

        if (requireGrounded && characterController != null)
        {
            isGrounded = characterController.isGrounded;
        }

        if (!hasMoveInput || !isGrounded)
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
