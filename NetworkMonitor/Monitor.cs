using CsvHelper;
using CsvHelper.Configuration;
using NetworkMonitor.Objects;
using NetworkMonitor.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace NetworkMonitor
{
    public partial class Monitor : Form
    {
        private ConfigObject configObject = new ConfigObject();

        private readonly HttpClient _client = new HttpClient();


        public Monitor()
        {
            InitializeComponent();
            initiateConfigs();
        }

        private void Monitor_Load(object sender, EventArgs e)
        {
            refreshTimer.Enabled = true;
            totalTimer.Enabled = true;
            stepTimer.Enabled = true;
            refreshTimer.Start();
            this.ShowInTaskbar = false;

            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info; 
            this.notifyIcon.BalloonTipText = "Network Monitor";
            this.notifyIcon.BalloonTipTitle = "Network Monitor";
            this.notifyIcon.Text = "Network Monitor";
            this.Visible = false;
            this.Opacity = 0;
        }

        private void initiateConfigs()
        {
            getJsonConfig();

            string filename = Environment.CurrentDirectory;
            filename += "/config/config.json";
            configObject = JsonConvert.DeserializeObject<ConfigObject>(File.ReadAllText(filename));

            refreshTimer.Interval = configObject.refreshTime * 1000;
        }

        private async void refreshTimer_Tick(object sender, EventArgs e)
        {
            refreshTimer.Stop();
            string path = Environment.CurrentDirectory;
            path += "/logs/";

            string today = DateTime.Today.ToShortDateString().Replace("/", "-");
            string filename = path + today + "-monitor.csv";

            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
                System.IO.Directory.CreateDirectory(path);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var stream = File.Open(filename, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {

                string newRegister = DateTime.Now.ToString();
                Stopwatch totalTime = Stopwatch.StartNew();

                csv.WriteField(DateTime.Now.ToString());

                foreach (string url in configObject.consultUrls)
                {
                    Stopwatch stepTime = Stopwatch.StartNew();
                    HttpResponseMessage response = await _client.GetAsync(url);

                    string newResponse = await response.Content.ReadAsStringAsync();

                    stepTime.Stop();
                    MonitorObject monitorObject = new MonitorObject();
                    csv.WriteField(response.StatusCode.ToString());
                    csv.WriteField(stepTime.Elapsed.ToString());

                }

                totalTime.Stop();
                Console.WriteLine(totalTime.Elapsed);

                csv.WriteField(totalTime.Elapsed.ToString());
                csv.NextRecord();

                initiateConfigs();
                refreshTimer.Start();
            }
        }

        private void getJsonConfig()
        {
            string path = Environment.CurrentDirectory;
            path += "/config/";

            string filename = path + "config.json";

            bool existsPath = System.IO.Directory.Exists(path);
            bool existsFile = File.Exists(filename);

            if (!existsPath)
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (!existsFile)
            {
                ConfigObject configObject = new ConfigObject();
                configObject.refreshTime = 15;
                configObject.consultUrls.Add("https://google.com");

                string json = JsonConvert.SerializeObject(configObject);
                File.WriteAllText(filename, json);
            }
                
        }

    }
}
