using System;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main(string[] args)
        {
            BinNode<char> charTree = Create(RandomChars(15));
            Print(charTree);

            BinNode<int> intTree = Create(RandomInts(15));
            Print(intTree);

            // Console.WriteLine("\n\nAverage Height of tree if chance=0.6: " + CheckRandom(0.6));

            BinNode<int> rngTree = new BinNode<int>(1);
            RandomlyPopulate(rngTree, new Random(), 0.6);
            Print(rngTree);
            Queue<int> q = TreeToQueue(rngTree);
            Console.WriteLine(q);
            PrintLayered(q);
        }

        private static void PrintLayered(Queue<int> q)
        {
            while (!q.IsEmpty())
            {
                int value = q.Remove();
                if (value == -2)
                    Console.Write("       \\n\n");
                else if (value == -1)
                    Console.Write(" null ");
                else
                    Console.Write(" " + value + " ");
            }
        }

        private static Queue<int> TreeToQueue(BinNode<int> t)
        {
            Queue<BinNode<int>> q = new Queue<BinNode<int>>();
            TreeToQueue(t, q, true, new BinNode<int>(default));
            return ExtractValues(q);
        }

        private static void TreeToQueue(BinNode<int> t, Queue<BinNode<int>> q, bool isLast, BinNode<int> nuller)
        {
            Queue<BinNode<int>> queue = new Queue<BinNode<int>>();
            var node = t;
            var lf = new BinNode<int>(-2);
            int full = (int)Math.Pow(2, Depth(t));
            int count = 1;
            queue.Insert(t);
            queue.Insert(lf);
            while (!queue.IsEmpty() && count < full)
            {
                // Console.WriteLine(count + " Midprint: " + ExtractValues(Copy(queue)));
                node = queue.Remove();
                q.Insert(node);
                if (node == lf)
                    if (!queue.IsEmpty())
                        queue.Insert(lf);
                if (node != lf)
                    count++;
                if (node == null)
                    node = new BinNode<int>(null, default, null);
                if (node != lf)
                {
                    queue.Insert(node.Left);
                    queue.Insert(node.Right);
                }
            }
            /*
            q.Insert(t);
            if (isLast)
                q.Insert(nuller);
            if (t != null)
            {
                TreeToQueue(t.Left, q, false, nuller);
                TreeToQueue(t.Right, q, isLast, nuller);
            }*/
        }

        private static Queue<int> ExtractValues(Queue<BinNode<int>> q)
        {
            Queue<int> result = new Queue<int>();
            while (!q.IsEmpty())
            {
                BinNode<int> node = q.Remove();
                result.Insert(node == null ? (-1) : node.Value);
            }
            return result;
        }

        private static Queue<T> Copy<T>(Queue<T> source)
        {
            Queue<T> temp = new Queue<T>();
            while (!source.IsEmpty())
                temp.Insert(source.Remove());
            Queue<T> result = new Queue<T>();
            while (!temp.IsEmpty())
            {
                var value = temp.Remove();
                result.Insert(value);
                source.Insert(value);
            }
            return result;
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

        public static BinNode<T> Create<T>(T[] array)
        {
            BinNode<T> root = new BinNode<T>(array[0]);
            Structure(root, array, 1, true);
            Structure(root, array, 2, false);

            return root;
        }

        public static void RandomlyPopulate(BinNode<int> t, Random rng, double baseChance=0.5, int level=0)
        {
            if (level > 10)
                return;
            double chance = Math.Pow(baseChance, level);
            if (rng.NextDouble() < chance)
            {
                t.Left = new BinNode<int>(rng.Next(10));
                RandomlyPopulate(t.Left, rng, baseChance, level + 1);
            }
            if (rng.NextDouble() < chance)
            {
                t.Right = new BinNode<int>(rng.Next(10));
                RandomlyPopulate(t.Right, rng, baseChance, level + 1);
            }
        }

        public static float CheckRandom(double chance)
        {
            float attempts = 1000f;
            int sum = 0;
            for (int i = 0; i < attempts; i++)
            {
                BinNode<int> tree = new BinNode<int>(1);
                RandomlyPopulate(tree, new Random(), chance);
                sum += Depth(tree);
            }
            return sum / attempts;
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
            Console.WriteLine();
        }

        private static void PrintLevel<T>(BinNode<T> t, int current, int level, int stop, bool isMostLeft = false)
        {
            if (current >= stop)
                return;
            if (current == level)
            {
                string indent = GetIndent(level, stop);
                string halfIndent = indent[(indent.Length / 2)..];
                Console.Write(isMostLeft ? halfIndent : indent);
                Console.Write(t == null ? "-" : t.Value.ToString());
            }
            if (t == null)
                t = new BinNode<T>(null, default, null);
            PrintLevel(t.Left, current + 1, level, stop, isMostLeft);
            PrintLevel(t.Right, current + 1, level, stop);
        }

        private static void PrintSlashes<T>(BinNode<T> t, int current, int level, int stop, bool isMostLeft = false, bool isLeft = false)
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
                }
                else
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

        private static int Depth<T>(BinNode<T> t)
        {
            if (t == null)
                return 0;
            return Math.Max(Depth(t.GetLeft()), Depth(t.GetRight())) + 1;
        }

        private static string GetIndent(int level, int depth)
        {
            return new string(' ', (int)(Math.Pow(2, depth - level) - 1));
        }

        private static string GetLeftmostIndent(float level, int depth)
        {
            return new string(' ', (int)(3 * Math.Pow(2, depth - level - 2)));
        }

        private static string GetLeftIndent(float level, int depth)
        {
            if (level == depth - 1)
                return " ";
            return new string(' ', (int)(3 * Math.Pow(2, depth - level - 1) - 1));
        }

        private static string GetRightIndent(float level, int depth)
        {
            if (level == depth - 1)
                return " ";
            return new string(' ', (int)(Math.Pow(2, depth - level - 1) - 1));
        }
    }
}
