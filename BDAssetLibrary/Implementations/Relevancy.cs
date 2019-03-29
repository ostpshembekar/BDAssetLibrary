using BDAssetLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDAssetLibrary.Implementations
{
    public class Relevancy : IRelevancy
    {
        public Dictionary<string, int> FindRelevancy(IList<string> items, IList<string> queries)
        {
            if (items == null || items.Count == 0) return null;
            if (queries == null || queries.Count == 0)
                return ZeroRelevancy(items);

            var keyValuePairs = new Dictionary<string, int>();
            foreach (var query in queries)
            {
                Dictionary<string, int> relevancies = FindSimpleLinearRelevancy(items, query);
                foreach (var relevance in relevancies)
                {
                    if (keyValuePairs.ContainsKey(relevance.Key))
                        keyValuePairs[relevance.Key] += relevance.Value;
                    else
                        keyValuePairs.Add(relevance.Key, relevance.Value);
                }
            }
            return keyValuePairs;
        }

        /// <summary>
        /// Finds a simple linear relevancy using the lengths of strings in results and the query
        /// </summary>
        /// <param name="items"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private Dictionary<string, int> FindSimpleLinearRelevancy(IList<string> items, string query)
        {
            // if there was no query, the returned results cannot be matched against anything
            // hence we return zero relevancy
            if (string.IsNullOrWhiteSpace(query))
                return ZeroRelevancy(items);

            var dict = new Dictionary<string, int>();

            foreach (var item in items)
            {
                if (!dict.ContainsKey(item))
                {
                    var itemRelevancy = GetLongestCommonSubstringInfo(item, query);
                    // calculate what part of the item is in the query
                    double coverageRelevancy = CalculateCoverageRelevancy(item, query, itemRelevancy.MaxLength);
                    // calculate what part of the query is in the item
                    double querydensity = CalculateQueryDensity(query, itemRelevancy.MaxLength);
                    // calculate from where the item starts to match the query 
                    // (query that matches the item from the beginning is more relevant than the one that matches in the middle)
                    double positionRelevancy = CalculatePositionRelevancy(item, itemRelevancy.Start);
                    int relevancy = Convert.ToInt32(Math.Ceiling(coverageRelevancy * positionRelevancy * querydensity * 100));
                    dict.Add(item, relevancy);
                }
            }
            return dict;
        }

        private int FindSimpleLinearRelevancy(string item, string query)
        {
            var itemRelevancy = GetLongestCommonSubstringInfo(item, query);
            // calculate what part of the item is in the query
            double coverageRelevancy = CalculateCoverageRelevancy(item, query, itemRelevancy.MaxLength);
            // calculate what part of the query is in the item
            double querydensity = CalculateQueryDensity(query, itemRelevancy.MaxLength);
            // calculate from where the item starts to match the query 
            // (query that matches the item from the beginning is more relevant than the one that matches in the middle)
            double positionRelevancy = CalculatePositionRelevancy(item, itemRelevancy.Start);
            int relevancy = Convert.ToInt32(Math.Ceiling(coverageRelevancy * positionRelevancy * querydensity * 100));
            return relevancy;
        }

        private double CalculateQueryDensity(string query, int maxQueryLength)
        {
            if (string.IsNullOrEmpty(query)) return 0.0;
            return (double)(maxQueryLength) / query.Length;
        }

        private double CalculatePositionRelevancy(string item, int start)
        {
            if (string.IsNullOrEmpty(item)) return 0.0;
            return (double)(item.Length - start) / item.Length;
        }

        private double CalculateCoverageRelevancy(string item, string query, int maxLength)
        {
            if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(query)) return 0.0;
            int den = (item.Length >= query.Length) ? item.Length : query.Length;
            return (double)maxLength / den;
        }

        private Dictionary<string, int> ZeroRelevancy(IList<string> items)
        {
            var dict = new Dictionary<string, int>();
            foreach (var item in items)
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, 0);
                }
            }
            return dict;
        }

        private int GetCommonCharacterLength(string shorter, string longer)
        {
            int index = longer.IndexOf(shorter);

            if (index < 0)
                return 0;

            return shorter.Length;
        }

        private int LongestCommonSubstring(string str1, string str2)
        {
            // if either of the two strings are null or empty, return zero
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
                return 0;
            int[,] matrix = new int[str1.Length, str2.Length];
            int maxlength = 0;

            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] != str2[j])
                        matrix[i, j] = 0;
                    else
                    {
                        if (i == 0 || j == 0)
                            matrix[i, j] = 1;
                        else
                            matrix[i, j] = matrix[i - 1, j - 1] + 1;
                        if (matrix[i, j] > maxlength)
                            maxlength = matrix[i, j];
                    }
                }
            }
            return maxlength;
        }

        internal class ItemRelevancy
        {
            internal int MaxLength { get; set; }
            internal int Start { get; set; }
        }

        private ItemRelevancy GetLongestCommonSubstringInfo(string item, string query)
        {
            // if either of the two strings are null or empty, return zero
            if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(query))
            {
                return new ItemRelevancy { MaxLength = 0, Start = item.Length };
            }
            int[,] matrix = new int[item.Length, query.Length];
            int maxlength = 0;
            int start = item.Length;

            for (int i = 0; i < item.Length; i++)
            {
                for (int j = 0; j < query.Length; j++)
                {
                    if (item[i] != query[j])
                        matrix[i, j] = 0;
                    else
                    {
                        if (i == 0 || j == 0)
                            matrix[i, j] = 1;
                        else
                            matrix[i, j] = matrix[i - 1, j - 1] + 1;
                        if (matrix[i, j] > maxlength)
                        {
                            maxlength = matrix[i, j];
                            start = i + 1 - maxlength;
                        }
                    }
                }
            }
            return new ItemRelevancy { MaxLength = maxlength, Start = start };

        }
    }
}
