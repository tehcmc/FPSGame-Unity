using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentGUI : MonoBehaviour
{
	List<Attachment> attachments = new();
	Player player;
	RangedWeapon currentWeapon;

	float buttonWidth = 250;
	float buttonHeight = 50;

	// Start is called before the first frame update
	void Start()
	{
		player = FindObjectOfType<Player>();

		attachments = GameManager.Instance.AttachmentList;




	}
	private void OnGUI()
	{
		float buttonPos = 100;
		var wepRef = player.GetComponent<WeaponInventory>().CurrentWeapon;

		if (!wepRef) return;

		if (wepRef.GetType() == typeof(RangedWeapon) || wepRef == wepRef.GetType().IsSubclassOf(typeof(RangedWeapon)))
		{

			currentWeapon = (RangedWeapon)wepRef;

			if (currentWeapon && GameManager.Instance.debugMode)
			{
				// attach 
				foreach (Attachment attachment in attachments)
				{

					foreach (WeaponType type in attachment.ValidWeapons)
					{
						if (currentWeapon.WeaponType != type)
						{
							continue;
						}

						foreach (AttachPoint point in currentWeapon.AttachPoints)
						{

							if (attachment.MyAttachPoint != point.AttachmentPoint)
							{
								continue;
							}
							else if (GUI.Button(new Rect(10, buttonPos, buttonWidth, buttonHeight), new GUIContent($"{attachment.name} Type:{attachment.MyAttachPoint}")))
							{
								if (!currentWeapon) return;

								currentWeapon.SetupAttachment(attachment);

							}
							buttonPos += 50;

						}

					}

				}
				buttonPos = 100;

				//remove
				foreach (Attachment attachment in currentWeapon.AttachmentDictionary.Values)
				{

					if (GUI.Button(new Rect(Screen.width - buttonWidth - 10, buttonPos, buttonWidth, buttonHeight), new GUIContent($"{attachment.name} Type:{attachment.MyAttachPoint}")))
					{
						if (!currentWeapon) return;
						currentWeapon.RemoveAttachment(attachment.MyAttachPoint);

					}
					buttonPos += 50;


				}


			}

		}

	}

}
