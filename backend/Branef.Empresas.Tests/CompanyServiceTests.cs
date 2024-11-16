using Branef.Empresas.Data.Enums;
using Branef.Empresas.DB;
using Branef.Empresas.DependencyInjection;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using Branef.Empresas.Domain.Queries;

namespace Branef.Empresas.Tests
{
    public class CompanyServiceTests
    {
        private ManualDependencyInjection _manualDependencyInjection;

        private string RandomString(int length) => TestUtils.RandomString(length);

        private InsertOrUpdateCompanyCommand _CreateInsertOrUpdateCommand()
        {
            var utcNow = DateTimeOffset.UtcNow;

            return new InsertOrUpdateCompanyCommand
            {
                Id = Guid.NewGuid(),
                Name = RandomString(50),
                Size = Data.Enums.CompanySize.Pequena
            };
        }

        private InsertOrUpdateCompanyCommand _ConvertToInsertOrUpdateModel(CompanyModel model) =>
            new InsertOrUpdateCompanyCommand
            {
                Id = model.Id,
                Name = model.Name,
                Size = model.Size
            };


        [SetUp]
        public void Setup()
        {
            var builder = new ManualDependencyInjectionBuilder();

            _manualDependencyInjection = builder.BuildConfiguration(false).Build(services =>
            {
                services
                    .AddDomainInMemoryWriteDbContext()
                    .AddDomainRepositories()
                    .AddDomainQueryToExpressionAdapters()
                    .AddDomainsConverters()
                    .AddDomainServices();
            });
        }

        [Test]
        public async Task Should_Insert_Company_And_Generate_NewId()
        {
            var service = _manualDependencyInjection.GetService<ICompanyService<BranefWriteDbContext>>();

            if (service is null)
                Assert.Fail("Service not created!");

            var model = _CreateInsertOrUpdateCommand();
            var inserted = await service.InsertAsync(model);
            var affected = await service.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThan(0));
            Assert.That(inserted, Is.Not.Null);
            Assert.That(inserted.Id, Is.Not.Empty);
        }

        [Test]
        public async Task Should_Update_Company_With_Given_Information()
        {
            var service = _manualDependencyInjection.GetService<ICompanyService<BranefWriteDbContext>>();

            if (service is null)
                Assert.Fail("Service not created!");

            var model = _CreateInsertOrUpdateCommand();            
            var inserted = await service.InsertAsync(model);
            var affected = await service.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThan(0));

            var insertedModel = await service.GetByIdAsync(inserted.Id);

            if (insertedModel is null)
                Assert.Fail("Company was not inserted");

            var newName = RandomString(60);
            var newSize = CompanySize.Grande;

            var oldName = insertedModel.Name;
            var oldSize = insertedModel.Size;

            insertedModel.Name = newName;
            insertedModel.Size = newSize;            

            var convertedInsertOrUpdateModel = _ConvertToInsertOrUpdateModel(insertedModel);

            Assert.That(convertedInsertOrUpdateModel, Is.Not.Null);

            var updated = service.Update(inserted.Id, convertedInsertOrUpdateModel);
            affected = await service.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThan(0));
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.Name, Is.EqualTo(newName));
            Assert.That(updated.Size, Is.EqualTo(newSize));

            Assert.That(updated.Name, Is.Not.EqualTo(oldName));
            Assert.That(updated.Size, Is.Not.EqualTo(oldSize));
        }

        [Test]
        public async Task Should_Insert_Many_Companies_And_CommitAll_AtEnd()
        {
            var service = _manualDependencyInjection.GetService<ICompanyService<BranefWriteDbContext>>();

            if (service is null)
                Assert.Fail("Service not created!");

            var utcNow = DateTimeOffset.UtcNow;
            var entity1 = _CreateInsertOrUpdateCommand();
            var entity2 = _CreateInsertOrUpdateCommand();

            var insertedId1 = await service.InsertAsync(entity1);
            var insertedId2 = await service.InsertAsync(entity2);
            var affected = await service.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(2));

            var inserted1 = await service.GetByIdAsync(insertedId1.Id);
            var inserted2 = await service.GetByIdAsync(insertedId2.Id);

            Assert.That(affected, Is.GreaterThanOrEqualTo(2));
            Assert.That(inserted1, Is.Not.Null);
            Assert.That(inserted2, Is.Not.Null);

            Assert.That(inserted1.Id, Is.Not.Empty);
            Assert.That(inserted1.Name, Is.EqualTo(entity1.Name));
            Assert.That(inserted1.Size, Is.EqualTo(entity1.Size));

            Assert.That(inserted2.Id, Is.Not.Empty);
            Assert.That(inserted2.Name, Is.EqualTo(entity2.Name));
            Assert.That(inserted2.Size, Is.EqualTo(entity2.Size));
        }

        [Test]
        public async Task Should_Get_All_Companies_When_Pass_Null_As_Query()
        {
            var service = _manualDependencyInjection.GetService<ICompanyService<BranefWriteDbContext>>();

            if (service is null)
                Assert.Fail("Service not created!");

            var utcNow = DateTimeOffset.UtcNow;
            var entity1 = _CreateInsertOrUpdateCommand();
            var entity2 = _CreateInsertOrUpdateCommand();

            var inserted1 = await service.InsertAsync(entity1);
            var inserted2 = await service.InsertAsync(entity2);
            var affected = await service.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(2));

            var companies = await service.GetAsync();

            Assert.That(companies, Is.Not.Null);
            Assert.That(companies, Is.Not.Empty);
            Assert.That(companies.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Should_Get_AtLeast_OneCompany_When_Pass_A_Query()
        {
            var service = _manualDependencyInjection.GetService<ICompanyService<BranefWriteDbContext>>();

            if (service is null)
                Assert.Fail("Service not created!");

            var utcNow = DateTimeOffset.UtcNow;

            var model1 = _CreateInsertOrUpdateCommand();
            var model2 = _CreateInsertOrUpdateCommand();
            var model3 = _CreateInsertOrUpdateCommand();

            var inserted1 = await service.InsertAsync(model1);
            var inserted2 = await service.InsertAsync(model2);
            var inserted3 = await service.InsertAsync(model3);
            var affected = await service.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(2));

            var companies = await service.GetAsync(new CompanyQuery
            {
                Name = model1.Name
            });

            Assert.That(companies, Is.Not.Null);
            Assert.That(companies, Is.Not.Empty);
            Assert.That(companies.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Should_Get_All_Companies_When_Pass_Null_as_Query_With_Pagination()
        {
            var service = _manualDependencyInjection.GetService<ICompanyService<BranefWriteDbContext>>();

            if (service is null)
                Assert.Fail("Service not created!");

            var utcNow = DateTimeOffset.UtcNow;
            var inserteds = new HashSet<Guid>();

            for (var i = 0; i < 10; i++)
            {
                var entity = _CreateInsertOrUpdateCommand();
                var inserted = await service.InsertAsync(entity);
                inserteds.Add(inserted.Id);
            }

            var affected = await service.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(10));

            var result = await service.GetPaginatedAsync(1, 5);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Empty);
            Assert.That(result.Data.Count, Is.EqualTo(5));

            Assert.That(result.Count, Is.EqualTo(10));
            Assert.That(result.PageNumber, Is.EqualTo(1));
            Assert.That(result.PageSize, Is.EqualTo(5));
        }
    }
}
