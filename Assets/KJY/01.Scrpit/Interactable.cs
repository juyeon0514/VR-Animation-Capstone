using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("아이템 획득")]
    public Item itemToGive;

    [Header("아이템 사용")]
    public Item requiredItem;

    [Header("실행될 사건")]
    public UnityEvent onAction;

    [Header("상호작용 설정")]
    [SerializeField] private bool disableAfterSuccess = true;

    [Header("하이라이트")]
    [SerializeField] private bool useHighlight = true;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float highlightIntensity = 1.5f;

    [Header("UI 메시지")]
    [SerializeField] private string failMessage = "지금은 사용할 수 없습니다.";
    [SerializeField] private string successMessage = "";
    [SerializeField] private string getItemMessage = "";

    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private Material[][] highlightMaterials;

    private bool isHighlighted;
    private bool hasInteracted;

    public bool CanInteract => !hasInteracted;

    private void Awake()
    {
        if (!useHighlight)
        {
            return;
        }

        renderers = GetComponentsInChildren<Renderer>();

        originalMaterials = new Material[renderers.Length][];
        highlightMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
            highlightMaterials[i] = new Material[originalMaterials[i].Length];

            for (int j = 0; j < originalMaterials[i].Length; j++)
            {
                Material mat = new Material(originalMaterials[i][j]);

                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", highlightColor * highlightIntensity);

                highlightMaterials[i][j] = mat;
            }
        }
    }

    public void SetHighlight(bool value)
    {
        if (!useHighlight)
        {
            return;
        }

        if (hasInteracted)
        {
            value = false;
        }

        if (isHighlighted == value)
        {
            return;
        }

        isHighlighted = value;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = value ? highlightMaterials[i] : originalMaterials[i];
        }
    }

    public void OnInteract(PlayerInteraction player)
    {
        if (hasInteracted)
        {
            return;
        }

        if (requiredItem != null)
        {
            if (player.inventory.equippedItem == requiredItem)
            {
                Debug.Log(requiredItem.itemName + " 사용 성공!");

                if (!string.IsNullOrEmpty(successMessage) && InteractionUI.Instance != null)
                {
                    InteractionUI.Instance.ShowMessage(successMessage);
                }

                onAction.Invoke();

                if (disableAfterSuccess)
                {
                    DisableInteraction();
                }
            }
            else
            {
                Debug.Log(requiredItem.itemName + "이(가) 필요합니다.");

                if (InteractionUI.Instance != null)
                {
                    InteractionUI.Instance.ShowMessage(failMessage);
                }
            }

            return;
        }

        if (itemToGive != null)
        {
            player.inventory.AddItem(itemToGive);
            Debug.Log(itemToGive.itemName + " 획득!");

            if (InteractionUI.Instance != null)
            {
                if (!string.IsNullOrEmpty(getItemMessage))
                {
                    InteractionUI.Instance.ShowMessage(getItemMessage);
                }
                else
                {
                    InteractionUI.Instance.ShowMessage(itemToGive.itemName + " 획득!");
                }
            }

            DisableInteraction();
            gameObject.SetActive(false);
        }
        else
        {
            onAction.Invoke();

            if (disableAfterSuccess)
            {
                DisableInteraction();
            }
        }
    }

    private void DisableInteraction()
    {
        hasInteracted = true;
        SetHighlight(false);
    }
}
