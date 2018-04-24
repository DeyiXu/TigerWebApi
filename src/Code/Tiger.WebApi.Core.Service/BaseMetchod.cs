using System.Collections.Generic;

namespace Tiger.WebApi.Core.Service
{
    public class BaseMetchod : ITigerMethod
    {
        protected IDictionary<string, string> _args;
        public BaseMetchod(IDictionary<string, string> args)
        {
            this._args = args;
        }

        public virtual object Invoke()
        {
            return "Api Metchod Error.";
        }
    }
}
