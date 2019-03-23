using System;
using System.Collections.Generic;

namespace RoboCleaner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Map curr_map = FileReader.GetData();

            Console.WriteLine(curr_map.NumDirtyRooms);

            Sequence input_seq = new Sequence(curr_map);
            List<Sequence> seq_list = new List<Sequence>();
            seq_list.Add(input_seq);

            Console.WriteLine("Input Breadth or Depth");
            String Input = Console.ReadLine();

            List<Sequence> result_seq = new List<Sequence>();
            if (Input == "Breadth")
            {
                result_seq.Add(Next_breadth(seq_list));

                foreach (Sequence seq in result_seq)
                {
                    foreach (Map map in seq.PreviousStates)
                    {
                        map.PrintMap();
                    }

                    seq.CurrentState.PrintMap();

                    Console.WriteLine("________________"); ;
                }
            }

            if (Input == "Depth")
            {
                result_seq = Next_Depth(seq_list);
                Console.WriteLine(result_seq.Count);
                Sequence fastest_route = null;
                if (result_seq != null)
                {
                    foreach (Sequence seq in result_seq)
                    {
                        if (seq.PreviousStates.Count > 0)
                        {
                            if (fastest_route == null)
                            {
                                fastest_route = seq;
                            }
                            if (seq.PreviousStates.Count <= fastest_route.PreviousStates.Count)
                            {
                                fastest_route = seq;
                            }
                        }
                    }
                }

                Console.WriteLine("Num moves: " + (fastest_route.PreviousStates.Count + 1));
                foreach (Map map in fastest_route.PreviousStates)
                {
                    Console.WriteLine(map.NumDirtyRooms);
                    map.PrintMap();
                }

                fastest_route.CurrentState.PrintMap();
            }
        }


        public static Sequence Next_breadth(List<Sequence> input_seq)
        {
            List<Sequence> foremost_expanded_nodes = input_seq;
            Sequence Success_Sequence = null;
            List<Sequence> ref_sequence = new List<Sequence>(foremost_expanded_nodes);
            
            while (Success_Sequence == null)
            { 
                foreach (Sequence state in foremost_expanded_nodes.ToArray())
                {

                    Map up_move = state.CurrentState.Up();
                    Map down_move = state.CurrentState.Down();
                    Map left_move = state.CurrentState.Left();
                    Map right_move = state.CurrentState.Right();
                    Map suck = state.CurrentState.Clean();

                    List<Map> potential_moves = new List<Map>();
                    potential_moves.Add(up_move);
                    potential_moves.Add(down_move);
                    potential_moves.Add(left_move);
                    potential_moves.Add(right_move);
                    potential_moves.Add(suck);

                    foreach (Map move_map in potential_moves)
                    {
                        if (move_map != null && state.CheckPrevious())
                        {
                            if (move_map.NumDirtyRooms == 0)
                            {
                                List<Map> prev_clone = new List<Map>(state.PreviousStates);
                                prev_clone.Add((Map)state.CurrentState.Clone());
                                Success_Sequence = new Sequence(prev_clone, (Map)move_map.Clone());

                                return Success_Sequence;
                            }
                            else
                            {
                                List<Map> prev_clone = new List<Map>(state.PreviousStates);
                                prev_clone.Add((Map)state.CurrentState.Clone());
                                ref_sequence.Add(new Sequence(prev_clone, (Map)move_map.Clone()));
                            }
                        }
                    }

                    ref_sequence.Remove(state);
                }

                foremost_expanded_nodes = ref_sequence;
            }

            return Success_Sequence;
        }

        public static List<Sequence> Next_Depth(List<Sequence> foremost_expanded_nodes)
        {
            List<Sequence> ref_sequence = new List<Sequence>(foremost_expanded_nodes);
            Sequence select_sequence = ref_sequence[0];
            List<Sequence> success_sequence = new List<Sequence>();

            while (ref_sequence.Count >= 1)
            {
                Sequence select_clone = (Sequence)select_sequence.Clone();
                Map up_move = select_clone.CurrentState.Up();
                Map down_move = select_clone.CurrentState.Down();
                Map left_move = select_clone.CurrentState.Left();
                Map right_move = select_clone.CurrentState.Right();
                Map suck = select_clone.CurrentState.Clean();

                List<Map> potential_moves = new List<Map>();
                potential_moves.Add(up_move);
                potential_moves.Add(down_move);
                potential_moves.Add(left_move);
                potential_moves.Add(right_move);
                potential_moves.Add(suck);

                bool select_new_seq = false;

                int ref_sequence_count = ref_sequence.Count;

                foreach (Map option in potential_moves)
                {
                    // need to check state hasn't been explored by another sequence
                    if (option != null && select_clone.CheckPrevious())
                    {
                        List<Map> prev_clone = new List<Map>(select_clone.PreviousStates);
                        prev_clone.Add((Map)select_clone.CurrentState.Clone());
                        ref_sequence.Remove(select_sequence);

                        //if a solution has been found!
                        if (option.NumDirtyRooms == 0)
                        {
                            success_sequence.Add(new Sequence(prev_clone, (Map)option.Clone()));

                            if (ref_sequence.Count >= 1)
                            {
                                select_sequence = ref_sequence[0];
                            }
                            else
                            {
                                return success_sequence;
                            }

                            select_new_seq = true;
                        }
                        else
                        {
                            //expands selected path further, if possible
                            if (!select_new_seq)
                            {
                                //success path has been found
                                Sequence new_seq = new Sequence(prev_clone, (Map)option.Clone());
                                //check that new the new path move doesnn't already exist in sequence
                                if (new_seq.CheckPrevious() && !select_clone.CheckSequence(new_seq))
                                {
                                    //assigns new sequence as selected_sequence value
                                    select_sequence = new_seq;
                                    select_new_seq = true;
                                }
                            }
                            else
                            {
                                Sequence new_seq = new Sequence(prev_clone, (Map)option.Clone());
                                if (!select_clone.CheckSequence(new_seq) && new_seq.CheckPrevious())
                                {
                                    ref_sequence.Add(new_seq);
                                }
                            }
                        }
                    }
                }

                //if there is no solution
                //i.e. has reached the end of expansion for that possible node (hence the number of nodes has not increased)
                if (ref_sequence_count == ref_sequence.Count && ref_sequence.Count >= 1)
                {
                    ref_sequence.Remove(select_sequence);
                    select_sequence = ref_sequence[0];
                }
            }
            
            //if all are null or a soltuion has been found get new sequence from ref sequence
            return success_sequence;
        }
    }
}
