using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;

namespace ParkingSimulation
{
    public partial class MainForm : Form
    {
        private ParkingManager manager;
        private Statistics stats;

        public MainForm()
        {
            InitializeComponent();

            // Налаштування осей графіка
            chartStats.AxisX.Clear();
            chartStats.AxisY.Clear();

            chartStats.AxisX.Add(new Axis
            {
                Title = "Cars",
                Labels = new[] { "Metric" }
            });

            chartStats.AxisY.Add(new Axis
            {
                Title = "Count",
                MinValue = 0
            });

            // Початкове налаштування
            stats = new Statistics();
            manager = new ParkingManager(
                spotCount: 5,
                logCallback: LogMessage,
                stats: stats,
                spotStatusUpdater: UpdateSpotStatus
            );

            // Кнопки: старт дозволено, стоп — ні
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
        }

        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogMessage), message);
                return;
            }

            listBoxLogs.Items.Insert(0, $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] — {message}");
        }

        private void UpdateSpotStatus(int spotId, bool occupied, int? carId)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateSpotStatus(spotId, occupied, carId)));
                return;
            }

            var panel = Controls.Find($"spotPanel{spotId}", true).FirstOrDefault() as Panel;
            var label = Controls.Find($"spotLabel{spotId}", true).FirstOrDefault() as Label;

            if (panel != null) panel.BackColor = occupied ? System.Drawing.Color.IndianRed : System.Drawing.Color.LightGray;
            if (label != null) label.Text = occupied ? $"Spot {spotId} — Car {carId}" : $"Spot {spotId} — Free";
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // Ініціалізуємо графік перед стартом
            chartStats.Series.Clear();
            chartStats.Series = new SeriesCollection
{
          new ColumnSeries
    {
        Title = "Cars",
        Values = new ChartValues<int> { 5 },
        Fill = System.Windows.Media.Brushes.SteelBlue // стиль
    }
};



            manager.StartSimulation();
            LogMessage("▶️ Симуляція запущена вручну.");

            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            manager.StopSimulation();

            string filename = $"parking_stats_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            stats.SaveToTxt(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));
            LogMessage($"📄 Статистика збережена: {filename}");

            buttonStop.Enabled = false;
            buttonStart.Enabled = true;
        }
    }
}
