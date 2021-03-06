﻿using ParkingSpace.Models;
using ParkingSpace.Services;
using ParkingSpace.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Should;

namespace ParkingSpace.Facts.Services {
  public class ParkingTicketServiceFacts {
    public class GeneralUsage {

      [Fact]
      public void HasDefaultValues() {
        var s = new ParkingTicketService();
        Assert.Equal(0, s.GateId);
        //Assert.Equal(1, s.NextId);
      }

    }

    public class CreateParkingTicketMethod {
      private readonly ITestOutputHelper output;

      public CreateParkingTicketMethod(ITestOutputHelper output) {
        this.output = output;
      }

      [Fact]
      public void ReturnParkingTicket() {
        using (var app = new App(testing: true)) {
          var s = app.ParkingTickets;

          var t = s.CreateParkingTicket("1122");


          Assert.NotNull(t);
          Assert.Equal("1122", t.PlateNumber);
        }
      }

      [Fact]
      public void NewTicket_CheckDate() {
        using (var app = new App(testing: true)) {
          var s = app.ParkingTickets;
          var dt = DateTime.Now;
          SystemTime.SetDateTime(dt);

          var t = s.CreateParkingTicket("1122");

          Assert.NotEqual(default(DateTime), t.DateIn);
          Assert.Equal(dt, t.DateIn);
          Assert.Null(t.DateOut);
        }
      }

      [Fact]
      public void NewTicket_HasAutoRunningId() {
        using (var app = new App(testing: true)) {
          var s = app.ParkingTickets;

          var ticket1 = s.CreateParkingTicket("23");
          var ticketId1 = string.Format("00-{0:00000}", 1);

          displayTicket(ticket1);

          Assert.Equal(ticketId1, ticket1.Id);

          var ticket2 = s.CreateParkingTicket("555");
          var ticketId2 = string.Format("00-{0:00000}", 2);

          displayTicket(ticket2);

          Assert.Equal(ticketId2, ticket2.Id);
        }
      }

      [Fact]
      public void NewTicket_UsesGateIdFromService() {
        using (var app = new App(testing: true)) {
          var ticket = app.ParkingTickets.CreateParkingTicket("23");
          Assert.Equal(app.ParkingTickets.GateId, ticket.GateId);
        }
      }

      private void displayTicket(ParkingTicket t) {
        output.WriteLine("TICKET!!!");
        output.WriteLine($"Id:      {t.Id}");
        output.WriteLine($"Gate:    {t.GateId}");
        output.WriteLine($"Plate:   {t.PlateNumber}");
        output.WriteLine($"Date In: {t.DateIn}");
      }
      
      [Fact]
      public void NewTicket_HasInsertedToDatabase() {
        using (var app = new App(testing : true)) {
          var t = app.ParkingTickets.CreateParkingTicket("112233");

          var count = app.ParkingTickets
            .All().Count();

          Assert.Equal(1, count);

          var firstTicket = app.ParkingTickets
            .All().FirstOrDefault();

          Assert.Equal("112233", firstTicket.PlateNumber);
        }
      }
    }

    public class CheckoutMethod {
      [Fact]
      public void ShouldStampDateOutToTicket() {
        using (var app = new App(testing: true)) {
          var t1 = app.ParkingTickets.CreateParkingTicket("1111");

          app.ParkingTickets.Checkout(t1);

          Assert.NotNull(t1.DateOut);
        }

      }
    }

    public class GetActiveTicketProperty {

      [Fact]
      public void ShouldReturnOnlyActiveTickets() {
        using (var app = new App(testing: true)) {
          var t1 = app.ParkingTickets.CreateParkingTicket("1111");
          var t2 = app.ParkingTickets.CreateParkingTicket("1112");
          var t3 = app.ParkingTickets.CreateParkingTicket("1113");
          var t4 = app.ParkingTickets.CreateParkingTicket("1114");
          var t5 = app.ParkingTickets.CreateParkingTicket("1115");

          app.ParkingTickets.Checkout(t2);

          IEnumerable<ParkingTicket> tickets = app.ParkingTickets.ActiveTickets;

          Assert.Equal(4, tickets.Count());
          tickets.ShouldNotContain(t2);
          tickets.ShouldContain(t1);
          tickets.ShouldContain(t3);
          tickets.ShouldContain(t4);
          tickets.ShouldContain(t5);
        }
      }

    }
  }
}
