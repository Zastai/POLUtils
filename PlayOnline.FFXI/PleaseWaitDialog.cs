// $Id$

using System.Windows.Forms;

namespace PlayOnline.FFXI {

  public partial class PleaseWaitDialog : Form {

    public PleaseWaitDialog(string Message) {
      this.InitializeComponent();
      this.lblMessage.Text = Message;
    }

  }

}
