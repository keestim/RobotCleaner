using System;
using System.Collections.Generic;
using System.Text;

namespace RoboCleaner
{
    public class Sequence : ICloneable
    {
        Map current_state;
        List<Map> previous_states;
        
        public Sequence(List<Map> previous, Map current)
        {
            current_state = (Map)current.Clone();
            previous_states = previous;
        }

        //constructor used for when initializing the sequence obj
        public Sequence(Map start_map)
        {
            previous_states = new List<Map>();
            current_state = (Map)start_map.Clone();
        }

        public Map CurrentState
        {
            get
            {
                return current_state;
            }
        }
        
        public List<Map> PreviousStates
        {
            get
            {
                return previous_states;
            }
        }

        public object Clone()
        {
            List<Map> prev_state_clone = new List<Map>(previous_states);
            
            Sequence clone_obj = new Sequence(prev_state_clone, (Map)current_state.Clone());
            return clone_obj;
        }

        public bool CheckSequence(Sequence input_seq)
        {
            if (input_seq.CurrentState.Rooms.ToArray() == this.CurrentState.Rooms.ToArray() && input_seq.CurrentState.RobotPosition == this.CurrentState.RobotPosition)
            {
                if (input_seq.PreviousStates.ToArray() == this.PreviousStates.ToArray())
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool CheckPrevious()
        {
            foreach (Map selected in this.previous_states)
            {
                if (selected.RobotPosition[0].Equals(current_state.RobotPosition[0]) && selected.RobotPosition[1].Equals(current_state.RobotPosition[1]))
                {
                    //Console.WriteLine(current_state.NumDirtyRooms + " | " + selected.NumDirtyRooms);


                    if (current_state.NumDirtyRooms.Equals(selected.NumDirtyRooms))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
