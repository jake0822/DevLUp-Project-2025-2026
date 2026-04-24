using Unity.VisualScripting;
using UnityEngine;

public class Key : MonoBehaviour {
    [SerializeField] private GameObject keyModel;
    [SerializeField] private float rotateSpeed = 60f;
    [SerializeField] private AudioClip pickupSound;

    void Update() {
        Vector3 rotateVector = new Vector3(0f, rotateSpeed * Time.deltaTime, 0f);
        keyModel.transform.Rotate(rotateVector);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("player")) {
            PlayerKeys keyHandler = other.gameObject.GetComponent<PlayerKeys>();
            keyHandler.AddKey();
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            gameObject.SetActive(false);
        }
    }
}
