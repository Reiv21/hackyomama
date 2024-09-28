using UnityEngine;

public class CameraControls : MonoBehaviour {

    private void Update() {
        Vector3 pos = transform.position;
        float speed = 5;
        if (Input.GetKey(KeyCode.LeftShift)) {
            speed *= 3;
        }
        if (Input.GetKey(KeyCode.W)) {
            pos.y += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            pos.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            pos.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            pos.x += speed * Time.deltaTime;
        }
        transform.position = pos;
    }
}
