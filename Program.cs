using System;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main(string[] args)
        {
            BinNode<char> charTree = Create(RandomChars(127));
            Print(charTree);
            Console.WriteLine();
            BinNode<int> intTree = Create(RandomInts(127));
            Print(intTree);
        }

        private static int[] RandomInts(int length)
        {
            Random rng = new Random();
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
                array[i] = rng.Next(10);
            return array;
        }

        private static char[] RandomChars(int length)
        {
            Random rng = new Random();
            char[] array = new char[length];
            for (int i = 0; i < length; i++)
                array[i] = (char)rng.Next('A', 'Z');
            return array;
        }

        private static BinNode<T> Create<T>(T[] array)
        {
            BinNode<T> root = new BinNode<T>(array[0]);
            Structure(root, array, 1, true);
            Structure(root, array, 2, false);

            return root;
        }

        private static void Structure<T>(BinNode<T> parent, T[] array, int index, bool left)
        {
            if (index >= array.Length)
                return;
            BinNode<T> self = new BinNode<T>(array[index]);
            if (left)
                parent.SetLeft(self);
            else
                parent.SetRight(self);
            Structure(self, array, index * 2 + 1, true);
            Structure(self, array, index * 2 + 2, false);
        }

        public static void Print<T>(BinNode<T> t)
        {
            int depth = Depth(t);
            for (int level = 0; level < depth; level++)
            {
                PrintSlashes(t, 0, level, depth, true, false);
                Console.WriteLine();
                PrintLevel(t, 0, level, depth, true);
                Console.WriteLine();
            }
        }

        public static void PrintLevel<T>(BinNode<T> t, int current, int level, int stop, bool isMostLeft=false)
        {
            if (current >= stop)
                return;
            if (current == level)
            {
                string indent = GetIndent(level, stop);
                string halfIndent = indent[(indent.Length / 2)..];
                Console.Write(isMostLeft ? halfIndent : indent);
                Console.Write(t == null ? '-' : t.Value);
            }
            if (t == null)
                t = new BinNode<T>(null, default, null);
            PrintLevel(t.Left, current + 1, level, stop, isMostLeft);
            PrintLevel(t.Right, current + 1, level, stop);
        }

        public static void PrintSlashes<T>(BinNode<T> t, int current, int level, int stop, bool isMostLeft = false, bool isLeft = false)
        {
            if (level == 0)
                return;
            if (current == level)
            {
                if (isMostLeft)
                    Console.Write(GetLeftmostIndent(level, stop));
                else if (isLeft)
                {
                    Console.Write(GetLeftIndent(level, stop));
                } else
                {
                    Console.Write(GetRightIndent(level, stop));
                }
                Console.Write(t == null ? ' ' : isLeft ? '/' : '\\');
            }
            if (current >= stop)
                return;
            if (t == null)
                t = new BinNode<T>(null, default, null);
            PrintSlashes(t.Left, current + 1, level, stop, isMostLeft, true);
            PrintSlashes(t.Right, current + 1, level, stop, false, false);
        }

        public static int Depth<T>(BinNode<T> t)
        {
            if (t == null)
                return 0;
            return Math.Max(Depth(t.GetLeft()), Depth(t.GetRight())) + 1;
        }

        public static string GetIndent(int level, int depth)
        {
            return new string(' ', (int)(Math.Pow(2, depth - level) - 1));
        }

        public static string GetLeftmostIndent(float level, int depth)
        {
            return new string(' ', (int)(3 * Math.Pow(2, depth - level - 2)));
        }

        public static string GetLeftIndent(float level, int depth)
        {
            if (level == depth - 1)
                return " ";
            return new string(' ', (int)(3 * Math.Pow(2, depth - level - 1) - 1));
        }

        public static string GetRightIndent(float level, int depth)
        {
            if (level == depth - 1)
                return " ";
            return new string(' ', (int)(Math.Pow(2, depth - level - 1) - 1));
        }
    }
}