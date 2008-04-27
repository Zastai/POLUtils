using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.FFXI.Utils.MogSafe
{
    public partial class MainWindow : Form
    {
        private Dictionary<Character, ListViewItem> m_charicons = new Dictionary<Character, ListViewItem>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            foreach (Character c in Game.Characters)
            {
                ListViewItem item = new ListViewItem();
                m_charicons.Add(c, item);

                item.Text = (c.Name.StartsWith("Unknown Character (") == false) ? c.Name : c.Name.Split(new char[] { '(', ')' })[1];
                //TODO: Fix this
                c.CharacterNameChanged += new Character.CharacterNameChangedDelegate(c_CharacterNameChanged);

                listView1.Items.Add(item);
            }
        }

        void c_CharacterNameChanged(Character sender, string id, string oldname, string newname)
        {
            m_charicons[sender].Text = newname;
        }
    }
}