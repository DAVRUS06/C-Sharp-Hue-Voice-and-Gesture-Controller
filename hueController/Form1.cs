using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System.Runtime.InteropServices;

namespace hueController
{
    public partial class Form1 : Form
    {

        /* Kinect objects */
        private KinectSensor kinectSensor = null;                   /* Kinect sensor */
        private Body[] bodies = null;                               /* Array for bodies in room */
        private BodyFrameReader bodyFrameReader = null;             /* Reads body frames in kinect frames */
        private List<GestureDetector> gestureDetectorList = null;   /* List of gesture detectors */
        private ColorFrameReader colorFrameReader = null;
        private WriteableBitmap colorBitmap = null;



        /* Hue Connection objects */
        static hueConnect controller = new hueConnect();
        private List<SimpleLight> LightList = new List<SimpleLight>();
        private List<SimpleGroup> GroupList = new List<SimpleGroup>();
        public XY colorXY = new XY();
        
        /* Speech Recognition objects */
        static SpeechSynthesizer ss = new SpeechSynthesizer();  /* For Text-to-Speech TTS */
        static SpeechRecognitionEngine sre;                     /* For speech recognition */
        static bool talk = false;                               /* For having TTS on/off */
        Choices groupNames;                                     /* Holds the names of the groups */
        Choices lightNames;                                     /* Holds the names of the lights */
        Choices lightColors;                                    /* Holds the names of the colors */
        Choices brightnessRange;                                /* Holds the values for brightness */
        Choices saturationRange;                                /* Holds the values for saturation */
        Choices OnOff;                                          /* Holds the values for on/off */
        GrammarBuilder gb_TurnLightOnOff;                       /* Holds the grammar layout for turning lights on/off */
        Grammar g_TurnLightsOnOff;                              /* Holds the finalized grammar object for turning lights on/off */
        GrammarBuilder gb_changeColor;                          /* Holds the grammar layout for changing colors*/
        Grammar g_changeColor;                                  /* Holds the finalized grammar object for changing colors*/
        GrammarBuilder gb_changeBri;                            /* Holds the grammar layout for changing brightness*/
        Grammar g_changeBri;                                    /* Holds the finalized grammar object for changing brightness*/
        GrammarBuilder gb_changeSat;                            /* Holds the grammar layout for changing saturation*/
        Grammar g_changeSat;                                    /* Holds the finalized grammar object for changing saturation*/
        GrammarBuilder gb_colorloop;                            /* Holds the grammar layout for changing colorloop*/
        Grammar g_colorLoop;                                    /* Holds the finalized grammar object for changing colorloop*/
        GrammarBuilder gb_TurnAllOnOff;                         /* Holds the grammar layout for turning all lights on/off*/
        Grammar g_TurnAllOnOff;                                 /* Holds the finalized grammar object for turning all lights on/off*/
        GrammarBuilder gb_changeAllColors;                      /* Holds the grammar layout for changing all light colors */
        Grammar g_changeAllColors;                              /* Holds the finalized grammar object for changing all light colors */
        GrammarBuilder gb_turnGroupOnOff;                       /* Holds the grammar layout for turning a group on/off */
        Grammar g_turnGroupOnOff;                               /* Holds the finalized grammar object for turning a group on/off*/
        GrammarBuilder gb_changeGroupColor;                     /* Holds the grammar layout for changing a group color*/
        Grammar g_changeGroupColor;                             /* Holds the finalized grammar object for changing a group color */
        GrammarBuilder gb_changeGroupBrightness;                /* Holds the grammar layout for changing a group brightness */
        Grammar g_changeGroupBrightness;                        /* Holds the finalized grammar object for changing a group brightness */

        /* Class that holds constants */
        RGBColors AllColors = new RGBColors();


        /* Used for stopping the gestures from triggering rapidly */
        DateTime ccLastTrigger;
        DateTime lightsonLastTrigger;
        DateTime lightsoffLastTrigger;
        bool ccOn = false;

