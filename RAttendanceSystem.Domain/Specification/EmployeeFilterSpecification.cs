using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Specification
{
    public class EmployeeFilterSpecification : SpecificationBase<Employee>
    {
        public EmployeeFilterSpecification(
            Expression<Func<Employee, bool>>? criteria = null,
            Expression<Func<Employee, object>>? orderBy = null,
            Expression<Func<Employee, object>>? orderByDescending = null,
            int? skip = null,
            int? take = null)
            : base(criteria, orderBy, orderByDescending, skip, take)
        {
        }
    }

    public class EmployeeFilterSpecificationBuilder
    {
        private Expression<Func<Employee, bool>> _criteria;
        private Expression<Func<Employee, object>>? _orderBy;
        private Expression<Func<Employee, object>>? _orderByDescending;
        private int? _skip;
        private int? _take;        

        public EmployeeFilterSpecificationBuilder()
        {
            _criteria = x => true;
        }

        public EmployeeFilterSpecificationBuilder WithId(Guid? id)
        {
            if (id.HasValue && id != Guid.Empty)
            {
                _criteria = _criteria.AndAlso(x => x.Id == id);
            }
            return this;
        }

        public EmployeeFilterSpecificationBuilder WithFullName(string? fullName)
        {
            if (!string.IsNullOrEmpty(fullName))
            {
                _criteria = _criteria.AndAlso(x => x.FullName.Contains(fullName));
            }
            return this;
        }

        public EmployeeFilterSpecificationBuilder WithCode(string? code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                _criteria = _criteria.AndAlso(x => x.Code.Contains(code));
            }
            return this;
        }

        public EmployeeFilterSpecificationBuilder WithDepartmentId(Guid? departmentId)
        {
            if (departmentId.HasValue && departmentId != Guid.Empty)
            {
                _criteria = _criteria.AndAlso(x => x.DepartmentId == departmentId);
            }
            return this;
        }

        public EmployeeFilterSpecificationBuilder WithTitleId(Guid? titleId)
        {
            if (titleId.HasValue && titleId != Guid.Empty)
            {
                _criteria = _criteria.AndAlso(x => x.TitleId == titleId);
            }
            return this;
        }

        public EmployeeFilterSpecificationBuilder WithOrderBy(Expression<Func<Employee, object>> orderBy)
        {
            _orderBy = orderBy;
            _orderByDescending = null;
            return this;
        }

        public EmployeeFilterSpecificationBuilder WithOrderByDescending(Expression<Func<Employee, object>> orderByDescending)
        {
            _orderByDescending = orderByDescending;
            _orderBy = null;
            return this;
        }

        public EmployeeFilterSpecificationBuilder WithPaging(int skip, int take)
        {
            if (skip >= 0 && take > 0)
            {
                _skip = skip;
                _take = take;
            }
            return this;
        }

        public EmployeeFilterSpecification Build()
        {
            return new EmployeeFilterSpecification(
                criteria: _criteria,
                orderBy: _orderBy,
                orderByDescending: _orderByDescending,
                skip: _skip,
                take: _take);
        }
    }
}
