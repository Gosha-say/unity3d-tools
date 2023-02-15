using UnityEngine;

public class CameraManager : MonoBehaviour {
	public Vector2 liftClamp;
	[SerializeField] private float speedNormal = .2f;
	[SerializeField] private float speedBoost = .35f;
	[SerializeField] private float lift = 1f;
	[SerializeField] private float sensitivity = .1f;
	[SerializeField] private float speed = .2f;

	Camera cam;
	private Vector3 _anchorPoint;
	private Quaternion _anchorRot;

	private void Awake() {
		cam = GetComponent<Camera>();
	}

	private void FixedUpdate() {
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");
		speed = Input.GetKey(KeyCode.LeftShift) ? speedBoost : speedNormal;
		Transform t;
		float liftDelta = Input.GetKey(KeyCode.Q) ? 1 * speed * 3 : Input.GetKey(KeyCode.E) ? -1 * speed * 3 : 0;
		lift = Mathf.Clamp(Mathf.Lerp(lift, lift + liftDelta, Time.deltaTime), liftClamp.x, liftClamp.y);

		(t = transform).Translate((Vector3.forward * v + Vector3.right * h) * speed);
		Vector3 position = t.position;
		position = new Vector3(position.x, lift, position.z);
		t.position = position;

		if(Input.GetMouseButtonDown(1)) {
			_anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
			_anchorRot = t.rotation;
		}
		if(Input.GetMouseButton(1)) {
			Quaternion rot = _anchorRot;
			Vector3 dif = _anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
			rot.eulerAngles += dif * sensitivity;
			t.rotation = rot;
		}
	}

	private void move() {
		Vector3 move = Vector3.zero;
		speed = Input.GetKey(KeyCode.LeftShift) ? speedBoost : speedNormal;
		if(Input.GetKey(KeyCode.W))
			move += Vector3.forward * speed;
		if(Input.GetKey(KeyCode.S))
			move -= Vector3.forward * speed;
		if(Input.GetKey(KeyCode.D))
			move += Vector3.right * speed;
		if(Input.GetKey(KeyCode.A))
			move -= Vector3.right * speed;
		if(Input.GetKey(KeyCode.E))
			move += Vector3.up * speed;
		if(Input.GetKey(KeyCode.Q))
			move -= Vector3.up * speed;
		transform.Translate(Vector3.LerpUnclamped(Vector3.zero, move, Time.deltaTime));
	}
}
