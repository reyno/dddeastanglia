using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.MediatR.Requests.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace api.tests.Requests.Categories {

    [TestClass]
    public class CreateRequestValidatorTests {

        [TestMethod]
        public async Task Empty_Title_Is_Invalid() {

            var db = DbContextHelper.CreateInMemoryDbContext<Db>();

            var request = new CreateRequest {
                Title = string.Empty
            };

            var validator = new CreateRequestValidator(
                db
                );

            var result = await validator.ValidateAsync(request);

            Assert.IsFalse(result.IsValid);

        }

    }

}
