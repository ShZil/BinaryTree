using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class BinNode<T>
    {
        public T Value
        { get; set; }
        public BinNode<T> Left
        { get; set; }
        public BinNode<T> Right
        { get; set; }

        public BinNode(T value)
        {
            this.Value = value;
            this.Left = null;
            this.Right = null;
        }

        public BinNode(BinNode<T> left, T value, BinNode<T> right)
        {
            this.Value = value;
            this.Left = left;
            this.Right = right;
        }

        public T GetValue()
        {
            return this.Value;
        }

        public void SetValue(T value)
        {
            this.Value = value;
        }

        public BinNode<T> GetLeft()
        {
            return this.Left;
        }

        public void SetLeft(BinNode<T> left)
        {
            this.Left = left;
        }

        public BinNode<T> GetRight()
        {
            return this.Right;
        }

        public void SetRight(BinNode<T> right)
        {
            this.Right = right;
        }

        public bool HasLeft()
        {
            return this.Left != null;
        }

        public bool HasRight()
        {
            return this.Right != null;
        }
    }
}