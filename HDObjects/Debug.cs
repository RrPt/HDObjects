using System;
using System.Collections.Generic;
using System.Text;

namespace HDObjects
{
    public delegate void DebuggingDelegate(string Text);

    static public class HDDebug
    {
        static public DebuggingDelegate DebugTo = null;

        static public void WriteLine(String txt)
        {
            if (DebugTo == null)
            {
                Console.WriteLine(txt);
            }
            else
            {
                DebugTo(txt);
            }
        }
    }
}
