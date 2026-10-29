using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        TextBox display;
        double firstNumber = 0;
        string op = "";
        bool newEntry = true;
        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        public Form1()
        {
            InitializeComponent();
            BuildUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        void BuildUI()
        {
            Text = "Тооны машин";
            ClientSize = new Size(320, 460);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 4,
                Padding = new Padding(4)
            };
            for (int i = 0; i < 4; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            for (int i = 0; i < 5; i++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            string[] keys = {
                "C", "±", "%", "÷",
                "7", "8", "9", "×",
                "4", "5", "6", "−",
                "1", "2", "3", "+",
                "0", ".", "="
            };

            foreach (var k in keys)
            {
                var btn = new Button
                {
                    Text = k,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 16),
                    Margin = new Padding(3)
                };
                btn.Click += Button_Click;
                grid.Controls.Add(btn);
                if (k == "0") grid.SetColumnSpan(btn, 2);
            }

            display = new TextBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 28),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                Text = "0"
            };

            // Fill-ийг эхэлж, Top-ийг дараа нэмнэ
            Controls.Add(grid);
            Controls.Add(display);
        }

        void Button_Click(object sender, EventArgs e)
        {
            string k = ((Button)sender).Text;

            if (display.Text == "Алдаа") Reset();

            if (char.IsDigit(k[0]))
            {
                if (newEntry || display.Text == "0") display.Text = k;
                else display.Text += k;
                newEntry = false;
            }
            else if (k == ".")
            {
                if (newEntry) { display.Text = "0."; newEntry = false; }
                else if (!display.Text.Contains(".")) display.Text += ".";
            }
            else if (k == "C")
            {
                Reset();
            }
            else if (k == "±")
            {
                double v = -double.Parse(display.Text, Inv);
                display.Text = v.ToString(Inv);
            }
            else if (k == "%")
            {
                double v = double.Parse(display.Text, Inv) / 100;
                display.Text = v.ToString(Inv);
                newEntry = true;
            }
            else if (k == "=")
            {
                Calculate();
                op = "";
                newEntry = true;
            }
            else // + − × ÷
            {
                if (op != "" && !newEntry) Calculate();
                firstNumber = double.Parse(display.Text, Inv);
                op = k;
                newEntry = true;
            }
        }

        void Calculate()
        {
            if (op == "") return;
            double second = double.Parse(display.Text, Inv);
            double result;

            switch (op)
            {
                case "+": result = firstNumber + second; break;
                case "−": result = firstNumber - second; break;
                case "×": result = firstNumber * second; break;
                case "÷":
                    if (second == 0) { display.Text = "Алдаа"; return; }
                    result = firstNumber / second;
                    break;
                default: result = second; break;
            }

            display.Text = result.ToString(Inv);
            firstNumber = result;
        }

        void Reset()
        {
            display.Text = "0";
            firstNumber = 0;
            op = "";
            newEntry = true;
        }
    }
}