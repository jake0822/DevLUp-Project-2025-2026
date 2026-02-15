using UnityEngine;

public class Key : MonoBehaviour {
    [SerializeField] private GameObject keyModel;
    [SerializeField] private float rotateSpeed = 60f;

    void Update() {
        Vector3 rotateVector = new Vector3(0f, rotateSpeed * Time.deltaTime, 0f);
        keyModel.transform.Rotate(rotateVector);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("key collected");
            Destroy(gameObject);
        }
    }
}
