using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWpf.Core
{
    public static class RandomStirrer
    {
        public static IList<T> RandomShuffle<T>(this IEnumerable<T> list)
        {
            var random = new Random();
            var shuffle = new List<T>(list);
            for (var i = 2; i < shuffle.Count; ++i)
            {
                var temp = shuffle[i];
                var nextRandom = random.Next(i - 1);
                shuffle[i] = shuffle[nextRandom];
                shuffle[nextRandom] = temp;
            }
            return shuffle;
        }
    }
}
