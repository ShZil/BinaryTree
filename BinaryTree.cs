using System;

namespace ConsoleApp1
{
    class Program
    {
        static readonly int rangeMax = 100;
        static readonly int rangeMin = 0;

        public static void Main(string[] args)
        {
            BinNode<char> charTree = Create(RandomChars(15));
            // Add generic

            BinNode<int> intTree = Create(RandomInts(15));
            Print(intTree);

            Console.WriteLine("\n\nAverage Height of tree if chance=0.6: " + CheckRandom(0.6));

            BinNode<int> rngTree = new BinNode<int>(1);
            RandomlyPopulate(rngTree, new Random(), 0.6);
            Print(rngTree);
            Queue<BinNode<int>> q = TreeToQueue(rngTree);
            int max = Width(rngTree);
            // Console.WriteLine(q);
            Console.WriteLine("\nMax Width: " + max);
            // PrintLayered(q);
            Print(intTree);
        }

        private static void PrintLayered(Queue<BinNode<int>> q)
        {
            while (!q.IsEmpty())
            {
                BinNode<int> node = q.Remove();
                if (node == null)
                    Console.Write(" null ");
                else if (node.Value == -1)
                    Console.Write(" \\n\n");
                else
                    Console.Write(" " + node.Value + " ");
            }
        }

        private static Queue<BinNode<int>> TreeToQueue(BinNode<int> t)
        {
            Queue<BinNode<int>> q = new Queue<BinNode<int>>();
            TreeToQueue(t, q);
            return q;
        }

        private static void TreeToQueue(BinNode<int> t, Queue<BinNode<int>> q)
        {
            Queue<BinNode<int>> queue = new Queue<BinNode<int>>();
            var lf = new BinNode<int>(-1);
            int full = (int)Math.Pow(2, Depth(t));
            int count = 1;
            queue.Insert(t);
            queue.Insert(lf);
            while (!queue.IsEmpty() && count < full)
            {
                // Console.WriteLine(count + " Midprint: " + ExtractValues(Copy(queue)));
                var node = queue.Remove();
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
        }

        private static int Width(int value)
        {
            return value.ToString().Length;
        }

        private static int Width(BinNode<int> t)
        {
            if (t == null)
                return 1;
            return Math.Max(Width(t.Value), Math.Max(Width(t.Left), Width(t.Right)));
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
                array[i] = rng.Next(rangeMin, rangeMax);
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

        public static void RandomlyPopulate(BinNode<int> t, Random rng, double baseChance = 0.5, int level = 0)
        {
            if (level > 10)
                return;
            double chance = Math.Pow(baseChance, level);
            if (rng.NextDouble() < chance)
            {
                t.Left = new BinNode<int>(rng.Next(rangeMin, rangeMax));
                RandomlyPopulate(t.Left, rng, baseChance, level + 1);
            }
            if (rng.NextDouble() < chance)
            {
                t.Right = new BinNode<int>(rng.Next(rangeMin, rangeMax));
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

        private static int Depth<T>(BinNode<T> t)
        {
            if (t == null)
                return 0;
            return Math.Max(Depth(t.GetLeft()), Depth(t.GetRight())) + 1;
        }

        private static string GetIndent(int level, int depth, int width)
        {
            // 2 ^ (d-l) - 1
            return new string(' ', (int)(Math.Pow(2, depth - level) - 1));
        }

        private static string GetLeftmostIndent(float level, int depth, int width)
        {
            // 3 * 2 ^ (d-l-2)
            return new string(' ', (int)(3 * Math.Pow(2, depth - level - 2)));
        }

        private static string GetLeftIndent(float level, int depth, int width)
        {
            // 3 * 2 ^ (d-l-1) - 1
            if (level == depth - 1)
                return new string(' ', width);
            return new string(' ', (int)(3 * Math.Pow(2, depth - level - 1) - 1));
        }

        private static string GetRightIndent(float level, int depth, int width)
        {
            // 2 ^ (d-l-1) - 1
            if (level == depth - 1)
                return " ";
            return new string(' ', (int)(Math.Pow(2, depth - level - 1) - 1));
        }

        // IDEA: make a new queue of INTS, this new queue will be used to print out the slashes, 1 will be a real number 0 will be a null.

        //Nimrod's probably shitty implementation lol
        public static void Print(BinNode<int> t)
        {
            int maxDepth = Depth(t);
            bool nl = true; //newline
            int currentDepth = 0;
            Queue<BinNode<int>> queue = TreeToQueue(t);
            int width = Width(t);
            while (!queue.IsEmpty())
            {
                var node = queue.Remove();

                if (node != null && node.Value == -1)
                {
                    Console.WriteLine();
                    nl = true;
                    currentDepth++;
                    PrintSlashes(Mapping(queue), currentDepth, maxDepth + width - 1, width);
                    Console.WriteLine();
                    continue;
                }

                string indent = GetIndent(currentDepth, maxDepth + width - 1, width);
                string halfIndent = indent.Substring(indent.Length / 2);

                Console.Write(nl ? halfIndent : indent);
                Console.Write(node == null ? new string('-', width) : node.Value + new string('X', width - node.Value.ToString().Length));
                nl = false;
            }
        }

        private static Queue<bool> Mapping(Queue<BinNode<int>> source)
        {
            Queue<BinNode<int>> temp = new Queue<BinNode<int>>();
            while (!source.IsEmpty())
                temp.Insert(source.Remove());

            Queue<bool> result = new Queue<bool>();
            bool flag = true;
            while (!temp.IsEmpty())
            {
                var value = temp.Remove();
                source.Insert(value);
                if (value != null && value.Value == -1)
                    flag = false;
                if (flag)
                    result.Insert(value != null);
            }

            return result;
        }

        private static void PrintSlashes(Queue<bool> q, int currentDepth, int maxDepth, int width)
        {
            if (currentDepth == 0)
                return;
            int count = 0;
            while (!q.IsEmpty())
            {
                if (count == 0)
                    Console.Write(GetLeftmostIndent(currentDepth, maxDepth, width));
                else if (count % 2 == 0)
                    Console.Write(GetLeftIndent(currentDepth, maxDepth, width));
                else
                    Console.Write(GetRightIndent(currentDepth, maxDepth, width));
                char slash = (count % 2 == 0) ? '/' : '\\';
                Console.Write(q.Remove() ? slash + new string(' ', width - 1) : " ");
                count++;
            }
        }
    }
}
