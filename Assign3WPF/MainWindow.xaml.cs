using System.IO.Compression;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assign3WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int iNumStudents; // number of students passed
        int iNumAssign; // number of assignments passed
        int iCurrentStudent = 0; // used to hold the current position in the list of student names

        string[] sStudentArr; // array to hold list of student names
        int[,] iAssignArr; // multidimensional array that stores assignment scores that correspond to the same index as the student

        StringBuilder sbAssignInfoDisp = new StringBuilder(); // used to populate data for assignment info for students
        StringBuilder sbStudentNames = new StringBuilder("STUDENT\n"); // used to populate list of student names, set in a separate textblock to avoid spacing issues

        StringBuilder sbFileOutputtxt = new StringBuilder();

        /// <summary>
        /// Main window that is used to initialize component
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// submits number of assignments and number of students to initialize arrays and allow for data input.
        /// Initial checks are to ensure both fields are not empty, are only numbers, and are between 1-10 for students and 1-99 for assignments.
        /// If all data passes initial checks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitCounts_Click(object sender, RoutedEventArgs e)
        {

            if (txtbxNumAssign == null || txtbxNumStudents == null)
            {
                lblCountsWarning.Content = "Please fill both fields";

            } else if (int.TryParse(txtbxNumAssign.Text, out iNumAssign) != true || int.TryParse(txtbxNumStudents.Text, out iNumStudents) != true)
            {
                lblCountsWarning.Content = "Please enter numbers only";

            } else if (iNumStudents > 10 || iNumStudents < 1)
            {
                lblCountsWarning.Content = "Please enter a number between 1-10";

            } else if (iNumAssign < 1 || iNumAssign > 100)
            {
                lblCountsWarning.Content = "Please enter a number between 1-100";

            } else
            {
                sStudentArr = new string[iNumStudents];
                iAssignArr = new int[iNumStudents, iNumAssign];

                lblAssignNumber.Content = "Enter Assignment Number(1-" + iNumAssign.ToString() + "):";

                string sWhiteSpace = "\t";

                for (int i = 0; i < iNumAssign; i++)
                {
                    sbAssignInfoDisp = sbAssignInfoDisp.Append("#" + (i+1).ToString() + sWhiteSpace);

                }

                sbAssignInfoDisp.Append("AVG" + sWhiteSpace + "GRADE\n");

                txtblkAssignDisp.Text = sbAssignInfoDisp.ToString();

                btnReset.IsEnabled = true;
                btnSaveStudentInfo.IsEnabled = true;
                btnSaveScore.IsEnabled = true;
                btnDispScores.IsEnabled = true;
                btnOutputToFile.IsEnabled = true;
                btnSubmitCounts.IsEnabled = false;
                btnSubmitCounts.IsEnabled = false;

                txtbxNumStudents.IsEnabled = false;
                txtbxNumAssign.IsEnabled = false;
                txtbxFileName.IsEnabled = true;

                for (int i = 0; i < iNumStudents; i++)
                {

                    for (int x = 0; x < iNumAssign; x++)
                    {
                        iAssignArr[i, x] = 0;
                    }

                    sStudentArr[i] = "Student #" + (i+1).ToString();
                }

            }

        }

        /// <summary>
        /// Will add student name to Student name array. When this is saved student is saved based on the current student variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveStudentInfo_Click(object sender, RoutedEventArgs e)
        {
            if (txtbxStudentInfo.Text == null)
            {
                sStudentArr[iCurrentStudent] = "Student #" + iCurrentStudent.ToString();
                txtbxStudentInfo.Text = sStudentArr[iCurrentStudent];

            } else
            {
                sStudentArr[iCurrentStudent] = txtbxStudentInfo.Text;
                txtbxStudentInfo.Text = sStudentArr[iCurrentStudent];

            }

            checkStudentIndex();

            lblStudentSaveSuccess.Foreground = Brushes.Green;
            lblStudentSaveSuccess.Content = "Student Saved: " + sStudentArr[iCurrentStudent];

        }

        /// <summary>
        /// This will pull the first student in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirstStudent_Click(object sender, RoutedEventArgs e)
        {
            iCurrentStudent = 0;

            lblStudentIndex.Content = "Student #" + (iCurrentStudent + 1);
            txtbxStudentInfo.Text = sStudentArr[iCurrentStudent];

            checkStudentIndex();

        }

        /// <summary>
        /// This will navigate to the previous student in the list. So if current index is 4, then previous student index would be 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevStudent_Click(object sender, RoutedEventArgs e)
        {
            iCurrentStudent--;

            lblStudentIndex.Content = "Student #" + (iCurrentStudent + 1);
            txtbxStudentInfo.Text = sStudentArr[iCurrentStudent];

            checkStudentIndex();

        }

        /// <summary>
        /// This will navigate to the next student in the list. So if current index is 4, then next student index would be 5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextStudent_Click(object sender, RoutedEventArgs e)
        {
            iCurrentStudent++;

            lblStudentIndex.Content = "Student #" + (iCurrentStudent + 1);
            txtbxStudentInfo.Text = sStudentArr[iCurrentStudent];

            checkStudentIndex();

        }

        /// <summary>
        /// This will navigate to the last student in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLastStudent_Click(object sender, RoutedEventArgs e)
        {
            iCurrentStudent = sStudentArr.Length - 1;

            lblStudentIndex.Content = "Student #" + (iCurrentStudent + 1);
            txtbxStudentInfo.Text = sStudentArr[iCurrentStudent];

            checkStudentIndex();

        }

        /// <summary>
        /// This will save the score for the entered assignment, or overwrite the previous score if it was entered incorrectly or otherwise.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveScore_Click(object sender, RoutedEventArgs e)
        {
            int iTempAssignNum;
            int iTempScore;

            if (int.TryParse(txtbxAssignNumber.Text, out iTempAssignNum) == false || int.TryParse(txtbxAssignScore.Text, out iTempScore) == false)
            {
                lblStudentInfoMsg.Foreground = Brushes.Red;
                lblStudentInfoMsg.Content = "Please use numbers only";

            } else if (iTempAssignNum > iNumAssign || iTempAssignNum < 1)
            {
                lblStudentInfoMsg.Foreground = Brushes.Red;
                lblStudentInfoMsg.Content = "Please enter a number between 1-" + iNumAssign.ToString();

            } else if (iTempScore < 0 || iTempScore > 100)
            {
                lblStudentInfoMsg.Foreground = Brushes.Red;
                lblStudentInfoMsg.Content = "Please enter a number between 0-99";
            } else
            {
                iAssignArr[iCurrentStudent, iTempAssignNum - 1] = iTempScore;

                btnDispScores.IsEnabled = true;
                lblStudentInfoMsg.Foreground = Brushes.Green;
                lblStudentInfoMsg.Content = "Assignment " + iTempAssignNum.ToString() + ": Info Saved";

                clearTxtBoxes();

            }


        }

        /// <summary>
        /// Takes all arrays and puts info into a string builder. Both arrays are put into their own TextBlocks.
        /// At the end of each loop, total score is added to be used to get a letter grade using a separate method and the 
        /// assignment total point average is calculated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDispScores_Click(object sender, RoutedEventArgs e)
        {
            this.sbAssignInfoDisp.Clear();
            this.sbStudentNames.Clear();

            createOutput();

            txtblkAssignDisp.Text = sbAssignInfoDisp.ToString();
            txtblkStudentNames.Text = sbStudentNames.ToString();

            svScrollDataDisplay.ScrollToEnd();

        }

        /// <summary>
        /// output to file method after button is clicked, this will create the stringBuilder output and send to clsOutputToFile class that will create a text file if the name does not already exist, to C:\Users\Public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnOutputToFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.sbFileOutputtxt.Clear();

                clsOutputToFile outputToFile = new clsOutputToFile();

                btnOutputToFile.IsEnabled = false;
                lblFileStatus.Content = "Writing to file";
                lblFileStatus.FontWeight = FontWeights.Bold;

                createOutput();

                string sFileName = txtbxFileName.Text.ToString();

                bool bFileCreated = false;

                await Task.Run(() => bFileCreated = outputToFile.writeToFile(this.sbFileOutputtxt, sFileName));

                if (bFileCreated)
                {
                    lblFileStatus.Content = "Finished Writing to File";
                    lblFileStatus.Foreground = Brushes.Green;

                }
                else
                {
                    lblFileStatus.Content = "Could not write to file";
                    lblFileStatus.Foreground = Brushes.Red;

                }

            }
            catch (Exception ex)
            {
                string message = $"An Error Occured: {ex.Message}\nStack Trace: {ex.StackTrace}";
                string caption = "Error Occured";

                MessageBoxButton buttons = MessageBoxButton.OK;

                MessageBox.Show(message, caption, buttons);

            }

        }

        /// <summary>
        /// method to create StringBuilder with details on students scores and names, that can be used either by display scores or output scores buttons.
        /// Duplicated StringBuilder to refrain from rebuilding most logic that was originally present.
        /// </summary>
        /// <returns></returns>
        private void createOutput()
        {
            this.sbFileOutputtxt.Clear();

            this.sbStudentNames.Append("STUDENT\n");
            this.sbFileOutputtxt.Append("STUDENT\t\t");

            int iStudentTotalScore = 0;

            for (int i = 0; i < iNumAssign; i++)
            {
                this.sbAssignInfoDisp = this.sbAssignInfoDisp.Append("#" + (i + 1).ToString() + "\t");
                this.sbFileOutputtxt.Append("#" + (i + 1).ToString() + "\t");

            }

            this.sbAssignInfoDisp.Append("AVG\tGRADE\n");
            this.sbFileOutputtxt.Append("AVG\tGRADE\n");

            for (int i = 0; i < iNumStudents; i++)
            {
                iStudentTotalScore = 0;

                this.sbStudentNames.Append(sStudentArr[i] + "\n");
                this.sbFileOutputtxt.Append(sStudentArr[i] + "\t");

                for (int x = 0; x < iNumAssign; x++)
                {
                    iStudentTotalScore = iStudentTotalScore + iAssignArr[i, x];
                    sbAssignInfoDisp.Append(iAssignArr[i, x] + "\t");
                    sbFileOutputtxt.Append(iAssignArr[i, x] + "\t");

                }


                string sTempLetterGrade = getLetterGrade(iStudentTotalScore / this.iNumAssign);

                sbAssignInfoDisp.Append(iStudentTotalScore / this.iNumAssign);
                sbFileOutputtxt.Append(iStudentTotalScore / this.iNumAssign);
                sbAssignInfoDisp.Append("\t");
                sbFileOutputtxt.Append("\t");
                sbAssignInfoDisp.Append(sTempLetterGrade);
                sbFileOutputtxt.Append(sTempLetterGrade);
                sbAssignInfoDisp.Append("\n");
                sbFileOutputtxt.Append("\n");

            }

        }

        /// <summary>
        /// This will return a letter grade depending on the average score of the student that has been entered
        /// </summary>
        /// <param name="iScore"></param>
        /// <returns></returns>
        private string getLetterGrade(int iScore)
        {
            if (iScore >= 93.00)
            {
                return "A";

            } else if (iScore >= 90.00 && iScore <= 92.99)
            {
                return "A-";

            } else if (iScore >= 87.00 && iScore <= 89.99)
            {
                return "B+";

            } else if (iScore >= 83.00 && iScore <= 86.99)
            {
                return "B";

            }
            else if (iScore >= 80.00 && iScore <= 82.99)
            {
                return "B-";

            }
            else if (iScore >= 77.00 && iScore <= 79.99)
            {
                return "C+";

            }
            else if (iScore >= 73.00 && iScore <= 76.99)
            {
                return "C";

            }
            else if (iScore >= 70.00 && iScore <= 72.99)
            {
                return "C-";

            }
            else if (iScore >= 67.00 && iScore <= 69.99)
            {
                return "D+";

            }
            else if (iScore >= 63.00 && iScore <= 66.99)
            {
                return "D";

            }
            else if (iScore >= 60.00 && iScore <= 62.99)
            {
                return "D-";

            }
            else
            {
                return "E";

            }

        }

        /// <summary>
        /// This checks the current student that is pulled up and disables buttons depending on current position
        /// </summary>
        private void checkStudentIndex()
        {
            clearTxtBoxes();

            if (this.iCurrentStudent == 0)
            {
                btnFirstStudent.IsEnabled = false;
                btnPrevStudent.IsEnabled = false;

                btnNextStudent.IsEnabled = true;
                btnLastStudent.IsEnabled = true;

            }
            else if (this.iCurrentStudent == this.iNumStudents - 1)
            {
                btnFirstStudent.IsEnabled = true;
                btnPrevStudent.IsEnabled = true;

                btnNextStudent.IsEnabled = false;
                btnLastStudent.IsEnabled = false;

            }
            else
            {
                btnFirstStudent.IsEnabled = true;
                btnPrevStudent.IsEnabled = true;
                btnNextStudent.IsEnabled = true;
                btnLastStudent.IsEnabled = true;

            }
        }

        /// <summary>
        /// this will reset all arrays and variables back to their original state to allow re-entry of all data including student names and assignments/scores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            iNumStudents = 0;
            iNumAssign = 0;
            iCurrentStudent = 0;
            sStudentArr = null;
            iAssignArr = null;

            sbAssignInfoDisp.Clear();
            sbStudentNames.Clear();

            btnDispScores.IsEnabled = false;
            btnSaveScore.IsEnabled = false;
            btnSaveStudentInfo.IsEnabled = false;
            btnFirstStudent.IsEnabled = false;
            btnPrevStudent.IsEnabled = false;
            btnNextStudent.IsEnabled = false;
            btnLastStudent.IsEnabled = false;
            btnReset.IsEnabled = false;

            btnSubmitCounts.IsEnabled = true;

            txtbxNumStudents.IsEnabled = true;
            txtbxNumAssign.IsEnabled = true;

            txtblkStudentNames.Text = "STUDENT";
            txtblkAssignDisp.Text = "#1\t#2\t#3\t#4\t#5\tAVG\tGRADE";

            txtbxStudentInfo.Clear();
            txtbxNumStudents.Clear();
            txtbxNumAssign.Clear();

            lblFileStatus.Content = "Choose File Name:";
            lblFileStatus.FontWeight = FontWeights.Regular;
            lblFileStatus.Foreground = Brushes.Black;

            clearTxtBoxes();

        }

        /// <summary>
        /// method to clear all textboxes called after reset button is clicked
        /// </summary>
        private void clearTxtBoxes()
        {
            txtbxAssignNumber.Clear();
            txtbxAssignScore.Clear();
            txtbxFileName.Clear();

        }

    }
}