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

            // ARRANGE
            var db = DbContextHelper.CreateInMemoryDbContext<Db>();

            var validator = new CreateRequestValidator(db);
            var request = new CreateRequest {
                Title = string.Empty
            };

            // ACT
            var result = await validator.ValidateAsync(request);

            // ASSERT
            Assert.IsFalse(result.IsValid);

        }

        [TestMethod]
        public async Task Duplicate_Title_Is_Invalid() {

            // ARRANGE
            var db = DbContextHelper.CreateInMemoryDbContext<Db>();
            db.Categories.Add(new DDDEastAnglia.Api.Data.Entities.Category { Title = "Title 1" });
            await db.SaveChangesAsync();

            var validator = new CreateRequestValidator(db);
            var request = new CreateRequest {
                Title = "Title 1"
            };

            // ACT
            var result = await validator.ValidateAsync(request);

            // ASSERT
            Assert.IsFalse(result.IsValid);

        }

    }

}
