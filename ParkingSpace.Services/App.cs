﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using ParkingSpace.DataAccess.Context;
using ParkingSpace.DataAccess;
using ParkingSpace.Models;
using ParkingSpace.Services.Core;
using ParkingSpace.DataAccess.Core;

namespace ParkingSpace.Services {
  public class App : RootClass {

    public bool TestingMode { get; private set; }

    //public App() : this(false) {
    //  //
    //}

    public App(bool testing = false)
      : base(testing) {

      this.TestingMode = testing;
    }

    protected override DbContext NewDbContext() {
      return new ParkingSpaceDb();
    }

    protected override void RegisterServices() {
      this.AddService<ParkingTicket, ParkingTicketService, ParkingTicketRepository>();
      this.AddService<Setting, SettingService, SettingRepository>();
    }

    protected override void RegisterServicesForUnitTests() {
      this.AddService<ParkingTicket, ParkingTicketService, FakeRepository<ParkingTicket>>();
      this.AddService<Setting, SettingService, FakeRepository<Setting>>();
    }

    public ParkingTicketService ParkingTickets {
      get {
        return this.Services<ParkingTicket, ParkingTicketService>();
      }
    }

    public SettingService Settings {
      get {
        return this.Services<Setting, SettingService>();
      }
    }
  }
}