using System;
using System.Collections.Generic;
using System.Text;

namespace RoboCleaner
{
    public class Map : ICloneable
    {
        private List<int[]> rooms;
        private int[] robot_position;
        private int max_x;
        private int max_y;

        public Map(List<int[]> input_rooms, int[] robot_start_position)
        {
            max_x = 0;
            max_y = 0;

            rooms = input_rooms;
            robot_position = robot_start_position;

            foreach(int[] room in rooms)
            {
                if (room[0] > max_x)
                {
                    max_x = room[0];
                }

                if (room[1] > max_y)
                {
                    max_y = room[1];
                }
            }
        }

        public int[] RobotPosition
        {
            get
            {
                return robot_position;
            }
        }

        public List<int[]> Rooms
        {
            get
            {
                return rooms;
            }
        }

        public int NumDirtyRooms
        {
            get
            {
                int count = 0;
                foreach (int[] room in rooms)
                {
                    if (room[2] == 1)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public object Clone()
        {
            int[] robo_pos_clone = new int[2];
            robot_position.CopyTo(robo_pos_clone, 0);

            List<int[]> rooms_copy = new List<int[]>();
            foreach (int[] room in rooms)
            {
                //cloning room fixed reference based issues!!!
                rooms_copy.Add((int[])room.Clone());
            }

            Map return_obj = new Map(rooms_copy, robo_pos_clone);

            return return_obj;
        }

        public Map Clean()
        {
            Map return_map = (Map)this.Clone();

            //return true if the room ends up in a clean state
            foreach (int[] room in return_map.rooms)
            {
                if (room[0] == this.robot_position[0] && room[1] == this.robot_position[1])
                {
                    room[2] = 2;
                    return return_map;
                }
            }

            

            return return_map;
        }

        public Map Up()
        {
            if (robot_position[1] < max_y)
            {
                if (GetPosition(robot_position[0], robot_position[1] + 1) != 0 && GetPosition(robot_position[0], robot_position[1] + 1) != 4)
                {
                    Map return_obj = (Map)this.Clone();
                    return_obj.robot_position[1] += 1;
                    return return_obj;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public Map Down()
        {
            if (robot_position[1] > 1)
            {
                if (GetPosition(robot_position[0], robot_position[1] - 1) != 0 && GetPosition(robot_position[0], robot_position[1] - 1) != 4)
                {
                    Map return_obj = (Map)this.Clone();
                    return_obj.robot_position[1] -= 1;
                    return return_obj;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public Map Left()
        {
            if (robot_position[0] > 1)
            {
                if (GetPosition(robot_position[0] - 1, robot_position[1]) != 0 && GetPosition(robot_position[0] - 1, robot_position[1]) != 4)
                {
                    Map return_obj = (Map)this.Clone();
                    return_obj.robot_position[0] -= 1;
                    return return_obj;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public Map Right()
        {
            if (robot_position[0] < max_x)
            {
                if (GetPosition(robot_position[0] + 1, robot_position[1]) != 0 && GetPosition(robot_position[0] + 1, robot_position[1]) != 4)
                {
                    Map return_obj = (Map)this.Clone();
                    return_obj.robot_position[0] += 1;
                    return return_obj;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public int GetPosition(int x, int y)
        {
            foreach(int[] room in rooms)
            {
                if (room[0] == x && room[1] == y)
                {
                    return room[2];
                }
            }

            return 4;
        }

        public void PrintMap()
        {
            for (int j = max_y; j >= 1; j--)
            {
                for (int i = 1; i <= max_x; i++)
                {
                    if (i == robot_position[0] && j == robot_position[1])
                    {
                        Console.Write("|R" + GetPosition(i, j));

                    }
                    else
                    {
                        Console.Write("| " + GetPosition(i, j));
                    }

                }

                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

    }
}
