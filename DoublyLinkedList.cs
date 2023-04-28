using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projob_Projekt
{
    internal class DoublyLinkedList<T>
    {
        Node<T>? head;
        Node<T>? tail;
        public DoublyLinkedList()
        {
            head = null;
            tail = null;
        }
    }
    public class Node<T>
    {
        T? val;
        Node<T>? next { get; set; }
        Node<T>? prev { get; set; }
        public Node(T val, Node<T>? next = null, Node<T>? prev = null)
        {
            this.val = val;
            this.next = next;
            this.prev = prev;
        }
    }
}
