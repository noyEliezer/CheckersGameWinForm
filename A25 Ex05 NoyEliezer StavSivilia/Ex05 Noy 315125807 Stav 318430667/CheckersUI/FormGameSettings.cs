using System;
using System.Windows.Forms;
using System.Drawing;
using Ex05.CheckersGame.CheckersLogic;
using Ex05.Properties;

namespace Ex05.CheckersGame.CheckersUI
{
    public class FormGameSettings : Form
    {
        private RadioButton[] m_ButtonsBoardSize;
        private TextBox m_TextBoxPlayer1Name;
        private TextBox m_TextBoxPlayer2Name;
        private CheckBox m_CheckBoxPlayer2;
        private Button m_ButtonDone;

        public FormGameSettings()
        {
            initializeComponent();
            initializeFormProperties();
            initializeControls();
        }
        
        public RadioButton[] ButtonsBoardSize
        {
            get 
            { 
                return m_ButtonsBoardSize; 
            }
            set 
            { 
                m_ButtonsBoardSize = value; 
            }
        }

        public TextBox TextBoxPlayer1Name
        {
            get
            { 
                return m_TextBoxPlayer1Name; 
            }
            set 
            { 
                m_TextBoxPlayer1Name = value;
            }
        }

        public TextBox TextBoxPlayer2Name
        {
            get 
            { 
                return m_TextBoxPlayer2Name;
            }
            set 
            { 
                m_TextBoxPlayer2Name = value;
            }
        }

        public CheckBox CheckBoxPlayer2
        {
            get 
            { 
                return m_CheckBoxPlayer2;
            }
            set 
            { 
                m_CheckBoxPlayer2 = value;
            }
        }

        public Button ButtonDone
        {
            get 
            { 
                return m_ButtonDone;
            }
            set 
            { 
                m_ButtonDone = value;
            }
        }

        private void initializeComponent() { }

        private void initializeFormProperties()
        {
            this.Text = "Game Settings";
            this.Icon = Resources.SettingsIcon;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(270, 225);
        }

        private void initializeControls()
        {
            Label boardSizeLabel = createBoardSizeSection();
            Label playersLabel = createPlayersSection();
            Button doneButton = createDoneButton();

            this.Controls.Add(boardSizeLabel);
            this.Controls.Add(playersLabel);
            this.Controls.Add(doneButton);
            this.AcceptButton = ButtonDone;
        }

        private Label createBoardSizeSection()
        {
            Label labelBoardSize = createLabel("Board Size:", new Point(10, 10), new Size(250, 60));
            
            initializeBoardSizeButtons();

            return labelBoardSize;
        }

        private void initializeBoardSizeButtons()
        {
            int[] validSizes = Board.ValidBoardSizes;

            ButtonsBoardSize = new RadioButton[validSizes.Length];
            for (int i = 0; i < validSizes.Length; i++)
            {
                ButtonsBoardSize[i] = createBoardSizeRadioButton(validSizes[i], i);
                Controls.Add(ButtonsBoardSize[i]);
            }

            ButtonsBoardSize[0].Checked = true;
        }

        private RadioButton createBoardSizeRadioButton(int i_Size, int i_Index)
        {
            RadioButton radioButton = new RadioButton();

            radioButton.Text = $"{i_Size} x {i_Size}";
            radioButton.Location = new Point(20 + i_Index * 80, 25);
            radioButton.AutoSize = true;

            return radioButton;
        }

        private Label createPlayersSection()
        {
            Label playersLabel = createLabel("Players:", new Point(10, 80), new Size(250, 100));

            initializePlayer1Controls(playersLabel);
            initializePlayer2Controls(playersLabel);

            return playersLabel;
        }

        private void initializePlayer1Controls(Label i_PlayersLabel)
        {
            Label player1Label = createLabel("Player 1:", new Point(20, 25), Size.Empty, true);
            
            TextBoxPlayer1Name = createTextBox(new Point(70, 22), new Size(120, 20));
            i_PlayersLabel.Controls.AddRange(new Control[] { player1Label, TextBoxPlayer1Name });
        }

