using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour {
	public Transform playerRoot;
	[SerializeField] float playerScreenRelativePosition = 3f;
	[SerializeField] float maxPlayerScreenPosition = .9f;
	[SerializeField] float minCameraOrthographicSize = 5f;
	[SerializeField] float cameraFloorOffset = -5f;
	[SerializeField] int cameraFloorBufferLength = 20;
	[SerializeField] float cameraDamping = .3f;
	[SerializeField] float yPadding = 2f;

	float cameraDepth = -10f;
	float cameraFloor = 0f;
	float orthographicWidth {
		get {
			return camera.orthographicSize * camera.aspect;
		}
	}
	LayerMask raycastMask;
	float[] cameraFloorBuffer;
	int bufferIndex = 0;

	// Use this for initialization
	void Start () {
		raycastMask = ~(1 << (playerRoot.gameObject.layer));
		cameraFloorBuffer = new float[cameraFloorBufferLength];

		cameraDepth = transform.position.z;
		transform.position = CalculateCameraTargetPosition();
		camera.orthographicSize = Mathf.Max(transform.position.y - cameraFloor, minCameraOrthographicSize);
	}
	
	void LateUpdate () {
		Vector3 targetPosition = CalculateCameraTargetPosition();
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraDamping);
		camera.orthographicSize = Mathf.Max(transform.position.y - cameraFloor, minCameraOrthographicSize);
		
	}

	Vector3 CalculateCameraTargetPosition() {
		Vector2 playerPosition = new Vector2(playerRoot.position.x, playerRoot.position.y + yPadding);

		RaycastHit2D hit = Physics2D.Raycast(playerPosition, new Vector2(0, -1), Mathf.Infinity, raycastMask);
		cameraFloorBuffer[bufferIndex++] = hit.point.y + cameraFloorOffset;
		if (bufferIndex >= cameraFloorBuffer.Length) bufferIndex = 0;

		float newCameraFloor = Mathf.Infinity;
		foreach (float yValue in cameraFloorBuffer) {
			newCameraFloor = Mathf.Min(newCameraFloor, yValue);
		}
		cameraFloor = newCameraFloor;

		float playerHeight = playerPosition.y - cameraFloor;
		float targetOrthographicHeight = playerHeight * (1 / maxPlayerScreenPosition) * .5f;
		targetOrthographicHeight = Mathf.Max(targetOrthographicHeight, minCameraOrthographicSize);

		float targetOrthographicWidth = targetOrthographicHeight * camera.aspect;

		return new Vector3(playerPosition.x + (targetOrthographicWidth * playerScreenRelativePosition),
		                   targetOrthographicHeight + cameraFloor, cameraDepth);
	}
}