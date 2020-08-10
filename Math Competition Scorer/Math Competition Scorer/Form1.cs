using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Math_Competition_Scorer
{
    public partial class form1 : Form
    {
        
        private int size;
        private Color green = Color.ForestGreen;
        private Color red = Color.DarkRed;
        List<string> kyLst = new List<string>();
        List<string> tiBreaker = new List<string>();
        List<List<string>> students = new List<List<string>>();
        private bool fileRead;
        private bool calcScores;
        private bool schoolsRead;
        private Rectangle myRec;

        public form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            openFileToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            printToolStripMenuItem.Enabled = false;
            listView2.Visible = false;
            //MessageBox.Show("Hello, Welcome to the Math Competition Scorer.\nTo compute scores a group of students go to File and open a text file.\nThen select the Compute Scores button at the lower right.\nTo export the results go to \"File\" and \"Save Results As\".\nBrowse to the directory you would like and then type the the name you would like the folder to save as.\nMore information on how to use can be foiund in the help menu.");
        }

        

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developed and Designed by Derek Olsen. For Information on how to use visit the \"Help\" Menu.", "About");
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                fileLoc.Text = file;
                fileOp.Text = "Files Opened";
                fileOp.ForeColor = green;

                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }
            }
            string filePath = fileLoc.Text;
            string[] Lines = File.ReadAllLines(filePath);
            using (var reader = new StreamReader(filePath))
            {
                string key = reader.ReadLine();
                string[] keyLst = (key.Split(new char[0]));
                string a;
                foreach (char c in keyLst[keyLst.Length - 1])
                {
                    a = c.ToString();
                    kyLst.Add(a);
                }
                
                string tie = reader.ReadLine();
                string[] tieLst = (tie.Split(new char[0]));

                string b;
                foreach (char d in tieLst[tieLst.Length - 1])
                {
                    b = d.ToString();
                    tiBreaker.Add(b);
                }


                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = Regex.Replace(line, @"\s+", ",");
                    var values = line.Split(',');
                    List<string> valuesTemp = new List<string>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        valuesTemp.Add(values[i] + " ");
                    }
                    students.Add(valuesTemp);
                }
                fileRead = true;
                label5.Text = "Compute Scores";
                richTextBox1.Text = "The Button at the bottom right of the window.";
            }
            
            for (int i = 0; i < students.Count; i++)
            {
                if (students[i].Count == 6)
                {
                    int cls = Convert.ToInt32(students[i][3]);
                    int schl = Convert.ToInt32(students[i][4]);
                    Master.studentMaster.Add(new Student(students[i][0], students[i][1], students[i][2], cls, schl, Master.schoolDict[schl], students[i][5]));
                }

                else if (students[i].Count == 5)
                {
                    int cls = Convert.ToInt32(students[i][2]);
                    int schl = Convert.ToInt32(students[i][3]);
                    Master.studentMaster.Add(new Student(students[i][0], students[i][1],"null", cls, schl, Master.schoolDict[schl], students[i][4]));
                }
            }
            numStu.Visible = true;
            numStu.Text = "The Number of students in this dataset is: " + Convert.ToString(Master.studentMaster.Count());
            button2.Enabled = true;
            openFileToolStripMenuItem.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveResults();
        }


        private void runResults()
        {
            Master.studentMaster.Sort(delegate (Student x, Student y) { return y.score.CompareTo(x.score); });
        }

       

        private void addAnIndidivualStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Individual_Student addStu = new Individual_Student();
            addStu.ShowDialog();
            this.Refresh();
            numStu.Text = "The Number of students in this dataset is: " + Convert.ToString(Master.studentMaster.Count());
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            How_To_Use howTo = new How_To_Use();
            howTo.ShowDialog();
            this.Refresh();
        }
        void saveResults()
        {
            string path = fileLoc.Text;
            string teamPath = "";
            path = path.Split('.')[0];
            teamPath = path + "\\Team Files";
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    DialogResult delete = MessageBox.Show("Files have already been written in that location. Would you like to delete and rewrite them?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (delete == DialogResult.Yes)
                    {
                        Directory.Delete(path, true);
                        saveResults();
                        //MessageBox.Show("The Directory has been deleted.\n Please go to \"File\" and \"Save Results\" to re-save the files.");

                        return;
                    }
                    else if (delete == DialogResult.No)
                    {
                        MessageBox.Show("The files won't be deleted.");
                        return;
                    }
                    else if (delete == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {

                    // Try to create the directory.
                 DirectoryInfo di = Directory.CreateDirectory(path);
                 Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                 DirectoryInfo df = Directory.CreateDirectory(teamPath);
                }
            }

            catch
            {
                    Console.WriteLine("The process failed.");
            }

            Master.studentMaster.Sort(delegate (Student x, Student y) { return y.score.CompareTo(x.score); });
            int num = 1;
            using (StreamWriter File = new StreamWriter(path + "\\Student Scores.txt"))
            {
                File.WriteLine("{0, -7} {1, -15} {2, -20} {3, -6}", "Place", "First Name", "Last Name", "Score");
                File.WriteLine("{0, -7} {1, -15} {2, -20} {3, -6}", "~~~~~", "~~~~~~~~~~", "~~~~~~~~~", "~~~~~~");
                foreach (Student s in Master.studentMaster)
                {
                    File.WriteLine("{0, 7} {1, -15} {2, -20} {3, 6}", Convert.ToString(num) + ". ", s.fName, s.lName, s.score);
                    num = num + 1;
                }
            }
            int schl = 0;
            int prevSchl = 0;
            prevSchl = 0;
            int count = 0;
            int aCount = 1;
            int aaCount = 1;
            int teamScore = 0;
            int studentTeamScore = 0;
            Dictionary<int, int> teamScores = new Dictionary<int, int>();
            Dictionary<int, int> studentTeamScores = new Dictionary<int, int>();
            var orderedlistPersons = Master.studentMaster.OrderBy(p => p.strSchool).ThenByDescending(p => p.score);
            var beginschool = orderedlistPersons.First();
            int uniqueSchls = 1;
            prevSchl = beginschool.school;
            //for every student in list ordered by school and then ascending score while the school number has not changed, take the top 3 students
            // if the school has more than 3 students skip the remaining until the school changes if the school has less than 3 students move on
            foreach (Student s in orderedlistPersons)
            {
                schl = s.school;
                if (schl == prevSchl && count <= 2)
                {
                    teamScore += s.count;
                    studentTeamScore += s.score;
                }

                else if (schl != prevSchl)
                {
                    uniqueSchls += 1;
                    teamScores.Add(prevSchl, teamScore);
                    studentTeamScores.Add(prevSchl, studentTeamScore);
                    teamScore = 0;
                    studentTeamScore = 0;
                    teamScore += s.count;
                    studentTeamScore += s.score;

                    count = 0;
                }
                count += 1;
                prevSchl = s.school;

            }
            teamScores.Add(prevSchl, teamScore);
            studentTeamScores.Add(prevSchl, studentTeamScore);
            prevSchl = 0;
            schl = 0;
            //Write One big file with all the teams
            using (StreamWriter File = new StreamWriter(path + "\\Team Scores.txt"))
            {
                File.WriteLine("{0,-30} {1, -15} {2, -12} {3, -8}", "School", "First Name", "Last Name", "Base Score");
                foreach (Student s in orderedlistPersons)
                {
                    schl = s.school;
                    if (schl != prevSchl)
                    {
                        File.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                        File.WriteLine("\n");
                        File.WriteLine("{0,-50} Team Base Score: {1,-8}", Master.schoolDict[schl], teamScores[schl]);
                        File.WriteLine("----------------------------------------------------------------------");
                        File.WriteLine("{0,-30} {1, -15} {2, -20} {3, -8}", s.strSchool, s.fName, s.lName, s.count);
                    }
                    else
                    {
                        File.WriteLine("{0,-30} {1, -15} {2, -20} {3, -8}", s.strSchool, s.fName, s.lName, s.count);
                    }
                    prevSchl = s.school;
                }
            }
           
            

            var sortedScores = from entry in teamScores orderby entry.Value descending select entry;
            using (StreamWriter File = new StreamWriter(path + "\\Ranked Teams.txt"))
            {
                File.WriteLine("{0,-30} {1, -6}|{2,-15}|{3,-8}|{4,-8}|{5,-14}|", "School", "Class", "Base Score", "Score", "Place", "Place In Class");
                File.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                count = 1;
                foreach (var kv in sortedScores)
                {
                    int clas = Master.studentMaster.FindIndex(item => item.school == kv.Key);
                    if (Master.studentMaster[clas].stCls == "A")
                    {
                        File.WriteLine("{0,-30} {1, 6}|{2,15}|{3,8}|{4,8}|{5,14}|", Master.schoolDict[kv.Key], Master.studentMaster[clas].stCls, kv.Value, studentTeamScores[kv.Key], count, aCount);
                        aCount += 1; 
                    }
                    else if (Master.studentMaster[clas].stCls == "AA")
                    {
                        File.WriteLine("{0,-30} {1, 6}|{2,15}|{3,8}|{4,8}|{5,14}|", Master.schoolDict[kv.Key], Master.studentMaster[clas].stCls, kv.Value, studentTeamScores[kv.Key], count, aaCount);
                        aaCount += 1;
                    }
                    int index = Master.teamMaster.FindIndex(item => item.district == kv.Key);
                    Master.teamMaster[index].place = count;
                    count += 1;
                    
                }
            }

            //Write the individual files for each team
            prevSchl = 0;
            foreach (Student s in orderedlistPersons)
            {
                schl = s.school;
                if (schl != prevSchl)
                {
                    using (StreamWriter file = new StreamWriter(teamPath + "\\" + Master.schoolDict[schl] + ".txt"))
                    {
                        file.WriteLine("{0,-30} {1, -15} {2, -12} {3, -8}    {4, -6} | {5, -4} |", "School", "First Name", "Last Name", "Base Count", "Score", "Place");
                        file.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~|");
                        file.WriteLine("{0,-30} Team Base Count: {1,-8}      Team Score: {2, -6} |{3,4}   |", Master.schoolDict[schl], teamScores[schl], studentTeamScores[schl], Master.teamMaster.Find(item => item.district == schl).place);
                        file.WriteLine("-----------------------------------------------------------------------------------------|");
                        file.WriteLine("{0,-30} {1, -15} {2, -20} {3, -5} {4,-6} |{5, 4}   |", s.strSchool, s.fName, s.lName, s.count, s.score, s.place);
                    }
                }

                else
                {
                    using (StreamWriter file = File.AppendText(teamPath + "\\" + Master.schoolDict[schl] + ".txt"))
                    {
                        file.WriteLine("{0,-30} {1, -15} {2, -20} {3, -5} {4,-6} |{5, 4}   |", s.strSchool, s.fName, s.lName, s.count, s.score, s.place);
                    }
                }
                prevSchl = s.school;
            }

            label6.Visible = true;
            label6.Text = "There are students from " + Convert.ToString(uniqueSchls) + " different schools in the group.";
            chart2.SaveImage(path + "\\Histogram.Png", ChartImageFormat.Png);
            DialogResult openExplorer = MessageBox.Show("The Files were successfully written at " + Convert.ToString(Directory.GetCreationTime(path)) + ".\n to the path:\n " + path + ".\n\nOpen File Explorer there?", "Success", MessageBoxButtons.YesNo);
            if (openExplorer == DialogResult.Yes)
            {
                Process.Start(@path);
            }
        }
    
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void form1_Load(object sender, EventArgs e)
        {

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog pD = new PrintDialog();
            PrintDocument pd = new PrintDocument();
            // Add a PrintPageEventHandler for the document 
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            pd.DefaultPageSettings.Landscape = true;
            pD.Document = pd;
            DialogResult result = pD.ShowDialog();
            if (result == DialogResult.OK)
            {
                pd.Print();
            }
        }
        void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            Font printFont = new Font("Arial", 10);
            // Create Rectangle structure, used to set the position of the chart Rectangle 
            myRec = new System.Drawing.Rectangle(10, 30, 1050, 900);
            chart2.Printing.PrintPaint(ev.Graphics, myRec);
        }
        

        private void removeAnIndividualStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove_Individual rem = new Remove_Individual();
            rem.ShowDialog();
            this.Refresh();
            numStu.Text = "The Number of students in this dataset is: " + Convert.ToString(Master.studentMaster.Count());
        }

        private void button2_Click(object sender, EventArgs e)
        {

            label5.Visible = false;
            richTextBox1.Visible = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = Master.studentMaster.Count();
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            foreach (Student s in Master.studentMaster)
            { 
                s.CalcSocre(kyLst, tiBreaker);
                progressBar1.PerformStep();
            }
            listView1.Items.Clear();
            int i = 0;
            Master.studentMaster.Sort(delegate (Student x, Student y) { return y.score.CompareTo(x.score); });
            foreach (Student s in Master.studentMaster)
            {
                i += 1;
                s.place = i;
                ListViewItem item = new ListViewItem();
                if (s.strSchool != "NoSchool")
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, s.strSchool, Convert.ToString(s.score) });
                }
                else
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, Convert.ToString(s.school), Convert.ToString(s.score) });
                }
                listView1.Items.Add(item);
                if (i % 2 == 0)
                {
                    item.BackColor = Color.LightGray;
                }
            }
            
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            calcScores = true;
            float sum = 0;
            float sco = 0;
            if (fileRead == true && calcScores == true)
            {
                drawHistogram2();
            }
            if (fileRead == true && calcScores == true)
            {
                drawHistogram();
            }
            foreach (Student s in Master.studentMaster)
            {
                sum = sum + s.count;
                sco = sco + s.score;
            }
            label3.Visible = true;
            label4.Visible = true;
            label3.Text = "Average Correct: " + Convert.ToString(Math.Round(sum/Master.studentMaster.Count(),2));
            label4.Text = "Average Score: " + Convert.ToString(Math.Round(sco / Master.studentMaster.Count(), 2));
            button2.Enabled = false;
            saveAsToolStripMenuItem.Enabled = true;
            printToolStripMenuItem.Enabled = true;
            saveResults();
        }
           

        private void firstNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            int i = 0;
            Master.studentMaster.Sort(delegate (Student x, Student y) { return x.fName.CompareTo(y.fName); });
            foreach (Student s in Master.studentMaster)
            {
                i += 1;
                ListViewItem item = new ListViewItem();
                if (s.strSchool != "NoSchool")
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, s.strSchool, Convert.ToString(s.score) });
                }
                else
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, Convert.ToString(s.school), Convert.ToString(s.score) });
                }
                listView1.Items.Add(item);
                if (i % 2 == 0)
                {
                    item.BackColor = Color.LightGray;
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void lastNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            listView1.Items.Clear();
            int i = 0;
            Master.studentMaster.Sort(delegate (Student x, Student y) { return x.lName.CompareTo(y.lName); });
            foreach (Student s in Master.studentMaster)
            {
                i += 1;
                ListViewItem item = new ListViewItem();
                if (s.strSchool != "NoSchool")
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, s.strSchool, Convert.ToString(s.score) });
                }
                else
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, Convert.ToString(s.school), Convert.ToString(s.score) });
                }
                listView1.Items.Add(item);
                if (i % 2 == 0)
                {
                    item.BackColor = Color.LightGray;
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void individualScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            listView1.Items.Clear();
            int i = 0;
            Master.studentMaster.Sort(delegate (Student x, Student y) { return y.score.CompareTo(x.score); });
            foreach (Student s in Master.studentMaster)
            {
                i += 1;
                ListViewItem item = new ListViewItem();
                if (s.strSchool != "NoSchool")
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, s.strSchool, Convert.ToString(s.score) });
                }
                else
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, Convert.ToString(s.school), Convert.ToString(s.score) });
                }
                listView1.Items.Add(item);
                if (i % 2 == 0)
                {
                    item.BackColor = Color.LightGray;
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void schoolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            listView1.Items.Clear();
            int i = 0;
            var orderedlistPersons = Master.studentMaster.OrderBy(p => p.strSchool).ThenByDescending(p => p.score);
            foreach (Student s in orderedlistPersons)
            {
                i += 1;
                ListViewItem item = new ListViewItem();
                if (s.strSchool != "NoSchool")
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, s.strSchool, Convert.ToString(s.score) });
                }
                else
                {
                    item = new ListViewItem(new[] { s.fName, s.lName, s.stCls, Convert.ToString(s.school), Convert.ToString(s.score) });
                }
                listView1.Items.Add(item);
                if (i % 2 == 0)
                {
                    item.BackColor = Color.LightGray;
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Oemtilde && e.KeyChar == unchecked((char)Keys.Alt) && e.KeyChar == unchecked((char)Keys.Control))
            {
                MessageBox.Show("Advanced Mode Activated", "You have now activated the advanced mode. Take the proper precautions when using these features. These features can be found under the \"edit\" menu.");
                addAnIndidivualStudentToolStripMenuItem.Enabled = true;
                addAnIndidivualStudentToolStripMenuItem.Visible = true;
                removeAnIndividualStudentToolStripMenuItem.Enabled = true;
                removeAnIndividualStudentToolStripMenuItem.Visible = true;
                changeAnIndividualScoreToolStripMenuItem.Enabled = true;
                changeAnIndividualScoreToolStripMenuItem.Visible = true;

            }
        }

        private void form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.RControlKey | Keys.Oemtilde | Keys.Alt))
            {
                MessageBox.Show("Advanced Mode Activated", "You have now activated the advanced mode. Take the proper precautions when using these features. These features can be found under the \"edit\" menu.");
                addAnIndidivualStudentToolStripMenuItem.Enabled = true;
                addAnIndidivualStudentToolStripMenuItem.Visible = true;
                removeAnIndividualStudentToolStripMenuItem.Enabled = true;
                removeAnIndividualStudentToolStripMenuItem.Visible = true;
                changeAnIndividualScoreToolStripMenuItem.Enabled = true;
                changeAnIndividualScoreToolStripMenuItem.Visible = true;

            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            
        }

        public void drawHistogram()
        {
            chart1.ChartAreas.Add("area1");
            Axis ax = chart1.ChartAreas[0].AxisX;
            Axis ay = chart1.ChartAreas[0].AxisY;
            ax.Minimum = 0;
            ax.Maximum = 41;
            ax.Interval = 1;
            ay.Minimum = 0;
            ay.Maximum = Master.studentMaster.Count + 1;
            ay.Interval = 5;
            chart1.Series.Add("Question");
            List<float> hist = new List<float>(new float[40]);
            for (int i = 0; i <= 39; i++)
            {
                foreach (Student s in Master.studentMaster)
                {
                    
                    if (s.answersTF[i] == true)
                    {
                        hist[i] = hist[i]+1;
                    }
                }
            }
            
            chart1.Series["Question"].ChartType = SeriesChartType.Column;
            for (int j = 1; j <= 40; j++)
            {
                chart1.Series["Question"].Points.AddY(hist[j-1]);
            }
            
            chart1.Series["Question"].ChartArea = "area1";
            chart1.Titles[1].IsDockedInsideChartArea = false;
            chart1.Titles[2].IsDockedInsideChartArea = false;
            chart1.Titles[1].DockedToChartArea = "area1";
            chart1.Titles[2].DockedToChartArea = "area1";
            chart1.Visible = true;
            chart1.Show();
            Refresh();
        }
        public void drawHistogram2()
        {
            chart2.ChartAreas.Add("area2");
            Axis ax = chart2.ChartAreas[0].AxisX;
            Axis ay = chart2.ChartAreas[0].AxisY;
            ax.Minimum = 0;
            ax.Maximum = 40;
            ax.Interval = 1;
            ay.Minimum = 0;
            ay.Maximum = 40;
            ay.Interval = 1;
            chart2.Series.Add("Question");
            List<float> hist1 = new List<float>(new float[40]);
            int num = 0;
            foreach (Student s in Master.studentMaster)
            {
                
                num = s.count;
                hist1[num - 1] = hist1[num -1] + 1;
            }


            chart2.Series["Question"].ChartType = SeriesChartType.Column;
            for (int j = 0; j <= 39; j++)
            {
                chart2.Series["Question"].Points.AddY(hist1[j]);
            }

            chart2.Series["Question"].ChartArea = "area2";
            chart2.Titles[1].IsDockedInsideChartArea = false;
            chart2.Titles[2].IsDockedInsideChartArea = false;
            chart2.Titles[1].DockedToChartArea = "area2";
            chart2.Titles[2].DockedToChartArea = "area2";
            chart2.Visible = true;
            chart2.Show();
            Refresh();


        }




            private void form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.RControlKey & Keys.Oemtilde))
            {
                MessageBox.Show("Advanced Mode Activated", "You have now activated the advanced mode. Take the proper precautions when using these features. These features can be found under the \"edit\" menu.");
                addAnIndidivualStudentToolStripMenuItem.Enabled = true;
                addAnIndidivualStudentToolStripMenuItem.Visible = true;
                removeAnIndividualStudentToolStripMenuItem.Enabled = true;
                removeAnIndividualStudentToolStripMenuItem.Visible = true;
                changeAnIndividualScoreToolStripMenuItem.Enabled = true;
                changeAnIndividualScoreToolStripMenuItem.Visible = true;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fileLoc.Text = "";
            schoolPath.Text = "";
            Master.studentMaster.Clear();
            Master.schoolDict.Clear();
            students.Clear();
            kyLst.Clear();
            tiBreaker.Clear();
            listView1.Items.Clear();
            listView2.Items.Clear();
            progressBar1.Value = 0;
            numStu.Visible = false;
            button2.Enabled = false;
            fileOp.Text = "File Not Opened";
            fileOp.ForeColor = red;
            chart1.ChartAreas.Clear();
            chart2.ChartAreas.Clear();
            chart1.Series.Clear();
            chart2.Series.Clear();
            fileRead = false;
            calcScores = false;
            label4.Visible = false;
            label3.Visible = false;
            label6.Visible = false;
            schoolsRead = false;
            openFileToolStripMenuItem.Enabled = false;
            openSchoolListToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = false;
            printToolStripMenuItem.Enabled = false;
            label5.Text = "Open School List";
            richTextBox1.Text = "This is the file with the list of schools and a corresponding unique number";
            label5.Visible = true;
            richTextBox1.Visible = true;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages[1])//your specific tabname
            {
                if (chart1.ChartAreas.Count >= 1)
                {
                    xAx.Visible = true;
                    yAx.Visible = true;
                    xBar.Visible = true;
                    yBar.Visible = true;
                }
                else
                {
                    if (fileRead == true && calcScores == true)
                    {
                        drawHistogram();
                    }

                    xAx.Visible = true;
                    yAx.Visible = true;
                    xBar.Visible = true;
                    yBar.Visible = true;
                }
            }
            else
            {
                xAx.Visible = false;
                yAx.Visible = false;
                xBar.Visible = false;
                yBar.Visible = false;
            }
            if (tabControl1.SelectedTab == tabControl1.TabPages[2])//your specific tabname
            {
                if (chart2.ChartAreas.Count >= 1)
                {

                }
                else
                {
                    if (fileRead == true && calcScores == true)
                    {
                        drawHistogram2();
                    }
                    
                }
            }
            if (tabControl1.SelectedTab == tabControl1.TabPages[0])
            {
                if (listView1.SelectedIndices.Count > 0)
                {
                    listView2.Visible = true;
                }
                else
                {
                    listView2.Visible = false;
                }
            }
            else
            {
                listView2.Visible = false;
            }


        }

        private void tab1Control1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void xBar_Scroll(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Interval = xBar.Value;
            Refresh();
        }

        private void yBar_Scroll(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Interval = yBar.Value;
            Refresh();
        }

        private void openSchoolListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                schoolPath.Text = file;

                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }

                string schlPath = schoolPath.Text;
                string[] Lines = File.ReadAllLines(schlPath);
                using (var reader = new StreamReader(schlPath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        line = Regex.Replace(line, @"\s+", ",");
                        var values = line.Split(',');
                        string schoolName = "";
                        for (int i = 1; i < values.Count(); i++)
                        {
                            schoolName += values[i] + " ";  
                        }
                        Master.schoolDict.Add(Convert.ToInt16(values[0]), schoolName);
                        Master.teamMaster.Add(new Team(schoolName, Convert.ToInt16(values[0])));
                    }
                }
                schoolsRead = true;
                openFileToolStripMenuItem.Enabled = true;
                openSchoolListToolStripMenuItem.Enabled = false;
            }
            label5.Text = "Open Scores File";
            richTextBox1.Text = "The file with the list of students and their scanned answers";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            listView2.Visible = true;
            listView2.Enabled = true;
            
            if (listView1.SelectedItems.Count > 0)

            {
                
                string listItem = listView1.SelectedItems[0].SubItems[4].Text;
                Student s = Master.studentMaster.FirstOrDefault(o => o.score == Convert.ToInt32(listItem));
                for (int i = 0; i <= 39; i++)
                {
                    ListViewItem itemA = new ListViewItem();
                    itemA = new ListViewItem(new[] { Convert.ToString(i+1), Convert.ToString(s.answersTF[i]), Convert.ToString(s.answers[i]),kyLst[i] });
                    if (s.answersTF[i] == true)
                    {
                        itemA.ForeColor = green;
                    }
                    else
                    {
                        itemA.ForeColor = red;
                    }
                    if (i % 2 == 0)
                    {
                        itemA.BackColor = Color.LightGray;
                    }
                    listView2.Items.Add(itemA);
                }
                
                listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                Refresh();

            }
            




        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
