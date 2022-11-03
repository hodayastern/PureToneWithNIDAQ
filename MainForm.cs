/******************************************************************************
*
* Example program:
*   ContGenVoltageWfm_IntClk
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to continuously output a periodic waveform
*   using an internal sample clock.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is output
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.
*   3.  Enter the desired rate for the generation. The onboard sample clock will
*       operate at this rate.
*   4.  Select the desired waveform type.
*   5.  The rest of the parameters in the Function Generator Parameters section
*       will affect the way the waveform is created, before it's sent to the
*       analog output of the board. Select the amplitude, number of samples per
*       buffer, and the number of cycles per buffer to be used as waveform data.
*
* Steps:
*   1.  Create a new task and an analog output voltage channel.
*   2.  Call the ConfigureSampleClock method to define the sample rate and a
*       continuous sample mode.
*   3.  Create a AnalogSingleChannelWriter and call the WriteMultiSample method
*       to write the waveform data to a buffer.
*   4.  Call Task.Start().
*   5.  When the user presses the stop button, stop the task.
*   6.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   7.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal output terminal matches the physical channel control.
*   In this example, the signal will output to the ao0 pin on your DAQ device.
*   For more information on the input and output terminals for your device, open
*   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
*   Considerations books in the table of contents.
*
* Microsoft Windows Vista User Account Control
*   Running certain applications on Microsoft Windows Vista requires
*   administrator privileges, 
*   because the application name contains keywords such as setup, update, or
*   install. To avoid this problem, 
*   you must add an additional manifest to the application that specifies the
*   privileges required to run 
*   the application. Some Measurement Studio NI-DAQmx examples for Visual Studio
*   include these keywords. 
*   Therefore, all examples for Visual Studio are shipped with an additional
*   manifest file that you must 
*   embed in the example executable. The manifest file is named
*   [ExampleName].exe.manifest, where [ExampleName] 
*   is the NI-provided example name. For information on how to embed the manifest
*   file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.Note: 
*   The manifest file is not provided with examples for Visual Studio .NET 2003.
*
******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.DAQmx;
using NationalInstruments.Examples;

namespace NationalInstruments.Examples.ContGenVoltageWfm_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.TextBox maximumTextBox;
        private System.Windows.Forms.TextBox minimumTextBox;
        private System.Windows.Forms.GroupBox functionGeneratorGroupBox;
        private System.Windows.Forms.Label frequencyLabel;
        internal System.Windows.Forms.Label amplitudeLabel;
        private Task myTask;
        private System.Windows.Forms.NumericUpDown samplesPerCycleNumeric;
        private System.Windows.Forms.Label samplesPerCycleLabel;
        internal System.Windows.Forms.NumericUpDown amplitudeNumeric;
        internal System.Windows.Forms.NumericUpDown durationNumeric;
        internal System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.Timer statusCheckTimer;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.ComponentModel.IContainer components;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.maximumTextBox = new System.Windows.Forms.TextBox();
            this.minimumTextBox = new System.Windows.Forms.TextBox();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.functionGeneratorGroupBox = new System.Windows.Forms.GroupBox();
            this.amplitudeLabel = new System.Windows.Forms.Label();
            this.amplitudeNumeric = new System.Windows.Forms.NumericUpDown();
            this.durationLabel = new System.Windows.Forms.Label();
            this.durationNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerCycleNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerCycleLabel = new System.Windows.Forms.Label();
            this.statusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            this.channelParametersGroupBox.SuspendLayout();
            this.functionGeneratorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerCycleNumeric)).BeginInit();
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(184, 414);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(70, 414);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.frequencyNumeric);
            this.timingParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(46, 150);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(247, 64);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.Location = new System.Drawing.Point(120, 24);
            this.frequencyNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.frequencyNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frequencyNumeric.Name = "frequencyNumeric";
            this.frequencyNumeric.Size = new System.Drawing.Size(112, 20);
            this.frequencyNumeric.TabIndex = 1;
            this.frequencyNumeric.Value = new decimal(new int[] {
            440,
            0,
            0,
            0});
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 26);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(88, 16);
            this.frequencyLabel.TabIndex = 0;
            this.frequencyLabel.Text = "Frequency (Hz):";
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumTextBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumTextBox);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(46, 14);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(247, 128);
            this.channelParametersGroupBox.TabIndex = 2;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(120, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(112, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev2/ao1";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // maximumTextBox
            // 
            this.maximumTextBox.Location = new System.Drawing.Point(120, 96);
            this.maximumTextBox.Name = "maximumTextBox";
            this.maximumTextBox.Size = new System.Drawing.Size(112, 20);
            this.maximumTextBox.TabIndex = 5;
            this.maximumTextBox.Text = "10";
            // 
            // minimumTextBox
            // 
            this.minimumTextBox.Location = new System.Drawing.Point(120, 60);
            this.minimumTextBox.Name = "minimumTextBox";
            this.minimumTextBox.Size = new System.Drawing.Size(112, 20);
            this.minimumTextBox.TabIndex = 3;
            this.minimumTextBox.Text = "-10";
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(16, 98);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum Value (V):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 62);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(104, 16);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum Value (V):";
            // 
            // functionGeneratorGroupBox
            // 
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.amplitudeNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.durationNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.durationLabel);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerCycleNumeric);
            this.functionGeneratorGroupBox.Controls.Add(this.samplesPerCycleLabel);
            this.functionGeneratorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.functionGeneratorGroupBox.Location = new System.Drawing.Point(45, 222);
            this.functionGeneratorGroupBox.Name = "functionGeneratorGroupBox";
            this.functionGeneratorGroupBox.Size = new System.Drawing.Size(248, 176);
            this.functionGeneratorGroupBox.TabIndex = 4;
            this.functionGeneratorGroupBox.TabStop = false;
            this.functionGeneratorGroupBox.Text = "Function Generator Parameters";
            // 
            // amplitudeLabel
            // 
            this.amplitudeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.amplitudeLabel.Location = new System.Drawing.Point(16, 138);
            this.amplitudeLabel.Name = "amplitudeLabel";
            this.amplitudeLabel.Size = new System.Drawing.Size(56, 16);
            this.amplitudeLabel.TabIndex = 6;
            this.amplitudeLabel.Text = "Amplitude:";
            // 
            // amplitudeNumeric
            // 
            this.amplitudeNumeric.DecimalPlaces = 1;
            this.amplitudeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.amplitudeNumeric.Location = new System.Drawing.Point(120, 136);
            this.amplitudeNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.amplitudeNumeric.Name = "amplitudeNumeric";
            this.amplitudeNumeric.Size = new System.Drawing.Size(112, 20);
            this.amplitudeNumeric.TabIndex = 7;
            this.amplitudeNumeric.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.amplitudeNumeric.ValueChanged += new System.EventHandler(this.amplitudeNumeric_ValueChanged);
            // 
            // durationNumeric
            // 
            this.durationNumeric.Location = new System.Drawing.Point(120, 56);
            this.durationNumeric.Maximum = new decimal(new int[] {
                10000000,
                0,
                0,
                0});
            this.durationNumeric.Minimum = new decimal(new int[] {
                1,
                0,
                0,
                0});
            this.durationNumeric.Name = "durationNumeric";
            this.durationNumeric.Size = new System.Drawing.Size(112, 20);
            this.durationNumeric.TabIndex = 5;
            this.durationNumeric.Value = new decimal(new int[] {
                1,
                0,
                0,
                0});
            // 
            // durationLabel
            // 
            this.durationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.durationLabel.Location = new System.Drawing.Point(16, 56);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(112, 16);
            this.durationLabel.TabIndex = 4;
            this.durationLabel.Text = "Duration:";
            // 
            // samplesPerCycleNumeric
            // 
            this.samplesPerCycleNumeric.Location = new System.Drawing.Point(120, 96);
            this.samplesPerCycleNumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.samplesPerCycleNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.samplesPerCycleNumeric.Name = "samplesPerCycleNumeric";
            this.samplesPerCycleNumeric.Size = new System.Drawing.Size(112, 20);
            this.samplesPerCycleNumeric.TabIndex = 5;
            this.samplesPerCycleNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // samplesPerCycleLabel
            // 
            this.samplesPerCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesPerCycleLabel.Location = new System.Drawing.Point(16, 98);
            this.samplesPerCycleLabel.Name = "samplesPerCycleLabel";
            this.samplesPerCycleLabel.Size = new System.Drawing.Size(112, 16);
            this.samplesPerCycleLabel.TabIndex = 4;
            this.samplesPerCycleLabel.Text = "Samples Per Cycle:";
            // 
            // statusCheckTimer
            // 
            this.statusCheckTimer.Tick += new System.EventHandler(this.statusCheckTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(338, 453);
            this.Controls.Add(this.functionGeneratorGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Voltage Generation - Int Clk";
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            this.channelParametersGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.PerformLayout();
            this.functionGeneratorGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.durationNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.amplitudeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerCycleNumeric)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new MainForm());
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // create the task and channel
                myTask = new Task();
                myTask.AOChannels.CreateVoltageChannel(physicalChannelComboBox.Text,
                    "",
                    Convert.ToDouble(minimumTextBox.Text), 
                    Convert.ToDouble(maximumTextBox.Text),
                    AOVoltageUnits.Volts);

                // verify the task before doing the waveform calculations
                myTask.Control(TaskAction.Verify);

                // calculate some waveform parameters and generate data

                string frequency = frequencyNumeric.Value.ToString();
                string samplesPerCycle = samplesPerCycleNumeric.Value.ToString();
                string duration = durationNumeric.Value.ToString();


                FunctionGenerator fGen = new FunctionGenerator(
                    myTask.Timing,
                    frequency,
                    duration,
                    samplesPerCycle,
                    amplitudeNumeric.Value.ToString());
                
                
                // configure the sample clock with the calculated rate
                Console.WriteLine("actual sample clock rate is: "+  fGen.ResultingSampleClockRate);

                int samplesPerChannel = Convert.ToInt32((Double.Parse(frequency) * Double.Parse(duration)) * int.Parse(samplesPerCycle));

                Console.WriteLine("actual sample per channel: " + samplesPerChannel);

                myTask.Timing.ConfigureSampleClock("",
                    fGen.ResultingSampleClockRate,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.FiniteSamples, samplesPerChannel);


                AnalogSingleChannelWriter writer = 
                    new AnalogSingleChannelWriter(myTask.Stream);

                //write data to buffer
                writer.WriteMultiSample(false,fGen.Data);
                
                //start writing out data
                myTask.Start();
                
                startButton.Enabled = false;
                stopButton.Enabled = true;

                statusCheckTimer.Enabled = true;
            }
            catch(DaqException err)
            {
                statusCheckTimer.Enabled = false;
                MessageBox.Show(err.Message);
                myTask.Dispose();
            }
            Cursor.Current = Cursors.Default;
        }
        
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            statusCheckTimer.Enabled = false;
            if (myTask != null)
            {
                try
                {
                    myTask.Stop();
                }
                catch(Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                myTask.Dispose();
                myTask = null;
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void statusCheckTimer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                // Getting myTask.IsDone also checks for errors that would prematurely
                // halt the continuous generation.
                if (myTask.IsDone)
                {
                    statusCheckTimer.Enabled = false;
                    myTask.Stop();
                    myTask.Dispose();
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                }
            }
            catch (DaqException ex)
            {
                statusCheckTimer.Enabled = false;
                System.Windows.Forms.MessageBox.Show(ex.Message);
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }

        private void amplitudeNumeric_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
