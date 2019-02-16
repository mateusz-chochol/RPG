using UnityEngine;

public class CameraEnabler : MonoBehaviour {

    public GameObject screenPointer;

    private void Start() {
        screenPointer.SetActive(true);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleCursorAndCameraRotation();
        }
    }

    public void ToggleCursorAndCameraRotation() {
        if (!GetComponent<CameraController>().isRotationFreezed) {
            ShowCursorAndStopCameraRotation();
        }
        else {
            HideCursorAndStartCameraRotation();
        }
    }

    public void ShowCursorAndStopCameraRotation() {
        GetComponent<CameraController>().isRotationFreezed = true;
        screenPointer.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursorAndStartCameraRotation() {
        screenPointer.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<CameraController>().isRotationFreezed = false;
    }
}
