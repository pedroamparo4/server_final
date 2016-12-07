using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class StateChangedEventArgs : EventArgs
    {
        public HttpServerState CurrentState { get; private set; }

        public HttpServerState PreviousState { get; private set; }

        public StateChangedEventArgs(HttpServerState previousState, HttpServerState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }
    }
}
