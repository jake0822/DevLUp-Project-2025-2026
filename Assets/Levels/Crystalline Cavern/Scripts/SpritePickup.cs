using UnityEngine;

public class SpritePickup : MonoBehaviour
{
    [SerializeField] private LightSprite spriteController;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private IntroDoor introDoor;

    private void FixedUpdate()
    {
        if (dialogManager.hasCompletedDialgue())
        {
            spriteController.UnlockSprite();
            introDoor.OpenDoor();
            Destroy(gameObject);
        }
    }
}
