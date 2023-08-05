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

				foreach (Attachment attachment in attachments) //get attachments in gamemanager attachment list
				{

					foreach (WeaponType type in attachment.ValidWeapons) //loop through all valid weapon types on current attachment
					{
						if (currentWeapon.WeaponType != type) // if current weapon is not a valid weapon type
						{
							continue; // go to next attachment
						}

						foreach (AttachPoint point in currentWeapon.AttachPoints) //if it IS a valid weapon, loop through all the valid attachment points on the weapon
						{

							if (attachment.MyAttachPoint != point.AttachmentPoint)
							{
								continue; // if the weapon doesn't have the point that this attachment attaches to, move to next attachment, display it
							}
							else if (GUI.Button(new Rect(10, buttonPos, buttonWidth, buttonHeight), new GUIContent($"{attachment.name} Type:{attachment.MyAttachPoint}"))) //if the button of this valid attachment is clicked
							{
								if (!currentWeapon) return;

								currentWeapon.SetupAttachment(attachment); //attach to weapon in valid slot

							}

							buttonPos += 50;//add to buttonPos, for next button

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
