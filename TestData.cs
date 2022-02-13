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
        public static DataItems CreateDataItems(int parent, int children)
        {
            DataItems dataItems = new();

            for (int i = 0; i < parent; i++)
            {
                DataItem dataItem = new()
                {
                    ParentName = "Parent" + i,
                    ParentNumber = i,
                    Children = CreateChildren(children)
                };

                dataItems.Add(dataItem);
            }

            return dataItems;
        }

        public static ObservableCollection<DataChildren> CreateChildren(int children)
        {
            ObservableCollection<DataChildren> Children = new();

            for (int i = 1; i <= children; i++)
            {
                DataChildren dataChildren = new()
                {
                    ChildName = "ChildName" + i,
                    ChildNumber = i,
                };

                Children.Add(dataChildren);
            }

            return Children;
        }
    }
}