        public Form1()
        {
            InitializeComponent();

            

            /* Read bridge info from file */
            textBoxIP.Text = controller.getIP();
            textBoxUserKey.Text = controller.getUserKey();

            /* Check the IP and userKey */
            if(controller.testConnection())
            {
                /* Display connection status */
                labelConnectStatus.Text = "Active";
                labelConnectStatus.ForeColor = System.Drawing.Color.Green;
                /* Start the system*/
                InitHueSystem();

                /* Start the kinect system */
                InitKinectSystem();
            }
            else
            {

                /* Display connection status */
                labelConnectStatus.Text = "Inactive - Check IP/UserToken";
                labelConnectStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        

        /* This function is responsible for setting up the kinect system on startup */
        private void InitKinectSystem()
        {
            /*Init the datetimes */
            ccLastTrigger = new DateTime();
            ccLastTrigger = DateTime.Now;
            lightsonLastTrigger = new DateTime();
            lightsonLastTrigger = DateTime.Now;
            lightsoffLastTrigger = new DateTime();
            lightsoffLastTrigger = DateTime.Now;

            /* Get the default kinect sensor */
            kinectSensor = KinectSensor.GetDefault();

            
            /* Set helper function for if its available */
            kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            /* Open the sensor */
            kinectSensor.Open();

            /* Open a reader for the bodyframes to be read */
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();

            /* Setup bodyframeevent notifier */
            bodyFrameReader.FrameArrived += Reader_BodyFrameArrived;

            /* Initilize the gesture detectors for the gestures */
            gestureDetectorList = new List<GestureDetector>();

            int maxBodies = kinectSensor.BodyFrameSource.BodyCount;
            /* Going to create a gesture detector for the 6 bodies capable of tracking */
            for (int i = 0; i < maxBodies; i++)
            {
                GestureDetector detector = new GestureDetector(kinectSensor, this);
                gestureDetectorList.Add(detector);
            }

            colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();
            colorFrameReader.FrameArrived += Reader_ColorFrameArrived;
            FrameDescription colorFrameDescription = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

        }


        /* Displays the new image from the camera onto the window for the user to see */
        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    var width = colorFrame.FrameDescription.Width;
                    var height = colorFrame.FrameDescription.Height;
                    var data = new byte[width * height * 32 / 8];
                    colorFrame.CopyConvertedFrameDataToArray(data, ColorImageFormat.Bgra);
                    var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
                    bitmap.UnlockBits(bitmapData);
                    pictureBoxKinect.Image = bitmap;

                }
            }
        }
        

