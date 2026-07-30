
using Btech.Package.Domain.Base;
using Btech.Package.Domain.Common;
using Btech.Package.Domain.Exceptions;
using IQT_FSD_2026.Application.Services.Interfaces;
using IQT_FSD_2026.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IQT_FSD_2026.API.Controllers;

//[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UserTaskController : ControllerBase
{ 
    private readonly IUserTaskService _service;
    private readonly ILogger<UserTaskController> _logger;
    public UserTaskController(IUserTaskService service, ILogger<UserTaskController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost, Route("GetList")]
    public ActionResult GetList([FromBody]UserTaskSearchCriteriaDto searchCriteriaDto)
    { 
        try
        {
            if (searchCriteriaDto.CurrentPage < 0 || searchCriteriaDto.PageSize <= 0)
                return ApiResult<string>.Failed("Page index and size must be greater than zero.");

            var listData = _service.GetList(searchCriteriaDto!);
            var listDataVM = listData.MapTo<PagedListDto<UserTaskDto>>(); 
            return ApiResult<PagedListDto<UserTaskDto>>.Success(listDataVM);  
        } 
        catch (Exception ex)
        { 
            return ApiResult<string>.Errors(ex);
        } 
    }
    
    [HttpGet, Route("Id/{userTaskId}/Get")]
    public ActionResult Get(long userTaskId)
    {
        try
        {
            if (userTaskId == 0) return ApiResult<string>.Failed("UserTask Id should be a valid Id."); 
            var UserTaskDto = _service.Get(userTaskId);
            if (UserTaskDto == null) return ApiResult<string>.Failed("Record does not exists."); 
            return ApiResult<UserTaskDto>.Success(UserTaskDto);
        }
        catch (Exception ex)
        {
            return ApiResult<string>.Errors(ex);
        }
    } 

    [HttpPost, Route("Save")]
    public ActionResult Save([FromBody]UserTaskDto entityDto)
    { 
        try
        {
            if (entityDto == null) return ApiResult<string>.Failed("Unable to process null object.");
            entityDto = _service.Save(entityDto);
            return ApiResult<UserTaskDto>.Success(entityDto);
        }
        catch (CoreLayerException ex)
        {
            return ApiResult<string>.Failed($"{ex?.InnerException?.Message ?? string.Empty}");
        }
        catch (Exception ex)
        {
            return ApiResult<string>.Errors(ex);
        }
    }

    [HttpDelete, Route("Id/{userTaskId}/Delete")]
    public ActionResult Delete(long userTaskId)
    {
        try
        {
            if (userTaskId == 0)
                return ApiResult<bool>.Failed("UserTask Id should be a valid Id.");

            var isDeleted = (bool)_service.Delete(userTaskId)!; 
            if (!isDeleted) return ApiResult<bool>.Failed("Record does not exists.");
             
            return ApiResult<bool>.Success(isDeleted, "Record deleted successfully!");
        }
        catch (Exception ex)
        {
            return ApiResult<bool>.Errors(ex);
        }
    } 
}
