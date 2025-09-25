using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Specification
{
    public class AttendanceWiFiFilterSpecification : SpecificationBase<AttendanceWiFi>
    {
        public AttendanceWiFiFilterSpecification(
            Expression<Func<AttendanceWiFi, bool>>? criteria = null,
            Expression<Func<AttendanceWiFi, object>>? orderBy = null,
            Expression<Func<AttendanceWiFi, object>>? orderByDescending = null,
            int? skip = null,
            int? take = null)
            : base(criteria, orderBy, orderByDescending, skip, take)
        {
        }
    }

    public class AttendanceWiFiFilterSpecificationBuilder
    {
        private Expression<Func<AttendanceWiFi, bool>> _criteria;
        private Expression<Func<AttendanceWiFi, object>>? _orderBy;
        private Expression<Func<AttendanceWiFi, object>>? _orderByDescending;
        private int? _skip;
        private int? _take;

        public AttendanceWiFiFilterSpecificationBuilder()
        {
            _criteria = x => true;
        }

        public AttendanceWiFiFilterSpecificationBuilder WithId(Guid? id)
        {
            if (id.HasValue && id != Guid.Empty)
            {
                _criteria = _criteria.AndAlso(x => x.Id == id);
            }
            return this;
        }

        public AttendanceWiFiFilterSpecificationBuilder WithLocation(string? location)
        {
            if (!string.IsNullOrEmpty(location))
            {
                _criteria = _criteria.AndAlso(x => x.Location.Contains(location));
            }
            return this;
        }

        public AttendanceWiFiFilterSpecificationBuilder WithSSID(string? ssid)
        {
            if (!string.IsNullOrEmpty(ssid))
            {
                _criteria = _criteria.AndAlso(x => x.SSID.Contains(ssid));
            }
            return this;
        }

        public AttendanceWiFiFilterSpecificationBuilder WithBSSID(string? bssid)
        {
            if (!string.IsNullOrEmpty(bssid))
            {
                _criteria = _criteria.AndAlso(x => x.BSSID.Contains(bssid));
            }
            return this;
        }

        public AttendanceWiFiFilterSpecificationBuilder WithOrderBy(Expression<Func<AttendanceWiFi, object>> orderBy)
        {
            _orderBy = orderBy;
            _orderByDescending = null;
            return this;
        }

        public AttendanceWiFiFilterSpecificationBuilder WithOrderByDescending(Expression<Func<AttendanceWiFi, object>> orderByDescending)
        {
            _orderByDescending = orderByDescending;
            _orderBy = null;
            return this;
        }

        public AttendanceWiFiFilterSpecificationBuilder WithPaging(int skip, int take)
        {
            if (skip >= 0 && take > 0)
            {
                _skip = skip;
                _take = take;
            }
            return this;
        }

        public AttendanceWiFiFilterSpecification Build()
        {
            return new AttendanceWiFiFilterSpecification(
                criteria: _criteria,
                orderBy: _orderBy,
                orderByDescending: _orderByDescending,
                skip: _skip,
                take: _take);
        }
    }
}
