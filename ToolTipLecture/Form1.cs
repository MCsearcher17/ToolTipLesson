using System.Runtime.InteropServices.Marshalling;
using static ToolTipLecture.Form1;


namespace ToolTipLecture
{
    public partial class Form1 : Form
    {
        public struct TableLocation
        {
            public int Row; 
            public int Column;
        }

        public TableLayoutPanel FormPanel;
        public Label UserWelcomeLabel;
        public Button SaveDataButton;
        public Button ClearListButton;
        public TextBox DataEntryTextBox;
        public ListBox DataListBox;
        public ControlBuilder.ToolTipInformation ToolTipInfo = new ControlBuilder.ToolTipInformation();

        public Form1()
        {
            InitializeComponent();
            Size buttonSize = new(400, 50);
            Size textBoxSize = new(400, 100);
            Size listBoxSize = new(400, 250);

            FormPanel = ControlBuilder.CreateTabelPanel(2, 3);
            UserWelcomeLabel = ControlBuilder.CreateLabel("Welcome to the ToolTip usage example!!");
            SaveDataButton = ControlBuilder.CreateButton("Save Text", buttonSize, Color.LightBlue);
            ClearListButton = ControlBuilder.CreateButton("Clear List Box", buttonSize, Color.LawnGreen);
            DataEntryTextBox = ControlBuilder.CreateTextBox(textBoxSize);
            DataListBox = ControlBuilder.CreateListBox(listBoxSize);

            this.Controls.Add(FormPanel);
            AddControlsToPanel(FormPanel, UserWelcomeLabel, new TableLocation { Column = 0, Row = 0 });
            AddControlsToPanel(FormPanel, DataEntryTextBox, new TableLocation { Column = 0, Row = 1 });
            AddControlsToPanel(FormPanel, SaveDataButton, new TableLocation { Column = 1, Row = 1 });
            AddControlsToPanel(FormPanel, ClearListButton, new TableLocation { Column = 1, Row = 2 });
            AddControlsToPanel(FormPanel, DataListBox, new TableLocation { Column = 0, Row = 2 });

            var userLabelToolTip = ControlBuilder.CreateToolTip(UserWelcomeLabel, ToolTipInfo, ControlBuilder.ControlType.Label);
            var saveButtonToolTip = ControlBuilder.CreateToolTip(SaveDataButton, ToolTipInfo, ControlBuilder.ControlType.Button);
            var clearButtonToolTip = ControlBuilder.CreateToolTip(ClearListButton, ToolTipInfo, ControlBuilder.ControlType.Button);
            var dataEntryTextBoxToolTip = ControlBuilder.CreateToolTip(DataEntryTextBox, ToolTipInfo, ControlBuilder.ControlType.TextBox);
            var dataListBoxToolTip = ControlBuilder.CreateToolTip(DataListBox, ToolTipInfo, ControlBuilder.ControlType.ListBox);


            //saveButtonToolTip.ToolTipIcon = ToolTipIcon.Info; // you can also set this icon inside the CreateToolTip method

            SaveDataButton.Click += new EventHandler(SaveDataButton_Click!);
            ClearListButton.Click += new EventHandler(ClearListButton_Click!);

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown!);
        }

