using UnityEngine;
using System.Collections;

public class EventInteract : MonoBehaviour
{
	public float reachRange = 1.8f;

	private Camera fpsCam;
	private GameObject player;

	private bool playerEntered;
	[SerializeField] private int id;

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
					if (Input.GetKeyUp(KeyCode.E) || Input.GetButtonDown("Fire1"))
					{
						EventBroadcaster.current.Interact(id);
					}
				}
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
}
