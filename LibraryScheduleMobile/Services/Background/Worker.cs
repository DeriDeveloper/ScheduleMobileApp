using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleMobileApp.Services.Background
{
    internal class Worker
    {
        static internal void ErrorNotify(Exception exception)
        {
            string error = DateTime.Now.ToString() + exception.ToString();

            Console.WriteLine(error);
        }
    }
}