        /* Alternative approach using a dictionary to store controls
         
         // Replace individual control fields with a dictionary
         private readonly Dictionary<string, Control> controls = new();
         private TableLayoutPanel FormPanel;

         public Form1()
         {
             InitializeComponent();
         
             FormPanel = CreatePanel();
             this.Controls.Add(FormPanel);
         
             // Define controls and their locations
             var controlDefinitions = new (string key, Control control, TableLocation location)[]
             {
                 ("UserWelcomeLabel", ControlBuilder.CreateLabel("Welcome to the ToolTip usage example!!"), new TableLocation { Column 0,   Row =  0 }),
                 ("DataEntryTextBox", ControlBuilder.CreateTextBox(new Size(400, 100)), new TableLocation { Column = 0, Row = 1 }),
                 ("SaveDataButton", ControlBuilder.CreateButton("Save Text", new Size(400, 50), Color.LightBlue), new TableLocati{   Column =  1,   Row = 1 }),
                 ("ClearListButton", ControlBuilder.CreateButton("Clear List Box", new Size(400, 50), Color.LawnGreen)newTableLocation        { Column =     1, Row = 2 }),
                 ("DataListBox", ControlBuilder.CreateListBox(new Size(400, 250)), new TableLocation { Column = 0, Row = 2 }),
             };
         
             // Add controls to dictionary and panel
             foreach (var (key, control, location) in controlDefinitions)
             {
                 controls[key] = control;
                 AddControlsToPanel(FormPanel, control, location);
             }
         
             // Access controls via dictionary
             var UserWelcomeLabel = (Label)controls["UserWelcomeLabel"];
             var SaveDataButton = (Button)controls["SaveDataButton"];
             var ClearListButton = (Button)controls["ClearListButton"];
             var DataEntryTextBox = (TextBox)controls["DataEntryTextBox"];
             var DataListBox = (ListBox)controls["DataListBox"];
         
             // ToolTips
             ControlBuilder.CreateToolTip(UserWelcomeLabel, "This is a label control", "Label Information");
             ControlBuilder.CreateToolTip(SaveDataButton, "This is a button control", "Button Information").ToolTipIcon=ToolTipIcon.Info;
             ControlBuilder.CreateToolTip(DataEntryTextBox, "This is a text box control", "Text Box Information");
             ControlBuilder.CreateToolTip(DataListBox, "This is a list box control", "List Box Information");
         
             SaveDataButton.Click += SaveDataButton_Click!;
             ClearListButton.Click += ClearListButton_Click!;
         }
         */

        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveDataButton.PerformClick();
                DataEntryTextBox.Clear();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                ClearListButton.PerformClick();
            }
        }

        public void AddControlsToPanel(TableLayoutPanel panel, Control ctrl, TableLocation tableLocation)
        {
            panel.Controls.Add(ctrl, tableLocation.Column, tableLocation.Row);
        }

        public void SaveDataButton_Click(object sender, EventArgs e)
        {
            DataListBox.Items.Add(DataEntryTextBox.Text + "\t" + DateTime.Now.ToString("T"));
        }

        public void ClearListButton_Click(object sender, EventArgs e)
        {
            DataEntryTextBox.Clear();
            DataListBox.Items.Clear();
        }
    }

    public static class ControlBuilder
    {
        public enum ControlType
        {
            Label,
            Button,
            TextBox,
            ListBox,
            ComboBox,
            TablePanel,
            FlowPanel
        }

        public struct ToolTipInformation
        {
            public string Button;
            public string TextBox;
            public string ListBox;
            public string Label;
            public string Unknown;

            public ToolTipInformation()
            {
                Button = "This is a button control";
                TextBox = "This is a text box control";
                ListBox = "This is a list box control";
                Label = "This is a label control";
                Unknown = "No information available";
            }
        }

        public static Label CreateLabel(string text,
                         Size? size = null)
        {
            Label label = new()
            {
                Text = text,
                Size = size ?? DefaultProperties.DefaultControlSizes.DefaultLabelSize,
                Font = DefaultProperties.DefaultFont,
                AutoSize = false,
            };
            return label;
        }

        public static Button CreateButton(string text,
                                   Size? size = null,
                                   Color? color = null,
                                   AnchorStyles? anchorStyle = null)
        {
            Button button = new()
            {
                Text = text,
                Size = size ?? DefaultProperties.DefaultControlSizes.DefaultButtonSize,
                BackColor = color ?? DefaultProperties.DefaultButtonColor,
                Font = DefaultProperties.DefaultFont,
                Anchor = anchorStyle ?? DefaultProperties.DefaultAnchorStyle,
            };
            return button;
        }

        // If you want to use this method again, you need to change the background image path
        public static TableLayoutPanel CreateTabelPanel(int? columnCount = null, int? rowCount = null)
        {
            TableLayoutPanel panel = new()
            {
                Dock = DockStyle.Fill,
                BackgroundImage = Resources.Resource1.background_1, 
                BackgroundImageLayout = ImageLayout.Stretch,
                Padding = new Padding(30),
                AutoScroll = true,
                ColumnCount = columnCount ?? DefaultProperties.DefaultTableProperties.ColumnCount,
                RowCount = rowCount ?? DefaultProperties.DefaultTableProperties.RowCount,
            };
            return panel;
        }

        public static ToolTip CreateToolTip(Control control,
                                            ToolTipInformation toolTipInfo,
                                            ControlType type,
                                            int autoPopDelay = 5000,
                                            int initialDelay = 500,
                                            int reshowDelay = 200,
                                            bool showAlways = true)
        {
            string text = GetToolTipText(toolTipInfo, type);
            string title = GetToolTipTitle(type);

            ToolTip toolTip = new()
            {
                ToolTipTitle = title,
                AutoPopDelay = autoPopDelay,
                InitialDelay = initialDelay,
                ReshowDelay = reshowDelay,
                ShowAlways = showAlways
            };
            
            toolTip.SetToolTip(control, text);

            return toolTip;
        }

        public static string GetToolTipText(ToolTipInformation toolTipInfo, ControlType type) => type switch
        {
            ControlType.Button => toolTipInfo.Button,
            ControlType.TextBox => toolTipInfo.TextBox,
            ControlType.ListBox => toolTipInfo.ListBox,
            ControlType.Label => toolTipInfo.Label,
            _ => toolTipInfo.Unknown
        };

        public static string GetToolTipTitle(ControlType type) => $"{type}";

        public static TextBox CreateTextBox(Size? size = null,
                                     Color? color = null,
                                     AnchorStyles? anchorStyle = null)
        {
            TextBox textBox = new()
            {
                Size = size ?? DefaultProperties.DefaultControlSizes.DefaultTextBoxSize,
                BackColor = color ?? Color.White,
                Font = DefaultProperties.DefaultFont,
                Anchor = anchorStyle ?? DefaultProperties.DefaultAnchorStyle,
            };
            return textBox;
        }

        public static ListBox CreateListBox(Size? size = null,
                                     Color? color = null,
                                     AnchorStyles? anchorStyle = null)
        {
            ListBox listBox = new()
            {
                Size = size ?? DefaultProperties.DefaultControlSizes.DefaultListBoxSize,
                BackColor = color ?? Color.White,
                Font = DefaultProperties.DefaultFont,
                Anchor = anchorStyle ?? DefaultProperties.DefaultAnchorStyle,
            };
            return listBox;
        }

        public static ComboBox CreateComboBox(Size? size = null,
                                     Color? color = null,
                                     AnchorStyles? anchorStyle = null)
        {
            ComboBox comboBox = new()
            {
                Size = size ?? DefaultProperties.DefaultControlSizes.DefaultTextBoxSize,
                BackColor = color ?? Color.White,
                Font = DefaultProperties.DefaultFont,
                Anchor = anchorStyle ?? DefaultProperties.DefaultAnchorStyle,
            };
            return comboBox;
        }

        public static FlowLayoutPanel CreateFlowPanel()
        {
            FlowLayoutPanel panel = new()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                AutoScroll = true,
                BackColor = Color.Aqua,
            };
            return panel;
        }
    }

    public class DefaultProperties
    {
        public static readonly Font DefaultFont = new("Arial", 12F, FontStyle.Bold);
        public static readonly Color DefaultButtonColor = Color.LightBlue;
        public static readonly AnchorStyles DefaultAnchorStyle = AnchorStyles.Top | AnchorStyles.Right;

        public struct DefaultTableProperties
        {
            public const int ColumnCount = 2;
            public const int RowCount = 2;
        }

        public struct DefaultControlSizes
        {
            public static readonly Size DefaultLabelSize = new(300, 30);
            public static readonly Size DefaultButtonSize = new(100, 50);
            public static readonly Size DefaultTextBoxSize = new(200, 30);
            public static readonly Size DefaultListBoxSize = new(200, 100);
        }
    }
}

