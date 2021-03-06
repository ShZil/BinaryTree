using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Node<T>
    {
        private T value;
        private Node<T> next;

        public Node(T value)
        {
            this.value = value;
            this.next = null;
        }

        public Node(T value, Node<T> next)
        {
            this.value = value;
            this.next = next;
        }

        public T GetValue()
        {
            return this.value;
        }

        public void SetValue(T value)
        {
            this.value = value;
        }

        public Node<T> GetNext()
        {
            return this.next;
        }

        public void SetNext(Node<T> next)
        {
            this.next = next;
        }

        public bool HasNext()
        {
            return this.next != null;
        }
    }
}
