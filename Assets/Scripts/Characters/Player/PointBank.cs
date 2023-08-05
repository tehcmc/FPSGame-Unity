using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBank : MonoBehaviour
{

	[SerializeField] int startPoints = 0;
	public int currentPoints { get; protected set; } = 0;


	void Start()
	{
		currentPoints = startPoints;
	}



	public bool CanSubtractPoints(int val)
	{

		if (currentPoints - val >= 0)
		{
			return true;
		}
		return false;

	}

	public void SubtractPoints(int val)
	{
		ModifyPoints(-Mathf.Abs(val));
	}
	public void AddPoints(int val)
	{
		ModifyPoints(Mathf.Abs(val));
	}

	void ModifyPoints(int val)
	{

		if (val == 0) return;
		if (currentPoints + val >= int.MaxValue) return; //prevent int wraparound

		var oldPoints = currentPoints;
		currentPoints += val;
		currentPoints = Mathf.Clamp(currentPoints, 0, int.MaxValue);
		Debug.Log($"current points: {currentPoints}");
	}

}
