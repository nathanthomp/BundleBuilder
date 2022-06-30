using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBuilt.Revit.BundleBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Tests
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void IsAbstractBaseClass()
        {
            Type t = typeof(ViewModel);
            Assert.IsTrue(t.IsAbstract);
        }

        [TestMethod]
        public void IsIDataErrorInfo()
        {
            Assert.IsTrue(typeof(IDataErrorInfo).IsAssignableFrom(typeof(ViewModel)));
        }

        [TestMethod]
        public void IsObservableObject()
        {
            Assert.IsTrue(typeof(ViewModel).BaseType == typeof(ObservableObject));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void IDataErrorInfo_ErrorProperty_IsNotSupported()
        {
            var viewModel = new StubViewModel();
            var value = viewModel.Error;
        }

        [TestMethod]
        public void IndexerPropertyValidatesPropertyNameWithInvalidValue()
        {
            var viewModel = new StubViewModel();
            Assert.IsNotNull(viewModel["RequiredProperty"]);

        }

        [TestMethod]
        public void IndexerPropertyValidatesPropertyNameWithValidValue()
        {
            var viewModel = new StubViewModel
            {
                RequiredProperty = "Some value"
            };
            Assert.IsNull(viewModel["RequiredProperty"]);
        }

        [TestMethod]
        public void IndexerReturnsErrorMessageForRequestedInvalidProperty()
        {
            var viewModel = new StubViewModel
            {
                RequiredProperty = null,
                SomeOtherProperty = null
            };

            var msg = viewModel["SomeOtherProperty"];
            Assert.AreEqual("The SomeOtherProperty field is required.", msg);
        }

        class StubViewModel : ViewModel
        {
            [Required]
            public string RequiredProperty
            {
                get; set;
            }

            [Required]
            public string SomeOtherProperty
            {
                get; set;
            }
        }

    }
}
