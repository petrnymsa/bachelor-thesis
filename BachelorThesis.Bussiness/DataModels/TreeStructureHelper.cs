﻿using System;

namespace BachelorThesis.Bussiness.DataModels
{
    public static class TreeStructureHelper
    {
        public static T FindByIdentificator<T>(T node, string identificator) where T : class, IIdentifiable, IChildrenAware<T>
        {
            if (node == null)
                return null;

            if (string.Equals(node.GetIdentificator(), identificator, StringComparison.InvariantCulture))
                return node;

            foreach (var child in node.GetChildren())
            {
                var found = FindByIdentificator(child, identificator);
                if (found != null)
                    return found;
            }

            return null;
        }

        public static void Traverse<T, TOpt>(T node, TOpt parameter, Action<T, TOpt> action) where T : IChildrenAware<T>
        {
            foreach (var child in node.GetChildren())
            {
                action(child, parameter);
                Traverse(child,parameter,action);
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