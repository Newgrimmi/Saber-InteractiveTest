using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Test2
{
    class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random;
        public string Data;
    }

    class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(Stream s)
        {
            Dictionary<ListNode, int> dictionary = new Dictionary<ListNode, int>();
            int id = 0;
            for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
            {
                dictionary.Add(currentNode, id);
                id++;
            }
            using (BinaryWriter writer = new BinaryWriter(s))
            {
                for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
                {
                    writer.Write(currentNode.Data);
                    writer.Write(dictionary[currentNode.Random]);
                }
            }
            Console.WriteLine("List serialized");

        }

        public void Deserialize(Stream s)
        {
            Dictionary<int, Tuple<String, int>> dictionary = new Dictionary<int, Tuple<String, int>>();
            int counter = 0;
            using (BinaryReader reader = new BinaryReader(s))
            {
                while (reader.PeekChar() != -1)
                {
                    String data = reader.ReadString();
                    int randomId = reader.ReadInt32();
                    dictionary.Add(counter, new Tuple<String, int>(data, randomId));
                    counter++;
                }
                Console.WriteLine("File readed");
            }
            Count = counter;
            Head = new ListNode();
            ListNode current = Head;
            Dictionary<int, int> dictionaryRndIndex = new Dictionary<int, int>();
            Dictionary<int, ListNode> dictionaryNodeIndex = new Dictionary<int, ListNode>();

            for (int i = 0; i < Count; i++)
            {
                dictionaryRndIndex.Add(i, dictionary.ElementAt(i).Value.Item2);
                dictionaryNodeIndex.Add(i, current);
                current.Data = dictionary.ElementAt(i).Value.Item1;
                current.Next = new ListNode();
                if (i != this.Count - 1)
                {
                    current.Next.Previous = current;
                    current = current.Next;
                }
                else
                {
                    Tail = current;
                }
            }
            counter = 0;

            for (ListNode currentNode = Head; currentNode.Next != null; currentNode = currentNode.Next)
            {
                currentNode.Random = dictionaryNodeIndex[dictionaryRndIndex[counter]];
                counter++;
            }

            Console.WriteLine("List deserialized");
        }
    }
}
