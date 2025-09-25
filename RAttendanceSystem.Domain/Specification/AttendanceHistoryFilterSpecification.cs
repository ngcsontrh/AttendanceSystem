using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Specification
{
    public class AttendanceHistoryFilterSpecification : SpecificationBase<AttendanceHistory>
    {
        public AttendanceHistoryFilterSpecification(
            Expression<Func<AttendanceHistory, bool>>? criteria = null,
            Expression<Func<AttendanceHistory, object>>? orderBy = null,
            Expression<Func<AttendanceHistory, object>>? orderByDescending = null,
            int? skip = null,
            int? take = null)
            : base(criteria, orderBy, orderByDescending, skip, take)
        {
        }
    }

    public class AttendanceHistoryFilterSpecificationBuilder
    {
        private Expression<Func<AttendanceHistory, bool>> _criteria;
        private Expression<Func<AttendanceHistory, object>>? _orderBy;
        private Expression<Func<AttendanceHistory, object>>? _orderByDescending;
        private int? _skip;
        private int? _take;

        public AttendanceHistoryFilterSpecificationBuilder()
        {
            _criteria = x => true;
        }

        public AttendanceHistoryFilterSpecificationBuilder WithEmployeeId(Guid? employeeId)
        {
            if (employeeId.HasValue && employeeId != Guid.Empty)
            {
                _criteria = _criteria.AndAlso(x => x.EmployeeId == employeeId);
            }
            return this;
        }

        public AttendanceHistoryFilterSpecificationBuilder WithDateRange(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate.HasValue)
            {
                _criteria = _criteria.AndAlso(x => x.CheckInTime.Date >= fromDate.Value.Date);
            }
            if (toDate.HasValue)
            {
                _criteria = _criteria.AndAlso(x => x.CheckInTime.Date <= toDate.Value.Date);
            }
            return this;
        }

        public AttendanceHistoryFilterSpecificationBuilder WithDay(DateTime? day)
        {
            if (day.HasValue)
            {
                _criteria = _criteria.AndAlso(x => x.CheckInTime.Date == day.Value.Date);
            }
            return this;
        }

        public AttendanceHistoryFilterSpecificationBuilder WithMonth(int? year, int? month)
        {
            if (year.HasValue && month.HasValue)
            {
                _criteria = _criteria.AndAlso(x => x.CheckInTime.Year == year && x.CheckInTime.Month == month);
            }
            return this;
        }

        public AttendanceHistoryFilterSpecificationBuilder WithOrderBy(Expression<Func<AttendanceHistory, object>> orderBy)
        {
            _orderBy = orderBy;
            _orderByDescending = null;
            return this;
        }

        public AttendanceHistoryFilterSpecificationBuilder WithOrderByDescending(Expression<Func<AttendanceHistory, object>> orderByDescending)
        {
            _orderByDescending = orderByDescending;
            _orderBy = null;
            return this;
        }

        public AttendanceHistoryFilterSpecificationBuilder WithPaging(int skip, int take)
        {
            if (skip >= 0 && take > 0)
            {
                _skip = skip;
                _take = take;
            }
            return this;
        }

        public AttendanceHistoryFilterSpecification Build()
        {
            return new AttendanceHistoryFilterSpecification(
                criteria: _criteria,
                orderBy: _orderBy,
                orderByDescending: _orderByDescending,
                skip: _skip,
                take: _take);
        }
    }
}