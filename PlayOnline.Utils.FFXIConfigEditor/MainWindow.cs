using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIConfigEditor {

  public class MainWindow : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.GroupBox grpGlobalConfig;
    private System.Windows.Forms.Label lblGUIResolution;
    private System.Windows.Forms.TextBox txtGUIWidth;
    private System.Windows.Forms.Label lblGUIX;
    private System.Windows.Forms.TextBox txtGUIHeight;
    private System.Windows.Forms.Label lbl3DResolution;
    private System.Windows.Forms.TextBox txt3DWidth;
    private System.Windows.Forms.Label lbl3DX;
    private System.Windows.Forms.TextBox txt3DHeight;
    private System.Windows.Forms.PictureBox picWarning;
    private System.Windows.Forms.Label lblWarning;
    private System.Windows.Forms.GroupBox grpCharConfig;
    private System.Windows.Forms.ComboBox cmbCharacters;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnApply;
    private System.Windows.Forms.Label lblColor1;
    private System.Windows.Forms.Label lblColor2;
    private System.Windows.Forms.Label lblColor3;
    private System.Windows.Forms.Label lblColor6;
    private System.Windows.Forms.Label lblColor5;
    private System.Windows.Forms.Label lblColor4;
    private System.Windows.Forms.Label lblColor20;
    private System.Windows.Forms.Label lblColor21;
    private System.Windows.Forms.Label lblColor22;
    private System.Windows.Forms.Label lblColor19;
    private System.Windows.Forms.Label lblColor18;
    private System.Windows.Forms.Label lblColor17;
    private System.Windows.Forms.Label lblColor12;
    private System.Windows.Forms.Label lblColor13;
    private System.Windows.Forms.Label lblColor14;
    private System.Windows.Forms.Label lblColor11;
    private System.Windows.Forms.Label lblColor10;
    private System.Windows.Forms.Label lblColor9;
    private System.Windows.Forms.Label lblColor15;
    private System.Windows.Forms.Label lblColor23;
    private System.Windows.Forms.Label lblColor7;
    private System.Windows.Forms.Label lblColor16;
    private System.Windows.Forms.Label lblColor8;

    private System.ComponentModel.Container components = null;

    #endregion
    private System.Windows.Forms.ColorDialog dlgChooseColor;
    private System.Windows.Forms.TextBox txtSoundEffects;
    private System.Windows.Forms.Label lblSoundEffects;

    private Label[] ColorLabels;

    public MainWindow() {
      this.InitializeComponent();
      this.Icon = Icons.POLConfig;
      this.picWarning.Image = Icons.POLConfigWarn.ToBitmap();
      this.lblWarning.Text = I18N.GetText("SettingsWarning");
      this.cmbCharacters.Items.Clear();
#if DEBUG
      foreach (Character C in Game.Characters)
	this.cmbCharacters.Items.Add(new CharacterConfig(C));
#endif
      if (this.cmbCharacters.Items.Count == 0) {
	this.grpCharConfig.Visible = false;
	this.Height -= this.grpCharConfig.Height + this.grpCharConfig.Top - (this.grpGlobalConfig.Top + this.grpGlobalConfig.Height);
      }
      else {
	this.ColorLabels = new Label[] {
	  this.lblColor1,  this.lblColor2,  this.lblColor3,  this.lblColor4,  this.lblColor5,  this.lblColor6,
	  this.lblColor7,  this.lblColor8,  this.lblColor9,  this.lblColor10, this.lblColor11, this.lblColor12,
	  this.lblColor13, this.lblColor14, this.lblColor15, this.lblColor16, this.lblColor17, this.lblColor18,
	  this.lblColor19, this.lblColor20, this.lblColor21, this.lblColor22, this.lblColor23
	};
      }
      this.LoadSettings();
    }

    private void LoadSettings() {
      // Load Global settings
    RegistryKey ConfigKey = POL.OpenAppConfigKey(AppID.FFXI);
      if (ConfigKey == null)
	this.grpGlobalConfig.Enabled = false;
      else {
	this.txtGUIWidth.Text     = String.Format("{0}", ConfigKey.GetValue("0001"));
	this.txtGUIHeight.Text    = String.Format("{0}", ConfigKey.GetValue("0002"));
	this.txt3DWidth.Text      = String.Format("{0}", ConfigKey.GetValue("0003"));
	this.txt3DHeight.Text     = String.Format("{0}", ConfigKey.GetValue("0004"));
	this.txtSoundEffects.Text = String.Format("{0}", ConfigKey.GetValue("0029"));
      }
      // Select the first character to trigger loading its config
      if (this.cmbCharacters.Items.Count > 0)
	this.cmbCharacters.SelectedIndex = 0;
      this.btnApply.Enabled = false;
    }

    private void SaveSettings() {
      if (this.btnApply.Enabled) {
	if (this.grpGlobalConfig.Enabled) {
	RegistryKey ConfigKey = POL.OpenAppConfigKey(AppID.FFXI);
	  try { ConfigKey.SetValue("0001", int.Parse(this.txtGUIWidth.Text));     } catch { }
	  try { ConfigKey.SetValue("0002", int.Parse(this.txtGUIHeight.Text));    } catch { }
	  try { ConfigKey.SetValue("0003", int.Parse(this.txt3DWidth.Text));      } catch { }
	  try { ConfigKey.SetValue("0004", int.Parse(this.txt3DHeight.Text));     } catch { }
	  try { ConfigKey.SetValue("0029", int.Parse(this.txtSoundEffects.Text)); } catch { }
	}
	this.btnApply.Enabled = false;
      }
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.grpGlobalConfig = new System.Windows.Forms.GroupBox();
      this.txtSoundEffects = new System.Windows.Forms.TextBox();
      this.lblSoundEffects = new System.Windows.Forms.Label();
      this.txt3DHeight = new System.Windows.Forms.TextBox();
      this.txt3DWidth = new System.Windows.Forms.TextBox();
      this.txtGUIHeight = new System.Windows.Forms.TextBox();
      this.txtGUIWidth = new System.Windows.Forms.TextBox();
      this.lbl3DX = new System.Windows.Forms.Label();
      this.lblGUIX = new System.Windows.Forms.Label();
      this.lbl3DResolution = new System.Windows.Forms.Label();
      this.lblGUIResolution = new System.Windows.Forms.Label();
      this.lblWarning = new System.Windows.Forms.Label();
      this.picWarning = new System.Windows.Forms.PictureBox();
      this.grpCharConfig = new System.Windows.Forms.GroupBox();
      this.lblColor16 = new System.Windows.Forms.Label();
      this.lblColor8 = new System.Windows.Forms.Label();
      this.lblColor15 = new System.Windows.Forms.Label();
      this.lblColor23 = new System.Windows.Forms.Label();
      this.lblColor7 = new System.Windows.Forms.Label();
      this.lblColor12 = new System.Windows.Forms.Label();
      this.lblColor13 = new System.Windows.Forms.Label();
      this.lblColor14 = new System.Windows.Forms.Label();
      this.lblColor11 = new System.Windows.Forms.Label();
      this.lblColor10 = new System.Windows.Forms.Label();
      this.lblColor9 = new System.Windows.Forms.Label();
      this.lblColor20 = new System.Windows.Forms.Label();
      this.lblColor21 = new System.Windows.Forms.Label();
      this.lblColor22 = new System.Windows.Forms.Label();
      this.lblColor19 = new System.Windows.Forms.Label();
      this.lblColor18 = new System.Windows.Forms.Label();
      this.lblColor17 = new System.Windows.Forms.Label();
      this.lblColor4 = new System.Windows.Forms.Label();
      this.lblColor5 = new System.Windows.Forms.Label();
      this.lblColor6 = new System.Windows.Forms.Label();
      this.lblColor3 = new System.Windows.Forms.Label();
      this.lblColor2 = new System.Windows.Forms.Label();
      this.lblColor1 = new System.Windows.Forms.Label();
      this.cmbCharacters = new System.Windows.Forms.ComboBox();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnApply = new System.Windows.Forms.Button();
      this.dlgChooseColor = new System.Windows.Forms.ColorDialog();
      this.grpGlobalConfig.SuspendLayout();
      this.grpCharConfig.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpGlobalConfig
      // 
      this.grpGlobalConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.grpGlobalConfig.Controls.Add(this.txtSoundEffects);
      this.grpGlobalConfig.Controls.Add(this.lblSoundEffects);
      this.grpGlobalConfig.Controls.Add(this.txt3DHeight);
      this.grpGlobalConfig.Controls.Add(this.txt3DWidth);
      this.grpGlobalConfig.Controls.Add(this.txtGUIHeight);
      this.grpGlobalConfig.Controls.Add(this.txtGUIWidth);
      this.grpGlobalConfig.Controls.Add(this.lbl3DX);
      this.grpGlobalConfig.Controls.Add(this.lblGUIX);
      this.grpGlobalConfig.Controls.Add(this.lbl3DResolution);
      this.grpGlobalConfig.Controls.Add(this.lblGUIResolution);
      this.grpGlobalConfig.Controls.Add(this.lblWarning);
      this.grpGlobalConfig.Controls.Add(this.picWarning);
      this.grpGlobalConfig.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpGlobalConfig.Location = new System.Drawing.Point(4, 4);
      this.grpGlobalConfig.Name = "grpGlobalConfig";
      this.grpGlobalConfig.Size = new System.Drawing.Size(394, 152);
      this.grpGlobalConfig.TabIndex = 0;
      this.grpGlobalConfig.TabStop = false;
      this.grpGlobalConfig.Text = "Global Settings";
      // 
      // txtSoundEffects
      // 
      this.txtSoundEffects.Location = new System.Drawing.Point(164, 124);
      this.txtSoundEffects.Name = "txtSoundEffects";
      this.txtSoundEffects.Size = new System.Drawing.Size(32, 20);
      this.txtSoundEffects.TabIndex = 11;
      this.txtSoundEffects.Text = "";
      this.txtSoundEffects.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.txtSoundEffects.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // lblSoundEffects
      // 
      this.lblSoundEffects.Location = new System.Drawing.Point(12, 128);
      this.lblSoundEffects.Name = "lblSoundEffects";
      this.lblSoundEffects.Size = new System.Drawing.Size(152, 12);
      this.lblSoundEffects.TabIndex = 10;
      this.lblSoundEffects.Text = "Simultaneous Sound Effects:";
      // 
      // txt3DHeight
      // 
      this.txt3DHeight.Location = new System.Drawing.Point(344, 96);
      this.txt3DHeight.Name = "txt3DHeight";
      this.txt3DHeight.Size = new System.Drawing.Size(40, 20);
      this.txt3DHeight.TabIndex = 9;
      this.txt3DHeight.Text = "";
      this.txt3DHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.txt3DHeight.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // txt3DWidth
      // 
      this.txt3DWidth.Location = new System.Drawing.Point(292, 96);
      this.txt3DWidth.Name = "txt3DWidth";
      this.txt3DWidth.Size = new System.Drawing.Size(40, 20);
      this.txt3DWidth.TabIndex = 8;
      this.txt3DWidth.Text = "";
      this.txt3DWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.txt3DWidth.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // txtGUIHeight
      // 
      this.txtGUIHeight.Location = new System.Drawing.Point(157, 96);
      this.txtGUIHeight.Name = "txtGUIHeight";
      this.txtGUIHeight.Size = new System.Drawing.Size(40, 20);
      this.txtGUIHeight.TabIndex = 7;
      this.txtGUIHeight.Text = "";
      this.txtGUIHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.txtGUIHeight.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // txtGUIWidth
      // 
      this.txtGUIWidth.Location = new System.Drawing.Point(104, 96);
      this.txtGUIWidth.Name = "txtGUIWidth";
      this.txtGUIWidth.Size = new System.Drawing.Size(40, 20);
      this.txtGUIWidth.TabIndex = 6;
      this.txtGUIWidth.Text = "";
      this.txtGUIWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.txtGUIWidth.TextChanged += new System.EventHandler(this.Something_Changed);
      // 
      // lbl3DX
      // 
      this.lbl3DX.Location = new System.Drawing.Point(334, 100);
      this.lbl3DX.Name = "lbl3DX";
      this.lbl3DX.Size = new System.Drawing.Size(8, 12);
      this.lbl3DX.TabIndex = 5;
      this.lbl3DX.Text = "x";
      // 
      // lblGUIX
      // 
      this.lblGUIX.Location = new System.Drawing.Point(146, 100);
      this.lblGUIX.Name = "lblGUIX";
      this.lblGUIX.Size = new System.Drawing.Size(8, 12);
      this.lblGUIX.TabIndex = 4;
      this.lblGUIX.Text = "x";
      // 
      // lbl3DResolution
      // 
      this.lbl3DResolution.Location = new System.Drawing.Point(208, 100);
      this.lbl3DResolution.Name = "lbl3DResolution";
      this.lbl3DResolution.Size = new System.Drawing.Size(80, 16);
      this.lbl3DResolution.TabIndex = 3;
      this.lbl3DResolution.Text = "3D Resolution:";
      // 
      // lblGUIResolution
      // 
      this.lblGUIResolution.Location = new System.Drawing.Point(12, 100);
      this.lblGUIResolution.Name = "lblGUIResolution";
      this.lblGUIResolution.Size = new System.Drawing.Size(84, 12);
      this.lblGUIResolution.TabIndex = 2;
      this.lblGUIResolution.Text = "GUI Resolution:";
      // 
      // lblWarning
      // 
      this.lblWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.lblWarning.Location = new System.Drawing.Point(52, 16);
      this.lblWarning.Name = "lblWarning";
      this.lblWarning.Size = new System.Drawing.Size(328, 68);
      this.lblWarning.TabIndex = 1;
      this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // picWarning
      // 
      this.picWarning.Location = new System.Drawing.Point(12, 32);
      this.picWarning.Name = "picWarning";
      this.picWarning.Size = new System.Drawing.Size(32, 32);
      this.picWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picWarning.TabIndex = 0;
      this.picWarning.TabStop = false;
      // 
      // grpCharConfig
      // 
      this.grpCharConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
	| System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.grpCharConfig.Controls.Add(this.lblColor16);
      this.grpCharConfig.Controls.Add(this.lblColor8);
      this.grpCharConfig.Controls.Add(this.lblColor15);
      this.grpCharConfig.Controls.Add(this.lblColor23);
      this.grpCharConfig.Controls.Add(this.lblColor7);
      this.grpCharConfig.Controls.Add(this.lblColor12);
      this.grpCharConfig.Controls.Add(this.lblColor13);
      this.grpCharConfig.Controls.Add(this.lblColor14);
      this.grpCharConfig.Controls.Add(this.lblColor11);
      this.grpCharConfig.Controls.Add(this.lblColor10);
      this.grpCharConfig.Controls.Add(this.lblColor9);
      this.grpCharConfig.Controls.Add(this.lblColor20);
      this.grpCharConfig.Controls.Add(this.lblColor21);
      this.grpCharConfig.Controls.Add(this.lblColor22);
      this.grpCharConfig.Controls.Add(this.lblColor19);
      this.grpCharConfig.Controls.Add(this.lblColor18);
      this.grpCharConfig.Controls.Add(this.lblColor17);
      this.grpCharConfig.Controls.Add(this.lblColor4);
      this.grpCharConfig.Controls.Add(this.lblColor5);
      this.grpCharConfig.Controls.Add(this.lblColor6);
      this.grpCharConfig.Controls.Add(this.lblColor3);
      this.grpCharConfig.Controls.Add(this.lblColor2);
      this.grpCharConfig.Controls.Add(this.lblColor1);
      this.grpCharConfig.Controls.Add(this.cmbCharacters);
      this.grpCharConfig.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpCharConfig.Location = new System.Drawing.Point(4, 160);
      this.grpCharConfig.Name = "grpCharConfig";
      this.grpCharConfig.Size = new System.Drawing.Size(394, 208);
      this.grpCharConfig.TabIndex = 1;
      this.grpCharConfig.TabStop = false;
      this.grpCharConfig.Text = "Per-Character Settings";
      // 
      // lblColor16
      // 
      this.lblColor16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor16.Location = new System.Drawing.Point(256, 180);
      this.lblColor16.Name = "lblColor16";
      this.lblColor16.Size = new System.Drawing.Size(64, 20);
      this.lblColor16.TabIndex = 23;
      this.lblColor16.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor8
      // 
      this.lblColor8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor8.Location = new System.Drawing.Point(188, 180);
      this.lblColor8.Name = "lblColor8";
      this.lblColor8.Size = new System.Drawing.Size(64, 20);
      this.lblColor8.TabIndex = 22;
      this.lblColor8.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor15
      // 
      this.lblColor15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor15.Location = new System.Drawing.Point(256, 156);
      this.lblColor15.Name = "lblColor15";
      this.lblColor15.Size = new System.Drawing.Size(64, 20);
      this.lblColor15.TabIndex = 21;
      this.lblColor15.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor23
      // 
      this.lblColor23.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor23.Location = new System.Drawing.Point(324, 156);
      this.lblColor23.Name = "lblColor23";
      this.lblColor23.Size = new System.Drawing.Size(64, 20);
      this.lblColor23.TabIndex = 20;
      this.lblColor23.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor7
      // 
      this.lblColor7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor7.Location = new System.Drawing.Point(188, 156);
      this.lblColor7.Name = "lblColor7";
      this.lblColor7.Size = new System.Drawing.Size(64, 20);
      this.lblColor7.TabIndex = 19;
      this.lblColor7.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor12
      // 
      this.lblColor12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor12.Location = new System.Drawing.Point(256, 84);
      this.lblColor12.Name = "lblColor12";
      this.lblColor12.Size = new System.Drawing.Size(64, 20);
      this.lblColor12.TabIndex = 18;
      this.lblColor12.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor13
      // 
      this.lblColor13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor13.Location = new System.Drawing.Point(256, 108);
      this.lblColor13.Name = "lblColor13";
      this.lblColor13.Size = new System.Drawing.Size(64, 20);
      this.lblColor13.TabIndex = 17;
      this.lblColor13.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor14
      // 
      this.lblColor14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor14.Location = new System.Drawing.Point(256, 132);
      this.lblColor14.Name = "lblColor14";
      this.lblColor14.Size = new System.Drawing.Size(64, 20);
      this.lblColor14.TabIndex = 16;
      this.lblColor14.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor11
      // 
      this.lblColor11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor11.Location = new System.Drawing.Point(256, 60);
      this.lblColor11.Name = "lblColor11";
      this.lblColor11.Size = new System.Drawing.Size(64, 20);
      this.lblColor11.TabIndex = 15;
      this.lblColor11.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor10
      // 
      this.lblColor10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor10.Location = new System.Drawing.Point(256, 36);
      this.lblColor10.Name = "lblColor10";
      this.lblColor10.Size = new System.Drawing.Size(64, 20);
      this.lblColor10.TabIndex = 14;
      this.lblColor10.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor9
      // 
      this.lblColor9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor9.Location = new System.Drawing.Point(256, 12);
      this.lblColor9.Name = "lblColor9";
      this.lblColor9.Size = new System.Drawing.Size(64, 20);
      this.lblColor9.TabIndex = 13;
      this.lblColor9.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor20
      // 
      this.lblColor20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor20.Location = new System.Drawing.Point(324, 84);
      this.lblColor20.Name = "lblColor20";
      this.lblColor20.Size = new System.Drawing.Size(64, 20);
      this.lblColor20.TabIndex = 12;
      this.lblColor20.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor21
      // 
      this.lblColor21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor21.Location = new System.Drawing.Point(324, 108);
      this.lblColor21.Name = "lblColor21";
      this.lblColor21.Size = new System.Drawing.Size(64, 20);
      this.lblColor21.TabIndex = 11;
      this.lblColor21.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor22
      // 
      this.lblColor22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor22.Location = new System.Drawing.Point(324, 132);
      this.lblColor22.Name = "lblColor22";
      this.lblColor22.Size = new System.Drawing.Size(64, 20);
      this.lblColor22.TabIndex = 10;
      this.lblColor22.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor19
      // 
      this.lblColor19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor19.Location = new System.Drawing.Point(324, 60);
      this.lblColor19.Name = "lblColor19";
      this.lblColor19.Size = new System.Drawing.Size(64, 20);
      this.lblColor19.TabIndex = 9;
      this.lblColor19.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor18
      // 
      this.lblColor18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor18.Location = new System.Drawing.Point(324, 36);
      this.lblColor18.Name = "lblColor18";
      this.lblColor18.Size = new System.Drawing.Size(64, 20);
      this.lblColor18.TabIndex = 8;
      this.lblColor18.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor17
      // 
      this.lblColor17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor17.Location = new System.Drawing.Point(324, 12);
      this.lblColor17.Name = "lblColor17";
      this.lblColor17.Size = new System.Drawing.Size(64, 20);
      this.lblColor17.TabIndex = 7;
      this.lblColor17.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor4
      // 
      this.lblColor4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor4.Location = new System.Drawing.Point(188, 84);
      this.lblColor4.Name = "lblColor4";
      this.lblColor4.Size = new System.Drawing.Size(64, 20);
      this.lblColor4.TabIndex = 6;
      this.lblColor4.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor5
      // 
      this.lblColor5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor5.Location = new System.Drawing.Point(188, 108);
      this.lblColor5.Name = "lblColor5";
      this.lblColor5.Size = new System.Drawing.Size(64, 20);
      this.lblColor5.TabIndex = 5;
      this.lblColor5.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor6
      // 
      this.lblColor6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor6.Location = new System.Drawing.Point(188, 132);
      this.lblColor6.Name = "lblColor6";
      this.lblColor6.Size = new System.Drawing.Size(64, 20);
      this.lblColor6.TabIndex = 4;
      this.lblColor6.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor3
      // 
      this.lblColor3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor3.Location = new System.Drawing.Point(188, 60);
      this.lblColor3.Name = "lblColor3";
      this.lblColor3.Size = new System.Drawing.Size(64, 20);
      this.lblColor3.TabIndex = 3;
      this.lblColor3.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor2
      // 
      this.lblColor2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor2.Location = new System.Drawing.Point(188, 36);
      this.lblColor2.Name = "lblColor2";
      this.lblColor2.Size = new System.Drawing.Size(64, 20);
      this.lblColor2.TabIndex = 2;
      this.lblColor2.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // lblColor1
      // 
      this.lblColor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblColor1.Location = new System.Drawing.Point(188, 12);
      this.lblColor1.Name = "lblColor1";
      this.lblColor1.Size = new System.Drawing.Size(64, 20);
      this.lblColor1.TabIndex = 1;
      this.lblColor1.DoubleClick += new System.EventHandler(this.ColorLabel_DoubleClick);
      // 
      // cmbCharacters
      // 
      this.cmbCharacters.DisplayMember = "CharacterName";
      this.cmbCharacters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbCharacters.Location = new System.Drawing.Point(8, 20);
      this.cmbCharacters.Name = "cmbCharacters";
      this.cmbCharacters.Size = new System.Drawing.Size(140, 21);
      this.cmbCharacters.TabIndex = 0;
      this.cmbCharacters.SelectedIndexChanged += new System.EventHandler(this.cmbCharacters_SelectedIndexChanged);
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnClose.Location = new System.Drawing.Point(209, 372);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(60, 24);
      this.btnClose.TabIndex = 100;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnCancel.Location = new System.Drawing.Point(273, 372);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(60, 24);
      this.btnCancel.TabIndex = 101;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnApply
      // 
      this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnApply.Enabled = false;
      this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnApply.Location = new System.Drawing.Point(337, 372);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new System.Drawing.Size(60, 24);
      this.btnApply.TabIndex = 102;
      this.btnApply.Text = "&Apply";
      this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
      // 
      // dlgChooseColor
      // 
      this.dlgChooseColor.AnyColor = true;
      this.dlgChooseColor.FullOpen = true;
      // 
      // MainWindow
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(402, 400);
      this.Controls.Add(this.btnApply);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.grpCharConfig);
      this.Controls.Add(this.grpGlobalConfig);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "MainWindow";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "FFXI Configuration Editor";
      this.grpGlobalConfig.ResumeLayout(false);
      this.grpCharConfig.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    #region Button Events

    private void btnApply_Click(object sender, System.EventArgs e) {
      this.SaveSettings();
    }

    private void btnCancel_Click(object sender, System.EventArgs e) {
      this.Close();
    }

    private void btnClose_Click(object sender, System.EventArgs e) {
      this.SaveSettings();
      this.Close();
    }

    #endregion

    private void cmbCharacters_SelectedIndexChanged(object sender, System.EventArgs e) {
    CharacterConfig CC = this.cmbCharacters.SelectedItem as CharacterConfig;
      if (CC != null && CC.Colors != null) {
	for (int i = 0; i < 23; ++i)
	  this.ColorLabels[i].BackColor = CC.Colors[i];
      }
    }

    private void ColorLabel_DoubleClick(object sender, System.EventArgs e) {
    Label L = sender as Label;
      this.dlgChooseColor.Color = L.BackColor;
      if (this.dlgChooseColor.ShowDialog(this) == DialogResult.OK) {
	L.BackColor = this.dlgChooseColor.Color;
	this.btnApply.Enabled = true;
      }
    }

    private void Something_Changed(object sender, System.EventArgs e) {
      this.btnApply.Enabled = true;
    }

  }

}
