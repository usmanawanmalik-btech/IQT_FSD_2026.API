
using Btech.Package.Cache.Memory;
using Btech.Package.Domain.Base;
using Btech.Package.Domain.Common; 
using Btech.Package.Domain.Enums;
using Btech.Package.Domain.Exceptions;
using Btech.Package.Domain.Exceptions.Helper;
using Btech.Package.EntityFramework.Interfaces;
using IQT_FSD_2026.Application.Services.Interfaces;
using IQT_FSD_2026.Domain.Dtos; 
using IQT_FSD_2026.Infrastructure.DbContexts;
using IQT_FSD_2026.Infrastructure.DbModels;
using IQT_FSD_2026.Infrastructure.Repositories;

using System.Linq.Expressions; 

namespace IQT_FSD_2026.Application.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IUnitOfWork<ApplicationDbContext> _uow;
        private readonly IUserTaskRepository _repository;
        private readonly IInMemoryCacheService _cache;

        public UserTaskService(
            IUnitOfWork<ApplicationDbContext> uow,
            IUserTaskRepository repository,
            IInMemoryCacheService cache)
        {
            _uow = uow;
            _repository = repository;
            _cache = cache;
        } 

        public List<UserTaskDto> GetAll()
        {
            var list = new List<UserTaskDto>();
            try
            {
                var efEntityList = _repository.ToEnumerable().ToList();
                list = efEntityList.Select(c => c.MapTo<UserTaskDto>()).OrderBy(o=>o.Title).ToList()!;
            }
            catch (DataLayerException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var corEx = ex.MapToCoreLayerException();
                throw corEx;
            }
            return list;

        }
        public List<UserTaskDto> GetBySearchCriteria(UserTaskSearchCriteriaDto searchCriteria)
        {
            throw new NotImplementedException();
        }
        public PagedListDto<UserTaskDto> GetList(UserTaskSearchCriteriaDto searchCriteria)
        {
            var pagedList = new PagedListDto<UserTaskDto>();

            if (searchCriteria == null)
                throw new CoreLayerException(ErrorCode.CoreLayerError, "Search criteria cannot be null.");
            else if (searchCriteria.CurrentPage < 0 || searchCriteria.PageSize <= 0)
                throw new CoreLayerException(ErrorCode.CoreLayerError, "Page index and size must be greater than zero.");

            try
            {
                Expression<Func<UserTask, bool>> applyFilter =
                   w => w.CompanyId == searchCriteria.CompanyId &&
                        w.BranchId == searchCriteria.BranchId &&
                        w.LocationId == searchCriteria.LocationId;

                bool allowAdditionalFilter = !searchCriteria.DateFilter!.IsObjectNull();
                Expression<Func<UserTask, bool>> additionalFilter = null!; 
               
                var efEntityList =
                    _repository.ToPagedList(searchCriteria.CurrentPage, searchCriteria.PageSize,
                        allowAdditionalFilter, additionalFilter, applyFilter, null, o => o.Title); 

                pagedList = efEntityList.MapTo<PagedListDto<UserTaskDto>>()!;
                return pagedList;
            }
            catch (DataLayerException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var corEx = ex.MapToCoreLayerException();
                throw corEx;
            }
        }
        public UserTaskDto Get(long _id)
        {
            try
            {
                if (_id <= 0) throw new CoreLayerException(ErrorCode.CoreLayerError, "UserTask id must be greater than zero.");
                
                var efEntity = _repository.FirstOrDefault(f => f.TaskId == _id, null);
                if (efEntity == null) throw new CoreLayerException(ErrorCode.CoreLayerError, $"UserTask with id {_id} not found.");
                  
                var entity = efEntity.MapTo<UserTaskDto>()!;
                return entity;
            }
            catch (DataLayerException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var corEx = ex.MapToCoreLayerException();
                throw corEx;
            }
        }
        public UserTaskDto Save(UserTaskDto _entity)
        {
            try
            {
                _uow.BeginTransaction();

                var efEntity = new UserTask();
                if (_entity.TaskId.IsObjectNotNullOrNotZero())
                {
                    efEntity = _repository.FirstOrDefault(f => f.TaskId == _entity.TaskId);
                    if (efEntity == null) 
                        throw new CoreLayerException(ErrorCode.CoreLayerError, $"UserTask with Id {_entity.TaskId} not found.");
                }

                efEntity.AutoMapper(_entity); 

                if (_entity.IsAdded) _repository.Add(efEntity!); 
                if (_entity.IsModified) _repository.Update(efEntity!);
 
                _uow.SaveAndCommit();

                _entity = efEntity?.MapTo<UserTaskDto>()!; 
                return _entity;
            }
            catch (DataLayerException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var corEx = ex.MapToCoreLayerException();
                throw corEx;
            }
        }
        public bool Delete(long _id)
        {
            try
            {
                if (_id <= 0) throw new CoreLayerException(ErrorCode.CoreLayerError, "UserTask Id must be greater than zero.");
                var efEntity = _repository.FirstOrDefault(f => f.TaskId == _id);
                if (efEntity == null) throw new CoreLayerException(ErrorCode.CoreLayerError, $"UserTask with Id {_id} not found.");

                _uow.BeginTransaction(); 
                _repository.Delete(efEntity);
                _uow.SaveAndCommit();

                return true;
            }
            catch (DataLayerException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var corEx = ex.MapToCoreLayerException();
                throw corEx;
            }
        } 
        public bool ForceDelete(long _id)
        {
            try
            {
                if (_id <= 0) throw new CoreLayerException(ErrorCode.CoreLayerError, "UserTask Id must be greater than zero.");
                var efEntity = _repository.FirstOrDefault(f => f.TaskId == _id);
                if (efEntity == null) throw new CoreLayerException(ErrorCode.CoreLayerError, $"UserTask with Id {_id} not found.");

                _uow.BeginTransaction();
                _repository.ForceDelete(efEntity);
                _uow.SaveAndCommit();

                return true;
            }
            catch (DataLayerException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                var corEx = ex.MapToCoreLayerException();
                throw corEx;
            }
        }  
    }
}
