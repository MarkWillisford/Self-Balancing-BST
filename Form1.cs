using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WillisfordProg260CourseProject;

// Mark Willisford
// Prog 260, Course Project
// Notes and citations - TODO

namespace WillisfordProg260CourseProject
{
    public partial class Form1 : Form
    {
        BST _bst = new BST();
        Book TheBreaking = new Book("The Breaking", "Mark Willisford", 5, 2018, 1);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            int ISBN;
            if (int.TryParse(textISBN.Text, out ISBN))
            {
                Book loadedBook = _bst.Find(ISBN);
                textTitle.Text = loadedBook.Title;
                textAuthor.Text = loadedBook.Author;
                textRating.Text = loadedBook.Rating.ToString();
                textYear.Text = loadedBook.PubYear.ToString();                       
            }
            else
            {
                MessageBox.Show("Please input a number");
                textISBN.Text = "";
                textISBN.Select();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_bst.Add(TheBreaking))
            {
                MessageBox.Show("The Breaking successfully added");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int toAddYear;
            byte toAddRating;
            string toAddAuthor = textAuthor.Text;
            string toAddTitle = textTitle.Text;
            int toAddISBN;
            string errorMessage = "";
            bool success = true;

            if (textISBN.Text == null || !int.TryParse(textISBN.Text, out toAddISBN))
            {
                errorMessage = "The ISBN must be an integer";
                success = false;
                textISBN.Text = "";
            }
            if (toAddTitle == "")
            {
                errorMessage = errorMessage + "\r\n" + "Please enter a Title";
                success = false;
            }
            if (toAddAuthor == "")
            {
                errorMessage = errorMessage + "\r\n" + "Please enter an Author";
                success = false;
            }
            if (textRating.Text == null || !byte.TryParse(textRating.Text, out toAddRating) || toAddRating <1 || toAddRating > 5)
            {
                errorMessage = errorMessage + "\r\n" + "The Rating must be a valid integer between 1 and 5";
                success = false;
                textRating.Text = "";
            }
            if (textYear.Text == null || !int.TryParse(textYear.Text, out toAddYear))
            {
                errorMessage = errorMessage + "\r\n" + "The Year must be an integer";
                success = false;
                textYear.Text = "";
            }

            if (success == false)
            {
                lblWarning.Visible = true;
                lblWarning.Text = errorMessage;

                // this seemed like fun, thanks to Stackoverflow for the idea
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 5000;
                timer.Tick += (source, ex) => { lblWarning.Visible = false; lblWarning.Text = ""; timer.Stop(); };
                timer.Start();
                return;
            }

            // if we get past the return we can now assign the variables
            toAddISBN = Convert.ToInt32(textISBN.Text);
            toAddRating = Convert.ToByte(textRating.Text);
            toAddYear = Convert.ToInt32(textYear.Text);
            // 2.) create a book object
            Book toAddBook = new Book(toAddTitle, toAddAuthor, toAddRating, toAddYear, toAddISBN);
            // 3.) add it to the BST
            success = _bst.Add(toAddBook);
            // 4.) Varify recieved true (success) from #3
            if (!success)
            {
                // 5.) Should I add an error message? It isn't in the assignment. Maybe an invisible label
                lblWarning.Text = "ISBN is already used. Please try again";
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 5000;
                timer.Tick += (source, ex) => { lblWarning.Visible = false; lblWarning.Text = ""; timer.Stop(); };
                timer.Start();          // Okay I should have put this in a function and just called it. TODO
                textISBN.Text = "";
                textISBN.Focus();
                return;
            }
            else
            {
                textISBN.Text = "";             // TODO put this in a "reset form" function
                textTitle.Text = "";
                textAuthor.Text = "";
                textRating.Text = "";
                textYear.Text = "";
                textISBN.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int ISBN;
            // check input for int
            if (!int.TryParse(textISBN.Text, out ISBN))
            {
                MessageBox.Show("Please input a number");
                textISBN.Text = "";
                textISBN.Select();
            }
            else
            {
                // 1.) check if they are trying to delete The Breaking
                if (ISBN == 1)
                {
                    MessageBox.Show("You are trying to delete The Breaking" + "\r\n" + "I can not be stopped, I will publish next year");
                    textISBN.Text = "";
                    textISBN.Select();
                }
                else
                {
                    // 2.) delete node with the correct (passed in) int
                    bool success = _bst.Delete(ISBN);
                    // 3.) varify success
                    if (!success)
                    {
                        // 4.) "appropriete error" on fail. Though Kurt said he ignored it on his. /shrug I'll use the invisible label again
                        lblWarning.Visible = true;
                        lblWarning.Text = "Deletion failed, did you enter a valid ISBN number?";
                        textISBN.Text = "";
                        textISBN.Select();

                        // timer again, thanks to Stackoverflow for the idea
                        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                        timer.Interval = 5000;
                        timer.Tick += (source, ex) => { lblWarning.Visible = false; lblWarning.Text = ""; timer.Stop(); };
                        timer.Start();
                    }
                    else
                    {
                        textISBN.Text = "";
                        textTitle.Text = "";
                        textAuthor.Text = "";
                        textRating.Text = "";
                        textYear.Text = "";
                        textISBN.Focus();
                    }
                }
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            //TODO - redo with a few "minor" adjustments
            //List<Book> BooksToShow = new List<Book>();
            //BooksToShow = _bst.Show();
            dgvViewAll.DataSource = _bst.Show(); // BooksToShow;
            dgvViewAll.Update();
        }
    }
}
