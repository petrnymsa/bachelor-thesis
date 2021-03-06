﻿using System;

namespace BachelorThesis.Business.DataModels
{
    public static class TreeStructureHelper
    {
        public static void Traverse<T, TOpt>(T node, TOpt parameter, Action<T, TOpt> action) where T : IChildrenAware<T>
        {
            foreach (var child in node.GetChildren())
            {
                action(child, parameter);
                Traverse(child,parameter,action);
            }
        }

        public static void Traverse<T, TOpt>(T node, Func<T, TOpt> getParameter, Action<T, TOpt> action) where T : IChildrenAware<T>
        {
            action(node, getParameter(node));

            foreach (var child in node.GetChildren())
            {
                TOpt parameter = getParameter(child);
                action(child, parameter);
                Traverse(child, parameter, action);
            }
        }

        public static T Find<T>(T node, Predicate<T> predicate) where T : class, IChildrenAware<T>
        {
            if (node == null)
                return null;

            if (predicate(node))
                return node;

            foreach (var child in node.GetChildren())
            {
                var found = Find<T>(child, predicate);
                if (found != null)
                    return found;
            }

            return null;
        }
    }
}