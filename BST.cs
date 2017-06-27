using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WillisfordProg260CourseProject
{
    class BST
    {
        public BSTNode TopNode { get; set; }

        // Constructor
        public BST()
        {

        }

        public Book Find(int ISBN)
        {
            // •	Entering an ISBN and removing an existing book (with appropriate error message if there is no such book) (5 points)
            BSTNode current = TopNode;  // Start at the top
            while (current != null)     // This will cycle as long as there is a node to check
            {
                if (ISBN > current.bstKey)
                {
                    current = current.RightNode;
                }
                else if (ISBN < current.bstKey)
                {
                    current = current.LeftNode;
                }
                else
                {
                    return current.DataValue;
                }
            }

            // If we made it here we didn't find it. 
            // a default "DIDN"T FIND IT" Book to return
            return new Book(); 
        }

        public bool Add(Book newBook)
        {
            // •	Entry of a new book object, which has in it stored ith the correct information,  into the BST (5 points)
            BSTNode current = TopNode;  // Start at the top
            while (current != null)
            {
                if (newBook.ISBN < current.bstKey)      // Go left
                {
                    if(current.LeftNode == null)        // Nothing is there, so we put this new one there 
                    {
                        current.LeftNode = new BSTNode(newBook);
                        current.LeftNode.ParentNode = current;
                        adjustBalance(current, 1);
                        return true;
                    }
                    else
                    {
                        current = current.LeftNode;     // Something is there so go to it                 
                    }
                }
                else if (newBook.ISBN > current.bstKey)     // Go right
                {
                    if (current.RightNode == null)        // Nothing is there, so we put this new one there 
                    {
                        current.RightNode = new BSTNode(newBook);
                        current.RightNode.ParentNode = current;
                        adjustBalance(current, -1);
                        return true;
                    }
                    else
                    {
                        current = current.RightNode;     // Something is there so go to it                 
                    }
                }
                else                                        // Same ISBN, can't have dublicites - return false to trigger error catching 
                {
                    return false;
                }
            }

            current = new BSTNode(newBook);
            if (TopNode == null)
            {
                // this would be if there is nothing in the BST at all
                TopNode = current;
            }
            return true;
        }

        public bool Delete(int ISBN)
        {
            // •	Entering an ISBN and removing an existing book (with appropriate error message if there is no such book) (5 points)
            // Lets start at the top, 
            BSTNode current = TopNode;
            while (current != null)
            {
                if (ISBN > current.bstKey)
                {
                    current = current.RightNode;
                }
                else if (ISBN < current.bstKey)
                {
                    current = current.LeftNode;
                }

                // okay found it. 
                else
                {
                    // lets get the kids
                    BSTNode left = current.LeftNode;
                    BSTNode right = current.RightNode;

                    // what if there isn't a kid there?
                    if (right == null)
                    {
                        if (left == null)
                        {
                            // okay both are null . . .   
                            if (current == TopNode)
                            {
                                // we are at the top
                                TopNode = null;
                            }
                            else
                            {
                                // we are at the bottom
                                BSTNode parent = current.ParentNode;
                                if (parent.LeftNode == current)                     
                                {
                                    // we are on the left
                                    parent.LeftNode = null;
                                    adjustBalanceDelete(parent, -1);
                                }
                                else
                                {
                                    // we are on the right
                                    parent.RightNode = null;
                                    adjustBalanceDelete(parent, 1);
                                }
                            }
                        }
                        else
                        {
                            Reattach(current, left);
                            adjustBalanceDelete(current, 0);
                        }
                    }
                    else if(left == null)
                    {
                        Reattach(current, right);
                        adjustBalanceDelete(current, 0);
                    }
                    else
                    {
                        // okay we have two kids . . . 
                        BSTNode replacer = right;
                        if (replacer.LeftNode == null)
                        {
                            // what if it doesnt a have kid on the left?
                            BSTNode parent = current.ParentNode;

                            // move him up
                            replacer.ParentNode = parent;
                            replacer.LeftNode = left;
                            replacer.Balance = current.Balance;
                            left.ParentNode = replacer;

                            if (current == TopNode)
                            {
                                // if we are the top . . .   
                                TopNode = replacer;
                            }
                            else
                            {
                                // attach it to the right spot
                                if (parent.LeftNode == current)
                                {
                                    parent.LeftNode = replacer;
                                }
                                else
                                {
                                    parent.RightNode = replacer;
                                }
                            }

                            adjustBalanceDelete(replacer, 1);
                        }
                        else
                        {
                            // There is something to the left so we need to travel down and find the open slot
                            while (replacer.LeftNode != null)
                            {
                                replacer = replacer.LeftNode;
                            }

                            // we found the first open slot on the left. 
                            BSTNode parent = current.ParentNode;
                            BSTNode replacerParent = replacer.ParentNode;
                            BSTNode replacerRight = replacer.RightNode;

                            if (replacerParent.LeftNode == replacer)
                            {
                                replacerParent.LeftNode = replacerRight;                                
                            }
                            else
                            {
                                replacerParent.RightNode = replacerRight;
                            }

                            if (replacerRight != null)
                            {
                                replacerRight.ParentNode = replacerParent;
                            }

                            // we should be clear at this point to move the data up
                            replacer.ParentNode = parent;
                            replacer.LeftNode = left;
                            replacer.Balance = current.Balance;
                            replacer.RightNode = right;
                            right.ParentNode = replacer;
                            left.ParentNode = replacer;

                            if (current == TopNode)
                            {
                                TopNode = replacer;
                            }
                            else
                            {
                                if (parent.LeftNode == current)
                                {
                                    parent.LeftNode = replacer;
                                }
                                else
                                {
                                    parent.RightNode = replacer;
                                }
                            }

                            // We made it!  Now the last balancing act.
                            adjustBalanceDelete(replacerParent, -1);
                        }
                    }

                    // okay, we found it, deleted it and rebalanced.
                    return true;
                }
            }

            // didn't find it
            return false;
        }

        public List<Book> Show()
        {
            List<Book> ListOfBooks = new List<Book>();
        // •	Displaying a list of all the ISBN numbers in the BST (5 points)
        // First the really easy way - recursion
        // start at the top
            BSTNode current = TopNode;
            // the recursive call . . .    
            recursivePrint(current, ListOfBooks);
            return ListOfBooks;

            // wow boring.  Okay lets do it with out recursion, oh and make it sortable by clicking on the column headers. 
            // TODO something cool

            // I had initially wanted to write this whole program without recursion as that seemed too easy for a "course project."
            // unfortunately my prof mentioned he was going to talk about self balancing trees for a day after all. I wanted to turn
            // this in before he talked about it so there are a few spots where I left in the original recursive calls I first built.

            // As time alows I want to come back and update these calls so I can work through the logic
        }

        private void adjustBalance(BSTNode current, int balance)
        {
            while (current != null)
            {
                // set the new balance
                balance = (current.Balance += balance);

                //check if it 0
                if(balance == 0)
                {
                    return;
                }
                // check if it is 2 or -2
                else if (balance == 2)
                {
                    // now we need to know if the imbalance is on the inner node or not
                    if (current.LeftNode.Balance == 1)
                    {
                        AdjustRight(current);
                    }
                    else
                    {
                        AdjustLeftRight(current);
                    }
                    // And we're good
                    return;
                }
                else if (balance == -2)
                {
                    if (current.RightNode.Balance == -1)
                    {
                        AdjustLeft(current);
                    }
                    else
                    {
                        AdjustRightLeft(current);
                    }
                    return;
                }
                // finally if it is 1 or -1 then we need to move up the tree and adjust the parant
                BSTNode parent = current.ParentNode;
                if (parent != null)
                {
                    balance = parent.LeftNode == current ? 1 : -1;
                }

                // and head up to check that one.
                current = parent;
            }
        }

        private BSTNode AdjustLeft(BSTNode current)
        {
            // I need to move three objects
            BSTNode right = current.RightNode;
            BSTNode rightLeft = right.LeftNode;
            BSTNode parent = current.ParentNode;
            // lets move things around
            right.ParentNode = parent;
            right.LeftNode = current;
            current.RightNode = rightLeft;
            current.ParentNode = right;

            // Confused yet?  I would be if I weren't looking at my diagram
            // Now we check for floaters and special cases

            if (rightLeft != null)
            {
                rightLeft.ParentNode = current;     // reset his parent
            }
            if (current == TopNode)
            {
                TopNode = right;                    // set the new top if needed
            }
            else if (parent.RightNode == current)
            {
                parent.RightNode = right;
            }
            else
            {
                parent.LeftNode = right;
            }

            // whew. Okay that wasn't so bad
            right.Balance++;
            current.Balance = -right.Balance;

            return right;       // in case I need it. 
        }

        private BSTNode AdjustRight(BSTNode current)
        {
            // mirrored AdjustLeft, an easy copy, paste, adjust

            // I need to move three objects
            BSTNode left = current.LeftNode;
            BSTNode leftRight = left.RightNode;
            BSTNode parent = current.ParentNode;
            // lets move things around
            left.ParentNode = parent;
            left.RightNode = current;
            current.LeftNode = leftRight;
            current.ParentNode = left;

            if (leftRight != null)
            {
                leftRight.ParentNode = current;     // reset his parent
            }
            if (current == TopNode)
            {
                TopNode = left;                    // set the new top if needed
            }
            else if (parent.RightNode == current)
            {
                parent.LeftNode = left;
            }
            else
            {
                parent.RightNode = left;
            }

            left.Balance++;
            current.Balance = -left.Balance;

            return left;       // in case I need it. 
        }

        // Okay, easy ones are done. Now the semi-tough one. (still looking forward to deleting)
        private BSTNode AdjustLeftRight(BSTNode current)
        {
            BSTNode left = current.LeftNode;
            BSTNode leftRight = left.RightNode;
            BSTNode parent = current.ParentNode;
            BSTNode leftRightRight = leftRight.RightNode;
            BSTNode leftRightLeft = leftRight.LeftNode;

            // This one has a lot of moving pieces
            leftRight.ParentNode = parent;
            current.LeftNode = leftRightRight;
            left.RightNode = leftRightLeft;
            leftRight.LeftNode = left;
            leftRight.RightNode = current;
            left.ParentNode = leftRight;
            current.ParentNode = leftRight;

            // Good lord!  Now the special cases
            if (leftRightRight != null)
            {
                leftRightRight.ParentNode = current;
            }

            if (leftRightLeft != null)
            {
                leftRightLeft.ParentNode = left;
            }

            if (current == TopNode)
            {
                TopNode = leftRight;
            }
            else if (parent.LeftNode == current)
            {
                parent.LeftNode = leftRight;
            }
            else
            {
                parent.RightNode = leftRight;
            }

            // Reset balances . . .    
            if (leftRight.Balance == -1)
            {
                current.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                current.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                current.Balance = -1;
                left.Balance = 0;
            }

            leftRight.Balance = 0;
            return leftRight;
        }

        private BSTNode AdjustRightLeft(BSTNode current)
        {
            BSTNode right = current.RightNode;
            BSTNode rightLeft = right.LeftNode;
            BSTNode parent = current.ParentNode;
            BSTNode rightLeftLeft = rightLeft.LeftNode;
            BSTNode rightLeftRight = rightLeft.RightNode;

            // Once the pattern imerges this gets much easier
            rightLeft.ParentNode = parent;
            current.RightNode = rightLeftLeft;
            right.LeftNode = rightLeftRight;
            rightLeft.RightNode = right;
            rightLeft.LeftNode = current;
            right.ParentNode = rightLeft;
            current.ParentNode = rightLeft;

            if (rightLeftLeft != null)
            {
                rightLeftLeft.ParentNode = current;
            }

            if (rightLeftRight != null)
            {
                rightLeftRight.ParentNode = right;
            }

            if (current == TopNode)
            {
                TopNode = rightLeft;
            }
            else if (parent.RightNode == current)
            {
                parent.RightNode = rightLeft;
            }
            else
            {
                parent.LeftNode = rightLeft;
            }

            // Reset balances . . .    
            if (rightLeft.Balance == 1)
            {
                current.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                current.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                current.Balance = 1;
                right.Balance = 0;
            }

            rightLeft.Balance = 0;
            return rightLeft;
        }

        private void Reattach(BSTNode TargetToOverwrite, BSTNode SourceData)
        {
            // I suppose I should have called it something else. but eh
            BSTNode left = SourceData.LeftNode;
            BSTNode right = SourceData.RightNode;

            TargetToOverwrite.Balance = SourceData.Balance;
            TargetToOverwrite.bstKey = SourceData.bstKey;
            TargetToOverwrite.DataValue = SourceData.DataValue;
            TargetToOverwrite.LeftNode = left;
            TargetToOverwrite.RightNode = right;

            if (left != null){
                left.ParentNode = TargetToOverwrite;
            }

            if (right != null)
            {
                right.ParentNode = TargetToOverwrite;
            }
        }

        private void adjustBalanceDelete(BSTNode current, int balance)
        {
            // almost a copy of the normal balance but this one can loop
            while (current != null)
            {
                // set the new balance
                balance = current.Balance += balance;

                // check if we need to readjust
                if (balance == 2)
                {   
                    // if the left tree is not negative (ie all the way left) we need to adjust to the right and continue
                    // from the resulting node

                    if (current.LeftNode.Balance >= 0)
                    {
                        current = AdjustRight(current);

                        // if it is now negative we can exit. We are done
                        if (current.Balance == -1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        // other wise we need to do the double move
                        current = AdjustLeftRight(current);
                    }
                }
                else if (balance == -2)
                {
                    // basicly the same comments for the right. :-)
                    if (current.RightNode.Balance <= 0)
                    {
                        current = AdjustLeft(current);
                        if (current.Balance == 1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        current = AdjustRightLeft(current);
                    }
                }
                else if (balance != 0)
                {
                    return;
                }

                // the parent still needs to be adjusted
                BSTNode parent = current.ParentNode;

                if(parent != null)
                {
                    balance = parent.LeftNode == current ? -1 : 1;
                }

                current = parent;   // go up a level and check again.
            }
        }

        private void recursivePrint(BSTNode current, List<Book> ListOfBooks)
        {
            // another remaining recursive method
            if (current != null)
            {
                recursivePrint(current.LeftNode, ListOfBooks);
                ListOfBooks.Add(current.DataValue);
                recursivePrint(current.RightNode, ListOfBooks);
            }
        }
    }
}
