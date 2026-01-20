using AutoMapper;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Categories;
using FitNetClean.Application.Features.ContraIndications;
using FitNetClean.Application.Features.Equipment;
using FitNetClean.Application.Features.Exercises;
using FitNetClean.Application.Features.Workouts;
using FitNetClean.Application.Features.WorkoutGroups;
using FitNetClean.Domain.Entities;

namespace FitNetClean.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ContraIndication mappings
        CreateMap<ContraIndication, ContraIndicationDto>();
        CreateMap<ContraIndication, ContraIndicationDetailDto>();
        CreateMap<CreateContraIndicationRequest, ContraIndication>();
        CreateMap<UpdateContraIndicationRequest, ContraIndication>();
        
        // Category mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<UpdateCategoryRequest, Category>();
        
        // Equipment mappings
        CreateMap<Domain.Entities.Equipment, EquipmentDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<Domain.Entities.Equipment, EquipmentDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<CreateEquipmentRequest, Domain.Entities.Equipment>();
        CreateMap<UpdateEquipmentRequest, Domain.Entities.Equipment>();
        
        // Exercise mappings
        CreateMap<Exercise, ExerciseDto>();
        CreateMap<Exercise, ExerciseDetailDto>()
            .ForMember(dest => dest.Repetition, opt => opt.MapFrom(src => src.Repetition.ToString()))
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => src.Equipment))
            .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom(src => src.ContraIndicationList));
        CreateMap<CreateExerciseRequest, Exercise>();
        CreateMap<UpdateExerciseRequest, Exercise>();
        
        // Workout mappings
        CreateMap<Workout, WorkoutDto>();
        CreateMap<Workout, WorkoutDetailDto>()
            .ForMember(dest => dest.WorkoutGroups, opt => opt.MapFrom(src => src.WorkoutGroupList));
        CreateMap<CreateWorkoutRequest, Workout>();
        CreateMap<UpdateWorkoutRequest, Workout>();
        
        // WorkoutGroup mappings
        CreateMap<WorkoutGroup, WorkoutGroupDto>();
        CreateMap<WorkoutGroup, WorkoutGroupDetailDto>()
            .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.ExerciseList));
        CreateMap<CreateWorkoutGroupRequest, WorkoutGroup>();
        CreateMap<UpdateWorkoutGroupRequest, WorkoutGroup>();
    }
}
