﻿using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Clients;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Public.Tags;
using Xunit;

namespace Volo.CmsKit.Tags
{
    public class TagAdminAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly ITagAdminAppService _tagAdminAppService;
        private ICurrentUser _currentUser;
        private readonly CmsKitTestData _cmsKitTestData;

        public TagAdminAppService_Tests()
        {
            _tagAdminAppService = GetRequiredService<ITagAdminAppService>();
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _currentUser = Substitute.For<ICurrentUser>();
            services.AddSingleton(_currentUser);
        }

        [Fact]
        public async Task ShouldCreateProperly()
        {
            var list = await _tagAdminAppService.CreateAsync(new TagCreateDto
            {
                EntityType = "any_new_type",
                Name = "1",
            });

            list.Id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task ShouldThrowException_WhenTagAlreadyExist()
        {
            await Should.ThrowAsync<BusinessException>(async () => await _tagAdminAppService.CreateAsync(new TagCreateDto
            {
                EntityType = _cmsKitTestData.Content_1_EntityType,
                Name = _cmsKitTestData.Content_1_Tags[0],
            }));
        }
    }
}
