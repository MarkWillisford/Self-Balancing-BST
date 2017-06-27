using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WillisfordProg260CourseProject
{
    class BSTNode
    {
        public int bstKey;
        public BSTNode ParentNode;
        public BSTNode LeftNode;
        public BSTNode RightNode;
        public Book DataValue; 
        public int Balance;

        public BSTNode(Book value)
        {
            bstKey = value.ISBN;
            DataValue = value;
        }
    }
}
