using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_INotifyDataErrorInfo
{
    public static class TestData
    {
        public static DataProducts CreateDataItems(int products)
        {
            DataProducts dataItems = new();

            for (int i = 0; i < products; i++)
            {
                DataProduct dataItem = new()
                {
                    Product = "Product" + i,
                    Stock = i,                   
                };
                
                dataItems.Add(dataItem);
            }

            return dataItems;
        }
    }
}