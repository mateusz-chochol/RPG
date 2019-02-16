using UnityEngine;

public class CameraController : MonoBehaviour {

    public float cameraMoveSpeed = 120.0f;
    public Transform cameraFollowObject;
    public float clampAngle = 80f;
    public float inputSensitivity = 150f;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    private float rotationX = 0f;
    private float rotationY = 0f;
    public bool isRotationFreezed = false;

    private void Start() {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotationX = rotation.x;
        rotationY = rotation.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        //Rotation part
        if (!isRotationFreezed) {
            float inputX = Input.GetAxis("RightStickHorizontal");
            float inputZ = Input.GetAxis("RightStickVertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            finalInputX = inputX + mouseX;
            finalInputZ = inputZ + mouseY;

            rotationX += finalInputZ * inputSensitivity * Time.deltaTime;
            rotationY += finalInputX * inputSensitivity * Time.deltaTime;

            rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);

            Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
            transform.rotation = localRotation;
        }
    }

    private void LateUpdate() {
        CameraUpdater();
    }

    private void CameraUpdater() {
        //Movement part
        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, cameraFollowObject.position, step);
    }
}

#region Previous Camera Controller
/*using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;
    public Vector3 offset;
    public float pitch;
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;
    public float currentZoom;
    public float rotationSpeed;

    private float rotationInputXAxis = 0f;
    private float rotationInputYAxis = 0f;
    private Vector3 desiredOffset;
    private float desiredZoom;

    private void Start() {
        desiredOffset = offset;
        desiredZoom = currentZoom;
    }

    private void Update() {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        rotationInputXAxis += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        rotationInputYAxis += Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
    }

    private void LateUpdate() {
        transform.position = player.position - offset * currentZoom;
        transform.LookAt(player.position + Vector3.up * pitch);
        transform.RotateAround(player.position, Vector3.right, rotationInputYAxis);
        transform.RotateAround(player.position, Vector3.up, rotationInputXAxis);
    }
}*/
#endregion
#region Unnecesarry variables for the top camera script
/*private Vector3 followPosition;
public GameObject cameraObject;
public GameObject playerObject;
public float cameraDistanceXToPlayer;
public float cameraDistanceYToPlayer;
public float cameraDistanceZToPlayer;
public float smoothX;
public float smoothY;*/
#endregion
