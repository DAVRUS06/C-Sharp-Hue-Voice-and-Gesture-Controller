using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

/*  The hueConnect class is responsible for performing the operations with the hue system.
 *      All operations are done with the API that the bridge provides to the network. 
 *      As of right now the userKey and BridgeIP need to be supplied by the user for connection.
 *      The IP address and userKey can be aquired by following the Philips Hue Developer getting started guide.
 *      https://developers.meethue.com/develop/get-started-2/
    
     */

namespace hueController
{
    /* This class will hold the functions for interacting with the HUE system. */
    class hueConnect
    {
        private string UserKey = "";
        private string BridgeIP = "";
        private JObject LightResponse;
        private JObject GroupResponse;
        private JObject TestResponse;
        private List<SimpleLight> LightList = new List<SimpleLight>();
        private List<SimpleGroup> GroupList = new List<SimpleGroup>();

        /* Return the list of the lights in the system */
        public List<SimpleLight> GetLightList()
        {
            return LightList;
        }

        /* Return the list of the groups in the system */
        public List<SimpleGroup> GetGroupList()
        {
            return GroupList;
        }

        /* Function for setting the color of a light */
        public bool SetLightColor(RGB color, string lightID)
        {
            string response = "";

            /* base URL for this procedure */
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/lights/" + lightID + "/state";

            /* Get the hue value from the RGB values */
            int hue = RGBtoHUE(color.R, color.G, color.B);

            /* Set the sat to max so colors show */
            int sat = 254;

            /* Make the REST PUT API Call */
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "PUT";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    hue,
                    sat
                });
                streamWriter.Write(json);
            }
            /* Deal with the response */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
                return true;
            }
        }

        /* Set the color of a group */
        public bool SetGroupColor(RGB color, string groupID)
        {
            string response = "";

            /* base URL for this procedure */
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/groups/" + groupID + "/action";

            /* Get the hue value from the RGB values */
            int hue = RGBtoHUE(color.R, color.G, color.B);

            /* Set the sat to max so colors show */
            int sat = 254;

            /* Make the REST PUT API Call */
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "PUT";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    hue,
                    sat
                });
                streamWriter.Write(json);
            }
            /* Deal with the response */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
                return true;
            }
        }

        /* Set the light to be on or off */
        public bool TurnLightOnOff(bool on, string lightID)
        {
            string response = "";
            /* base URL for this procedure */
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/lights/" + lightID + "/state";

            /* Make the REST PUT API Call */
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "PUT";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    on
                });
                streamWriter.Write(json);
            }

            /* Deal with the response */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
                return true;
            }
        }

        /* Set all lights to be on or off */
        public bool TurnGroupOnOff(bool on, string groupID)
        {
            string response = "";
            /* base URL for this procedure */
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/groups/" + groupID + "/action";

            /* Make the REST PUT API Call */
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "PUT";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    on
                });
                streamWriter.Write(json);
            }

            /* Deal with the response */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
                return true;
            }
        }

        /* Set colorloop on or off */
        public bool SetColorLoop(string effect)
        {
            if (effect.Equals("none") || effect.Equals("colorloop"))
            {
                string response = "";
                /* base URL for this procedure */
                string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/groups/0/action";

                /* Turn the saturation of the light to max */
                int sat = 254;

                /* Make the REST PUT API Call */
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "PUT";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                        effect,
                        sat
                    });
                    streamWriter.Write(json);
                }

                /* Deal with the response */
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                    return true;
                }
            }
            else
                return false;
        }

        /* Set the saturation of the light */
        public bool SetLightSat(int sat, string lightID)
        {

            string response = "";
            /* base URL for this procedure */
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/lights/" + lightID + "/state";

            /* Make the REST PUT API Call */
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "PUT";

            /* Make sure saturation is between 0-254 */
            if (sat < 0)
                sat = 0;
            else if (sat > 254)
                sat = 254;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    sat
                });
                streamWriter.Write(json);
            }

            /* Deal with the response */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
                return true;
            }
        }

        /* Set the brightness of the light */
        public bool SetLightBright(int bri, string lightID)
        {
            string response = "";
            /* base URL for this procedure */
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/lights/" + lightID + "/state";

            /* Make the REST PUT API Call */
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "PUT";

            /* Make sure brightness is between 1-254 */
            if (bri < 1)
                bri = 1;
            else if (bri > 254)
                bri = 254;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    bri
                });
                streamWriter.Write(json);
            }

            /* Deal with the response */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
                return true;
            }
        }

        /* Set the brightness of the group */
        public bool SetGroupBrightness(int bri, string groupID)
        {
            string response = "";
            /* base URL for this procedure */
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/groups/" + groupID + "/action";

            /* Make the REST PUT API Call */
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "PUT";

            /* Make sure brightness is between 1-254 */
            if (bri < 1)
                bri = 1;
            else if (bri > 254)
                bri = 254;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    bri
                });
                streamWriter.Write(json);
            }

            /* Deal with the response */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
                return true;
            }
        }

        /* Get the groups in the system */
        public void GetGroups()
        {
            string response = "";
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/groups/";

            /* Request the group information from the bridge */
            WebRequest groupRequest;
            groupRequest = WebRequest.Create(baseURL);
            Stream objStream;
            objStream = groupRequest.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string line = "";
            while (line != null)
            {
                line = objReader.ReadLine();
                if (line != null)
                    response = response + line;
            }

            GroupResponse = JObject.Parse(response);
            int numOfGroups = GroupResponse.Count;
            /* Get name and id of each group and store them */
            for (int i = 0; i < numOfGroups; i++)
            {
                SimpleGroup tempGroup = new SimpleGroup();
                tempGroup.Name = (string)GroupResponse["" + (i + 1) + ""]["name"];
                tempGroup.ID = (i + 1).ToString();
                GroupList.Add(tempGroup);
            }
        }

        /* Test connection to the bridge */
        public bool testConnection()
        {
            string response = "";
            /* If IP is blank return false */
            if (BridgeIP == "")
                return false;
            /* Base URL for testing the IP address */
            string baseURL = "http://" + BridgeIP + "/api/";

            /* Test the connection */
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
            /* Get response */
            /* Deal with the response */
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            /* If it failed then return false */
            if (httpResponse == null || httpResponse.StatusCode != HttpStatusCode.OK)
                return false;

            /* Check to make sure the userKey is valid. */
            /* base URL for this procedure */
            baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/lights/";
            /* Request the light information from the bridge. */
            WebRequest lightRequest;
            lightRequest = WebRequest.Create(baseURL);
            Stream objStream;
            objStream = lightRequest.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string line = "";
            while (line != null)
            {
                line = objReader.ReadLine();
                if (line != null)
                    response = response + line;
            }
            if (response.Contains("unauthorized"))
                return false;
            else
                TestResponse = JObject.Parse(response);

            int testCount = TestResponse.Count;
            /* If the response fails then return false */
            if (TestResponse["error"] != null)
                return false;


            return true;
        }

        /* Get the lights in the system */
        public void GetLights()
        {
            string response = "";
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/lights/";

            /* Request the light information from the bridge. */
            WebRequest lightRequest;
            lightRequest = WebRequest.Create(baseURL);
            Stream objStream;
            objStream = lightRequest.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string line = "";
            while (line != null)
            {
                line = objReader.ReadLine();
                if (line != null)
                    response = response + line;
            }

            LightResponse = JObject.Parse(response);
            int numOfLights = LightResponse.Count;
            /* Get name and id of each light and store them. */
            for (int i = 0; i < numOfLights; i++)
            {
                SimpleLight templight = new SimpleLight();

                templight.Name = (string)LightResponse["" + (i + 1) + ""]["name"];
                templight.ID = (i + 1).ToString();
                LightList.Add(templight);
            }
        }

        /* Get the color of light 1 */
        public XY GetLightOneColor()
        {
            string response = "";
            string baseURL = "http://" + BridgeIP + "/api/" + UserKey + "/lights/1/";

            /* Request the light information from the bridge. */
            WebRequest lightRequest;
            lightRequest = WebRequest.Create(baseURL);
            Stream objStream;
            objStream = lightRequest.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string line = "";
            while (line != null)
            {
                line = objReader.ReadLine();
                if (line != null)
                    response = response + line;
            }

            LightResponse = JObject.Parse(response);
            int numOfLights = LightResponse.Count;
            /* Get name and id of each light and store them. */
            for (int i = 0; i < numOfLights; i++)
            {
                SimpleLight templight = new SimpleLight();

                templight.Name = (string)LightResponse["" + (i + 1) + ""]["name"];
                templight.ID = (i + 1).ToString();
                LightList.Add(templight);
            }

            XY temp = new XY();
            temp.X = (float)LightResponse["" + 1 + ""]["state"]["xy"][0];
            temp.Y = (float)LightResponse["" + 1 + ""]["state"]["xy"][1];
            return temp;
        }

        /* Function to convert RGB to the hue values used by the system */
        public int RGBtoHUE(int r, int g, int b)
        {
            /* Get the max and min of the RGB values */
            float min = Math.Min(Math.Min(r, g), b);
            float max = Math.Max(Math.Max(r, g), b);

            /* If min equals max then return 0, otherwise it will divide by 0 */
            if (min == max)
                return 0;

            /* Calculate the hue value of the RGB values */
            double hue = 0.0;
            if (r == max)
                hue = (g - b) / (max - min);
            else if (g == max)
                hue = 2.0 + (b - r) / (max - min);
            else
                hue = 4.0 + (r - g) / (max - min);

            /* Get the hue angle */
            hue = hue * 60;
            if (hue < 0)
                hue = hue + 360;

            /* Convert to uint16 */
            hue = hue * 182.0;

            /* Convert to int and return it */
            return Convert.ToInt32(Math.Round(hue));
        }

        /* Function to convert the XY values from the bulb to RGB, from the HUE dev guide */
        public RGB XYtoRGB(float x, float y, int brightness)
        {
            /* Get X Y Z values */
            float z = 1.0f - x - y;
            float Y = brightness;
            float X = (Y / y) * x;
            float Z = (Y / y) * z;

            /* Convert to RGB */
            float r = X * 1.656492f - Y * 0.354851f - Z * 0.255038f;
            float g = -X * 0.707196f + Y * 1.655397f + Z * 0.036152f;
            float b = X * 0.051713f - Y * 0.121364f + Z * 1.011530f;

            /* Reverse Gamma Correction */
            r = r <= 0.0031308f ? 12.92f * r : (1.0f + 0.055f) * (float)Math.Pow(r, (1.0f / 2.4f)) - 0.055f;
            g = g <= 0.0031308f ? 12.92f * g : (1.0f + 0.055f) * (float)Math.Pow(g, (1.0f / 2.4f)) - 0.055f;
            b = b <= 0.0031308f ? 12.92f * b : (1.0f + 0.055f) * (float)Math.Pow(b, (1.0f / 2.4f)) - 0.055f;

            /* Values are 0-1 so need to convert to 0-255 */
            int red = (int)Math.Round(r * 255.0);
            int green = (int)Math.Round(g * 255.0);
            int blue = (int)Math.Round(b * 255.0);
            RGB temp = new RGB(red, green, blue);

            /* Return the new RGB object */
            return temp;
        }

        /* Function to set the userKey if a user specifies one */
        public void setUserKey(string k)
        {
            UserKey = k;
        }

        /* Function to set the IP address if a user specifies one */
        public void setIP(string ip)
        {
            BridgeIP = ip;
        }
        
        /* Function to get the current userkey */
        public string getUserKey()
        {
            return UserKey;
        }

        /* Function to get the current bridge IP */
        public string getIP()
        {
            return BridgeIP;
        }
    }
}