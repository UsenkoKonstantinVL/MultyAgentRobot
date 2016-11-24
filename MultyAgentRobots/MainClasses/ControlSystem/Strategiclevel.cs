using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultyAgentRobots.MainClasses.ControlSystem;

namespace MultyAgentRobots.MainClasses.ControlSystem
{
    public class StrategicLevel
    {
        public Condition MyCondition { get; set; }
        GraphManager manager;
        public StrategicLevel(GraphManager grm)
        {
            manager = grm;
            MyCondition = Condition.Waiting;
        }


        int[] data;

        public void Run(int[] _data)
        {
            data = _data;
            switch(MyCondition)
            {
                case Condition.Waiting:
                    {
                        FunctionWaiting();
                        break;
                    }
                case Condition.GoToPoint:
                    {
                        break;
                    }
                case Condition.Research:
                    {
                        break;
                    }

            }
        }

        private void FunctionWaiting()
        {
            if(!manager.IsInitialized)
            {

            }
        }
    }

    public enum Condition
    {
        Waiting,
        GoToPoint,
        Research,
        GoToHome,
        AtHome
    }
}
