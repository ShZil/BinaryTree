using System;

namespace ConsoleApp1
{
    class Program
    {
        static readonly int rangeMax = 10;
        static readonly int rangeMin = 0;

        public static void Main(string[] args)
        {
            BinNode<char> charTree = Create(RandomChars(15));
            PrintRecursive(charTree);

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

        public static void PrintRecursive<T>(BinNode<T> t)
        {
            int depth = Depth(t);
            for (int level = 0; level < depth; level++)
            {
                PrintSlashesRecursive(t, 0, level, depth, true, false);
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
                string halfIndent = indent[(indent.Length / 2)..]; //TODO explain to me what the .. is
                Console.Write(isMostLeft ? halfIndent : indent);
                Console.Write(t == null ? "-" : t.Value.ToString());
            }
            if (t == null)
                t = new BinNode<T>(null, default, null);
            PrintLevel(t.Left, current + 1, level, stop, isMostLeft);
            PrintLevel(t.Right, current + 1, level, stop);
        }

        private static void PrintSlashesRecursive<T>(BinNode<T> t, int current, int level, int stop, bool isMostLeft = false, bool isLeft = false)
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
            PrintSlashesRecursive(t.Left, current + 1, level, stop, isMostLeft, true);
            PrintSlashesRecursive(t.Right, current + 1, level, stop, false, false);
        }

        private static int Depth<T>(BinNode<T> t)
        {
            if (t == null)
                return 0;
            return Math.Max(Depth(t.GetLeft()), Depth(t.GetRight())) + 1;
        }

        private static string GetIndent(int level, int depth)
        {
            // 2 ^ (d-l) - 1
            return new string(' ', (int)(Math.Pow(2, depth - level) - 1));
        }

        private static string GetLeftmostIndent(float level, int depth)
        {
            // 3 * 2 ^ (d-l-2)
            return new string(' ', (int)(3 * Math.Pow(2, depth - level - 2)));
        }

        private static string GetLeftIndent(float level, int depth)
        {
            // 3 * 2 ^ (d-l-1) - 1
            if (level == depth - 1)
                return " ";
            return new string(' ', (int)(3 * Math.Pow(2, depth - level - 1) - 1));
        }

        private static string GetRightIndent(float level, int depth)
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
                    PrintSlashes(Mapping(queue), currentDepth, maxDepth);
                    Console.WriteLine();
                    continue;
                }

                string indent = GetIndent(currentDepth, maxDepth);
                string halfIndent = indent.Substring(indent.Length / 2);

                Console.Write(nl ? halfIndent : indent);
                Console.Write(node == null ? "-" : node.Value);
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

        private static void PrintSlashes(Queue<bool> q, int currentDepth, int maxDepth)
        {
            if (currentDepth == 0)
                return;
            int count = 0;
            while (!q.IsEmpty())
            {
                if (count == 0)
                    Console.Write(GetLeftmostIndent(currentDepth, maxDepth));
                else if (count % 2 == 0)
                    Console.Write(GetLeftIndent(currentDepth, maxDepth));
                else
                    Console.Write(GetRightIndent(currentDepth, maxDepth));
                char slash = (count % 2 == 0) ? '/' : '\\';
                Console.Write(q.Remove() ? slash : ' ');
                count++;
            }
        }

        /*
        Full 16 Tree:
                       1
               2               3
           4       5       6       7
         8   9   A   B   C   D   E   F
        - - - - - - - - - - - - - - - -

        Leftmost Indents:
        0: 15
        1: 7
        2: 3
        3: 1
        4: 0
        n: 2^(d - n) - 1

        Separation Indents:
        0: [undefined]
        1: 15
        2: 7
        3: 3
        4: 1
        n: 2^(4 - n + 1) - 1



        Full Lined 16 Tree:
                       1
                   /       \
               2               3
             /   \           /   \
           4       5       6       7
          / \     / \     / \     / \
         8   9   A   B   C   D   E   F

        Slash Indents (→, ↓):
        0+: 11, 7
        1+: 5, 3, 11, 3
        2+: 2, 1, 5, 1, 5, 1, 5, 1

        leftmost = 11, 5, 2
        alternate^ = 7, 3, 1 = 2^(3-n) - 1   // 2^(d-n-1) - 1
        alternateV = 11, 5
        altercount = 1, 3, 7 = 2^(n+1) - 1


        Full Something something tree:
                                       1
                               /               \
                       2                               3
                   /       \                       /       \
               4               5               6               7
             /   \           /   \           /   \           /   \
           8       9       A       B       C       D       E       F
          / \     / \     / \     / \     / \     / \     / \     / \
         G   H   I   J   K   L   M   N   O   P   Q   R   S   T   U   V

        d = 5

        Leftmost:
        0+: 23
        1+: 11,
        2+: 5,
        3+: 2   // 3*2^(5-n) - 1   // 3*2^(d-n) - 1

        alternateV:
        1+: 23
        2+: 11
        3+: 5
        n: (n-1) / 2 ?


        alternate^:
        0+: 15
        1+: 7
        2+: 3
        3+: 1

        */
    }
}
