using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace RoboCleaner
{

    public static class FileReader
    {
        public static Map GetData()
        {
            Console.WriteLine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            List<int[]> position_list = new List<int[]>();
            int[] robo_arr;

            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("../../../input_data.txt"))
                {
                    string line;
                    int x;
                    int y;
                    int[] list_arr = new int[3];
                    robo_arr = new int[2];
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Read the stream to a string, and write the string to the console.
                        String[] values = line.Split(" ");

                        String postion = values[0];
                        String type = values[1];
                        
                        x = Int32.Parse(postion.Substring(1,1));
                        y = Int32.Parse(postion.Substring(3,1));
                        
                        list_arr[0] = x;
                        list_arr[1] = y;
                        
                        switch (type)
                        {
                            case "nil":
                                list_arr[2] = 0;
                                position_list.Add((int[])list_arr.Clone());
                                break;

                            case "dirty":
                                list_arr[2] = 1;
                                position_list.Add((int[])list_arr.Clone());
                                break;

                            case "clean":
                                list_arr[2] = 2;
                                position_list.Add((int[])list_arr.Clone());
                                break;

                            case "robo_start":
                                robo_arr[0] = list_arr[0];
                                robo_arr[1] = list_arr[1];
                                break;
                        }
                    }
                    
                    return new Map(position_list, robo_arr);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
