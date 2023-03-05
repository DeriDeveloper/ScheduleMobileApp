using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleMobileApp.Interfaces
{
    public interface IPath
    {
        string GetDatabasePath(string filename);
    }
}
