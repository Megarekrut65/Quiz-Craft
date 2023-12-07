using System;
using Global.Data;
using Main;
using UnityEngine;

namespace Quiz
{
    public class ResponseSender
    {
        private readonly string _token;
        private readonly string _uid;

        public ResponseSender(string uid, string token)
        {
            _uid = uid;
            _token = token;
        }

        public void Send(MonoBehaviour behaviour, int question, int answer, Action<Error, string> callback)
        {
            WWWForm form = new WWWForm();
            form.AddField("uid", _uid);
            form.AddField("question_id", question);
            form.AddField("answer_id", answer);

            Fetcher.Post(behaviour, "game-responses/create", _token, form, callback);
        }
    }
}