using Unity.VisualScripting;
using UnityEngine;

public class SpritePickup : MonoBehaviour
{
    [SerializeField] private LightSprite spriteController;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private IntroDoor introDoor;

    [SerializeField] private GameObject spriteVisual;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float oscillateFrequency = 0.5f;
    [SerializeField] private float oscillateAmplitude = 0.1f;

    private float baseY;

    private void Start()
    {
        baseY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (dialogManager.hasCompletedDialgue())
        {
            spriteController.UnlockSprite();
            introDoor.OpenDoor();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float oscillateAmount = Mathf.Sin(Time.time * oscillateFrequency) * oscillateAmplitude;
        Vector3 newPosition = transform.position;
        newPosition.y = baseY + oscillateAmount;
        transform.position = newPosition;

        if (playerTransform != null && spriteVisual != null)
        {
            Vector3 targetPosition = new Vector3(playerTransform.position.x, spriteVisual.transform.position.y, playerTransform.position.z);
            spriteVisual.transform.LookAt(targetPosition);
            spriteVisual.transform.Rotate(0, 175, 0);  // Rotate to correct for model offset
        }
    }
}