using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business.DataModels;
using FluentAssertions;
using NUnit.Framework;

namespace BachelorThesis.Tests
{

    public class Node : IChildrenAware<Node>
    {
        public int Id { get; set; }
        public int? Tester { get; set; }

        public int TesterId { get; set; }

        private List<Node> nodes = new List<Node>();

        public void AddNode(Node child)
        {
            nodes.Add(child);
        }

        public List<Node> GetChildren() => nodes;
        private static int nextId= 0;

        public Node(int testerId)
        {
            Id = nextId++;
            Tester = null;
            TesterId = testerId;
        }
    }

    [TestFixture]
    public class TreeStructureHelperTests
    {
        private int? GetTester(Node n) => n.TesterId;

        [Test]
        public void Traverse_SetProperty_ShouldTraverseAll()
        {
            var tree = new List<Node>();

            var a = new Node(0);
            a.AddNode(new Node(1));
            a.AddNode(new Node(2));

            var b = new Node(3);
            b.AddNode(new Node(4));

            tree.Add(a);
            tree.Add(b);

            foreach (var treeNode in tree)
            {
                TreeStructureHelper.Traverse<Node, int?>(treeNode, GetTester, (node, o) => node.Tester = o);
            }

            foreach (var node in tree)
            {
                node.Tester.Should().Be(node.TesterId);
            }
         
        }
    }
}
