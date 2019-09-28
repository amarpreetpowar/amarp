// MP1: HTML Validator
// Implementation of a simple stack for HtmlTags.

// You should implement this class.
// You should add comments and improve the documentation.

using System;
using System.Collections.Generic;

namespace HtmlValidator
{
    public class MyStack
    {
        // A List to hold HtmlTag objects.
        // Use this to implement a stack.
        private List<HtmlTag> stack_internal;

        /// <summary>
        /// Create an empty stack.
        /// </summary>
        public MyStack()
        {
            this.stack_internal = new List<HtmlTag>();
        }

        /// <summary>
        /// Pushes a tag onto the top of the stack.
        /// </summary>
        /// <param name = "tag"> the Html Tag to push onto the stack </param>
        public void Push(HtmlTag tag)
        {
            stack_internal.Insert(0, tag);
        }

        /// <summary> 
        /// Removes the tag at the top of the stack.
        /// Should throw an exception if the stack is empty. 
        /// </summary>
        /// <returns> Value of tag removed </returns>
        public HtmlTag Pop()
        {
            HtmlTag popValue = null;

            try
            {
                popValue = stack_internal[0];
                stack_internal.RemoveAt(0);
            }
            catch (ArgumentOutOfRangeException)
            {
                //Exception if stack is empty
                Console.WriteLine("Stack is empty.");
            }
            return popValue;
        }


        /// <summary>
        /// Looks at the HtmlTag at the top of the stack but does 
        /// not actually remove the tag from the stack. 
        /// Should throw an exception if the stack is empty.
        /// </summary>
        /// <returns> HtmlTag at the top of the stack </returns>
        /// 

        public HtmlTag Peek()
        {
            HtmlTag topTag = null;

            try
            {
                topTag = stack_internal[0];
            }
            catch (ArgumentOutOfRangeException)
            {
                //Exception if stack is empty
                Console.WriteLine("Stack is empty.");
            }
            return topTag;
        }
            

        /// <summary>
        /// Tests if the stack is empty.
        /// </summary>
        /// <returns> Returns true if the stack is empty; false otherwise. </returns>
        public bool IsEmpty()
        {
            if (stack_internal.Count == 0)
            {
                return true;
            }
            return false; 
        }
    }
}
