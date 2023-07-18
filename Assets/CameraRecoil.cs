using Unity.VisualScripting;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
	private Vector3 currentRotation;
	Vector3 targetRotation;

	[SerializeField] float recoilX;
	[SerializeField] float recoilY;
	[SerializeField] float recoilZ;

	[SerializeField] float snappiness;
	[SerializeField] float returnSpeed;



	void Start()
	{

	}

	void Update()
	{
		targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
		currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

		transform.localRotation = Quaternion.Euler(currentRotation);

	}

	public void RecoilFire(float xVal, float yVal, float zVal)
	{
		targetRotation += new Vector3(xVal, Random.Range(-yVal, yVal), Random.Range(-zVal, zVal));
	}
}
