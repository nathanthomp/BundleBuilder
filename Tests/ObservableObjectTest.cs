using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBuilt.Revit.BundleBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ObservableObjectTest
    {
        [TestMethod]
        public void PropertyChangedEventHandlerIsRaised()
        {
            var obj = new StubObservableObject();

            bool raised = false;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.IsTrue(e.PropertyName == "ChangedProperty");
                raised = true;
            };

            obj.ChangedProperty = "Some value";

            if (!raised)
            {
                Assert.Fail("PropertyChanged was never invoked");
            }
        }

        class StubObservableObject : ObservableObject
        {
            private string changedProperty;

            public string ChangedProperty
            {
                get { return changedProperty; }
                set 
                { 
                    changedProperty = value;
                    NotifyPropertyChanged();
                }
            }

        }
    }
}
