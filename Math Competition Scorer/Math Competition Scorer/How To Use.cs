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
    public partial class How_To_Use : Form
    {
        List<string> helpMenu = new List<string>();
        List<string> helpText = new List<string>();
        public How_To_Use()
        {
            InitializeComponent();
            helpMenu.Add("Opening a File");
            helpMenu.Add("Computing Scores");
            helpMenu.Add("Sorting");
            helpMenu.Add("Exporting Results");
            helpMenu.Add("Adding a Student");
            helpMenu.Add("Removing a Student");
            helpMenu.Add("Changing a Score");
            helpText.Add("To open a file navigate to the file menu at the top of the main window. From there select \"Open File\", This will create a new dialog from which you can browse to the file location. Currently the only supported file type is .csv");
            helpText.Add("To compute scores you must have a valid file open before the button will be enabled. To open a file see \"Opening a File\"");
            helpText.Add("Sorting is done through the \"Edit\" menu. There are various ways to sort, the different options can be seen in the Edit menu dropdown.");
            helpText.Add("Exporting is done through creating a text file to a save location. To export results, navigate to the \"File\" menu and select \"Save As\" this will open a save file dialog from which a destination can be chosen for where the text file is written. \n(Note: your wondows account must have permission to save in the selected directory for the export to be successful.)");
            helpText.Add("Adding a student is found in the \"Edit\" menu. To add a student select \"Add An Individual Student\" doing so will prompt a new window with various text fields. All fields must be completed in order to for the \"Add Student\" button to be selectable. Be careful with this as not all text boxes are data verified and an improper data entry may crash the program.");
            helpText.Add("To remove a Navigate again to the \"Edit\" menu. From there  select the \"Remove an Individual Student\" This will populate a new form with a list off all students currently in the master list. Select (highlight) the student you wish to remove and then select \"Delete Selected Student\". It will prompt for confirmation with the student's name in them prompt as a secondary check.");
            helpText.Add("To Change a score navigate to the \"Edit\" menu and select \"Change An Individual Score\". Note, this feature can only be used to change a student's score and not a team or school score. However Final school and team scores will be recalculated upon closing this form.");

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            richTextBox1.Text = helpText[index];
        }

        private void How_To_Use_Load(object sender, EventArgs e)
        {
            foreach (string s in helpMenu)
            {
                listBox1.Items.Add(s);
            }
        }
    }
}