        private void initializePlayer2Controls(Label i_PlayersLabel)
        {
            CheckBoxPlayer2 = createCheckBox("Player 2:", new Point(3, 55));
            TextBoxPlayer2Name = createTextBox(new Point(70, 52), new Size(120, 20), "[Computer]", false);
            CheckBoxPlayer2.CheckedChanged += Player2CheckBox_CheckedChanged;
            i_PlayersLabel.Controls.AddRange(new Control[] { CheckBoxPlayer2, TextBoxPlayer2Name });
        }

        private Button createDoneButton()
        {
            ButtonDone = createButton("Done", new Point(185, 190), new Size(75, 23), DialogResult.OK);
            ButtonDone.Click += buttonDone_Click;

            return ButtonDone;
        }

        private Label createLabel(string i_Text, Point i_Location, Size i_Size, bool i_AutoSize = false)
        {
            Label label = new Label();

            label.Text = i_Text;
            label.Location = i_Location;
            label.Size = i_AutoSize ? Size.Empty : i_Size;
            label.AutoSize = i_AutoSize;

            return label;
        }

        private TextBox createTextBox(Point i_Location, Size i_Size, string i_InitialText = "", bool i_Enabled = true)
        {
            TextBox textBox = new TextBox();
           
            textBox.Location = i_Location;
            textBox.Size = i_Size;
            textBox.Text = i_InitialText;
            textBox.Enabled = i_Enabled;

            return textBox;
        }

        private CheckBox createCheckBox(string i_Text, Point i_Location, bool i_AutoSize = true)
        {
            CheckBox checkBox = new CheckBox();
            
            checkBox.Location = i_Location;
            checkBox.Text = i_Text;
            checkBox.AutoSize = i_AutoSize;

            return checkBox;
        }

        private Button createButton(string i_Text, Point i_Location, Size i_Size, DialogResult i_DialogResult = DialogResult.None)
        {
            Button button = new Button();
            
            button.Location = i_Location;
            button.Size = i_Size;
            button.Text = i_Text;
            button.DialogResult = i_DialogResult;

            return button;
        }

        internal int GetSelectedBoardSize()
        {
            int[] validSizes = Board.ValidBoardSizes;
            int validSize = validSizes[0];

            for (int i = 0; i < ButtonsBoardSize.Length; i++)
            {
                if (ButtonsBoardSize[i].Checked)
                {
                    validSize = validSizes[i];
                }
            }

            return validSize;
        }

        private void Player2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxPlayer2Name.Enabled = CheckBoxPlayer2.Checked;
            if (!CheckBoxPlayer2.Checked)
            {
                TextBoxPlayer2Name.Text = "[Computer]";
            }
            else
            {
                TextBoxPlayer2Name.Text = string.Empty;
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (isValidInput())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.None;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        private bool isValidInput()
        {
            return isValidRequiredFields() && isValidPlayersNames();
        }

        private bool isValidPlayersNames()
        {
            Player.ValidatePlayerName(TextBoxPlayer1Name.Text);

            if (CheckBoxPlayer2.Checked)
            {
                Player.ValidatePlayerName(TextBoxPlayer2Name.Text);
            }

            return true;
        }

        private bool isValidRequiredFields()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(TextBoxPlayer1Name.Text))
            {
                MessageBox.Show("Please enter Player 1 name", "Missing Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }
            else if (CheckBoxPlayer2.Checked && string.IsNullOrWhiteSpace(TextBoxPlayer2Name.Text))
            {
                MessageBox.Show("Please enter Player 2 name", "Missing Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            return isValid;
        }

        internal string GetPlayer1Name()
        {
            return TextBoxPlayer1Name.Text;
        }

        internal string GetPlayer2Name()
        {
            return IsPlayer2Human() ? TextBoxPlayer2Name.Text : "Computer";
        }

        internal bool IsPlayer2Human()
        {
            return CheckBoxPlayer2.Checked;
        }
    }
}
