// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Threading;
using System.Windows.Forms;
using PlayOnline.Core;

namespace POLUtils {

  public class POLUtils {

    internal static bool KeepGoing;

    private static void KaBOOM(object sender, UnhandledExceptionEventArgs args) {
      if (args.IsTerminating)
        MessageBox.Show("POLUtils has encountered an exception and needs to close.\nPlease report this to Pebbles:\n\n" + args.ExceptionObject.ToString(), "Oops", MessageBoxButtons.OK, MessageBoxIcon.Stop);
      else if (!(args.ExceptionObject is ThreadAbortException))
        MessageBox.Show("POLUtils has encountered an exception.\nPlease report this to Pebbles:\n\n" + args.ExceptionObject.ToString(), "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    [STAThread]
    public static int Main(string[] Arguments) {
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(POLUtils.KaBOOM);
      Application.EnableVisualStyles();
      if (POL.AvailableRegions == POL.Region.None)
        MessageBox.Show(I18N.GetText("Text:NoPOL"), I18N.GetText("Caption:NoPOL"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
      else {
        // The loop is for the benefit of language change
        POLUtils.KeepGoing = true;
        while (POLUtils.KeepGoing) {
          POLUtils.KeepGoing = false;
          Application.Run(new POLUtilsUI());
        }
      }
      return 0;
    }
  }

}
