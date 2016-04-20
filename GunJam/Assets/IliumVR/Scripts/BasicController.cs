using UnityEngine;

using IliumVR;

[AddComponentMenu("Ilium VR/Basic Controller")]
public class BasicController : MonoBehaviour
{
	public Client ivrClient;
    public IliumRewired iliumRewired;
	private GamePacket packet;
	private FeedbackPacket feedback;

	private Transform camMarker;
	private Gun GunScript;

	//
	// Joystick
	//

	[HideInInspector]
	public Transform joyMovementSpace;

	public Transform externalMovementSpace;
	public bool useExternalMovementSpace;

	public byte joystickDeadXMin = 96;
	public byte joystickDeadXMax = 160;
	public byte joystickDeadYMin = 96;
	public byte joystickDeadYMax = 160;

	//
	// ACTUAL GUN STUFF
	//

	public int clipSize = 30;
	public int maxBulletStore = 30 * 4;

	[HideInInspector]
	public int clipLeft;

	[HideInInspector]
	public int bulletStore;

	[HideInInspector]
	public bool triggered = false;

	[HideInInspector]
	public bool reloading = false;

	// Use this for initialization
	void Start ()
	{
		//Initialize client
		ivrClient = new Client(RuntimeMode.Game, RuntimeOptions.Default | RuntimeOptions.YResetsYaw, 1000);
		if (ivrClient.IsDisposed)
			ivrClient = null;

		//Select controller
		string ctrlStr;
		ivrClient.ListControllers(out ctrlStr);
		if (!string.IsNullOrEmpty(ctrlStr))
		{
			string[] controllers = ctrlStr.Split('\n');
			if (controllers.Length != 0)
				ivrClient.SelectController(controllers[0]); //HACK only use first device
		}

		//Select camera
		string camStr = null;
		ivrClient.ListCameras(out camStr);
		if (!string.IsNullOrEmpty(camStr))
		{
			string[] cameras = camStr.Split('\n');
			if (cameras.Length != 0)
				ivrClient.SelectCamera(cameras[0]); //HACK only use first device
		}

		//Select joystick movement space
		Transform gunMoveSpace = transform.Find("ivrMovementSpace");
		if (!useExternalMovementSpace && gunMoveSpace != null && gunMoveSpace.gameObject.activeInHierarchy)
			joyMovementSpace = gunMoveSpace;
		else
			joyMovementSpace = externalMovementSpace;

		//Find camera origin
		camMarker = transform.Find("MarkerOrigin");

		//Set clip sizes
		//TODO allow changes in editor
		clipLeft = clipSize;
		bulletStore = maxBulletStore;

		GunScript = GetComponentInChildren<Gun>();

		ivrClient.Start();
	}

	void Update()
	{
		//Quit on ESC
		if (Input.GetKey("escape"))
			Application.Quit();
	}

	void LateUpdate()
	{
		if (ivrClient == null)
			return;
		
		ErrorCode err = ivrClient.GetGamePacket(ref packet);

		if (err != ErrorCode.Ok)
			Debug.Log(Error.GetLastTrace());

		//Set position
		IliumVR.Geometry.Vector3 ivrp = packet.Pose.Position;
		Vector3 p = new Vector3(-ivrp.X, ivrp.Y, ivrp.Z);
		if (p.x != 0 || p.y != 0 || p.z != 0)
			transform.position = transform.TransformPoint(camMarker.InverseTransformPoint(externalMovementSpace.TransformPoint(p)));

		//Set rotation
		IliumVR.Geometry.Quaternion ivrq = packet.Pose.Rotation;
		Quaternion q = new Quaternion(-ivrq.W, ivrq.X, ivrq.Y, -ivrq.Z);
		transform.rotation = q;

		feedback.Vibration = 0;
		feedback.VibrationTime = 0;
        //iliumRewired.packet=(packet);
        ButtonState b=packet.Buttons;
        if(iliumRewired!=null){
            iliumRewired.GetSourceButtonValues(b);
            iliumRewired.GetSourceAxisValues(packet.Analog);    
        }
        
        
		//Gun firing
		if (!triggered)
		{
			if (packet.Buttons.Trigger && clipLeft > 0)
			{
				triggered = true;
				clipLeft--;
				feedback.Vibration = 255;
				feedback.VibrationTime = 100000;
				BroadcastMessage("OnIvrGunTriggerPressed");
				Debug.Log("clip(" + clipLeft + "/" + clipSize + ") store(" + bulletStore + "/" + maxBulletStore + ")");
			}
		}
		else if (!packet.Buttons.Trigger)
			triggered = false;

		//Gun reloading
		if (!reloading)
		{
			if (packet.Buttons.Slide)
			{
				int numBulletsToReload = clipSize - clipLeft;
				if (bulletStore < numBulletsToReload)
					numBulletsToReload = bulletStore;

				bulletStore -= numBulletsToReload;
				clipLeft += numBulletsToReload;
				BroadcastMessage("OnIvrGunReloadStart");
				Debug.Log("RELOADING: clip(" + clipLeft + "/" + clipSize + ") store(" + bulletStore + "/" + maxBulletStore + ")");
				reloading = true;
			}
		}
		else if (!packet.Buttons.Slide)
		{
			reloading = false;
			BroadcastMessage("OnIvrGunReloadEnd");
		}

		//Joystick X/Y
		//float moveX = 0, moveY = 0;

		//convert from [0, 256] to [-1, 1]
		//if (packet.Analog.X < joystickDeadXMin || packet.Analog.X > joystickDeadXMax)
		//moveX = ((float)packet.Analog.X / 256.0f) * 2.0f - 1.0f;

		//convert from [0, 256] to [-1, 1]
		//if (packet.Analog.Y < joystickDeadYMin || packet.Analog.Y > joystickDeadYMax)
		//moveY = ((float)packet.Analog.Y / 256.0f) * 2.0f - 1.0f;

		//ABXY
		GunScript.hasLaserPointer = packet.Buttons.A;

		//head yaw
		feedback.HeadYaw = externalMovementSpace.rotation.eulerAngles.y;

		ivrClient.SendFeedbackPacket(ref feedback);

	}

	void OnApplicationQuit()
	{
		if (ivrClient != null)
		{
			ivrClient.Dispose();
			ivrClient = null;
		}
	}
}
