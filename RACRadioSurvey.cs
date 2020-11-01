using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using MissionPlanner.Controls.PreFlight;
using MissionPlanner.Controls;
using System.Linq;
using GMap.NET;
using System.Security.Policy;
using OpenTK.Graphics.OpenGL;

namespace MissionPlanner.RACRadioSurvey
{
    public class RACRadioSurveyPlugin : MissionPlanner.Plugin.Plugin
    {

        public PointLatLng posCenter;
        public double Lat;
        public double Lng;
        public int alt;
        public int points;
        public int radius;
        public int direction;
        public int delay;
        public double speed;


        public override string Name
        {
            get { return "RACRadioSurvey"; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        public override string Author
        {
            get { return "Schaffer Andras (EOSBandi)"; }
        }

        //[DebuggerHidden]
        public override bool Init()
		//Init called when the plugin dll is loaded
        {
            loopratehz = 0;  //Loop runs every second (The value is in Hertz, so 2 means every 500ms, 0.1f means every 10 second...) 

            return true;	 // If it is false then plugin will not load
        }

        public override bool Loaded()
		//Loaded called after the plugin dll successfully loaded
        {


            System.Windows.Forms.ToolStripMenuItem men = new System.Windows.Forms.ToolStripMenuItem() { Text = "Radio Tower Survey" };
            men.Click += startSurveyClick;
            Host.FPMenuMap.Items.Add(men);

            return true;     //If it is false plugin will not start (loop will not called)
        }

        public override bool Loop()
		//Loop is called in regular intervalls (set by loopratehz)
        {

            return true;	//Return value is not used
        }

        public override bool Exit()
		//Exit called when plugin is terminated (usually when Mission Planner is exiting)
        {
            return true;	//Return value is not used
        }

        public void CreateSurvey()
        {
            radius = (int)(radius / CurrentState.multiplierdist);

            double startangle = 0; // TODO: Calculate angle
            PointLatLngAlt center = new PointLatLngAlt(Lat, Lng,alt);

            startangle = center.GetBearing(Host.cs.PlannedHomeLocation);

            double a = startangle;
            double step = 360.0f / points;
            if (direction == -1)
            {
                a += 360;
                step *= -1;
            }
            for (; a <= (startangle + 360) && a >= 0; a += step)
            {

                float d = radius;
                float R = 6371000;

                var lat2 = Math.Asin(Math.Sin(Lat * MathHelper.deg2rad) * Math.Cos(d / R) +
                                     Math.Cos(Lat * MathHelper.deg2rad) * Math.Sin(d / R) * Math.Cos(a * MathHelper.deg2rad));
                var lon2 = Lng * MathHelper.deg2rad +
                           Math.Atan2(Math.Sin(a * MathHelper.deg2rad) * Math.Sin(d / R) * Math.Cos(Lat * MathHelper.deg2rad),
                               Math.Cos(d / R) - Math.Sin(Lat * MathHelper.deg2rad) * Math.Sin(lat2));

                PointLatLng pll = new PointLatLng(lat2 * MathHelper.rad2deg, lon2 * MathHelper.rad2deg);

                Host.AddWPtoList(MAVLink.MAV_CMD.WAYPOINT, delay, 0, 0, 0, pll.Lng, pll.Lat, alt);
            }
            Host.InsertWP(0, MAVLink.MAV_CMD.DO_SET_ROI, 0, 0, 0, 0, Lng, Lat, alt);
            Host.InsertWP(1, MAVLink.MAV_CMD.DO_CHANGE_SPEED, 0, speed, 0, 0, 0, 0, 0);
        }

        void startSurveyClick(object sender, EventArgs e)
        {
            using (Form settings = new MissionPlanner.RACRadioSurvey.SurveyParamsInput(this))
            {
                MissionPlanner.Utilities.ThemeManager.ApplyThemeTo(settings);
                var ret = settings.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    CreateSurvey();
                }
            }
        }
    }
}