using System;
using System.Collections.Generic;
using System.Text;

namespace Visitare
{
    public class Question
    {
        public int Id { get; set; }
        public IList<string> Answers { get; set; }
        public int GoodAnswer { get; set; }
        public string Question1 { get; set; }
    }
}
