namespace Api.MappingProfiles
{
    using AutoMapper;
    using Domain.Entities;
    using Domain.Models.Request.Sample;
    using Domain.Models.Response.Sample;

    /// <summary>
    /// AutoMapper profile for mapping between domain entities and response models.
    /// </summary>
    public class SampleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleProfile"/> class.
        /// </summary>
        public SampleProfile()
        {
            this.CreateMap<SampleEntity, GetSampleResponseModel>();
            this.CreateMap<CreateSampleRequestModel, SampleEntity>();
            this.CreateMap<SampleEntity, CreateSampleResponseModel>();
            this.CreateMap<UpdateSampleRequestModel, SampleEntity>();
            this.CreateMap<SampleEntity, UpdateSampleResponseModel>();
        }
    }
}
