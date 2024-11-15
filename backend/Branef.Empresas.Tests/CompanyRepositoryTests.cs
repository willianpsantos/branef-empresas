using Branef.Empresas.Data.Entities;
using Branef.Empresas.Data.Enums;
using Branef.Empresas.DB;
using Branef.Empresas.DependencyInjection;
using Branef.Empresas.Domain.Interfaces.Base;
using System;

namespace Branef.Empresas.Tests
{
    public class CompanyRepositoryTests
    {
        private ManualDependencyInjection _manualDependencyInjection;

        private string RandomString(int length) => TestUtils.RandomString(length);

        private Company _CreateEntity()
        {
            var utcNow = DateTimeOffset.UtcNow;

            return new Company
            {
                Id = Guid.NewGuid(),
                Name = RandomString(50),
                Size = Data.Enums.CompanySize.Pequena,
                IsDeleted = false
            };
        }


        [SetUp]
        public void Setup()
        {
            var builder = new ManualDependencyInjectionBuilder();

            _manualDependencyInjection = builder.BuildConfiguration(false).Build(services =>
            {
                services
                    .AddDomainInMemoryWriteDbContext()
                    .AddDomainRepositories();
            });
        }
        
        [Test]
        public async Task Should_Insert_NewCompany_And_Generate_NewId()
        {
            var repository = _manualDependencyInjection.GetService<IRepository<Company, BranefWriteDbContext>>();

            if (repository is null)
                Assert.Fail("Repository not created!");

            var entity = _CreateEntity();
            var inserted = await repository.InsertAndSaveChangesAsync(entity);

            Assert.That(inserted, Is.Not.Null);
            Assert.That(inserted.Id, Is.Not.Empty);
            Assert.That(inserted.Name, Is.Not.Null.Or.Not.Empty);
            Assert.That(inserted.IsDeleted, Is.False);
        }

        [Test]
        public async Task Should_Insert_Many_Companies_And_CommitAll_AtEnd()
        {
            var repository = _manualDependencyInjection.GetService<IRepository<Company, BranefWriteDbContext>>();

            if (repository is null)
                Assert.Fail("Repository not created!");

            var utcNow = DateTimeOffset.UtcNow;
            var entity1 = _CreateEntity();
            var entity2 = _CreateEntity();

            var inserted1 = await repository.InsertAsync(entity1);
            var inserted2 = await repository.InsertAsync(entity2);
            var affected = await repository.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(2));
            Assert.That(inserted1, Is.Not.Null);
            Assert.That(inserted2, Is.Not.Null);

            Assert.That(inserted1.Id, Is.Not.Empty);
            Assert.That(inserted1.Name, Is.Not.Null.Or.Empty);
            Assert.That(inserted1.IsDeleted, Is.False);
            Assert.That(inserted1.IncludedAt.GetValueOrDefault().Date, Is.EqualTo(DateTimeOffset.UtcNow.Date));


            Assert.That(inserted2.Id, Is.Not.Empty);
            Assert.That(inserted2.Name, Is.Not.Null.Or.Empty);
            Assert.That(inserted2.IsDeleted, Is.False);
            Assert.That(inserted2.IncludedAt.GetValueOrDefault().Date, Is.EqualTo(DateTimeOffset.UtcNow.Date));
        }

