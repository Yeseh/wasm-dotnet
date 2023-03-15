﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Wasi.SourceGenerator
{
    internal static class StringExtensions
    {
        public static string ToUpperSnakeCase(this string text)
        {
            return text.ToLowerSnakeCase().ToUpperInvariant();
        }
            
        public static string ToLowerSnakeCase(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Length < 2)
            {
                return text;
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
