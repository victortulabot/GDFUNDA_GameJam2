using UnityEngine;
using System.Collections;

public class EventInteract : MonoBehaviour
{
	public float reachRange = 1.8f;

	private Camera fpsCam;
	private GameObject player;

	private bool isFixed = false;
	private bool allowInteractions;

	private bool playerEntered;
	private bool showInteractMsg;
	[SerializeField] private int id;

	private GUIStyle guiStyle;
	private string msg;

	private int rayLayerMask;

	void Start()
	{
		//Initialize moveDrawController if script is enabled.
		player = GameObject.FindGameObjectWithTag("Player");

		fpsCam = Camera.main;
		if (fpsCam == null) //a reference to Camera is required for rayasts
		{
			Debug.LogError("A camera tagged 'MainCamera' is missing.");
		}

		//the layer used to mask raycast for interactable objects only
		LayerMask iRayLM = LayerMask.NameToLayer("InteractRaycast");
		rayLayerMask = 1 << iRayLM.value;

		setupGui();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)     //player has collided with trigger
		{
			playerEntered = true;

		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player)     //player has exited trigger
		{
			playerEntered = false;
			showInteractMsg = false;
		}
	}

	void Update()
	{
		if (playerEntered)
		{

			//center point of viewport in World space.
			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hit;

			//if raycast hits a collider on the rayLayerMask
			if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, reachRange, rayLayerMask))
			{
				MoveableObject moveableObject = null;
				//is the object of the collider player is looking at the same as me?
				if (!isEqualToParent(hit.collider, out moveableObject))
				{   //it's not so return;
					return;
				}

				if (moveableObject != null)     //hit object must have MoveableDraw script attached
				{
					setupGui();
					showInteractMsg = true;

					if (Input.GetKeyUp(KeyCode.E) || Input.GetButtonDown("Fire1"))
					{
						if(GameObject.Find("EventBroadcaster").GetComponent<EventBroadcaster>().allowInteractions)
                        {
							isFixed = true;
							showInteractMsg = false;
							msg = "Fixed";
							EventBroadcaster.current.Interact(id);
						}
					}
				}
			}
			else
			{
				showInteractMsg = false;
			}
		}
	}

	//is current gameObject equal to the gameObject of other.  check its parents
	private bool isEqualToParent(Collider other, out MoveableObject draw)
	{
		draw = null;
		bool rtnVal = false;
		try
		{
			int maxWalk = 6;
			draw = other.GetComponent<MoveableObject>();

			GameObject currentGO = other.gameObject;
			for (int i = 0; i < maxWalk; i++)
			{
				if (currentGO.Equals(this.gameObject))
				{
					rtnVal = true;
					if (draw == null) draw = currentGO.GetComponentInParent<MoveableObject>();
					break;          //exit loop early.
				}

				//not equal to if reached this far in loop. move to parent if exists.
				if (currentGO.transform.parent != null)     //is there a parent
				{
					currentGO = currentGO.transform.parent.gameObject;
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(e.Message);
		}

		return rtnVal;

	}

	#region GUI Config

	//configure the style of the GUI
	private void setupGui()
	{
		guiStyle = new GUIStyle();
		guiStyle.fontSize = 16;
		guiStyle.fontStyle = FontStyle.Bold;
		guiStyle.normal.textColor = Color.white;

		if (GameObject.Find("EventBroadcaster").GetComponent<EventBroadcaster>().allowInteractions)
			msg = "Press E/Fire1 to Fix";
		else
			msg = "Listening...";
	}

	private string getGuiMsg(bool isFixed)
	{
		string rtnVal;
		if (isFixed)
		{
			rtnVal = "Press E/Fire1 to Fix";
		}
		else
		{
			rtnVal = "Fixed";
		}

		return rtnVal;
	}

	void OnGUI()
	{
		if (showInteractMsg)  //show on-screen prompts to user for guide.
		{
			GUI.Label(new Rect (50,Screen.height - 50,200,50), msg,guiStyle);
		}
	}		
	//End of GUI Config --------------
	#endregion
}