        [Test]
        public async Task Should_Update_Company_With_Given_Information()
        {
            var repository = _manualDependencyInjection.GetService<IRepository<Company, BranefWriteDbContext>>();

            if (repository is null)
                Assert.Fail("Repository not created!");

            var entity = _CreateEntity();
            var inserted = await repository.InsertAndSaveChangesAsync(entity);

            var newName = RandomString(60);
            var newCompanySize = CompanySize.Grande;

            var oldName = inserted.Name;
            var oldSize = inserted.Size;

            inserted.Name = newName;
            inserted.Size = newCompanySize;

            var updated = await repository.UpdateAndSaveChangesAsync(inserted);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.Size, Is.Not.EqualTo(oldSize));
            Assert.That(updated.Name, Is.Not.EqualTo(oldName));
            Assert.That(updated.IsDeleted, Is.False);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.Name, Is.EqualTo(newName));
            Assert.That(updated.Size, Is.EqualTo(newCompanySize));

            Assert.That(updated.UpdatedAt, Is.Not.Null);
            Assert.That(updated.UpdatedAt?.Date == DateTimeOffset.UtcNow.Date, Is.True);
        }
               

        [Test]
        public async Task Should_Get_All_Companies_When_Pass_Null_As_Query()
        {
            var repository = _manualDependencyInjection.GetService<IRepository<Company, BranefWriteDbContext>>();

            if (repository is null)
                Assert.Fail("Repository not created!");

            var utcNow = DateTimeOffset.UtcNow;
            var entity1 = _CreateEntity();
            var entity2 = _CreateEntity();

            var inserted1 = await repository.InsertAsync(entity1);
            var inserted2 = await repository.InsertAsync(entity2);
            var affected = await repository.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(2));

            var inserteds = new Company[] { inserted1, inserted2 };
            var count = 0;

            await foreach (var item in repository.GetAsync())
            {
                if (!inserteds.Any(_ => _.Id == item.Id))
                {
                    Assert.Fail("Got record different from inserted companies");
                    continue;
                }

                count++;
            }

            Assert.That(inserteds.Length, Is.EqualTo(count));
        }

        [Test]
        public async Task Should_Get_AtLeast_OneCompany_When_Pass_A_Query()
        {
            var repository = _manualDependencyInjection.GetService<IRepository<Company, BranefWriteDbContext>>();

            if (repository is null)
                Assert.Fail("Repository not created!");

            var utcNow = DateTimeOffset.UtcNow;
            var entity1 = _CreateEntity();
            var entity2 = _CreateEntity();
            var entity3 = _CreateEntity();

            var inserted1 = await repository.InsertAsync(entity1);
            var inserted2 = await repository.InsertAsync(entity2);
            var inserted3 = await repository.InsertAsync(entity3);
            var affected = await repository.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(3));

            var inserteds = new Company[] { inserted1, inserted2, inserted3 };
            var count = 0;

            await foreach (var item in repository.GetAsync(_ => _.Name == entity2.Name))
            {
                if (!inserteds.Any(_ => _.Id == item.Id))
                {
                    Assert.Fail("Got record different from inserted companies");
                    continue;
                }

                count++;
            }

            Assert.That(count, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public async Task Should_Get_Companies_When_Pass_Null_as_Query_With_Pagination()
        {
            var repository = _manualDependencyInjection.GetService<IRepository<Company, BranefWriteDbContext>>();

            if (repository is null)
                Assert.Fail("Repository not created!");

            var insertedBy = Guid.NewGuid();
            var utcNow = DateTimeOffset.UtcNow;
            var inserteds = new HashSet<Company>();
            
            for(var i = 0; i < 10; i++)
            {
                var entity = _CreateEntity();
                var inserted = await repository.InsertAsync(entity);
                inserteds.Add(inserted);
            }

            var affected = await repository.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(2));

            var insertedCount = await repository.CountAsync(null);

            Assert.That(insertedCount, Is.EqualTo(10));

            var count = 0;

            await foreach (var item in repository.GetAsync(null, 1, 5))
                count++;

            Assert.That(count, Is.EqualTo(5));
        }

        [Test]
        public async Task Should_Get_Companies_When_Pass_a_Query_With_Pagination()
        {
            var repository = _manualDependencyInjection.GetService<IRepository<Company, BranefWriteDbContext>>();

            if (repository is null)
                Assert.Fail("Repository not created!");

            var insertedBy = Guid.NewGuid();
            var utcNow = DateTimeOffset.UtcNow;
            var inserteds = new HashSet<Company>();

            for (var i = 0; i < 10; i++)
            {
                var entity = _CreateEntity();
                var inserted = await repository.InsertAsync(entity);
                inserteds.Add(inserted);
            }

            var affected = await repository.SaveChangesAsync();

            Assert.That(affected, Is.GreaterThanOrEqualTo(10));

            var thirdElement = inserteds.ElementAt(3);
            var insertedCount = await repository.CountAsync(x => x.Name == thirdElement.Name);

            Assert.That(insertedCount, Is.AtMost(10));

            var count = 0;

            await foreach (var item in repository.GetAsync(x => x.Name == thirdElement.Name, 1, 5))
                count++;

            Assert.That(count, Is.AtMost(5));
        }
    }
}