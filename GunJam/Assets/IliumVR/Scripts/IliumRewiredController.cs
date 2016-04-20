using UnityEngine;
using System;
using System.Collections;
using Rewired;
using IliumVR;

public class IliumRewiredController : MonoBehaviour {

    private Client ivrClient;
	private GamePacket packet;
	private FeedbackPacket feedback;
    private Transform camMarker;
    /*
    * ReWired values
    */
    public int playerId;
    //the tag the custom controller corresponds to
    public string controllerTag;
    public bool useUpdateCallbacks=false;
    private CustomController controller;
	//hardcoding for now until IliumVR releases a new controller with more buttons
    private float[] axisValues = new float[2];
    private bool[] buttonValues = new bool[7];
    /*
    * Ilium Thumbstick
    */
    [HideInInspector]
	public Transform joyMovementSpace;

	public Transform externalMovementSpace;
	public bool useExternalMovementSpace;

	
     [NonSerialized]
    private bool initialized;
    private void Awake() {
        
        Initialize();
    }
	void Start () {
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

		

		
		ivrClient.Start();
	}
	
     private void Initialize() {
            Debug.Log("Initializing");
            // Subscribe to the input source update event so we can update our source element data before controllers are updated
            ReInput.InputSourceUpdateEvent += OnInputSourceUpdate;

            // Get the touch controller
           

            // Get expected element counts
           

            

            // Find the controller we want to manage
            Player player = ReInput.players.GetPlayer(playerId); // get the player
            controller = player.controllers.GetControllerWithTag<CustomController>(controllerTag); // get the controller
            
            if(controller == null) {
                Debug.LogError("A matching controller was not found for tag \"" + controllerTag + "\"");
            }

            // Verify controller has the number of elements we're expecting
            if(controller.buttonCount != buttonValues.Length || controller.axisCount != axisValues.Length) { // controller has wrong number of elements
                Debug.LogError("Controller has wrong number of elements!");
            }

            // Callback Update Method:
            // Set callbacks to retrieve current element values.
            // This is a different way of updating the element values in the controller.
            // You set an update function for axes and buttons and these functions will be called
            // to retrieve the current source element values on every update loop in which input is updated.
            if(useUpdateCallbacks && controller != null) {
                controller.SetAxisUpdateCallback(GetAxisValueCallback);
                controller.SetButtonUpdateCallback(GetButtonValueCallback);
            }

            initialized = true;
        }
        
    void LateUpdate()	{
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
		transform.localRotation = q;
        
        //GetSourceAxisValues(packet.Analog);
        //GetSourceButtonValues(packet.Buttons);
        
		//head yaw
		feedback.HeadYaw = externalMovementSpace.rotation.eulerAngles.y;
        //TODO Remove this?
		//feedback.Vibration = 0;
		//feedback.VibrationTime = 0;
		//ivrClient.SendFeedbackPacket(ref feedback);

	}
    
     private void OnInputSourceUpdate() {
            // This will be called every time the input sources are updated
            // It may be called in Update, Fixed Update, and/or OnGUI depending on the UpdateLoop setting in InputManager
            //due to the packet being set in LateUpdate, this could cause a one frame delay. Need more research.
            GetSourceAxisValues(packet.Analog);
            GetSourceButtonValues(packet.Buttons);
            if(!useUpdateCallbacks){
                SetControllerAxisValues();
                SetControllerButtonValues();
            }
        }
        
         private float GetAxisValueCallback(int index) {
            // This will be called by each axis element in the Custom Controller when updating its raw value
            // Get the current value from the source axis at index
            if(index >= axisValues.Length) return 0.0f;
            return axisValues[index];
        }

        private bool GetButtonValueCallback(int index) {
            // This will be called by each button element in the Custom Controller when updating its raw value
            // Get the current value from the source button at index
            if(index >= buttonValues.Length) return false;
            return buttonValues[index];
        }
        
        private void SetControllerAxisValues() {
            // Set the element values directly in the controller
            for(int i = 0; i < axisValues.Length; i++) {
                controller.SetAxisValue(i, axisValues[i]);
            }
        }

        private void SetControllerButtonValues() {
            // Set the element values directly in the controller
            for(int i = 0; i < buttonValues.Length; i++) {
                controller.SetButtonValue(i, buttonValues[i]);
            }
        }
        
	
    //Currently Rewired does not support vibration on Custom Controllers, so this is publicly accessible for now.
    public void sendFeedback(byte force, uint time){
        //clamp force to 255
        force=(force>(byte)255)?(byte)255:force;
        feedback.Vibration=force;
        feedback.VibrationTime=time;
        ivrClient.SendFeedbackPacket(ref feedback);
    }
    
    public void recenter(){
        
    }
    
    
    
    /**
        Map the analog values to the float array.
    */
     public void GetSourceAxisValues(AnalogState analog) {
            // Get the current element values from our source and store them
            float temp=0;
            for(int i = 0; i < axisValues.Length; i++) {
                switch(i){
                    case 0:
		                temp = (float)analog.X;
                       // Debug.Log("Raw X:"+temp);
                    break;
                    case 1:
                        temp=(float)analog.Y;
                    break;
                    default:
                    break;
                }
                //don't handle deadzones, ReWired does that for us.
                temp = (temp / 128.0f) - 1.0f;
                axisValues[i]=temp;
            }
        }
        
        
        
     public void GetSourceButtonValues(ButtonState buttons) {
            // Map the button states to our bool array
            for(int i = 0; i < buttonValues.Length; i++) {
                bool state=false;
                switch(i){
                    case 0:
                        state=buttons.Trigger;
                    break;
                    case 1:
                        state = buttons.Slide;
                    break;
                    case 2:
                        state = buttons.ClipInserted;
                    break;
                    case 3:
                        state=buttons.A;
                    break;
                    case 4:
                        state=buttons.B;
                    break;
                    case 5:
                        state=buttons.X;
                    break;
                    case 6:
                        state=buttons.Y;
                    break;
                    default:
                    break;
                }
                buttonValues[i] = state;
            }
        }
}
