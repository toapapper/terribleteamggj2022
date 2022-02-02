using UnityEngine;
public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	Transform player;
	[SerializeField]
	float speed;

	[SerializeField] 
	CameraBounds2D bounds;
	Vector2 maxXPositions, maxYPositions;


	void Start()
	{
		bounds.Initialize(GetComponentInChildren<Camera>());
		maxXPositions = bounds.maxXlimit;
		maxYPositions = bounds.maxYlimit;
	}

	void FixedUpdate()
	{
		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = new Vector3(Mathf.Clamp(player.position.x, maxXPositions.x, maxXPositions.y), Mathf.Clamp(player.position.y, maxYPositions.x, maxYPositions.y), currentPosition.z);
		transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * speed);

		//if (targetPosition.x > minX && targetPosition.x < maxX && targetPosition.y > minY && targetPosition.y < maxY)
		//	transform.position = newPos;
	}
}
