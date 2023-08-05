using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
	[SerializeField] protected string objectName = string.Empty;
	public string ObjectName { get => objectName; set => objectName = value; }

	public virtual bool CanPurchase()
	{
		return true;
	}
	public virtual void PurchaseItem()
	{

	}

	public static System.Type GetMyType(BaseObject obj)
	{
		return obj.GetType();
	}
}
