using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace codeRetrievalApp.Lib
{
    public static class Util
    {
        public static String[] DodgerBlue = { "const", "goto", "public", "protected", "private", "class", "interface", "abstract", "implements", "extends", "new",
        "import", "package","byte","char","boolean","short","int","float","long","double","void","if","else","while","for","switch","case","default","do","break",
        "contine","return","instanceof","static","final","super","this","native","strictfp","synchronized","transient","volatile","catch","try","finally","throw","throws","enum","assert"};
        public static String[] LimeGreen = { "true", "false", "null" };
        private const string TRANSLATION = "Translation";

        public static List<String> SplitCodeLine(String codeline)
        {
            List<String> _idSegments = new List<string>();
            String content = codeline;
            String backup = codeline;
            String commentPattern = "//[^\n]*$";
            String strPattern = "\"([^\"]*)\"";
            String tokenPattern = "[^a-zA-Z0-9]";
            List<String> found = new List<string>();
            var commentFound = Regex.Matches(content, commentPattern);
            foreach (Match comment in commentFound)
            {
                found.Add(comment.Value);
                content = content.Replace(comment.Value, "");
            }
            var strFound = Regex.Matches(content, strPattern);
            foreach (Match str in strFound)
            {
                found.Add(str.Value);
                content = content.Replace(str.Value, "");
            }
            var tokenFound = Regex.Matches(content, tokenPattern);
            foreach (Match token in tokenFound)
            {
                try
                {
                    if (token.Value != "")
                    {
                        found.Add(token.Value);
                        content = content.Replace(token.Value, ",");
                    }
                }
                catch
                {
                    continue;
                }
            }
            String[] identifiers = content.Split(',');
            foreach (var id in identifiers)
            {
                if (id != "")
                {
                    found.Add(id);
                }
            }
            while (found.Count > 0)
            {
                for (int i = 0; i < found.Count; i++)
                {
                    if (backup.StartsWith(found[i]))
                    {
                        _idSegments.Add(found[i]);
                        backup = backup.Remove(0, found[i].Length);
                        found.RemoveAt(i);
                        break;
                    }
                }
            }
            return _idSegments;
        }

        public static String GetSpanOfId(String Identifier)
        {
            int i = 0;
            for (i = 0; i < Identifier.Length; i++)
            {
                if (Identifier[i] != ' ')
                {
                    break;
                }
            }
            if (i == Identifier.Length)
            {
                return "<Span/>" + Identifier;
            }
            if (Identifier.StartsWith("\"") && Identifier.EndsWith("\""))
            {
                //Salmon
                return "<Span Foreground=\"#FFFA8072\">" + Identifier + "</Span>";
            }
            else if (Identifier.StartsWith("//"))
            {
                //FirestGreen
                return "<Span Foreground=\"#FF228B22\">" + Identifier + "</Span>";
            }
            else if (Util.LimeGreen.Contains(Identifier))
            {
                //LimeGreen
                return "<Span Foreground=\"#FF32CD32\">" + Identifier + "</Span>";
            }
            else if (Util.DodgerBlue.Contains(Identifier))
            {
                //DodgerBlue
                return "<Span Foreground=\"#FF1E90FF\">" + Identifier + "</Span>";
            }
            else if (Util.IsNumber(Identifier))
            {
                //Feldspar
                return "<Span Foreground=\"#FFD19275\">" + Identifier + "</Span>";
            }
            else
            {
                return "<Span>" + Identifier + "</Span>";
            }
        }

        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]+$";
            const string SixteenPattern = "^[0][xX][0-9a-fA-F]+$";
            Regex rx = new Regex(pattern);
            Regex rx16 = new Regex(SixteenPattern);
            var amatch = rx.IsMatch(s);
            var bmatch = rx16.IsMatch(s);
            return amatch || bmatch;
        }

        public static Visual GetVisual(this UIElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            try
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
                visual.Properties.InsertVector3(TRANSLATION, Vector3.Zero);
            }
            catch
            {
                visual = null;
            }
            return visual;
        }

        public static async Task WaitForSizeChangedAsync(this FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException(nameof(frameworkElement));
            }

            var initW = frameworkElement.ActualWidth;
            var initH = frameworkElement.ActualHeight;

            while (frameworkElement.ActualWidth == initW && frameworkElement.ActualHeight == initH)
            {
                var tcs = new TaskCompletionSource<object>();

                SizeChangedEventHandler handler = null;

                handler = (sender, e) =>
                {
                    frameworkElement.SizeChanged -= handler;
                    tcs.SetResult(null);
                };

                frameworkElement.SizeChanged += handler;

                await tcs.Task;
            }
        }

        public static CompositionAnimationBuilder StartBuildAnimation(this Visual visual)
        {
            return new CompositionAnimationBuilder(visual);
        }

        public static void SetTranslation(this Visual set, Vector3 value)
        {
            set.Properties.InsertVector3(TRANSLATION, value);
        }

        public static Vector3 GetTranslation(this Visual visual)
        {
            visual.Properties.TryGetVector3(TRANSLATION, out Vector3 value);
            return value;
        }

        public static string GetTranslationPropertyName(this Visual visual)
        {
            return AnimateProperties.Translation.GetPropertyValue();
        }

        public static string GetTranslationXPropertyName(this Visual visual)
        {
            return AnimateProperties.TranslationX.GetPropertyValue();
        }

        public static string GetTranslationYPropertyName(this Visual visual)
        {
            return AnimateProperties.TranslationY.GetPropertyValue();
        }

        public static string GetPropertyValue(this AnimateProperties property)
        {
            switch (property)
            {
                case AnimateProperties.Translation:
                    return TRANSLATION;

                case AnimateProperties.TranslationX:
                    return $"{TRANSLATION}.X";

                case AnimateProperties.TranslationY:
                    return $"{TRANSLATION}.Y";

                case AnimateProperties.Opacity:
                    return "Opacity";

                case AnimateProperties.RotationAngleInDegrees:
                    return "RotationAngleInDegrees";

                default:
                    throw new ArgumentException("Unknown properties");
            }
        }
    }
}
