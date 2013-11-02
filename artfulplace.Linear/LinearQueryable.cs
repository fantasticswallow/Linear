using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace artfulplace.Linear
{
    public class LinearQueryable<T> : IQueryable, IQueryable<T>,IEnumerable<T>
    {

        public LinearQueryable(IQueryable<T> _source)
        {
            this.source = _source;
            this._expression = Expression.Constant(_source,typeof(IQueryable<T>));
        }

        public LinearQueryable(IQueryable<T> _source,Expression expr)
        {
            this.source = _source;
            this._expression = expr;
        }

        private IQueryable<T> source { get; set; }

        public Type ElementType
        {
            get 
            {
                return _expression.Type;
            }
        }

        private Expression _expression;

        public Expression Expression
        {
            get 
            {
                return _expression;
            }
        }

        public IQueryProvider Provider
        {
            get 
            {
                return source.Provider; 
            }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return source.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return source.GetEnumerator();
        }
    }
}
