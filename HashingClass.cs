using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HashingTechniques
{
    // V value, değeri temsil eder, key değeri long olarak seçildiğinden sadece value değeri Generic tipte oluşturuldu.
    class MyHashingSet<V> 
    {
        //Separate Chaining Insertion örneği, burada örnek olması için linked list kullandım, daha performanslı -->
        //--> HashSet için BinarySearchTree,AVLTree,RedBlack Tree gibi yapılar da kullanılabilir.

        //LinkedList array , indeksleri HasIndexGenerator den alınır
        private MySinglyLinkedList<long>[] myHashList;
        public int Size { get; private set; }
        public MyHashingSet(int size)
        {
            myHashList = new MySinglyLinkedList<long>[size];//HashSet'i oluşturan dizi; dizi elemanları MySinglyLinkedList nesnelerini içerir.
            this.Size = size;
        }
        private long HashIndexGenerator(long key) //myHashList dizisinin indekslerini oluşturan HashCode üreteci
        {
            int seedKey=Convert.ToInt32(key/2);
            return (5 * key + new Random(seedKey).Next(7654)) % Size;
        }
        public void AddHashSet(long key, V value)
        {
            if (myHashList[HashIndexGenerator(key)] == null)
            {
                myHashList[HashIndexGenerator(key)] = new MySinglyLinkedList<long>();
                myHashList[HashIndexGenerator(key)].AddLast(key, value);
            }
            else
            {
                myHashList[HashIndexGenerator(key)].AddLast(key, value);
            }
        } //HashSet'e eleman ekler
        public long RemoveHashElement(long key)
        {
            if (myHashList[HashIndexGenerator(key)] != null)
            {
               return myHashList[HashIndexGenerator(key)].Remove(key);
            }

            return default;
        } //HashSet'den eleman siler
        public V SearchForValue(long key)
        {
            V result = myHashList[HashIndexGenerator(key)] == null
                ? default
                : myHashList[HashIndexGenerator(key)].GetValueAtKey(key);
            return result;
        } //Key'e göre değeri döndürür
        public void ShowAllHashList()
        {
            for (int i = 0; i < Size; i++)
            {
                Console.Write($"myHasList[{i}]: ");
                if (myHashList[i] != null)
                {
                    myHashList[i].ShowList();
                }

                Console.WriteLine();
            }
        }

        #region MyLinkedList<K,V>
        //V değeri üst sınıftan gelir, bu nedenle ikinci kez belirtmenize gerek yok.
        internal class MySinglyLinkedList<K> where K : IEquatable<K>, IComparable<K>
        {
            private int size;
            private Node head;
            private Node tail;

            public MySinglyLinkedList()
            {
                size = 0;
                head = null;
                tail = null;
            }

            private class Node
            {
                public K key;
                public V value;
                public Node next;

                public Node(K key, V value)
                {
                    this.key = key;
                    this.value = value;
                    this.next = null;
                }

            }

            public void AddLast(K key, V value)
            {
                Node node = new Node(key, value);
                if (head == null)
                {
                    head = tail = node;
                    size++;
                    return;
                }

                tail.next = node;
                tail = node;
                size++;
            }

            public K RemoveFirst()
            {

                if (IsEmpty())
                {
                    return default;
                }

                K temp = head.key;

                if (head == tail)
                {
                    head = tail = null;
                }
                else
                {
                    head = head.next;
                }

                size--;
                return temp;
            }

            public K RemoveLast()
            {
                if (IsEmpty())
                {
                    return default;
                }

                if (head == tail)
                {
                    K key = head.key;
                    head = tail = null;
                    size--;
                    return key;
                }

                Node current = head, previous = null;
                while (current != tail)
                {
                    previous = current;
                    current = current.next;
                }

                previous.next = null;
                tail = previous;
                size--;
                return current.key;
            }

            public K Remove(K key)
            {
                if (IsEmpty())
                    return default;

                Node current = head, previous = null;
                while (current != null)
                {
                    if (current.key.CompareTo(key) == 0)
                    {
                        if (current == head)
                        {
                            return RemoveFirst();
                        }

                        if (current == tail)
                        {
                            return RemoveLast();
                        }

                        previous.next = current.next;
                        size--;
                        return current.key;
                    }

                    previous = current; //Previous ve Current pointerlarla senkronize arama yapilir.
                    current = current.next;
                }

                return default;
            }

            public V GetValueAtKey(K key)
            {
                if (head == null)
                    throw new Exception("LinkedList is empty");

                Node tempNode = head;

                for (int i = 0; i < size; i++)
                {
                    if (tempNode.key.CompareTo(key) == 0)
                        return tempNode.value;
                    tempNode = tempNode.next;
                }

                return default;
            }

            public bool Contain(K key)
            {
                Node node = head;
                while (node != null)
                {
                    if (node.key.CompareTo(key) == 0)
                    {
                        return true;
                    }

                    node = node.next;
                }

                return false;
            }

            public int Size() => size;
            public bool IsEmpty() => size == 0;

            public void ShowList()
            {
                Node node = head;
                while (node != null)
                {
                    Console.Write(node.key);
                    Console.Write(" ");
                    node = node.next;
                }

            }
        }

        #endregion

    }
}

