namespace ParkingSimulation
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxLogs;
        private System.Windows.Forms.Label labelServed;
        private System.Windows.Forms.Label labelRejected;
        private LiveCharts.WinForms.CartesianChart chartStats;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;

        private void InitializeComponent()
        {
            this.listBoxLogs = new System.Windows.Forms.ListBox();
            this.labelServed = new System.Windows.Forms.Label();
            this.labelRejected = new System.Windows.Forms.Label();
            this.chartStats = new LiveCharts.WinForms.CartesianChart();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();

            // listBoxLogs
            this.listBoxLogs.FormattingEnabled = true;
            this.listBoxLogs.Location = new System.Drawing.Point(12, 12);
            this.listBoxLogs.Size = new System.Drawing.Size(560, 200);

            // labelServed
            this.labelServed.AutoSize = true;
            this.labelServed.Location = new System.Drawing.Point(12, 220);
            this.labelServed.Text = "Served: 0";

            // labelRejected
            this.labelRejected.AutoSize = true;
            this.labelRejected.Location = new System.Drawing.Point(150, 220);
            this.labelRejected.Text = "Left: 0";

            // chartStats
            this.chartStats.Location = new System.Drawing.Point(12, 250);
            this.chartStats.Size = new System.Drawing.Size(560, 100);

            // buttonStop
            this.buttonStop.Text = "Stop Simulation";
            this.buttonStop.Location = new System.Drawing.Point(10, 360);
            this.buttonStop.Size = new System.Drawing.Size(120, 30);
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);

            // buttonStart
            this.buttonStart.Text = "Start Simulation";
            this.buttonStart.Location = new System.Drawing.Point(150, 360);
            this.buttonStart.Size = new System.Drawing.Size(120, 30);
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);

            // MainForm
            this.ClientSize = new System.Drawing.Size(584, 500);
            this.Controls.Add(this.listBoxLogs);
            this.Controls.Add(this.labelServed);
            this.Controls.Add(this.labelRejected);
            this.Controls.Add(this.chartStats);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);

            // Панелі для місць
            for (int i = 0; i < 5; i++)
            {
                var panel = new System.Windows.Forms.Panel();
                panel.Name = $"spotPanel{i}";
                panel.Size = new System.Drawing.Size(100, 50);
                panel.Location = new System.Drawing.Point(10 + i * 110, 410);
                panel.BackColor = System.Drawing.Color.LightGray;

                var label = new System.Windows.Forms.Label();
                label.Name = $"spotLabel{i}";
                label.Text = $"Spot {i} — Free";
                label.Location = new System.Drawing.Point(5, 15);
                label.AutoSize = true;

                panel.Controls.Add(label);
                this.Controls.Add(panel);
            }

            this.Text = "Parking Lot Simulation";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