        /* Function to check if the kinect is available */
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            if (kinectSensor.IsAvailable)
            {
                //labelGestureOnOff.Text = "Active";
                //labelGestureOnOff.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                //labelGestureOnOff.Text = "Inactive";
                //labelGestureOnOff.ForeColor = System.Drawing.Color.Red;
            }
        }

        /* Function to handle if a bodyframe has arrived */
        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if(bodyFrame != null)
                {
                    if(bodies == null)
                    {
                        /* Init the body array if it hasn't been alraedy, set to max of 6*/
                        bodies = new Body[bodyFrame.BodyCount];
                    }
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataReceived = true;
                }
            }

            if(dataReceived)
            {
                if(bodies != null)
                {
                    /* Make sure to update the bodies if we lost/gained some */
                    int maxBodies = kinectSensor.BodyFrameSource.BodyCount;
                    for(int i = 0; i < maxBodies; ++i)
                    {
                        Body body = bodies[i];
                        ulong trackingId = body.TrackingId;

                        /* If the trackinkgID changed then update the gesture detector */

                        if(trackingId != gestureDetectorList[i].TrackingId)
                        {

                            gestureDetectorList[i].TrackingId = trackingId;

                            /*if the body is not being tracked then pause the detector */
                            gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                }
            }
        }



        /* This function is responsible for setting up the hueConnect on startup as well as voice commands */
        private void InitHueSystem()
        {
           

            /* Get the light list for use later */
            controller.GetLights();
            LightList = controller.GetLightList();
            /* Get the group list for use later */
            controller.GetGroups();
            GroupList = controller.GetGroupList();

            /* Put the color choices in the textbox */
            for (int i = 0; i < AllColors.ColorChoices.Length; i++)
            {
                textBoxColorChoices.Text += AllColors.ColorChoices[i] + Environment.NewLine;
            }

            /* Print the name of the lights to the textbox */
            for (int i = 0; i < LightList.Count; i++)
            {
                textBoxLightNames.Text += LightList[i].Name + Environment.NewLine;
            }

            /* Print the name of the groups to the textbox */
            for (int i = 0; i < GroupList.Count; i++)
            {
                textBoxGroupNames.Text += GroupList[i].Name + Environment.NewLine;
            }

            /* Start the speech recognition setup */
            try
            {
                /* Set the text to speech to the default output */
                ss.SetOutputToDefaultAudioDevice();

                /* Set up the recognition engine */
                CultureInfo info = new CultureInfo("en-us");
                sre = new SpeechRecognitionEngine(info);
                sre.SetInputToDefaultAudioDevice();
                sre.SpeechRecognized += Sre_SpeechRecognized;

                /* Get the names of the lights and put them in the choices variable */
                lightNames = new Choices();
                for (int i = 0; i < LightList.Count; i++)
                {
                    lightNames.Add(LightList[i].Name);
                }

                /* Get the names of the groups and put them in the choices variable */
                groupNames = new Choices();
                for (int i = 0; i < GroupList.Count; i++)
                {
                    groupNames.Add(GroupList[i].Name);
                }

                /* Get the names of the colors and put them in the choices variable */
                lightColors = new Choices();
                for (int i = 0; i < AllColors.ColorChoices.Count(); i++)
                {
                    lightColors.Add(AllColors.ColorChoices[i]);
                }

                /* Range of numbers for brightness */
                brightnessRange = new Choices();
                for (int i = 1; i < 101; i++)
                {
                    brightnessRange.Add(i.ToString());
                }

                /* Range of numbers for saturation */
                saturationRange = new Choices();
                for (int i = 0; i < 101; i++)
                {
                    saturationRange.Add(i.ToString());
                }

                /* For turning lights on/off and setting colorloop on/off */
                OnOff = new Choices();
                OnOff.Add("on");
                OnOff.Add("off");

                /* Start building the grammar variables */

                /* For turning lights on or off
                    EX: Turn light 1 on/off */
                gb_TurnLightOnOff = new GrammarBuilder();
                gb_TurnLightOnOff.Append("Turn");
                gb_TurnLightOnOff.Append(lightNames);
                gb_TurnLightOnOff.Append(OnOff);
                g_TurnLightsOnOff = new Grammar(gb_TurnLightOnOff);

                /* For turning all lights on or off
                    EX: Turn all on/off */
                gb_TurnAllOnOff = new GrammarBuilder();
                gb_TurnAllOnOff.Append("Turn all");
                gb_TurnAllOnOff.Append(OnOff);
                g_TurnAllOnOff = new Grammar(gb_TurnAllOnOff);

                /* For changing a light color
                    EX: Change light 1 color blue */
                gb_changeColor = new GrammarBuilder();
                gb_changeColor.Append("Change");
                gb_changeColor.Append(lightNames);
                gb_changeColor.Append("color");
                gb_changeColor.Append(lightColors);
                g_changeColor = new Grammar(gb_changeColor);

                /* For changing all lights color
                    EX: Change all lights color blue */
                gb_changeAllColors = new GrammarBuilder();
                gb_changeAllColors.Append("Change all lights color");
                gb_changeAllColors.Append(lightColors);
                g_changeAllColors = new Grammar(gb_changeAllColors);

                /* For changing the brightness of a light 
                    EX: Change light 1 brightness 100 percent */
                gb_changeBri = new GrammarBuilder();
                gb_changeBri.Append("Change");
                gb_changeBri.Append(lightNames);
                gb_changeBri.Append("brightness");
                gb_changeBri.Append(brightnessRange);
                gb_changeBri.Append("percent");
                g_changeBri = new Grammar(gb_changeBri);

                /* For changing the saturation of a light
                    EX: Change light 1 saturation 100 percent */
                gb_changeSat = new GrammarBuilder();
                gb_changeSat.Append("Change");
                gb_changeSat.Append(lightNames);
                gb_changeSat.Append("saturation");
                gb_changeSat.Append(saturationRange);
                gb_changeSat.Append("percent");
                g_changeSat = new Grammar(gb_changeSat);

                /* For turning on/off colorloop 
                    EX: Color cycle on/off */
                gb_colorloop = new GrammarBuilder();
                gb_colorloop.Append("Color cycle");
                gb_colorloop.Append(OnOff);
                g_colorLoop = new Grammar(gb_colorloop);

                /* For turning a single group on/off 
                    EX: Turn group groupname on/off */
                gb_turnGroupOnOff = new GrammarBuilder();
                gb_turnGroupOnOff.Append("Turn group");
                gb_turnGroupOnOff.Append(groupNames);
                gb_turnGroupOnOff.Append(OnOff);
                g_turnGroupOnOff = new Grammar(gb_turnGroupOnOff);

                /* For changing the color of a group 
                    EX: Change group groupname color blue */
                gb_changeGroupColor = new GrammarBuilder();
                gb_changeGroupColor.Append("Change group");
                gb_changeGroupColor.Append(groupNames);
                gb_changeGroupColor.Append("color");
                gb_changeGroupColor.Append(lightColors);
                g_changeGroupColor = new Grammar(gb_changeGroupColor);

                /* For changing the color of a group 
                    EX: Change group groupname brightness 100 percent */
                gb_changeGroupBrightness = new GrammarBuilder();
                gb_changeGroupBrightness.Append("Change group");
                gb_changeGroupBrightness.Append(groupNames);
                gb_changeGroupBrightness.Append("brightness");
                gb_changeGroupBrightness.Append(brightnessRange);
                gb_changeGroupBrightness.Append("percent");
                g_changeGroupBrightness = new Grammar(gb_changeGroupBrightness);


                /* Load all grammars into the engine */
                sre.LoadGrammar(g_TurnLightsOnOff);
                sre.LoadGrammar(g_changeColor);
                sre.LoadGrammar(g_changeBri);
                sre.LoadGrammar(g_changeSat);
                sre.LoadGrammar(g_colorLoop);
                sre.LoadGrammar(g_TurnAllOnOff);
                sre.LoadGrammar(g_changeAllColors);
                sre.LoadGrammar(g_changeGroupColor);
                sre.LoadGrammar(g_turnGroupOnOff);
                sre.LoadGrammar(g_changeGroupBrightness);


                /* Start the recognition */
                sre.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch (Exception e)
            {

            }
        }

        /* Function that the speech recognition engine uses for matching functions to commands */
        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txtResult = e.Result.Text;
            float confidenceLevel = e.Result.Confidence;
            textBoxDetectedCommands.Text += "I heard your say = " + txtResult + Environment.NewLine;

            /* Low confidence should return as it may have been misheard */
            if(confidenceLevel < 0.5)
            {
                textBoxActionsTaken.Text += "Low confidence of command \"" + 
                                            txtResult + "\", Confidence Score: " + confidenceLevel +
                                            " Command ignored" + Environment.NewLine;
                return;
            }

            if (txtResult.IndexOf("Turn") >= 0 && txtResult.IndexOf("all") >= 0 && txtResult.IndexOf("group") == -1 && txtResult.IndexOf("brightness") == -1 && txtResult.IndexOf("saturation") == -1)
            {
                /* For turning all lights on or off
                    EX: Turn all on/off */
                string[] tokens = txtResult.Split(' ');
                if (tokens[tokens.Count() - 1].Equals("on"))
                {
                    textBoxActionsTaken.Text += "Turned all of the lights on." + Environment.NewLine;
                    for (int i = 0; i < LightList.Count; i++)
                    {
                        controller.TurnGroupOnOff(true, "0");
                    }
                }
                else
                {
                    textBoxActionsTaken.Text += "Turned all of the lights off." + Environment.NewLine;
                    for (int i = 0; i < LightList.Count; i++)
                    {
                        controller.TurnGroupOnOff(false, "0");
                    }
                }
            }
            else if (txtResult.IndexOf("Turn") >= 0 && txtResult.IndexOf("group") == -1 && txtResult.IndexOf("brightness") == -1 && txtResult.IndexOf("saturation") == -1)
            {
                /* For turning lights on or off
                    EX: Turn light 1 on/off */
                string[] tokens = txtResult.Split(' ');
                /* Grab the light name */
                string lightname = "";
                for (int i = 1; i < tokens.Count() - 1; i++)
                {
                    if (i != tokens.Count() - 2)
                        lightname += tokens[i] + " ";
                    else
                        lightname += tokens[i];
                }

                string id = getLightID(lightname);
                if (tokens[tokens.Count() - 1].Equals("on"))
                {
                    controller.TurnLightOnOff(true, id);
                    textBoxActionsTaken.Text += "Turned " + lightname + " on." + Environment.NewLine;
                }
                else
                {
                    controller.TurnLightOnOff(false, id);
                    textBoxActionsTaken.Text += "Turned " + lightname + " off." + Environment.NewLine;
                }
            }
            else if (txtResult.IndexOf("Change") >= 0 && txtResult.IndexOf("color") >= 0 && txtResult.IndexOf("all") == -1 && txtResult.IndexOf("group") == -1 && txtResult.IndexOf("saturation") == -1 && txtResult.IndexOf("brightness") == -1)
            {
                /* For changing a light color
                    EX: Change light 1 color blue */
                string[] tokens = txtResult.Split(' ');
                // Grab the light name
                string lightname = "";
                for (int i = 1; i < tokens.Count() - 2; i++)
                {
                    if (i < tokens.Count() - 3)
                        lightname += tokens[i] + " ";
                    else
                        lightname += tokens[i];

                }
                string id = getLightID(lightname);
                string color = tokens[tokens.Count() - 1];
                RGB chosenColor = AllColors.returnColor(color);
                controller.SetLightSat(254, id);
                controller.SetLightColor(chosenColor, id);
                textBoxActionsTaken.Text += "Changed color of " + lightname + " to " + color + Environment.NewLine;
            }
            else if (txtResult.IndexOf("Change") >= 0 && txtResult.IndexOf("color") >= 0 && txtResult.IndexOf("all") >= 0 && txtResult.IndexOf("group") == -1 && txtResult.IndexOf("saturation") == -1 && txtResult.IndexOf("brightness") == -1)
            {
                /* For changing all lights color
                    EX: Change all lights color blue */
                string[] tokens = txtResult.Split(' ');
                string color = tokens[tokens.Count() - 1];
                RGB chosenColor = AllColors.returnColor(color);
                controller.SetGroupColor(chosenColor, "0");
                textBoxActionsTaken.Text += "Changed color of all lights to " + color + Environment.NewLine;
            }
            else if (txtResult.IndexOf("Change") >= 0 && txtResult.IndexOf("brightness") >= 0 && txtResult.IndexOf("percent") >= 0 && txtResult.IndexOf("saturation") == -1 && txtResult.IndexOf("group") == -1)
            {
                /* For changing the brightness of a light 
                    EX: Change light 1 brightness 100 percent */
                string[] tokens = txtResult.Split(' ');
                /* Grab the light name */
                string lightname = "";
                for (int i = 1; i < tokens.Count() - 3; i++)
                {
                    if (i < tokens.Count() - 4)
                        lightname += tokens[i] + " ";
                    else
                        lightname += tokens[i];
                }
                string id = getLightID(lightname);
                int bright = Int32.Parse(tokens[tokens.Count() - 2]);
                /*User gives a values 1-100, convert it to be 1-254 */
                int bri = (int)(254.0 * ((double)bright / 100.0));
                if (bri < 1)
                    bri = 1;
                else if (bri > 254)
                    bri = 254;
                controller.SetLightBright(bri, id);
                textBoxActionsTaken.Text += "Changed the brightness of " + lightname + " to " + bri + Environment.NewLine;
            }
            else if (txtResult.IndexOf("Change") >= 0 && txtResult.IndexOf("saturation") >= 0 && txtResult.IndexOf("percent") >= 0 && txtResult.IndexOf("brightness") == -1 && txtResult.IndexOf("group") == -1)
            {
                /* For changing the saturation of a light
                    EX: Change light 1 saturation 100 percent */
                textBoxActionsTaken.Text = "sat" + Environment.NewLine;
                string[] tokens = txtResult.Split(' ');
                /* Grab the light name */
                string lightname = "";
                for (int i = 1; i < tokens.Count() - 3; i++)
                {
                    if (i < tokens.Count() - 4)
                        lightname += tokens[i] + " ";
                    else
                        lightname += tokens[i];

                }
                string id = getLightID(lightname);
                int saturation = Int32.Parse(tokens[tokens.Count() - 2]);
                /*User gives a values 1-100, convert it to be 1-254 */
                int sat = (int)(254.0 * ((double)saturation / 100.0));
                if (sat < 1)
                    sat = 1;
                else if (sat > 254)
                    sat = 254;
                controller.SetLightSat(saturation, id);
                textBoxActionsTaken.Text += "Changed the saturation of " + lightname + " to " + sat + Environment.NewLine;
            }
            else if (txtResult.IndexOf("Color") >= 0 && txtResult.IndexOf("cycle") >= 0 && txtResult.IndexOf("group") == -1)
            {
                /* For turning on/off colorloop 
                    EX: Color cycle on/off */
                string[] tokens = txtResult.Split(' ');
                string effect = "";
                if (tokens[tokens.Count() - 1].Equals("on"))
                {
                    effect = "colorloop";
                    textBoxActionsTaken.Text += "Turned colorloop on." + Environment.NewLine;
                }
                else
                {
                    effect = "none";
                    textBoxActionsTaken.Text += "Turned colorloop off." + Environment.NewLine;
                }

                controller.SetColorLoop(effect);
            }
            else if(txtResult.IndexOf("Turn") >= 0 && txtResult.IndexOf("group") >= 0)
            {

                /* For turning a single group on/off 
                    EX: Turn group groupname on/off */
                
                string[] tokens = txtResult.Split(' ');
                // Grab the light name
                string groupName = "";
                for (int i = 2; i < tokens.Count() - 1; i++)
                {
                    if (i < tokens.Count() - 2)
                        groupName += tokens[i] + " ";
                    else
                        groupName += tokens[i];

                }
                string id = getGroupID(groupName);
                if (tokens[tokens.Count() - 1].Equals("on"))
                {
                    controller.TurnGroupOnOff(true, id);
                    textBoxActionsTaken.Text += "Turned " + groupName + " on." + Environment.NewLine;
                }
                else
                {
                    controller.TurnGroupOnOff(false, id);
                    textBoxActionsTaken.Text += "Turned " + groupName + " off." + Environment.NewLine;
                }
            }
            else if(txtResult.IndexOf("Change") >= 0 && txtResult.IndexOf("group") >= 0 && txtResult.IndexOf("color") >= 0 && txtResult.IndexOf("brightness") == -1)
            {
                /* For changing the color of a group 
                    EX: Change group groupname color blue */
                string[] tokens = txtResult.Split(' ');
                // Grab the light name
                string groupName = "";
                for (int i = 2; i < tokens.Count() - 2; i++)
                {
                    if (i < tokens.Count() - 3)
                        groupName += tokens[i] + " ";
                    else
                        groupName += tokens[i];

                }
                string id = getGroupID(groupName);
                string color = tokens[tokens.Count() - 1];
                RGB chosenColor = AllColors.returnColor(color);
                controller.SetGroupColor(chosenColor, id);
                textBoxActionsTaken.Text += "Changed color of " + groupName + " to " + color + Environment.NewLine;
            }
            else if(txtResult.IndexOf("Change") >= 0 && txtResult.IndexOf("group") >= 0 && txtResult.IndexOf("brightness") >= 0 && txtResult.IndexOf("percent") >=0)
            {
                /* For changing the color of a group 
                    EX: Change group groupname brightness 100 percent */
                string[] tokens = txtResult.Split(' ');

                /* Grab the name of the group */
                string groupName = "";
                for (int i = 2; i < tokens.Count() - 2; i++)
                {
                    if (i < tokens.Count() - 3)
                        groupName += tokens[i] + " ";
                    else
                        groupName += tokens[i];

                }
                string id = getGroupID(groupName);
                int bright = Int32.Parse(tokens[tokens.Count() - 2]);
                /*User gives a values 1-100, convert it to be 1-254 */
                int bri = (int)(254.0 * ((double)bright / 100.0));
                if (bri < 1)
                    bri = 1;
                else if (bri > 254)
                    bri = 254;
                controller.SetGroupBrightness(bri, id);
                textBoxActionsTaken.Text += "Changed the brightness of " + groupName + " to " + bri + Environment.NewLine;

            }
            else
                textBoxActionsTaken.Text += "Unrecognized command." + Environment.NewLine;
        }

        /* Used to get the ID of a light */
        private string getLightID(string name)
        {
            for(int i = 0; i < LightList.Count; i++)
            {
                if (name.Equals(LightList[i].Name))
                    return LightList[i].ID;
            }
            return "0";
        }

        /* Used to get the ID of a group */
        private string getGroupID(string name)
        {
            for(int i = 0; i < GroupList.Count; i++)
            {
                if (name.Equals(GroupList[i].Name))
                    return GroupList[i].ID;
            }
            return "0";
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        /* Makes sure that the textbox scroll to the bottom when needed */
        private void textBoxDetectedCommands_TextChanged(object sender, EventArgs e)
        {
            textBoxDetectedCommands.SelectionStart = textBoxDetectedCommands.Text.Length;
            textBoxDetectedCommands.ScrollToCaret();
        }

        /* Makes sure that the textbox scroll to the bottom when needed */
        private void textBoxActionsTaken_TextChanged(object sender, EventArgs e)
        {
            textBoxActionsTaken.SelectionStart = textBoxActionsTaken.Text.Length;
            textBoxActionsTaken.ScrollToCaret();
        }

        /* Save IP and userKey */
        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            string tempIP = controller.getIP();
            string tempUserKey = controller.getUserKey();

            controller.setIP(textBoxIP.Text);
            controller.setUserKey(textBoxUserKey.Text);

            /* Check the IP and userKey */
            if (controller.testConnection())
            {
                /* Display connection status */
                labelConnectStatus.Text = "Active";
                labelConnectStatus.ForeColor = System.Drawing.Color.Green;
                /* Start the system*/
                InitHueSystem();
            }
            else
            {

                /* Display connection status */
                labelConnectStatus.Text = "Inactive - Check IP/UserToken";
                labelConnectStatus.ForeColor = System.Drawing.Color.Red;

                /* Reset IP and Key to previous settings */
                controller.setIP(tempIP);
                controller.setUserKey(tempUserKey);
            }

        }

        /* Handles the closing event, cleans up all the variables for the kinect */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /**/
            if (this.colorFrameReader != null)
            {
                // ColorFrameReder is IDisposable
                this.colorFrameReader.Dispose();
                this.colorFrameReader = null;
            }

            if (bodyFrameReader != null)
            {
                bodyFrameReader.FrameArrived -= Reader_BodyFrameArrived;
                bodyFrameReader.Dispose();
                bodyFrameReader = null;
            }

            if(gestureDetectorList != null)
            {
                /* Dispose each member in the list */
                for(int i = 0; i < gestureDetectorList.Count; i++)
                {
                    gestureDetectorList[i].Dispose();
                }
                gestureDetectorList.Clear();
                gestureDetectorList = null;
            }

            if(kinectSensor != null)
            {
                /* Set the kinect to null */
                kinectSensor.IsAvailableChanged -= Sensor_IsAvailableChanged;
                kinectSensor.Close();
                kinectSensor = null;
            }


        }

        /* Function the gesutureDetector will call when it gest a lights off gesture */
        public void lightsOffGesture(float confidence)
        {
            /*Only trigger if it has been longer than 5 seconds since the last one. */
            DateTime temp = new DateTime();
            temp = DateTime.Now;
            if((temp - lightsoffLastTrigger).TotalSeconds > 5.0)
            {
                lightsoffLastTrigger = DateTime.Now;
                SetText("Received the lights off gesture with a confidence of " + confidence + Environment.NewLine);
                controller.TurnGroupOnOff(false, "0");
            }
        }

        /* Function the gestureDetector will call when it gets a lights on gesture */
        public void lightsOnGesture(float confidence)
        {
            /*Only trigger if it has been longer than 5 seconds since the last one. */
            DateTime temp = new DateTime();
            temp = DateTime.Now;
            if ((temp - lightsonLastTrigger).TotalSeconds > 5.0)
            {
                lightsonLastTrigger = DateTime.Now;
                SetText("Received the lights on gesture with a confidence of " + confidence + Environment.NewLine);
                controller.TurnGroupOnOff(true, "0");
            }
            
        }

        /* Function the gestureDetector will call when it gets a color cycle gesture */
        public void colorCycleGesture(float confidence)
        {
            /*Only trigger if it has been longer than 5 seconds since the last one. */
            DateTime temp = new DateTime();
            temp = DateTime.Now;
            if ((temp - ccLastTrigger).TotalSeconds > 5.0)
            {
                ccLastTrigger = DateTime.Now;
                SetText("Received the color cycle gesture with a confidence of " + confidence + Environment.NewLine);
                if(ccOn)
                {
                    controller.SetColorLoop("none");
                    ccOn = false;
                }
                else
                {
                    controller.SetColorLoop("colorloop");
                    ccOn = true;

                }
                    
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Start the kinect system */
            //InitKinectSystem();
            //Thread newThread = new Thread(InitKinectSystem);
            
            //newThread.Start();

           
            

        }

        delegate void setTextCallback(string text);
        public void SetText(string text)
        {
            if(this.textBoxActionsTaken.InvokeRequired)
            {
                setTextCallback d = new setTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBoxActionsTaken.Text += text;
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                return colorBitmap;
            }
        }

        













    }
}
