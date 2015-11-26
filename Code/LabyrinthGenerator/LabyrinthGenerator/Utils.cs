using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGenerator
{
    public static class Utils
    {
        public static IEnumerable<Node> Flatten(this Node root)
        {
            var stack = new Stack<Node>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                foreach (var child in current.Nodes)
                    stack.Push(child);
            }
        }

        public static IEnumerable<TSource> FancyFlatten<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> selectorFunction,
            Func<TSource, IEnumerable<TSource>> getChildrenFunction)
        {
            // Add what we have to the stack
            var flattenedList = source.Where(selectorFunction);

            // Go through the input enumerable looking for children,
            // and add those if we have them
            foreach (TSource element in source)
            {
                flattenedList =
                    flattenedList.Concat(getChildrenFunction(element).FancyFlatten(selectorFunction, getChildrenFunction));
            }
            return flattenedList;
        }
    }
}
