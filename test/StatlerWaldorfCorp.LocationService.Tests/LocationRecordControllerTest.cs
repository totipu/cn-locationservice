
using Xunit;
using System.Collections.Generic;
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Controllers;
using StatlerWaldorfCorp.LocationService.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace StatlerWaldorfCorp.LocationService
{
    public class LocationRecordControllerTest
    {
        [Fact]
        public void ShouldAdd()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = Guid.NewGuid(), MemberID = memberGuid, Timestamp = 1 });

            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = Guid.NewGuid(), MemberID = memberGuid, Timestamp = 2 });

            Assert.Equal(2, repository.AllForMember(memberGuid).Count());

        }

        [Fact]
        public void ShoulReturnEmptyListForNewMember()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            ICollection<LocationRecord> locationRecords = 
                ((controller.GetLocationsForMember(memberGuid) as ObjectResult).Value as ICollection<LocationRecord>);

            Assert.Equal(0, locationRecords.Count());
        }

        [Fact]
        public void ShouldTrackAllLocationsForMember()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = Guid.NewGuid(), MemberID = memberGuid, Timestamp = 1,
                    Latitude = 12.3f });

            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = Guid.NewGuid(), MemberID = memberGuid, Timestamp = 2,
                    Latitude = 23.4f });

            // lokacija za random membera
            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = Guid.NewGuid(), MemberID = Guid.NewGuid(), Timestamp = 3,
                    Latitude = 23.4f });

            ICollection<LocationRecord> locationRecords = 
                ((controller.GetLocationsForMember(memberGuid) as ObjectResult).Value as ICollection<LocationRecord>);

            Assert.Equal(2, locationRecords.Count());
        }

        [Fact]
        public void ShouldTrackNullLatestForNewMember()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            LocationRecord latest = 
                ((controller.GetLatestForMember(memberGuid) as ObjectResult).Value as LocationRecord);

            Assert.Null(latest);
        }

        [Fact]
        public void ShouldTrackLatestLocationsForMember()
        {
            ILocationRecordRepository repository = new InMemoryLocationRecordRepository();
            LocationRecordController controller = new LocationRecordController(repository);
            Guid memberGuid = Guid.NewGuid();

            Guid latestId = Guid.NewGuid();

            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = Guid.NewGuid(), MemberID = memberGuid, Timestamp = 1,
                    Latitude = 12.3f });

            // zadnja lokacija
            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = latestId, MemberID = memberGuid, Timestamp = 3,
                    Latitude = 23.4f });

            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = Guid.NewGuid(), MemberID = memberGuid, Timestamp = 2,
                    Latitude = 23.4f });

            // lokacija za random membera
            controller.AddLocation(memberGuid, 
                new LocationRecord() { ID = Guid.NewGuid(), MemberID = Guid.NewGuid(), Timestamp = 4,
                    Latitude = 23.4f });

            LocationRecord latest = 
                ((controller.GetLatestForMember(memberGuid) as ObjectResult).Value as LocationRecord);
            
            Assert.NotNull(latest);
            Assert.Equal(latestId, latest.ID);        
        }
    }
}