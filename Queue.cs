using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Queue<T>
    {
        private Node<T> first;
        private Node<T> last;

        public Queue()
        {
            this.first = null;
            this.last = null;
        }

        public bool IsEmpty()
        {
            return first == null;
        }

        public void Insert(T x)
        {
            if (this.first == null)
            {
                this.first = new Node<T>(x, null);
                this.last = this.first;
            }
            else
            {
                Node<T> pos = new Node<T>(x, null);
                this.last.SetNext(pos);
                this.last = pos;
            }
        }

        public T Remove()
        {
            var value = first.GetValue();
            first = first.GetNext();
            return value;
        }

        public T Head()
        {
            return first.GetValue();
        }

        public override string ToString()
        {
            Node<T> pos = first;
            if (pos == null)
                return "[]";
            string s = "[" + pos.GetValue();
            while (pos.HasNext())
            {
                pos = pos.GetNext();
                s += ", " + pos.GetValue();
            }
            return s + "]";

        }
    }
}
