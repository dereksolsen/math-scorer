using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Math_Competition_Scorer
{
    public partial class Remove_Individual : Form
    {

        int index;

        public Remove_Individual()
        {
            InitializeComponent();
        }

        private void reloadForm()
        {
            Master.studentMaster.Sort(delegate (Student x, Student y) { return x.fName.CompareTo(y.fName); });
            foreach (Student s in Master.studentMaster)
            {
                ListViewItem item = new ListViewItem(new[] { s.fName, s.lName, Convert.ToString(s.school) });
                listBox1.Items.Add(item);
            }
            listBox1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Remove_Individual_Load(object sender, EventArgs e)
        {
            Master.studentMaster.Sort(delegate (Student x, Student y) { return x.fName.CompareTo(y.fName); });
            foreach (Student s in Master.studentMaster)
            {
                ListViewItem item = new ListViewItem(new[] { s.fName, s.lName, Convert.ToString(s.school) });
                listBox1.Items.Add(item);
            }
            listBox1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        


        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove " + Master.studentMaster[index].fName +" " + Master.studentMaster[index].lName +" from the competition?", "Warning",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Student temp = Master.studentMaster[index];
                Master.studentMaster.Remove(temp);
                Remove_Individual fr = new Remove_Individual();
                fr.Show();
                this.Close();

            }
            else if (result == DialogResult.No)
            {
                this.Close();
                MessageBox.Show(Master.studentMaster[index].fName + " " + Master.studentMaster[index].lName + " will not be removed.");
            }
            else if (result == DialogResult.Cancel)
            {
                this.Close();
            }
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            index = listBox1.FocusedItem.Index;
            Console.WriteLine(index);
        }
    }
}
