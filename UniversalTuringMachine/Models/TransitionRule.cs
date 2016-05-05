using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTuringMachine.Models
{
    public class TransitionRule
    {
        public string currentState;
        public string currentChar;
        public string nextState;
        public string nextChar;
        public string direction;
    }
}
