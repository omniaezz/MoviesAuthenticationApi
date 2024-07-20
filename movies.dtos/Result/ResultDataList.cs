using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movies.dtos.Result
{
    public class ResultDataList<TEntity>
    {
        public List<TEntity> Entites { get; set; }
        public int Count { get; set; }
    }
}
