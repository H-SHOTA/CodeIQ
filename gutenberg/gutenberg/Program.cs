using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace gutenberg
{
    class Program
    {
        /// <summary>
        /// 活字辞書
        /// </summary>
        static Dictionary<string, Queue<string>> dict = new Dictionary<string, Queue<string>>();

        static void Main(string[] args)
        {
            CreateDict();
            var wordList = GetWordList("STAY HUNGRY, STAY FOOLISH");
            if (wordList != null)
            {
                Console.WriteLine(wordList);
            }
            return;
        }

        /// <summary>
        /// 活字辞書から活字の位置を取り出す
        /// </summary>
        /// <param name="sentence">印刷する文章</param>
        /// <returns></returns>
        static string GetWordList(string sentence)
        {
            string wordList = "";
            var charList = sentence.ToCharArray();

            foreach (var word in charList)
            {
                // 活字を見つけたら、活字辞書からボックスの位置を取り出す。
                // 探している途中で辞書内に活字が存在しない場合はnullを返して処理終了とする。
                if (dict[word.ToString()].Count != 0)
                {
                    wordList += string.Format("{0}\n", dict[word.ToString()].Dequeue());
                }
                else
                {
                    return null;
                }
            }
            return wordList;
        }

        /// <summary>
        /// 活字の辞書を作成する
        /// </summary>
        static void CreateDict()
        {
            // ファイルの読み込み
            var lines = System.IO.File.ReadLines("gutenberg.csv").ToArray();
            //活字辞書の作成
            for (var y = 0; y < lines.Count(); y++)
            {
                // 正規表現で「""」で括られてる文字をリストで取り出す
                Regex regex = new Regex("\".\"");
                var charList = regex.Matches(lines[y]);
                for (var x = 0; x < charList.Count; x++)
                {
                    if (dict.ContainsKey(charList[x].Value.Replace("\"", "")))
                    {
                        dict[charList[x].Value.Replace("\"", "")].Enqueue((y + 1) + "," + (x + 1));
                    }
                    else
                    {
                        dict.Add(charList[x].Value.Replace("\"", ""), new Queue<string>(new string[] { (y + 1) + "," + (x + 1) }));
                    }
                }
                y++;
            }
        }
    }
}
