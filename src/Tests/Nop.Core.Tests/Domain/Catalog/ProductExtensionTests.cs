﻿using System;
using Nop.Core.Domain.Catalog;
using Nop.Tests;
using NUnit.Framework;

namespace Nop.Core.Tests.Domain.Catalog
{
    [TestFixture]
    public class ProductExtensionTests
    {
        [Test]
        public void Can_parse_required_product_ids()
        {
            var ids = TestHelper.GetProduct().ParseRequiredProductIds();
            ids.Length.ShouldEqual(3);
            ids[0].ShouldEqual(1);
            ids[1].ShouldEqual(4);
            ids[2].ShouldEqual(7);
        }

        [Test]
        public void Should_be_available_when_startdate_is_not_set()
        {
            var product = TestHelper.GetProduct(false);
            product.IsAvailable(new DateTime(2010, 01, 03)).ShouldEqual(true);
        }

        [Test]
        public void Should_be_available_when_startdate_is_less_than_somedate()
        {
            TestHelper.GetProduct().IsAvailable(new DateTime(2010, 01, 03)).ShouldEqual(true);
        }

        [Test]
        public void Should_not_be_available_when_startdate_is_greater_than_somedate()
        {
            TestHelper.GetProduct().IsAvailable(new DateTime(2009, 12, 31)).ShouldEqual(false);
        }

        [Test]
        public void Should_be_available_when_enddate_is_not_set()
        {
            var product = TestHelper.GetProduct(setAvailableEndDateTimeUtc: false);
            product.IsAvailable(new DateTime(2010, 01, 03)).ShouldEqual(true);
        }

        [Test]
        public void Should_be_available_when_enddate_is_greater_than_somedate()
        {
            TestHelper.GetProduct().IsAvailable(new DateTime(2010, 01, 01)).ShouldEqual(true);
        }

        [Test]
        public void Should_not_be_available_when_enddate_is_less_than_somedate()
        {
            TestHelper.GetProduct().IsAvailable(new DateTime(2010, 01, 04)).ShouldEqual(false);
        }
    }
}
