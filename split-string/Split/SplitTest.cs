namespace Geunho.Net.Test
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SplitTest
    {
        #region Constants
        const string _stringToSplit = "apple:: banana::mango ";
        const string _splitToken = "::";
        const int _numberOfLoops = 10000;

        static Regex _preCompiledRegex = new Regex(_splitToken, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        #endregion

        [TestMethod]
        public void RegexSplitShouldSlowerThanStringSplit()
        {
            TimeSpan _elapsedUsingCompiledRegex;
            TimeSpan _elapsedUsingRegex;
            TimeSpan _elapsedUsingString;

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            #region pre-compiled Regex loop

            for (int i = 0; i < _numberOfLoops; i++)
            {
                _preCompiledRegex.Split(_stringToSplit);
            }

            stopwatch.Stop();
            _elapsedUsingCompiledRegex = stopwatch.Elapsed;

            Console.WriteLine("Time elapsed (pre-compiled Regex): {0} ms", _elapsedUsingCompiledRegex.TotalMilliseconds);

            #endregion

            stopwatch.Restart();

            #region Regex loop

            for (int i = 0; i < _numberOfLoops; i++)
            {
                Regex.Split(_stringToSplit, _splitToken, RegexOptions.IgnorePatternWhitespace);
            }

            stopwatch.Stop();
            _elapsedUsingRegex = stopwatch.Elapsed;

            Console.WriteLine("Time elapsed (Regex): {0} ms", _elapsedUsingRegex.TotalMilliseconds);

            #endregion

            stopwatch.Restart();

            #region String loop

            string[] splitTokens = new string[] { _splitToken };
                        
            for (int i = 0; i < _numberOfLoops; i++)
            {
                _stringToSplit.Split(splitTokens, StringSplitOptions.RemoveEmptyEntries);
            }

            stopwatch.Stop();
            _elapsedUsingString = stopwatch.Elapsed;           

            Console.WriteLine("Time elapsed (String): {0} ms", _elapsedUsingString.TotalMilliseconds);

            #endregion

            stopwatch.Reset();            

            Assert.IsTrue(_elapsedUsingRegex.Ticks > _elapsedUsingString.Ticks, "Is Regex split slower than String split?");
            Assert.IsTrue(_elapsedUsingRegex.Ticks > _elapsedUsingCompiledRegex.Ticks, "Is Regex split slower than pre-compiled Regex split?");
        }
    }
}
