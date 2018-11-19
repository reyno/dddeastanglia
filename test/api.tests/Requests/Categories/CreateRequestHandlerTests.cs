using AutoMapper;
using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.MediatR.Requests.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace api.tests.Requests.Categories {

    [TestClass]
    public class CreateRequestHandlerTests {

        [TestMethod]
        public async Task Category_Is_Created() {

            // ARRANGE
            var db = DbContextHelper.CreateInMemoryDbContext<Db>();
            var mapper = new Mapper(new MapperConfiguration(options => {
                options.AddProfile<AutoMapperProfile>();
            }));
            
            var handler = new CreateRequestHandler(db, mapper);
            var request = new CreateRequest {
                Title = "A Title"
            };

            // ACT
            var result = await handler.Handle(request, CancellationToken.None);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreNotEqual(default(int), result.Id);
            Assert.AreNotEqual(0, db.Categories.Count());


        }

    }
}
