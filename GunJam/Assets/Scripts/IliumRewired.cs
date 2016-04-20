using UnityEngine;
using System;
using System.Collections;
using Rewired;
    
using IliumVR;

public class IliumRewired : MonoBehaviour {
        public int playerId;
        public string controllerTag;
        public bool useUpdateCallbacks;
        public BasicController bc;
        public bool problem=false;
        public string errorCode="";
        private float[] axisValues;
        private bool[] buttonValues;
        private CustomController controller;
        //IliumVR stuff
        private Client ivrClient;
        public GamePacket packet;
	    private FeedbackPacket feedback;
        
        [NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
        private bool initialized;
	// Use this for initialization
	void Start () {
	    ivrClient=bc.ivrClient;
	}
	
	// Update is called once per frame
	void Update () {
	     if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
         if(!initialized) Initialize(); // Reinitialize after a recompile in the editor
	}
    
    private void Awake() {
           
            Initialize();
        }

        private void Initialize() {
            Debug.Log("Initializing");
            // Subscribe to the input source update event so we can update our source element data before controllers are updated
            ReInput.InputSourceUpdateEvent += OnInputSourceUpdate;

            // Get the touch controller
           

            // Get expected element counts
           

            // Set up arrays to store our current source element values
            axisValues = new float[2];
            buttonValues = new bool[7];

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
        
        private void OnInputSourceUpdate() {
            // This will be called every time the input sources are updated
            // It may be called in Update, Fixed Update, and/or OnGUI depending on the UpdateLoop setting in InputManager
            // If you need to know what update loop this was called in, check currentUpdateLoop

            // Update the source element values from our source, whatever that may be
            
            if (ivrClient == null){
                Debug.Log("ivrClient is null!");
                
                //ivrClient=bc.ivrClient;
                return;    
            }
			problem=false;
		
		    //ErrorCode err = ivrClient.GetGamePacket(ref packet);
            
          //  GetSourceAxisValues(packet);
           // GetSourceButtonValues(packet);

            // Set the current values directly in the controller
            if(!useUpdateCallbacks) { // if not using update callbacks, set the values directly, otherwise controller values will be updated via callbacks
                SetControllerAxisValues();
                SetControllerButtonValues();
            }
        }
        
         public void GetSourceAxisValues(AnalogState ivrPacket) {
             
             
            // Get the current element values from our source and store them
            for(int i = 0; i < axisValues.Length; i++) {
                switch(i){
                    case 0:
                        
                        axisValues[i]=ivrPacket.X/126;
                    break;
                    case 1:
                        axisValues[i]=ivrPacket.Y/126;
                    break;
                    default:
                    break;
                }
                /*
                if(i % 2 != 0) {// odd
                    axisValues[i] = touchController.joysticks[i/2].position.y;
                } else { // even
                    axisValues[i] = touchController.joysticks[i / 2].position.x;
                }
                */
            }
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

        public void GetSourceButtonValues(ButtonState ivrPacket) {
            
            
            // Get the current element values from our source and store them
            for(int i = 0; i < buttonValues.Length; i++) {
                bool state=false;
                switch(i){
                    case 0:
                        
                        state=ivrPacket.Trigger;
                        problem=state;
                        //state=Input.GetKey(KeyCode.Space);
                    break;
                    case 1:
                        state = ivrPacket.Slide;
                    break;
                    case 2:
                        state = ivrPacket.ClipInserted;
                    break;
                    case 3:
                        state=ivrPacket.A;
                    break;
                    case 4:
                        state=ivrPacket.B;
                    break;
                    case 5:
                        state=ivrPacket.X;
                    break;
                    case 6:
                        state=ivrPacket.Y;
                    break;
                    default:
                    break;
                }
                buttonValues[i] = state;
            }
        }
    
    // Callbacks

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
}
