using System.Collections.Generic;

namespace CaseBackend.Application.Query.Responses
{
    public class Response<T>
    {
        public Response()
        {
            this.Messages = new List<string>();
        }

        public Response(T value)
            : this()
        {
            this.Value = value;
        }


        #region Properties

        public T Value { get; set; }

        public List<string> Messages { get; set; }

        public bool IsValid => this.Messages.Count == 0;

        #endregion

        #region Methods

        public void AddValue(T value) => this.Value = value;

        public void AddMessages(params string[] messages) => this.Messages.AddRange(messages);

        #endregion
    }
}
