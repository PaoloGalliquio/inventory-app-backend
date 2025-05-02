using System;
using System.Collections.Generic;
using System.Linq;

namespace inventory_app_backend.Validators
{
    public class ValidatorResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Dictionary<string, List<string>> errors { get; set; }

        public ValidatorResult()
        {
            success = true;
            message = string.Empty;
            errors = new Dictionary<string, List<string>>();
        }

        public bool HasErrors()
        {
            return !success || errors?.SelectMany(x => x.Value).Any() == true;
        }

        public static ValidatorResult GetSuccessfulResult(string message = null)
        {
            return new ValidatorResult
            {
                message = message ?? string.Empty,
                success = true,
                errors = new Dictionary<string, List<string>>()
            };
        }

        internal void AddError(string v1, string v2)
        {
            success = false;
            if (errors == null)
            {
                errors = new Dictionary<string, List<string>>();
            }
            if (!errors.ContainsKey(v1))
            {
                errors[v1] = new List<string>();
            }
            errors[v1].Add(v2);
            message = v2;
        }

        public void ClearErrors()
        {
            success = true;
            message = string.Empty;
            errors = new Dictionary<string, List<string>>();
        }
    }
}