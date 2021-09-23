using System.IO;

namespace Avanade.PapoDeDev.UnitTest.Domain.Logging
{
    //static methods appears as 100% in test coverage, even without tests
    public /*static*/ class BankLogger : IBankLogger
    {
        public /*static*/ void Log(string text)
        {
            using (var sw = new StreamWriter(path: "Log.txt", append: true))
            {
                sw.WriteLine(text);
            }
        }
    }
}