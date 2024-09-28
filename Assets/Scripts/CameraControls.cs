using UnityEngine;

public class CameraControls : MonoBehaviour {

    private void Start() {
        float width = GridManager.instance.width;
        float height = GridManager.instance.height;

        Camera.main.transform.position = new Vector3((width / 2) - 0.5f, (height / 2) - 0.5f, -10);
    }

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


        // Zoom in/out
        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.mouseScrollDelta.y * 0.1f;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1, 100);
    }
}
