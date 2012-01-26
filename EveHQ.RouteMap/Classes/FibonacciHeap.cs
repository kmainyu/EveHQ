// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2012  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 26 January 20121, and as distributed, it includes or is 
// derivative of works licensed under the GNU General Public License.
//
// EveHQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveHQ.RouteMap
{
    public class FibonacciHeap<T> where T : IComparable<T>
    {
        private Node min;
        private int n;

        public void clear()
        {
            min = null;
            n = 0;
        }

        private void consolidate()
        {
            // The magic 45 comes from log base phi of Integer.MAX_VALUE,
            // which is the most elements we will ever hold, and log base
            // phi represents the largest degree of any root list node.
            //@SuppressWarnings("unchecked")
            var A = new Node[45];
            // For each root list node look for others of the same degree.
            Node start = min;
            Node w = min;
            do
            {
                Node x = w;
                // Because x might be moved, save its sibling now.
                Node nextW = w.right;
                int d = x.degree;
                while (A[d] != null)
                {
                    // Make one of the nodes a child of the other.
                    Node y = A[d];
                    if (x.isGreaterThan(y))
                    {
                        Node temp = y;
                        y = x;
                        x = temp;
                    }
                    if (y == start)
                    {
                        // Because removeMin() arbitrarily assigned the min
                        // reference, we have to ensure we do not miss the
                        // end of the root node list.
                        start = start.right;
                    }
                    if (y == nextW)
                    {
                        // If we wrapped around we need to check for this case.
                        nextW = nextW.right;
                    }
                    // Node y disappears from root list.
                    y.link(x);
                    // We've handled this degree, go to next one.
                    A[d] = null;
                    d++;
                }
                // Save this node for later when we might encounter another
                // of the same degree.
                A[d] = x;
                // Move forward through list.
                w = nextW;
            } while (w != start);
            // The node considered to be min may have been changed above.
            min = start;
            // Find the minimum key again.
            foreach (var a in A)
            {
                if (a != null && a.isLessThan(min))
                    min = a;
            }
        }

        public void decreaseKey(Node x, float k)
        {
            decreaseKey(x, k, false);
        }

        private void decreaseKey(Node x, float k, bool delete)
        {
            if (!delete && k > x.key)
                throw new Exception("cannot increase key value");
            x.key = k;
            Node y = x.parent;
            if (y != null && (delete || x.isLessThan(y)))
            {
                y.cut(x, min);
                y.cascadingCut(min);
            }
            if (delete || x.isLessThan(min))
                min = x;
        }

        public void delete(Node x)
        {
            // make x as small as possible
            decreaseKey(x, 0, true);
            // remove the smallest, which decreases n also
            removeMin();
        }

        public bool isEmpty()
        {
            return min == null;
        }

        public Node insert(T x, float key)
        {
            Node node = new Node(x, key);
            // concatenate node into min list
            if (min != null)
            {
                node.right = min;
                node.left = min.left;
                min.left = node;
                node.left.right = node;
                if (node.isLessThan(min))
                    min = node;
            }
            else
                min = node;
            n++;
            return node;
        }

        public Node removeMin()
        {
            Node z = min;
            if (z == null)
                return null;
            if (z.child != null)
            {
                z.child.parent = null;
                // for each child of z do...
                for (Node x = z.child.right; x != z.child; x = x.right)
                {
                    // set parent[x] to null
                    x.parent = null;
                }
                // merge the children into root list
                Node minleft = min.left;
                Node zchildleft = z.child.left;
                min.left = zchildleft;
                zchildleft.right = min;
                z.child.left = minleft;
                minleft.right = z.child;
            }
            // remove z from root list of heap
            z.left.right = z.right;
            z.right.left = z.left;
            if (z == z.right)
                min = null;
            else
            {
                min = z.right;
                consolidate();
            }
            // decrement size of heap
            n--;
            return z;
        }

        #region Nested type: Node

        public class Node
        {
            public Node(T data, float key)
            {
                datum = data;
                this.key = key;
                right = this;
                left = this;
            }

            public T datum { get; set; }
            public float key { get; set; }
            public Node parent { get; set; }
            public Node child { get; set; }
            public Node right { get; set; }
            public Node left { get; set; }
            public int degree { get; set; }
            public bool mark { get; set; }

            public void cascadingCut(Node min)
            {
                Node z = parent;
                // if there's a parent...
                if (z != null)
                {
                    if (mark)
                    {
                        // it's marked, cut it from parent
                        z.cut(this, min);
                        // cut its parent as well
                        z.cascadingCut(min);
                    }
                    else
                    {
                        // if y is unmarked, set it marked
                        mark = true;
                    }
                }
            }

            public void cut(Node x, Node min)
            {
                // remove x from childlist and decrement degree
                x.left.right = x.right;
                x.right.left = x.left;
                degree--;
                // reset child if necessary
                if (degree == 0)
                    child = null;
                else if (child == x)
                    child = x.right;
                // add x to root list of heap
                x.right = min;
                x.left = min.left;
                min.left = x;
                x.left.right = x;
                // set parent[x] to nil
                x.parent = null;
                // set mark[x] to false
                x.mark = false;
            }

            public void link(Node parent)
            {
                // Note: putting this code here in Node makes it 7x faster
                // because it doesn't have to use generated accessor methods,
                // which add a lot of time when called millions of times.
                // remove this from its circular list
                left.right = right;
                right.left = left;
                // make this a child of x
                this.parent = parent;
                if (parent.child == null)
                {
                    parent.child = this;
                    right = this;
                    left = this;
                }
                else
                {
                    left = parent.child;
                    right = parent.child.right;
                    parent.child.right = this;
                    right.left = this;
                }
                // increase degree[x]
                parent.degree++;
                // set mark false
                mark = false;
            }

            public bool isGreaterThan(Node n)
            {
                if (n == null)
                    return false;

                if (key > n.key || (key == n.key && datum.CompareTo(n.datum) > 0))
                    return true;
                return false;
            }

            public bool isLessThan(Node n)
            {
                if (n == null)
                    return false;

                if (key < n.key || (key == n.key && datum.CompareTo(n.datum) < 0))
                    return true;
                return false;
            }
        }

        #endregion
    }
}
