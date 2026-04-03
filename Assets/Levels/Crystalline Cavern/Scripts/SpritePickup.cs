using UnityEngine;

public class SpritePickup : MonoBehaviour
{
    [SerializeField] private LightSprite spriteController;

    private void OnTriggerEnter(Collider other)
    {
        spriteController.UnlockSprite();
        Destroy(gameObject);
    }
}
