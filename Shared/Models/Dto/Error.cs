using System.Collections.Generic;

namespace Shared.Models.Dto
{
    public class Error
    {
        public Error(string error, bool isShow)
        {
            Errors.Add(error);
            IsShow = isShow;
        }

        public Error(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }

        public List<string> Errors { get; private set; } = new();
        public bool IsShow { get; private set; }
    }
}