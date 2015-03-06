using ConsoleMoneyTransform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMoneyTransform.Abstractions
{
    public interface ITransformer
    {

        MoneyOrders GetTransformedResult(); 

    }
}
