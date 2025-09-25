using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Specification
{
    public class SystemNotificationFilterSpecification : SpecificationBase<SystemNotification>
    {
        public SystemNotificationFilterSpecification(
            Expression<Func<SystemNotification, bool>>? criteria = null,
            Expression<Func<SystemNotification, object>>? orderBy = null,
            Expression<Func<SystemNotification, object>>? orderByDescending = null,
            int? skip = null,
            int? take = null)
            : base(criteria, orderBy, orderByDescending, skip, take)
        {
        }
    }

    public class SystemNotificationFilterSpecificationBuilder
    {
        private Expression<Func<SystemNotification, bool>> _criteria;
        private Expression<Func<SystemNotification, object>>? _orderBy;
        private Expression<Func<SystemNotification, object>>? _orderByDescending;
        private int? _skip;
        private int? _take;

        public SystemNotificationFilterSpecificationBuilder()
        {
            _criteria = x => true;
        }

        public SystemNotificationFilterSpecificationBuilder WithId(Guid? id)
        {
            if (id.HasValue && id != Guid.Empty)
            {
                _criteria = _criteria.AndAlso(x => x.Id == id);
            }
            return this;
        }

        public SystemNotificationFilterSpecificationBuilder WithTitle(string? title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                _criteria = _criteria.AndAlso(x => x.Title.Contains(title));
            }
            return this;
        }

        public SystemNotificationFilterSpecificationBuilder WithContent(string? content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                _criteria = _criteria.AndAlso(x => x.Content.Contains(content));
            }
            return this;
        }

        public SystemNotificationFilterSpecificationBuilder WithReceiverId(Guid? receiverId)
        {
            if (receiverId.HasValue && receiverId != Guid.Empty)
            {
                _criteria = _criteria.AndAlso(x => x.ReceiverId == receiverId);
            }
            return this;
        }

        public SystemNotificationFilterSpecificationBuilder WithCreatedAtRange(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate.HasValue)
            {
                _criteria = _criteria.AndAlso(x => x.CreatedAt >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                _criteria = _criteria.AndAlso(x => x.CreatedAt <= toDate.Value);
            }
            return this;
        }

        public SystemNotificationFilterSpecificationBuilder WithOrderBy(Expression<Func<SystemNotification, object>> orderBy)
        {
            _orderBy = orderBy;
            _orderByDescending = null;
            return this;
        }

        public SystemNotificationFilterSpecificationBuilder WithOrderByDescending(Expression<Func<SystemNotification, object>> orderByDescending)
        {
            _orderByDescending = orderByDescending;
            _orderBy = null;
            return this;
        }

        public SystemNotificationFilterSpecificationBuilder WithPaging(int skip, int take)
        {
            if (skip >= 0 && take > 0)
            {
                _skip = skip;
                _take = take;
            }
            return this;
        }

        public SystemNotificationFilterSpecification Build()
        {
            return new SystemNotificationFilterSpecification(
                criteria: _criteria,
                orderBy: _orderBy,
                orderByDescending: _orderByDescending,
                skip: _skip,
                take: _take);
        }
    }
}