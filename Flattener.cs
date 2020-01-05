using System;
using System.Collections.Generic;
using System.IO;

namespace Flattener
{
    class Program
    {
        private const string vers = "\nFolder Flattener 0.1 by BA\n";

        private const string help = "You can:\n (1) Add specific root path. e.g. \"Flat.exe C:\\Example\" \nOr\n (2) Use the current directory as the root path with -c\". e.g. \"Flat.exe -c\"\nOptions:\n  -c[urrent] directory; \n  -d[elete] empty directories after flattening; e.g. \"Flat.exe -c -d\" = moving all the files from all the sub-directories under the current directories to the current directories.\n  -s[ub-cateory] flatten upon the parent sub-directories of the current directory, not upon the current directory itself. ";

        static void Main(string[] args)
        {
            string input_path = "";
            Console.WriteLine(vers);
            if (args.Length >= 1)
            {

                if (Array.IndexOf(args, "-c") >= 0)
                {
                    input_path = Environment.CurrentDirectory;
                }
                else if (Array.IndexOf(args, "-d") >= 0 || Array.IndexOf(args, "-s") >= 0) {
                    Console.WriteLine("ERROR: You need to specify a directory.\n");
                    Console.WriteLine(help);
                    Environment.Exit(0);
                }
                else
                {
                    input_path = args[0];
                    if (input_path.EndsWith("\\"))
                    {
                        input_path = input_path.Remove(input_path.Length - 1);
                    }
                }
            }
            else {
                Console.WriteLine(help);
                Environment.Exit(0);
            }

            try
            {
                if (Array.IndexOf(args, "-s") >= 0)
                {
                    string[] sub_dirs = Directory.GetDirectories(input_path);
                    foreach (var sub_dir in sub_dirs)
                    {
                        Flattener root_path = new Flattener(base_path: sub_dir);
                        root_path.Start_processing();
                        if (Array.IndexOf(args, "-d") >= 0)
                        {
                            root_path.Delete_directories(sub_dir);
                        }
                    }
                }
                else
                {
                    Flattener root_path = new Flattener(base_path: input_path);
                    root_path.Start_processing();
                    if (Array.IndexOf(args, "-d") >= 0)
                    {
                        root_path.Delete_directories(input_path);
                    }
                }
            }
            catch (System.IO.DirectoryNotFoundException) {
                Console.WriteLine("ERROR: The folder you specified does not exist: " + input_path);
                Environment.Exit(0);
            };            
            Console.WriteLine($"DONE: {Flattener.deleted_dir_count} sub-directories cleared; {Flattener.file_count} files flattened.");
        }
    }

    class Flattener
    {
        public static string base_path;
        public static int file_count = 0;
        public static int deleted_dir_count = 0;

        public Flattener(string base_path)
        {
            Flattener.base_path = base_path;

        }
        private void Copy_files(string item)
        {
            string new_path = $"{Flattener.base_path}\\{Path.GetFileName(item)}";
            if (File.Exists(new_path))
            {
                FileInfo file = new FileInfo(item);

                Console.Write($"FILENAME EXISTS, RENAMING & COPYING {new_path}");
                int Counter = 0;
                do
                {
                    new_path = $"{base_path}\\[{file.Directory.Name}]{Path.GetFileName(item)}";
                    if (Counter >= 1)
                    {
                        new_path = $"{base_path}\\[{file.Directory.Name}][{Counter}]{Path.GetFileName(item)}";
                    };
                    Counter++;
                } while (File.Exists(new_path));
                Console.Write(" TO: " + new_path + "\n");
            }
            else { Console.WriteLine($"COPYING [{item}] TO [{new_path}]"); };
            File.Move(item, new_path);
            Flattener.file_count++;
            
        }

        public void Start_processing()
        {
            string[] root_directories = Directory.GetDirectories(base_path);
            if (root_directories.Length == 0) {
                Console.WriteLine($"{base_path} has no sub directories under it.");                
            } else {
                Console.WriteLine($"PROCESSING: {base_path}");
            };
            foreach (string dir in root_directories)
            {
                Process_sub_dirs(dir);
            }
        }
        public void Process_sub_dirs(string base_path)
        {

            Console.WriteLine($"PROCESSING: {base_path}");

            string[] base_files = Directory.GetFiles(base_path);
            foreach (string file in base_files)
            {
                Copy_files(file);
            };

            var sub_directories = Directory.GetDirectories(base_path);
            foreach (string cur_dir in sub_directories)
            {
                Console.WriteLine($"PROCESSING: {cur_dir}");
                
                string[] cur_files = Directory.GetFiles(cur_dir);

                foreach (string file in cur_files)
                {
                    Copy_files(file);
                }

                string[] sub_dirs = Directory.GetDirectories(cur_dir);
                foreach (var sub_dir in sub_dirs)
                {
                    Process_sub_dirs(sub_dir);
                }
            }

        }

        internal void Delete_directories(string all_sub_dir)
        {
            string[] sub_dirs_to_delete = Directory.GetDirectories(all_sub_dir);
            foreach (string sub_dir in sub_dirs_to_delete)
            {
                //doublecheck if the sub-directories are empty before deleting.
                if (Directory.GetFiles(sub_dir).Length == 0) {
                    Directory.Delete(sub_dir);
                    deleted_dir_count++;
                };
                Console.WriteLine($"DELETED EMPTY DIRECTORY: {sub_dir}");
            }            
        }
    }
}
