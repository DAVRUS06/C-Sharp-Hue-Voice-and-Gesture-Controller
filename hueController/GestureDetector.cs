using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System.Windows.Forms;

/* The GestureDetector class is used for implementing the gesture database into the system.
        It also detects the gestures that are being seen by the kinect.
        It will load a file named hueGestures.gbd from the directory the program is ran in.
        */


namespace hueController
{
    class GestureDetector : IDisposable
    {

        /* My gesture databse that was trained in Visual Gesture Builder w/AdaBoost */
        private readonly string gestureDatabase = @"hueGestures.gbd";

        /* My gesture names that are in the database */
        private readonly string[] gestureNames = { "colorCycle", "newLightsOff", "newLightsOn" };

        /* Frame source for a gesture that will be tied to a trackindID */
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /* Frame reader which will be read frames coming from the kinect */
        private VisualGestureBuilderFrameReader vgbFrameReader = null;

        /* Variable to give access to UI elements */
        Form1 uiAccess = null;

        /* Constructor for the gestureDetector. Takes a kinectSensor object and a form object for interacting with the UI */
        public GestureDetector(KinectSensor kinectSensor, Form1 form)
        {
            if( kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");

            }

            if(form == null)
            {
                throw new ArgumentNullException("form");
            }

            /* Save my form */
            uiAccess = form;

            /* Create the vgb source, Tracking id will be set when a body is available */
            vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);

            /* Open the kinect sensor for the frame reader */
            vgbFrameReader = vgbFrameSource.OpenReader();
            if(vgbFrameReader != null)
            {
                vgbFrameReader.IsPaused = true;
                vgbFrameReader.FrameArrived += Reader_GestureFrameArrived;
            }

            /* Load the gestures from the database */
            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(gestureDatabase))
            {
                vgbFrameSource.AddGestures(database.AvailableGestures);
                
            }
        }

        /* Getter and setter for the tracking id*/
        public ulong TrackingId
        {
            get
            {
                return vgbFrameSource.TrackingId;
            }

            set
            {
                if(vgbFrameSource.TrackingId != value)
                {
                    vgbFrameSource.TrackingId = value;
                }
            }
        }

        /* Getter and setter for isPaused */
        public bool IsPaused
        {
            get
            {
                return vgbFrameReader.IsPaused;
            }

            set
            {
                if(vgbFrameReader.IsPaused != value)
                {
                    vgbFrameReader.IsPaused = value;
                }
            }
        }

        /* Handles gesture detection results from the sensor for the body tracking id */
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if(frame != null)
                {
                    /* Frame is not null , proceed */
                    /* Gest the gesutre results from the last frame that was received */
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                    if(discreteResults != null)
                    {
                        /* not null, proceed */
                        /* Find what gesture triggered */
                        foreach(Gesture gesture in vgbFrameSource.Gestures)
                        {
                            if(gesture.Name.Equals(gestureNames[0]) && gesture.GestureType == GestureType.Discrete)
                            {
                                /* Handle the color cycle gesture */
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);
                               
                                if(result != null)
                                {
                                    if(result.Confidence > 0.5)
                                    {
                                        uiAccess.colorCycleGesture(result.Confidence);
                                    }

                                }
                            }
                            else if(gesture.Name.Equals(gestureNames[1]) && gesture.GestureType == GestureType.Discrete)
                            {
                                /* Handle the newLightsOff gesture */
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if (result != null)
                                {
                                    if (result.Confidence > 0.5)
                                    {
                                        uiAccess.lightsOffGesture(result.Confidence);
                                    }

                                }
                            }
                            else if (gesture.Name.Equals(gestureNames[2]) && gesture.GestureType == GestureType.Discrete)
                            {
                                /* Handle the newLightsOn gesture */
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if (result != null)
                                {
                                    if (result.Confidence > 0.5)
                                    {
                                        uiAccess.lightsOnGesture(result.Confidence);
                                    }

                                }

                            }

                        }
                    }
                }
            }
        }

        /* Disposal of the class object */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /* Disposal of the class object */
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(vgbFrameReader != null)
                {
                    vgbFrameReader.FrameArrived -= Reader_GestureFrameArrived;
                    vgbFrameReader.Dispose();
                    vgbFrameReader = null;
                }

                if(vgbFrameSource != null)
                {
                    vgbFrameSource.Dispose();
                    vgbFrameSource = null;
                }
            }
        }


    }
}
