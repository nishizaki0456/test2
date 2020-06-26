using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices;

namespace _PLogger
{
    public partial class Form1 : Form
    {
        Chart[] chart = new Chart[4];
        TextBox[] count_textbox = new TextBox[4];
        TextBox[] max_textbox = new TextBox[4];
        TextBox[] min_textbox = new TextBox[4];
        TextBox[] plus_average_textbox = new TextBox[4];
        TextBox[] minus_average_textBox = new TextBox[4];
        int timer_count;
        string[] cols;
        List<Double>[] channels = new List<Double>[8];
        string filePath;

        public Form1()
        {
            InitializeComponent();
            array_declaration();
        }

        private void File_Open_Button_Click(object sender, EventArgs e)
        {
            graph();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                label1.Text = openFileDialog1.FileName;
                StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
                string filename = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
                StreamWriter file = new StreamWriter(filename, false, Encoding.UTF8);
                //チャンネル数分list変数を宣言
                for (int i = 0; i < int.Parse(Channel_Number_TextBox.Text) + 1; i++)
                {
                    if (channels[i] != null)
                    {
                        channels[i].Clear();
                    }
                    else
                    {
                        channels[i] = new List<Double>();
                    }
                }
                var skip = reader.ReadLine();  // ヘッダ読み飛ばし
                while (reader.Peek() >= 0)
                {
                    cols = reader.ReadLine().Split(',');
                    for (int n = 0; n < int.Parse(Channel_Number_TextBox.Text) + 1; n++)
                    {
                        channels[n].Add(double.Parse(cols[n]));
                    }
                }
                reader.Close();
            }
            else
            {
                return;
            }
        }

        private void Timer_Button_Click(object sender, EventArgs e)
        {
            timer_count = 0;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < int.Parse(Channel_Number_TextBox.Text); i++)
            {
                chart[i].Series["Test"].Points.AddXY(channels[0][timer_count], channels[i + 1][timer_count]);
            }
            if (timer_count >= channels[0].Count - 1)
            {
                timer1.Enabled = false;
                timer_count = 0;
            }
            timer_count++;
        }

        private void Graph_Display_Button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < int.Parse(Channel_Number_TextBox.Text); i++)
            {
                chart[i].ChartAreas[0].AxisX.Maximum = int.Parse(End_Number_TextBox.Text);
                chart[i].ChartAreas[0].AxisX.Minimum = int.Parse(Start_Number_TextBox.Text);
                chart[i].ChartAreas[0].AxisX.Interval = int.Parse(Interval_Number_TextBox.Text);
            }
            for (int i = int.Parse(Start_Number_TextBox.Text); i <= int.Parse(End_Number_TextBox.Text); i++)
            {
                for (int j = 0; j < int.Parse(Channel_Number_TextBox.Text); j++)
                {
                    chart[j].Series["Test"].Points.AddXY(channels[0][i], channels[j + 1][i]);
                }
            }
        }

        private void Calculation_Button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < int.Parse(Channel_Number_TextBox.Text); i++)
            {
                count_textbox[i].Text = channels[i + 1].Count.ToString();
                max_textbox[i].Text = channels[i + 1].Max().ToString();
                min_textbox[i].Text = channels[i + 1].Min().ToString();
                plus_average_textbox[i].Text = channels[i + 1].Where(n => n > 0).Average().ToString();
                minus_average_textBox[i].Text = channels[i + 1].Where(n => n < 0).Average().ToString();
            }
        }

        private void Reset_Button_Click(object sender, EventArgs e)
        {
            graph();
        }

        public void graph()
        {
            string chart_area1 = "Area1";
            string label = "Test";
            string title1 = "ch1";
            string title2 = "ch2";
            string title3 = "ch3";
            string title4 = "ch4";
            for (int i = 0; i < chart.Length; i++)
            {
                chart[i].Series.Clear();
                chart[i].ChartAreas.Clear();
                chart[i].Titles.Clear();
                chart[i].ChartAreas.Add(new ChartArea(chart_area1));
                chart[i].Series.Add(label);
                chart[i].Series[label].ChartType = SeriesChartType.Line;
                //chart[i].ChartAreas[0].BackColor = Color.FromArgb(255, 200, 200, 200);//ARGB                                                                                   //凡例の削除
                for (int j = 0; j < chart[i].Series.Count; j++)
                {
                    chart[i].Series[j].IsVisibleInLegend = false;
                    chart[i].Series[j].IsValueShownAsLabel = false;
                }
                chart[i].ChartAreas[0].AxisX.Title = "振動回数（回）";
                chart[i].ChartAreas[0].AxisY.Title = "漏洩量(Pa・m3/sec)";
            }
            chart[0].Titles.Add(title1);
            chart[1].Titles.Add(title2);
            chart[2].Titles.Add(title3);
            chart[3].Titles.Add(title4);
        }
        public void array_declaration()
        {
            chart[0] = chart1;
            chart[1] = chart2;
            chart[2] = chart3;
            chart[3] = chart4;
            count_textbox[0] = Count_TextBox1;
            count_textbox[1] = Count_TextBox2;
            count_textbox[2] = Count_TextBox3;
            count_textbox[3] = Count_TextBox4;
            max_textbox[0] = Max_TextBox1;
            max_textbox[1] = Max_TextBox2;
            max_textbox[2] = Max_TextBox3;
            max_textbox[3] = Max_TextBox4;
            min_textbox[0] = Min_TextBox1;
            min_textbox[1] = Min_TextBox2;
            min_textbox[2] = Min_TextBox3;
            min_textbox[3] = Min_TextBox4;
            plus_average_textbox[0] = Plus_Average_TextBox1;
            plus_average_textbox[1] = Plus_Average_TextBox2;
            plus_average_textbox[2] = Plus_Average_TextBox3;
            plus_average_textbox[3] = Plus_Average_TextBox4;
            minus_average_textBox[0] = Minus_Average_TextBox1;
            minus_average_textBox[1] = Minus_Average_TextBox2;
            minus_average_textBox[2] = Minus_Average_TextBox3;
            minus_average_textBox[3] = Minus_Average_TextBox4;
        }

        private void Csv_Save_Button_Click(object sender, EventArgs e)
        {
            string saveFileName;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileName = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }
            StreamWriter savefile = new StreamWriter(
                                        new FileStream(saveFileName, FileMode.Create));
            savefile.WriteLine("name" + "," + "count" + "," + "max" + "," + "min" + "," + "plus_average" + "," + "minus_average");
            for (int i = 0; i < int.Parse(Channel_Number_TextBox.Text); i++)
            {
                savefile.Write((i + 1) + "ch" + ",");
                savefile.Write(channels[i + 1].Count.ToString() + ",");
                savefile.Write(channels[i + 1].Max().ToString() + ",");
                savefile.Write(channels[i + 1].Min().ToString() + ",");
                savefile.Write(channels[i + 1].Where(n => n > 0).Average().ToString() + ",");
                savefile.WriteLine(channels[i + 1].Where(n => n < 0).Average().ToString());
            }
            // ファイルを閉じる
            savefile.Close();
        }
    }
}
