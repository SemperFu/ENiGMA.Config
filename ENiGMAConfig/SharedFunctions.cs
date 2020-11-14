using NStack;
using System.Text.RegularExpressions;

namespace ENiGMAConfig
{
    internal class SharedFunctions
    {
        public static bool PortCheck(ustring Port)
        {
            // Don't allow more than 4 digits and Match Digits only
            return (Port.Length > 5 || Regex.IsMatch(Port.ToString(), "[^0-9]+"));
        }
    }
}