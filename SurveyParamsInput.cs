using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.RACRadioSurvey
{
    public partial class SurveyParamsInput : Form
    {
        private RACRadioSurveyPlugin plugin;
        public SurveyParamsInput(RACRadioSurveyPlugin plugin)
        {
            this.plugin = plugin;
            InitializeComponent();
        }

        private void SurveyParamsInput_Load(object sender, EventArgs e)
        {
            tbPosLat.Text = plugin.Host.FPMenuMapPosition.Lat.ToString();
            tbPosLon.Text = plugin.Host.FPMenuMapPosition.Lng.ToString();
            tbRadius.Text = "100";
            tbPoints.Text = "20";
            tbAlt.Text = "50";
            tbDirection.Text = "1";
            tbDelay.Text = "0";
            tbSpeed.Text = "4";
        }

        private void myButton1_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            if (!int.TryParse(tbRadius.Text, out plugin.radius)) this.DialogResult = DialogResult.Cancel;
            if (!double.TryParse(tbPosLat.Text, out plugin.Lat)) this.DialogResult = DialogResult.Cancel;
            if (!double.TryParse(tbPosLon.Text, out plugin.Lng)) this.DialogResult = DialogResult.Cancel;
            if (!int.TryParse(tbAlt.Text, out plugin.alt)) this.DialogResult = DialogResult.Cancel;
            if (!int.TryParse(tbPoints.Text, out plugin.points)) this.DialogResult = DialogResult.Cancel;
            if (!int.TryParse(tbDirection.Text, out plugin.direction)) this.DialogResult = DialogResult.Cancel;
            if (!int.TryParse(tbDelay.Text, out plugin.delay)) this.DialogResult = DialogResult.Cancel;
            if (!double.TryParse(tbSpeed.Text, out plugin.speed)) this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void myButton2_Click(object sender, EventArgs e)
        { 
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
