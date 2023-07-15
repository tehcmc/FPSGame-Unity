
using UnityEngine;
using UnityEngine.UI;

public class DestroyOnEnd : MonoBehaviour
{

	public void DestroyObject()
	{
		Destroy(gameObject.transform.parent.gameObject);
	}

}
