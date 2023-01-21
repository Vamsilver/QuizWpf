using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWpf.Core
{
    class Question
    {
        public ObjectId Id { get; set; }
        public string Quaere { get; set; }
        public string Answer { get; set; }

        public Question(string quaere, string answer)
        {
            Quaere = quaere;
            if (answer.Length < 40)
                Answer = answer;
            else
                throw new Exception();
        }
    }
}
