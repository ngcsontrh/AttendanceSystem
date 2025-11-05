using AttendanceSystem.Application.Commons;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Features.LeaveRequest.Commands
{
    public record ApproveLeaveRequestCommand(
        Guid LeaveRequestId,
        Guid ApprovedById
    );

    public class ApproveLeaveRequestCommandHandler
    {
        private readonly ILogger<ApproveLeaveRequestCommandHandler> _logger;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IIdentityService _identityService;

        public ApproveLeaveRequestCommandHandler(
            ILogger<ApproveLeaveRequestCommandHandler> logger,
            ILeaveRequestRepository leaveRequestRepository,
            IEmployeeRepository employeeRepository,
            IIdentityService identityService
            )
        {
            _logger = logger;
            _leaveRequestRepository = leaveRequestRepository;
            _employeeRepository = employeeRepository;
            _identityService = identityService;
        }

        public async Task<Result> ExecuteAsync(ApproveLeaveRequestCommand request)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.LeaveRequestId);
            if (leaveRequest == null)
            {
                _logger.LogWarning("Không tìm thấy đơn xin nghỉ với ID {LeaveRequestId}.", request.LeaveRequestId);
                return Result.Fail(new NotFoundError("Không tìm thấy đơn xin nghỉ"));
            }
            var approver = await _employeeRepository.GetByIdAsync(request.ApprovedById);
            if (approver == null)
            {
                _logger.LogWarning("Không tìm thấy nhân viên phê duyệt với ID {ApprovedById}.", request.ApprovedById);
                return Result.Fail(new NotFoundError("Không tìm thấy nhân viên phê duyệt"));
            }            
            if (approver.UserId == null)
            {
                _logger.LogWarning("Nhân viên phê duyệt với ID {ApprovedById} không có tài khoản người dùng liên kết.", request.ApprovedById);
                return Result.Fail(new NotFoundError("Nhân viên phê duyệt không có tài khoản người dùng liên kết"));
            };
            var approverUserId = approver.UserId.Value;
            var employeeRole = await _identityService.GetRoleByUserIdAsync(approverUserId);
            if (employeeRole != AppConstraint.ManagerRole && employeeRole != AppConstraint.AdminRole)
            {
                _logger.LogWarning("Nhân viên phê duyệt với ID {ApprovedById} không có quyền phê duyệt đơn xin nghỉ.", request.ApprovedById);
                return Result.Fail(new BusinessError("Nhân viên phê duyệt không có quyền phê duyệt đơn xin nghỉ"));
            }
            leaveRequest.Status = Domain.Entities.LeaveStatus.Approved;
            leaveRequest.ApprovedById = approver.Id;
            leaveRequest.UpdatedAt = DateTime.Now;
            leaveRequest.UpdatedById = approverUserId;
            await _leaveRequestRepository.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
