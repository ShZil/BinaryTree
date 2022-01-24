using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Stack<T>
    {
        Node<T> head;

        public Stack()
        {
            this.head = null;
        }

        public bool IsEmpty()
        {
            return this.head == null;
        }

        public void Push(T value)
        {
            this.head = new Node<T>(value, this.head);
        }

        public T Pop()
        {
            T value = this.head.GetValue();
            this.head = this.head.GetNext();
            return value;
        }

        public T Top()
        {
            return this.head.GetValue();
        }

        public override string ToString()
        {
            string acc = "[" + this.head.GetValue().ToString();
            Node<T> pos = this.head.GetNext();
            while (pos != null)
            {
                acc += ", " + pos.GetValue().ToString();
                pos = pos.GetNext();
            }
            return acc + "]\n\n";
        }
    }
}
