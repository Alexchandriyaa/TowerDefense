using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera moving and autoscaling.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
	public float moveSensitivityX = 0.1f;
	public float moveSensitivityZ = 0.1f;
	public bool updateZoomSensitivity = true;
	public float orthoZoomSpeed = 0.05f;
	public float minZoom = 1.0f;
	public float maxZoom = 5f;  //best set at 20
	public bool invertMoveX = false;
	public bool invertMoveZ = false;
	public float mapWidth = 40f;
	public float mapLength = 30f;

	public float inertiaDuration =1f;

	private Camera _camera;

	private float minX, maxX, minY, maxY;
	private float horizontalExtent, verticalExtent;

	private float scrollVelocity = 0.0f;
	private float timeTouchPhaseEnded = 0;
	private Vector3 scrollDirection = Vector3.zero;
	public enum ControlType
	{
		ConstantWidth,	// Camera will keep constant width
		ConstantHeight	// Camera will keep constant height
	}

	// Camera control type
	public ControlType controlType;
	// Horizontal offset from focus object edges
	public float offsetX = 0f;
	// Vertical offset from focus object edges
	public float offsetY = 0f;
	// Camera speed when moving (dragging)
	public float dragSpeed = 2f;
	// Camera dragging at now vector
	private float moveX, moveY;
	void Start () 
	{
		_camera = Camera.main;

		//maxZoom = 0.5f * (mapWidth / _camera.aspect);  //use this if you need max zoom more than 20

		//if (mapWidth > mapLength)  //use this if you need max zoom more than 20
		//    maxZoom = 0.5f * (mapLength/ _camera.aspect);  //use this if you need max zoom more than 20

		if (_camera.orthographicSize > maxZoom)
			_camera.orthographicSize = maxZoom;

		CalculateLevelBounds ();
	}

	void Update () 
	{
		if (updateZoomSensitivity)
		{
			moveSensitivityX = _camera.orthographicSize / 5.0f;
			moveSensitivityZ = _camera.orthographicSize / 5.0f;
		}

		Touch[] touches = Input.touches;

		if (touches.Length < 1)
		{
			//if the camera is currently scrolling
			if (scrollVelocity != 0.0f)
			{
				//slow down over time
				float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
				float frameVelocity = Mathf.Lerp (scrollVelocity, 5.5f, t);
				_camera.transform.position += -(Vector3)scrollDirection.normalized * (frameVelocity * 0.5f) * Time.deltaTime;

				if (t >= 1.0f)
					scrollVelocity = 0.0f;
			}
		}

		if (touches.Length > 0)
		{
			//Single touch (move)
			if (touches.Length == 1) {
				if (touches [0].phase == TouchPhase.Began) {
					scrollVelocity = 0.0f;
				
					if (moveX != 0f) {
						// Allowed move horizontally
						if (controlType == ControlType.ConstantHeight) {
							bool permit = false;
							// Move to right
							if (moveX > 0f) {
								// If restrictive point does not reached
								if (_camera.transform.position.x + (_camera.orthographicSize * _camera.aspect) < maxX - offsetX) {
									permit = true;
								}
							}
						// Move to left
						else {
								// If restrictive point does not reached
								if (_camera.transform.position.x - (_camera.orthographicSize * _camera.aspect) > minX + offsetX) {
									permit = true;
								}
							}
							if (permit == true) {
								// Move camera
								transform.Translate (Vector3.right * moveX * dragSpeed, Space.World);
							}
						}
						moveX = 0f;
					}
					// Need to move camera vertically
					if (moveY != 0f) {
						// Allowed move vertically
						if (controlType == ControlType.ConstantWidth) {
							bool permit = false;
							// Move up
							if (moveY > 0f) {
								// If restrictive point does not reached
								if (_camera.transform.position.y + _camera.orthographicSize < maxY - offsetY) {
									permit = true;
								}
							}
						// Move down
						else {
								// If restrictive point does not reached
								if (_camera.transform.position.y - _camera.orthographicSize > minY + offsetY) {
									permit = true;
								}
							}
							if (permit == true) {
								// Move camera
								transform.Translate (Vector3.up * moveY * dragSpeed, Space.World);
							}
						}
						moveY = 0f;
					}
				}
			}

			//Double touch (zoom)
			if (touches.Length == 2)
			{
				Vector3 cameraViewsize = new Vector3 (_camera.pixelWidth, _camera.pixelHeight);

				Touch touchOne = touches[0];
				Touch touchTwo = touches[1];

				Vector3 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				Vector3 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

				float prevTouchDeltaMag = (touchOnePrevPos - touchTwoPrevPos).magnitude;
				float touchDeltaMag = (touchOne.position - touchTwo.position).magnitude;

				float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;
				_camera.orthographicSize += deltaMagDiff * orthoZoomSpeed;

				// Make sure the orthographic size never drops below zero.
				_camera.orthographicSize = Mathf.Max(_camera.orthographicSize, 3.2f);

				_camera.transform.position += _camera.transform.TransformDirection ((touchOnePrevPos + touchTwoPrevPos - cameraViewsize) * _camera.orthographicSize / cameraViewsize.y);

				_camera.orthographicSize += deltaMagDiff * orthoZoomSpeed;
			_camera.orthographicSize = Mathf.Clamp (_camera.orthographicSize, minZoom, maxZoom) - 0.001f;

				_camera.transform.position -= _camera.transform.TransformDirection (((Vector3)touchOne.position + (Vector3)touchTwo.position - cameraViewsize) * _camera.orthographicSize / cameraViewsize.y);

				CalculateLevelBounds ();
			}
		}
	}

	void CalculateLevelBounds ()
	{
		verticalExtent = _camera.orthographicSize;
		horizontalExtent = _camera.orthographicSize * Screen.width / Screen.height;
		minX = horizontalExtent - mapWidth / 4.5f;
		maxX = mapWidth / 4.5f - horizontalExtent;
		minY = verticalExtent - mapLength / 5.9f;
		maxY = mapLength / 6.0f - verticalExtent;
	}

	void LateUpdate ()
	{
		Vector3 limitedCameraPosition = _camera.transform.position;
		limitedCameraPosition.x = Mathf.Clamp (limitedCameraPosition.x, minX, maxX);
		limitedCameraPosition.y = Mathf.Clamp (limitedCameraPosition.y, minY, maxY);
		_camera.transform.position = limitedCameraPosition;
	}
	public void MoveX(float distance)
	{
		moveX = distance;
	}

	/// <summary>
	/// Need to move camera vertically.
	/// </summary>
	/// <param name="distance">Distance.</param>
	public void MoveY(float distance)
	{
		moveY = distance;
	}


}
