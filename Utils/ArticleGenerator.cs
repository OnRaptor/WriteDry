using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fare;

namespace WriteDry.Utils
{
    public static class ArticleGenerator
    {
        public static string GenerateArticle(IEnumerable<string> allExistringArticles)
        {
            Xeger xeger = new Xeger(@"[A-Z][0-9][1-9]{2}[A-Z][1-9]");
            var next = xeger.Generate();
            for (int i = 0; i < allExistringArticles.Count(); i++)
            {
                if (next == allExistringArticles.ElementAt(i))
                {
                    next = xeger.Generate();
                    i = 0;
                }
            }
            return next;
        }
    }
}
