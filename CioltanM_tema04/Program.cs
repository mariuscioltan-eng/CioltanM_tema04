using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace CioltanM_tema04
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (Window3D wnd = new Window3D())
            {
                wnd.Run(30.0, 0.0);
            }
        }
    }
}